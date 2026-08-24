import api from "@/api/axios";

const ModeloVehiculoService = {
  getActivas: async (filtro = "", marcaVehiculoId = "todos", tipoVehiculoId = "todos") => {
    const response = await api.get("/ModelosVehiculos/activas", { params: { filtro, marcaVehiculoId: marcaVehiculoId === "todos" ? undefined : marcaVehiculoId, tipoVehiculoId: tipoVehiculoId === "todos" ? undefined : tipoVehiculoId } });
    return response.data;
  },
  getInactivas: async (filtro = "", marcaVehiculoId = "todos", tipoVehiculoId = "todos") => {
    const response = await api.get("/ModelosVehiculos/inactivas", { params: { filtro, marcaVehiculoId: marcaVehiculoId === "todos" ? undefined : marcaVehiculoId, tipoVehiculoId: tipoVehiculoId === "todos" ? undefined : tipoVehiculoId } });
    return response.data;
  },
  crear: async (payload) => (await api.post("/ModelosVehiculos", payload)).data,
  actualizar: async (id, payload) => (await api.put(`/ModelosVehiculos/${id}`, payload)).data,
  desactivar: async (id) =>
    (await api.put(`/ModelosVehiculos/desactivar/${id}`, { modeloVehiculoId: id })).data,
  activar: async (id) =>
    (await api.put(`/ModelosVehiculos/activar/${id}`, { modeloVehiculoId: id })).data,
};

export default ModeloVehiculoService;
