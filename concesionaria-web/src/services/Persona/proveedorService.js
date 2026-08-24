import api from "@/api/axios";

const ProveedorService = {
    getActivas: async (filtro = "") => {
        const response = await api.get("/Proveedores/activas", { params: { filtro } });
        return response.data;
    },

    getInactivas: async (filtro = "") => {
        const response = await api.get("/Proveedores/inactivas", { params: { filtro } });
        return response.data;
    },

    crear: async (payload) => (await api.post("/Proveedores", payload)).data,

    actualizar: async (id, payload) => (await api.put(`/Proveedores/${id}`, payload)).data,

    desactivar: async (id) => (await api.put(`/Proveedores/desactivar/${id}`, { proveedorId: id, eliminado: true })).data,

    activar: async (id) => (await api.put(`/Proveedores/activar/${id}`, { proveedorId: id, eliminado: false })).data,
};


export default ProveedorService;