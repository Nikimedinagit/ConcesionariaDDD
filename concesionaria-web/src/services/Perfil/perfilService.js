import api from "@/api/axios";

export const usuarioService = {

  async getPerfil() {

    const response = await api.get(
      "/perfil"
    );

    return response.data;

  },

  async updateUsuario(payload) {

     const response = await api.put(
    "/perfil/usuario",
    payload
  );

    return response.data;

  },

    async updateEmpresa(data) {

    const response = await api.put(
      "/perfil/empresa",
      data
    );

    return response.data;

  },

  async updateAvatar(data) {

    const response = await api.put(
      "/perfil/usuario",
      data
    );

    return response.data;

  },


  async updateRecuperacion(data) {

    const response = await api.put(
      "/perfil/usuario",
      data
    );

    return response.data;

  },


  async updateContraseña(data) {

    const response = await api.post(
      "/perfil/seguridad",
      data
    );
    return response.data;

  }

};