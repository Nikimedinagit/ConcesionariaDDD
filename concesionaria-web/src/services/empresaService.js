import api from "@/api/axios";

export const empresaService = {

  async getPerfil() {

    const response = await api.get(
      "/empresas/perfil"
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

};