import React, { useEffect, useState, useContext } from "react";
import { api } from "../api";
import Navbar from "./Navbar";
import { AuthContext } from "../context/AuthContext";
import "./Styles/Dashboard.css";

const Dashboard = () => {
  const [projectCount, setProjectCount] = useState(0);
  const [taskCount, setTaskCount] = useState(0);
  const [overdueCount, setOverdueCount] = useState(0);
  // const { token } = useContext(AuthContext);

  // Fetch the token from localStorage
  const token = localStorage.getItem("token");

  const fetchDashboardData = async () => {
    try {
      api.defaults.headers.common["Authorization"] = `Bearer ${token}`;

      const projectsResponse = await api.get("/Projects");
      setProjectCount(projectsResponse.data.length);

      const tasksResponse = await api.get("/Task");
      setTaskCount(tasksResponse.data.length);

      const overdueTasks = await api.get("/Projects/overdue");
      setOverdueCount(overdueTasks.data.length);

    } catch (error) {
      console.error("Error fetching dashboard data", error);
      if (error.response && error.response.status === 401) {
        console.info("Session expired. Please log in again.");
        // Optionally, redirect to login or refresh token
      }
    }
  };

  useEffect(() => {
    fetchDashboardData();
  }, []);

  return (
    <div className="dashboard-container">
      <Navbar />
      <div className="dashboard-content">
        <h2>Dashboard</h2>
        <div className="dashboard-cards">
          <div className="dashboard-card">
            <h3>Projects</h3>
            <p>{projectCount}</p>
          </div>
          <div className="dashboard-card">
            <h3>Tasks</h3>
            <p>{taskCount}</p>
          </div>
          <div className="dashboard-card overdue">
            <h3>Overdue Tasks</h3>
            <p>{overdueCount}</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
