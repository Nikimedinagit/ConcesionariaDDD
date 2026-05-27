import { useState, useEffect } from 'react';
// Cambia el @/ por la ruta relativa desde la carpeta hooks hacia services
import CategoriaGastoService from "../services/categoriaGastoService"; 

export const useCategorias = (tipo = 'activas') => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let isMounted = true;
    setLoading(true);

    const fetchData = async () => {
      try {
        // Asegúrate de que los métodos existan en el servicio
        const result = tipo === 'activas' 
          ? await CategoriaGastoService.getActivas() 
          : await CategoriaGastoService.getInactivas();
        
        if (isMounted) setData(result);
      } catch (error) {
        console.error("Error en hook:", error);
      } finally {
        if (isMounted) setLoading(false);
      }
    };

    fetchData();
    return () => { isMounted = false; };
  }, [tipo]);

  return { data, loading };
};