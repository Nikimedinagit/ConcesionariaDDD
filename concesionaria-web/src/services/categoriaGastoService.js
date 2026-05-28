import api from "@/api/axios";

const CategoriaGastoService = {
  getActivas: async (filtro = "") => {
    const response = await api.get('/CategoriasGastos/activas', {
      params: { filtro } 
    });
    return response.data;
  },

  getInactivas: async (filtro = "") => {
    const response = await api.get('/CategoriasGastos/inactivas', {
      params: { filtro } 
    });
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