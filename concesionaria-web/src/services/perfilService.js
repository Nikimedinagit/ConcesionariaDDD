import api from "@/api/axios";

export const usuarioService = {

  async updateUsuario(data) {

    const response = await api.put(
      "/perfil/usuario",
      data
    );

    return response.data;

  },

  async cambiarPassword(data) {

    const response = await api.put(
      "/perfil/password",
      data
    );

    return response.data;

  },

};