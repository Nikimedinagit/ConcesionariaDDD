import api from "@/api/axios";

const CategoriaGastoService = {
  getActivas: async () => {
    const response = await api.get('/CategoriasGastos/activas');
    return response.data;
  },

  getInactivas: async () => {
    const response = await api.get('/CategoriasGastos/inactivas');
    return response.data;
  },

  crear: async (payload) => {
    const response = await api.post('/CategoriasGastos', payload);
    return response.data;
  },

  actualizar: async (id, payload) => {
    const response = await api.put(`/CategoriasGastos/${id}`, payload);
    return response.data;
  }
};

export default CategoriaGastoService;