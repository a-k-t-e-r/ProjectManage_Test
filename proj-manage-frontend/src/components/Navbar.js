import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "./Styles/Navbar.css";

const Navbar = () => {
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem("jwtToken");
    navigate("/");
  };

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/" className="navbar-item">
          Project Management App
        </Link>
      </div>

      <div className="navbar-menu">
        <Link to="/" className="navbar-item">
          Login
        </Link>

        <Link to="/projects" className="navbar-item">
          Projects
        </Link>

        <Link to="/dashboard" className="navbar-item">
          Dashboard
        </Link>

        <button className="navbar-item" onClick={logout}>
          Logout
        </button>

      </div>
    </nav>
  );
};

export default Navbar;
