import api from "@/api/axios";

const VehiculoService = {
    getActivas: async (filtro = "") => {
        const response = await api.get("/Vehiculos/activas", { params: { filtro } });
        return response.data;
    },

    getInactivas: async (filtro = "") => {
        const response = await api.get("/Vehiculos/inactivas", { params: { filtro } });
        return response.data;
    },

    crear: async (payload) => (await api.post("/Vehiculos", payload)).data,
    actualizar: async (id, payload) => (await api.put(`/Vehiculos/${id}`, payload)).data,
}

export default VehiculoService;