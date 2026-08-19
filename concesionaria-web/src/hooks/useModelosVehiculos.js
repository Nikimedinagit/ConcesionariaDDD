/* eslint-disable react-hooks/set-state-in-effect */
import { useCallback, useEffect, useState } from "react";
import ModeloVehiculoService from "@/services/Vehiculos/modeloVehiculoService";

export const useModelosVehiculos = (tipo = "activas", filtro = "", marcaVehiculoId = "todos", tipoVehiculoId = "todos") => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshTrigger, setRefreshTrigger] = useState(0);
  const refetch = useCallback(() => setRefreshTrigger((value) => value + 1), []);

  useEffect(() => {
    let isMounted = true;
    setLoading(true);

    const fetchData = async () => {
      try {
        const result = tipo === "activas"
          ? await ModeloVehiculoService.getActivas(filtro, marcaVehiculoId, tipoVehiculoId)
          : await ModeloVehiculoService.getInactivas(filtro, marcaVehiculoId, tipoVehiculoId);
        if (isMounted) setData(result);
      } catch (error) {
        console.error("Error al obtener los modelos de vehículos:", error);
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    fetchData();
    return () => { isMounted = false; };
  }, [tipo, filtro, marcaVehiculoId, tipoVehiculoId, refreshTrigger]);

  return { data, loading, refetch };
};
