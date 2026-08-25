import { createContext, useContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
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
        setUser({
          name: decoded.nombre || "Usuario",
          email: decoded.email || "",
          avatarURL: decoded.avatarUrl || "",
          role: decoded.role || "",
        });
      } catch {
        setUser(null);
      }
    } else {
      setUser(null);
    }
    setLoading(false);
  };

  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
    updateUserData();
  }, []);

  return (
    <AuthContext.Provider value={{ user, setUser, updateUserData, loading, theme, changeTheme }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
