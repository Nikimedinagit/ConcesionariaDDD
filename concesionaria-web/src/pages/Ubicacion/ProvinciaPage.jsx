import { useEffect, useMemo, useState } from "react";
import { Map } from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import ProvinciaTable from "@/components/tables/ProvinciaTable";
import { getProvincias } from "@/services/Ubicacion/localidadService";
import { toastService } from "@/services/toastService";

export const ProvinciaPage = () => {
  const [provincias, setProvincias] = useState([]);
  const [filtro, setFiltro] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getProvincias()
      .then((data) => setProvincias(data))
      .catch(() => {
        toastService.error("Error", {
          description: "No se pudieron cargar las provincias",
        });
      })
      .finally(() => setLoading(false));
  }, []);

  const provinciasFiltradas = useMemo(
    () =>
      provincias.filter((provincia) =>
        provincia.nombre?.toUpperCase().includes(filtro),
      ),
    [provincias, filtro],
  );

  return (
    <div>
      <PageHeader title="Provincias" icon={Map} />

      {loading ? (
        <div className="flex h-64 items-center justify-center">
          Cargando...
        </div>
      ) : (
        <ProvinciaTable data={provinciasFiltradas} onSearch={setFiltro} />
      )}
    </div>
  );
};
