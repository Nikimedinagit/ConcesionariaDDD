import api from "@/api/axios";

const VehiculoService = {
    getDisponibles: async (filtro = "") => {
        const response = await api.get("/Vehiculos/disponibles", { params: { filtro } });
        return response.data;
    },

    getReservados: async (filtro = "") => {
        const response = await api.get("/Vehiculos/reservados", { params: { filtro } });
        return response.data;
    },

    getVendidos: async (filtro = "") => {
        const response = await api.get("/Vehiculos/vendidos", { params: { filtro } });
        return response.data;
    },

    getEnServicio: async (filtro = "") => {
        const response = await api.get("/Vehiculos/servicio", { params: { filtro } });
        return response.data;
    },

    crear: async (payload) => (await api.post("/Vehiculos", payload)).data,
    actualizar: async (id, payload) => (await api.put(`/Vehiculos/${id}`, payload)).data,
};

export default VehiculoService;
