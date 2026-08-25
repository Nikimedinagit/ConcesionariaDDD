import api from "@/api/axios";

const permisosService = {
  obtenerCatalogo: async () => (await api.get("/Permisos/catalogo")).data,
  actualizarRol: async (roleId, permisos) =>
    (await api.put(`/Permisos/roles/${roleId}`, { permisos })).data,
};

export default permisosService;
