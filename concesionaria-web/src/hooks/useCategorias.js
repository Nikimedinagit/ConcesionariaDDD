import { useState, useEffect, useCallback } from 'react';
import CategoriaGastoService from "../services/categoriaGastoService"; 

export const useCategorias = (tipo = 'activas', filtro = '') => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const refetch = useCallback(() => setRefreshTrigger(prev => prev + 1), []);

  useEffect(() => {
    let isMounted = true;
    setLoading(true);

    const fetchData = async () => {
      try {
        const result = tipo === 'activas' 
          ? await CategoriaGastoService.getActivas(filtro) 
          : await CategoriaGastoService.getInactivas(filtro); 
        
        if (isMounted) setData(result);
      } catch (error) {
        console.error("Error en hook:", error);
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    fetchData();
    return () => { isMounted = false; };
  }, [tipo, filtro, refreshTrigger]); 

  return { data, loading, refetch };
};