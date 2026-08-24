/* eslint-disable react-hooks/set-state-in-effect */
import { useCallback, useEffect, useState } from "react";
import SucursalService from "@/services/Ubicacion/sucursalService";

export const useSucursales = (tipo = "activas", filtro = "") => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const refetch = useCallback(() => setRefreshTrigger((prev) => prev + 1), []);

  useEffect(() => {
    let isMounted = true;
    setLoading(true);

    const fetchData = async () => {
      try {
        const result =
          tipo === "activas"
            ? await SucursalService.getActivas(filtro)
            : await SucursalService.getInactivas(filtro);

        if (isMounted) setData(result);
      } catch (error) {
        console.error("Error en hook:", error);
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    fetchData();

    return () => {
      isMounted = false;
    };
  }, [tipo, filtro, refreshTrigger]);

  return { data, loading, refetch };
};
