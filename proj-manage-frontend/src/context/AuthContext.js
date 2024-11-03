import React, { createContext, useState } from "react";
import { jwtDecode } from "jwt-decode";
import { setAuthToken } from "../api";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = (token) => {
    localStorage.setItem("token", token);
    setAuthToken(token);

    const decodedToken = jwtDecode(token);
    const { unique_name, role } = decodedToken;

    // Set the user object with username and role
    setUser({
      username: unique_name,
      role,
    });
  };

  const logout = () => {
    localStorage.removeItem("token");
    setAuthToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
