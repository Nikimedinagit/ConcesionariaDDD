/* eslint-disable react-hooks/set-state-in-effect, react-refresh/only-export-components */
import { createContext, useContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";
import api from "@/api/axios";

const PermissionContext = createContext({
  can: () => false,
  permissions: [],
  isAdministrator: false,
});

function getTokenAccess() {
  const token = localStorage.getItem("token");

  if (!token) {
    return { permissions: [], isAdministrator: false };
  }

  try {
    const decoded = jwtDecode(token);
    const rawRoles = decoded.role ?? [];
    const roles = Array.isArray(rawRoles) ? rawRoles : [rawRoles];
    const rawPermissions = decoded["permiso.accion"] ?? [];

    return {
      permissions: Array.isArray(rawPermissions) ? rawPermissions : [rawPermissions],
      isAdministrator: roles.some(
        (role) => String(role).toUpperCase() === "ADMINISTRADOR",
      ),
    };
  } catch {
    return { permissions: [], isAdministrator: false };
  }
}

export function PermissionProvider({ children }) {
  const initialAccess = getTokenAccess();
  const [permissions, setPermissions] = useState(initialAccess.permissions);
  const [isAdministrator, setIsAdministrator] = useState(
    initialAccess.isAdministrator,
  );

  const refresh = async () => {
    const tokenAccess = getTokenAccess();
    setIsAdministrator(tokenAccess.isAdministrator);

    try {
      const response = await api.get("/Permisos/mis-permisos");
      setPermissions(Array.isArray(response.data) ? response.data : []);
    } catch {
      setPermissions(tokenAccess.permissions);
    }
  };

  useEffect(() => {
    refresh();

    const refreshOnFocus = () => refresh();
    window.addEventListener("focus", refreshOnFocus);

    const interval = window.setInterval(refresh, 15000);

    return () => {
      window.removeEventListener("focus", refreshOnFocus);
      window.clearInterval(interval);
    };
  }, []);

  const can = (permission) => isAdministrator || permissions.includes(permission);
  return (
    <PermissionContext.Provider
      value={{ can, permissions, isAdministrator, refresh }}
    >
      {children}
    </PermissionContext.Provider>
  );
}

export function usePermissions() {
  return useContext(PermissionContext);
}
