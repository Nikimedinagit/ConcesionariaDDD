import { createContext, useContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [activeSucursal, setActiveSucursal] = useState(null);
  const [loading, setLoading] = useState(true);
  const [theme, setTheme] = useState(() => localStorage.getItem("app-theme") || "default");

  const changeTheme = (themeId) => {
    document.documentElement.setAttribute("data-theme", themeId);
    localStorage.setItem("app-theme", themeId);
    setTheme(themeId);
  };

  const updateUserData = () => {
    const token = localStorage.getItem("token");
    if (token) {
      try {
        const decoded = jwtDecode(token);
        const rawRoles = decoded.role || [];
        const roles = Array.isArray(rawRoles) ? rawRoles : [rawRoles];
        const avatarURL = (decoded.avatarUrl || "").replace(
          /\/avatars\/av-(\d+)\.(png|jpg|jpeg)$/i,
          "/avatars/av-$1-optimized.webp",
        );

        const nextUser = {
          name: decoded.nombre || "Usuario",
          email: decoded.email || "",
          avatarURL,
          roles,
          role: roles[0] || "",
          empresaId: decoded.empresaId || "",
          sucursalId: decoded.sucursalId || "",
          sucursalNombre: decoded.sucursalNombre || "Sin sucursal asignada",
          sessionVersion: decoded.sessionVersion || "",
        };

        setUser(nextUser);

        const isAdministrator = roles.some(
          (role) => String(role).toUpperCase() === "ADMINISTRADOR",
        );
        const storageKey = `active-sucursal-${nextUser.empresaId}`;
        const savedSucursal = localStorage.getItem(storageKey);

        if (isAdministrator && savedSucursal) {
          try {
            setActiveSucursal(JSON.parse(savedSucursal));
          } catch {
            localStorage.removeItem(storageKey);
            setActiveSucursal({
              id: nextUser.sucursalId,
              nombre: nextUser.sucursalNombre,
            });
          }
        } else {
          setActiveSucursal({
            id: nextUser.sucursalId,
            nombre: nextUser.sucursalNombre,
          });
        }
      } catch {
        setUser(null);
        setActiveSucursal(null);
      }
    } else {
      setUser(null);
      setActiveSucursal(null);
    }
    setLoading(false);
  };

  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
    updateUserData();
  }, []);

  const changeActiveSucursal = (sucursal) => {
    if (!user?.roles?.some(
      (role) => String(role).toUpperCase() === "ADMINISTRADOR",
    )) return;

    const nextSucursal = {
      id: sucursal.sucursalId,
      nombre: sucursal.nombre,
    };

    localStorage.setItem(
      `active-sucursal-${user.empresaId}`,
      JSON.stringify(nextSucursal),
    );
    setActiveSucursal(nextSucursal);
  };

  return (
    <AuthContext.Provider value={{
      user,
      setUser,
      updateUserData,
      loading,
      theme,
      changeTheme,
      activeSucursal,
      changeActiveSucursal,
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
