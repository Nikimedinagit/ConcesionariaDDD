import { useCallback, useEffect, useState } from "react";
import CuentaService from "@/services/Contabilidad/cuentaService";

export const useCuentas = (
  tipo = "activas",
  filtro = "",
  tipoCuenta = "todos",
  nivel = "todos",
) => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const refetch = useCallback(() => {
    setRefreshTrigger((prev) => prev + 1);
  }, []);

  useEffect(() => {
    let isMounted = true;

    const fetchData = async () => {
      try {
        const result =
          tipo === "activas"
            ? await CuentaService.getActivas(filtro, tipoCuenta, nivel)
            : await CuentaService.getInactivas(filtro, tipoCuenta, nivel);

        if (isMounted) setData(result);
      } catch (error) {
        console.error("Error en hook de cuentas:", error);
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    fetchData();

    return () => {
      isMounted = false;
    };
  }, [tipo, filtro, tipoCuenta, nivel, refreshTrigger]);

  return { data, loading, refetch };
};
