import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? "https://localhost:7027/api",
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response?.status;
    const responseCode = error.response?.data?.code;
    const requestUrl = error.config?.url || "";
    const isLoginRequest = requestUrl.includes("/auth/login");
    const hasToken = Boolean(localStorage.getItem("token"));

    const sessionIsInvalid =
      responseCode === "SESSION_INVALIDATED" ||
      (status === 401 && hasToken && !isLoginRequest);

    if (sessionIsInvalid) {
      localStorage.removeItem("token");

      if (window.location.pathname !== "/") {
        window.location.replace("/?reason=session-invalidated");
      }
    }

    return Promise.reject(error);
  },
);

export default api;
