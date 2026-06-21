import api from "@/api/axios";

const UsuarioService = {
  getActivos: async (filtro = "") => {
    const response = await api.get("/Usuarios/activos", {
      params: { filtro },
    });
    return response.data;
  },

  getInactivos: async (filtro = "") => {
    const response = await api.get("/Usuarios/inactivos", {
      params: { filtro },
    });
    return response.data;
  },

  getRoles: async () => {
    const response = await api.get("/Usuarios/roles");
    return response.data;
  },

  crear: async (payload) => {
    const response = await api.post("/Usuarios", payload);
    return response.data;
  },

  actualizar: async (id, payload) => {
    const response = await api.put(`/Usuarios/${id}`, payload);
    return response.data;
  },

  cambiarPassword: async (id, payload) => {
    const response = await api.put(`/Usuarios/password/${id}`, payload);
    return response.data;
  },

  desactivar: async (id) => {
    const response = await api.put(`/Usuarios/desactivar/${id}`, {
      usuarioId: id,
    });
    return response.data;
  },

  activar: async (id) => {
    const response = await api.put(`/Usuarios/activar/${id}`, {
      usuarioId: id,
    });
    return response.data;
  },
};

export default UsuarioService;
