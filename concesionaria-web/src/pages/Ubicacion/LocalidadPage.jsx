import { useEffect, useMemo, useState } from "react";
import { MapPin } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import LocalidadTable from "@/components/tables/Ubicacion/LocalidadTable";
import { getLocalidades } from "@/services/Ubicacion/localidadService";
import { toastService } from "@/services/toastService";

export const LocalidadPage = () => {
  const [localidades, setLocalidades] = useState([]);
  const [filtro, setFiltro] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getLocalidades()
      .then((data) => setLocalidades(data))
      .catch(() => {
        toastService.error("Error", {
          description: "No se pudieron cargar las localidades",
        });
      })
      .finally(() => setLoading(false));
  }, []);

  const localidadesFiltradas = useMemo(
    () =>
      localidades.filter(
        (localidad) =>
          localidad.nombre?.toUpperCase().includes(filtro) ||
          localidad.provinciaNombre?.toUpperCase().includes(filtro) ||
          localidad.codigoPostal?.toUpperCase().includes(filtro),
      ),
    [localidades, filtro],
  );

  return (
    <div>
      <PageHeader title="Localidades" icon={MapPin} />

      {loading ? (
        <div className="flex h-64 items-center justify-center">Cargando...</div>
      ) : (
        <LocalidadTable data={localidadesFiltradas} onSearch={setFiltro} />
      )}
    </div>
  );
};
