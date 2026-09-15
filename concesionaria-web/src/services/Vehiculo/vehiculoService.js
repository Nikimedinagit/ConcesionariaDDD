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

    getImagenes: async (vehiculoId) =>
        (await api.get(`/Vehiculos/${vehiculoId}/imagenes`)).data,

    agregarImagen: async (vehiculoId, file, esPrincipal = false) => {
        const formData = new FormData();
        formData.append("imagen", file);
        formData.append("esPrincipal", String(esPrincipal));

        return (
            await api.post(`/Vehiculos/${vehiculoId}/imagenes`, formData)
        ).data;
    },

    eliminarImagen: async (vehiculoId, imagenId) =>
        (await api.delete(`/Vehiculos/${vehiculoId}/imagenes/${imagenId}`)).data,

    establecerImagenPrincipal: async (vehiculoId, imagenId) =>
        (
            await api.patch(
                `/Vehiculos/${vehiculoId}/imagenes/${imagenId}/principal`,
            )
        ).data,

    reordenarImagenes: async (vehiculoId, imagenes) =>
        (
            await api.put(`/Vehiculos/${vehiculoId}/imagenes/orden`, {
                imagenes,
            })
        ).data,
};

export default VehiculoService;
