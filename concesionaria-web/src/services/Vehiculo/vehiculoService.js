import api from "@/api/axios";

const VehiculoService = {
    getActivasDisponibles: async (filtro = "") => {
        const response = await api.get("/Vehiculos/activas-disponibles", { params: { filtro } });
        return response.data;
    },

    getActivasReservados: async (filtro = "") => {
        const response = await api.get("/Vehiculos/activas-reservados", { params: { filtro } });
        return response.data;
    },

    getActivasVendidos: async (filtro = "") => {
        const response = await api.get("/Vehiculos/activas-vendidos", { params: { filtro } });
        return response.data;
    },

    getActivasEnServicio: async (filtro = "") => {
        const response = await api.get("/Vehiculos/activas-en-servicio", { params: { filtro } });
        return response.data;
    },

    crear: async (payload) => (await api.post("/Vehiculos", payload)).data,
    actualizar: async (id, payload) => (await api.put(`/Vehiculos/${id}`, payload)).data,
}

export default VehiculoService;