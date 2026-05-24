import { useState, useEffect } from "react";
import { empresaService } from "../services/empresaService"; // El servicio que creamos antes
    
export const usePerfil = () => {
  const [perfil, setPerfil] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchPerfil = async () => {
      try {
        setLoading(true);
        const data = await empresaService.getPerfil();
        setPerfil(data);
      } catch (err) {
        console.error("Error al obtener el perfil:", err);
        setError(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPerfil();
  }, []);

  return { perfil, loading, error };
};