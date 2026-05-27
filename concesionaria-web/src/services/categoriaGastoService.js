import api from "@/api/axios";

const CategoriaGastoService = {
  getActivas: async () => {
    const response = await api.get('/CategoriasGastos/activas');
    return response.data;
  },
  getInactivas: async () => {
    const response = await api.get('/CategoriasGastos/inactivas');
    return response.data;
  }
};

export default CategoriaGastoService;