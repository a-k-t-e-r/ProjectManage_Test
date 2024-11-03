import React, { useEffect, useState } from "react";
import { api } from "../../api";
import "../Styles/ProjectForm.css";

const ProjectForm = ({ editingProject, onFetch }) => {
  const [projectName, setProjectName] = useState("");
  const [description, setDescription] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [budget, setBudget] = useState(0);
  const [owner, setOwner] = useState("");

  useEffect(() => {
    if (editingProject) {
      setProjectName(editingProject.projectName);
      setDescription(editingProject.description);
      setStartDate(
        new Date(editingProject.startDate).toISOString().split("T")[0]
      );
      setEndDate(new Date(editingProject.endDate).toISOString().split("T")[0]);
      setBudget(editingProject.budget);
      setOwner(editingProject.owner);
    } else {
      setProjectName("");
      setDescription("");
      setStartDate("");
      setEndDate("");
      setBudget(0);
      setOwner("");
    }
  }, [editingProject]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const projectData = {
      projectName,
      description,
      startDate,
      endDate,
      budget: parseFloat(budget),
      owner,
    };

    if (editingProject) {
      projectData.projectId = editingProject.projectId;
      await api.put(`/Projects/${editingProject.projectId}`, projectData);
    } else {
      await api.post("/Projects", projectData);
    }
    onFetch();
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Project Name"
        value={projectName}
        onChange={(e) => setProjectName(e.target.value)}
        required
      />
      <textarea
        placeholder="Description"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        required
      />
      <input
        type="date"
        value={startDate}
        onChange={(e) => setStartDate(e.target.value)}
        required
      />
      <input
        type="date"
        value={endDate}
        onChange={(e) => setEndDate(e.target.value)}
        required
      />
      <input
        type="number"
        placeholder="Budget"
        value={budget}
        onChange={(e) => setBudget(e.target.value)}
        required
      />
      <input
        type="text"
        placeholder="Owner"
        value={owner}
        onChange={(e) => setOwner(e.target.value)}
        required
      />
      <button type="submit">
        {editingProject ? "Update" : "Create"} Project
      </button>
    </form>
  );
};

export default ProjectForm;
