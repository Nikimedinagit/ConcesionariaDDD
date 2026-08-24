import api from "@/api/axios";

const ClienteService = {
  getActivas: async (filtro = "") => {
    const response = await api.get("/Clientes/activas", { params: { filtro } });
    return response.data;
  },

  getInactivas: async (filtro = "") => {
    const response = await api.get("/Clientes/inactivas", { params: { filtro } });
    return response.data;
  },

  crear: async (payload) => (await api.post("/Clientes", payload)).data,
  actualizar: async (id, payload) => (await api.put(`/Clientes/${id}`, payload)).data,
  desactivar: async (id) => (await api.put(`/Clientes/desactivar/${id}`, { clienteId: id, eliminado: true })).data,
  activar: async (id) =>
    (await api.put(`/Clientes/activar/${id}`, { clienteId: id, eliminado: false })).data,
};

export default ClienteService;
