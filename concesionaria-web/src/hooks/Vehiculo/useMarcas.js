/* eslint-disable react-hooks/set-state-in-effect */
import { useCallback, useEffect, useState } from "react";
import MarcaService from "@/services/Vehiculo/marcaService";

export const useMarcas = (tipo = "activas", filtro = "") => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const refetch = useCallback(() => {
    setRefreshTrigger((current) => current + 1);
  }, []);

  useEffect(() => {
    let isMounted = true;
    setLoading(true);

    const fetchData = async () => {
      try {
        const result =
          tipo === "activas"
            ? await MarcaService.getActivas(filtro)
            : await MarcaService.getInactivas(filtro);

        if (isMounted) {
          setData(result);
        }
      } catch (error) {
        console.error("Error al obtener las marcas:", error);
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    fetchData();

    return () => {
      isMounted = false;
    };
  }, [tipo, filtro, refreshTrigger]);

  return { data, loading, refetch };
};
