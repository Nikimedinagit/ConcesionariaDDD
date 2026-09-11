import { useCallback, useEffect, useState } from "react";
import VehiculoService from "@/services/Vehiculo/vehiculoService";

const consultasPorTipo = {
    disponibles: VehiculoService.getDisponibles,
    reservados: VehiculoService.getReservados,
    vendidos: VehiculoService.getVendidos,
    servicio: VehiculoService.getEnServicio,
};

export const useVehiculos = (tipo = "disponibles", filtro = "") => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [refreshTrigger, setRefreshTrigger] = useState(0);

    const refetch = useCallback(() => {
        setRefreshTrigger((current) => current + 1);
    }, []);

    useEffect(() => {
        let isMounted = true;

        const fetchData = async () => {
            setLoading(true);

            try {
                const consulta =
                    consultasPorTipo[tipo] ??
                    consultasPorTipo.disponibles;

                const result = await consulta(filtro);

                if (isMounted) {
                    setData(result);
                }
            } catch (error) {
                console.error("Error al obtener los vehículos:", error);

                if (isMounted) {
                    setData([]);
                }
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

    return {
        data,
        loading,
        refetch,
    };
};
