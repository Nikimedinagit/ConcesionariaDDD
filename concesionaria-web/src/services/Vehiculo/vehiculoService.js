import api from "@/api/axios";

const getParams = (filtro, sucursal) => ({
    filtro,
    ...(sucursal === "todas"
        ? { todasSucursales: true }
        : sucursal !== "actual"
          ? { sucursalId: sucursal }
          : {}),
});

const VehiculoService = {
    getDisponibles: async (filtro = "", sucursal = "actual") => {
        const response = await api.get("/Vehiculos/disponibles", { params: getParams(filtro, sucursal) });
        return response.data;
    },

    getReservados: async (filtro = "", sucursal = "actual") => {
        const response = await api.get("/Vehiculos/reservados", { params: getParams(filtro, sucursal) });
        return response.data;
    },

    getVendidos: async (filtro = "", sucursal = "actual") => {
        const response = await api.get("/Vehiculos/vendidos", { params: getParams(filtro, sucursal) });
        return response.data;
    },

    getEnServicio: async (filtro = "", sucursal = "actual") => {
        const response = await api.get("/Vehiculos/servicio", { params: getParams(filtro, sucursal) });
        return response.data;
    },

    crear: async (payload) => (await api.post("/Vehiculos", payload)).data,
    actualizar: async (id, payload) => (await api.put(`/Vehiculos/${id}`, payload)).data,
    eliminar: async (id) => (await api.delete(`/Vehiculos/${id}`)).data,
};

export default VehiculoService;
