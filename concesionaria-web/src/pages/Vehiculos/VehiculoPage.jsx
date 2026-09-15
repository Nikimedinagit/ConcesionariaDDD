import { useEffect, useState } from "react";
import { CarFront } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import { TooltipProvider } from "@/components/ui/tooltip";
import { VehiculoModal } from "@/components/modals/Vehiculo/VehiculoModal";
import { ConfirmDeleteModal } from "@/components/modals/ConfirmDeleteModal";
import { VehiculoCards } from "@/components/cards/VehiculoCards";
import { useVehiculos } from "@/hooks/Vehiculo/useVehiculos";
import VehiculoService from "@/services/Vehiculo/vehiculoService";
import ModeloVehiculoService from "@/services/Vehiculo/modeloVehiculoService";
import SucursalService from "@/services/Ubicacion/sucursalService";
import { toastService } from "@/services/toastService";
import { useAuth } from "@/context/AuthContext";

export const VehiculoPage = () => {
  const { user, activeSucursal } = useAuth();
  const isAdministrator = user?.roles?.some(
    (role) => String(role).toUpperCase() === "ADMINISTRADOR",
  );
  const [tipo, setTipo] = useState("disponibles");
  const [filtro, setFiltro] = useState("");
  const [debouncedFiltro, setDebouncedFiltro] = useState("");
  const sucursalFiltro = isAdministrator && activeSucursal?.id
    ? activeSucursal.id
    : "actual";
  const { data, loading, refetch } = useVehiculos(
    tipo,
    debouncedFiltro,
    sucursalFiltro,
  );
  const [modelos, setModelos] = useState([]);
  const [sucursales, setSucursales] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedVehiculo, setSelectedVehiculo] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [serverError, setServerError] = useState("");
  const [serverFieldErrors, setServerFieldErrors] = useState({});
  const [vehiculoToDelete, setVehiculoToDelete] = useState(null);
  const [deleteLoading, setDeleteLoading] = useState(false);

  useEffect(() => {
    const handler = setTimeout(() => setDebouncedFiltro(filtro), 500);
    return () => clearTimeout(handler);
  }, [filtro]);

  useEffect(() => {
    Promise.all([
      ModeloVehiculoService.getActivas(),
      SucursalService.getActivas(),
    ])
      .then(([modelosData, sucursalesData]) => {
        setModelos(modelosData);
        setSucursales(sucursalesData);
      })
      .catch(() =>
        toastService.error("Error", {
          description: "No se pudieron cargar los datos del formulario",
        }),
      );
  }, []);

  const openModal = (vehiculo = null) => {
    setServerError("");
    setServerFieldErrors({});
    setSelectedVehiculo(vehiculo);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedVehiculo(null);
    setServerError("");
    setServerFieldErrors({});
  };

  const saveVehicleImages = async (vehiculoId, imageChanges) => {
    const images = imageChanges?.images || [];
    const removedImageIds = imageChanges?.removedImageIds || [];
    const newImages = images.filter((image) => !image.isExisting);

    for (const imageId of removedImageIds) {
      await VehiculoService.eliminarImagen(vehiculoId, imageId);
    }

    const uploadedImages = [];

    for (const image of newImages) {
      const response = await VehiculoService.agregarImagen(
        vehiculoId,
        image.file,
        image.isPrincipal,
      );
      uploadedImages.push({
        ...image,
        id: response.imagen.imagenId,
        isExisting: true,
      });
    }

    const principalImage = images.find((image) => image.isPrincipal);
    if (principalImage?.isExisting) {
      await VehiculoService.establecerImagenPrincipal(
        vehiculoId,
        principalImage.id,
      );
    }

    const uploadedByLocalId = new Map(
      uploadedImages.map((image, index) => [newImages[index].id, image.id]),
    );
    const orderedImages = images.map((image, index) => ({
      imagenId: image.isExisting ? image.id : uploadedByLocalId.get(image.id),
      orden: index,
    }));

    if (orderedImages.length) {
      await VehiculoService.reordenarImagenes(vehiculoId, orderedImages);
    }
  };

  const handleSave = async (payload, imageChanges) => {
    setModalLoading(true);
    setServerError("");
    setServerFieldErrors({});

    try {
      let vehiculoId;

      if (payload.vehiculoId) {
        await VehiculoService.actualizar(payload.vehiculoId, payload);
        vehiculoId = payload.vehiculoId;
      } else {
        const response = await VehiculoService.crear(payload);
        vehiculoId = response.vehiculoId;
      }

      try {
        await saveVehicleImages(vehiculoId, imageChanges);
      } catch (imageUploadError) {
        const imageErrorData = imageUploadError.response?.data;
        const validationMessage =
          imageErrorData?.errors?.[0]?.errorMessage ||
          imageErrorData?.Errors?.[0]?.ErrorMessage;

        toastService.error("Vehículo guardado", {
          description:
            validationMessage ||
            imageErrorData?.message ||
            imageErrorData?.Message ||
            imageErrorData?.mensaje ||
            "Algunas operaciones de imágenes no pudieron completarse. Podés reintentarlas editando el vehículo.",
        });

        closeModal();
        refetch();
        return;
      }

      toastService.success("Éxito", {
        description: payload.vehiculoId
          ? "Vehículo actualizado correctamente"
          : "Vehículo e imágenes guardados correctamente",
      });

      closeModal();
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      const validationErrors = dataError?.errors || [];
      const fieldErrors = validationErrors.reduce((result, item) => {
        if (!item?.propertyName) return result;

        const fieldName =
          item.propertyName.charAt(0).toLowerCase() + item.propertyName.slice(1);
        result[fieldName] = item.errorMessage;
        return result;
      }, {});

      setServerFieldErrors(fieldErrors);
      setServerError(
        Object.keys(fieldErrors).length > 0
          ? "No se pudo guardar el vehículo. Revisá los campos marcados."
          : validationErrors[0]?.errorMessage ||
          dataError?.message ||
          dataError?.mensaje ||
          "Ocurrió un error al guardar el vehículo",
      );
    } finally {
      setModalLoading(false);
    }
  };

  const handleDelete = (vehiculo) => {
    setVehiculoToDelete(vehiculo);
  };

  const confirmDelete = async () => {
    if (!vehiculoToDelete) return;

    setDeleteLoading(true);

    try {
      await VehiculoService.eliminar(vehiculoToDelete.vehiculoId);
      toastService.success("Éxito", {
        description: "Vehículo eliminado correctamente",
      });
      setVehiculoToDelete(null);
      refetch();
    } catch (error) {
      const dataError = error.response?.data;
      toastService.error("Error", {
        description:
          dataError?.errors?.[0]?.errorMessage ||
          dataError?.message ||
          dataError?.mensaje ||
          "No se pudo eliminar el vehículo",
      });
    } finally {
      setDeleteLoading(false);
    }
  };

  return (
    <TooltipProvider delayDuration={300}>
      <div>
        <PageHeader title="Vehículos" icon={CarFront}>
          <AddButton onClick={() => openModal()}>Nuevo Vehículo</AddButton>
        </PageHeader>

        <VehiculoCards
          data={data}
          loading={loading}
          tipo={tipo}
          onTipoChange={setTipo}
          onSearch={setFiltro}
          sucursalFiltro={sucursalFiltro}
          onSucursalChange={() => {}}
          sucursales={sucursales}
          onEdit={openModal}
          onDelete={handleDelete}
        />

        {isModalOpen && (
          <VehiculoModal
            isOpen={isModalOpen}
            onClose={closeModal}
            onSave={handleSave}
            vehiculo={selectedVehiculo}
            modelos={modelos}
            loading={modalLoading}
            serverError={serverError}
            serverFieldErrors={serverFieldErrors}
          />
        )}

        <ConfirmDeleteModal
          isOpen={Boolean(vehiculoToDelete)}
          onClose={() => setVehiculoToDelete(null)}
          onConfirm={confirmDelete}
          loading={deleteLoading}
          title="Eliminar vehículo"
          description={`¿Querés eliminar el vehículo con patente ${vehiculoToDelete?.patente || ""}?`}
        />
      </div>
    </TooltipProvider>
  );
};
