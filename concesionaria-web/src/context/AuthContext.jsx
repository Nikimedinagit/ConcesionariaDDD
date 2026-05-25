// src/context/AuthContext.jsx
import { createContext, useContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const updateUserData = () => {
    const token = localStorage.getItem("token");
    if (token) {
      const decoded = jwtDecode(token);
      setUser({
        name: decoded.nombre || "Usuario",
        email: decoded.email || "",
        avatarURL: decoded.avatarUrl || "",
      });
    }
  };

  useEffect(() => {
    updateUserData();
  }, []);

  return (
    <AuthContext.Provider value={{ user, updateUserData }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);