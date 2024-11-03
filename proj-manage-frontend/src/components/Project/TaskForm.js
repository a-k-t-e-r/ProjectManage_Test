import React, { useEffect, useState } from "react";
import { api } from "../../api";
import { useParams } from "react-router-dom";
import TaskForm from "./TaskForm";

const TaskList = () => {
  const { projectId } = useParams(); // Get projectId from the route parameters
  const [tasks, setTasks] = useState([]);
  const [editingTask, setEditingTask] = useState(null);

  // Fetch tasks on component mount
  useEffect(() => {
    const fetchTasks = async () => {
      try {
        const response = await api.get(`/Task/${projectId}/Tasks`);
        setTasks(response.data);
      } catch (error) {
        console.error("Error fetching tasks:", error);
      }
    };
    fetchTasks();
  }, [projectId]);

  // Function to handle task deletion
  const handleDeleteTask = async (taskId) => {
    try {
      await api.delete(`/Task/${taskId}`);
      setTasks(tasks.filter((task) => task.taskId !== taskId)); // Remove the deleted task from the list
    } catch (error) {
      console.error("Error deleting task:", error);
    }
  };

  // Function to fetch tasks again after updating or creating
  const refreshTasks = async () => {
    try {
      const response = await api.get(`/Task/${projectId}/Tasks`);
      setTasks(response.data);
    } catch (error) {
      console.error("Error refreshing tasks:", error);
    }
  };

  // Function to handle editing a task
  const handleEditTask = (task) => {
    setEditingTask(task);
  };

  return (
    <div>
      <h2>Task List for Project {projectId}</h2>
      <TaskForm editingTask={editingTask} onFetch={refreshTasks} />
      <ul>
        {tasks.map((task) => (
          <li key={task.taskId}>
            <h3>{task.taskName}</h3>
            <p>Description: {task.description}</p>
            <p>Assigned To: {task.assignedTo}</p>
            <p>Start Date: {new Date(task.startDate).toLocaleDateString()}</p>
            <p>End Date: {new Date(task.endDate).toLocaleDateString()}</p>
            <p>Priority: {task.priority}</p>
            <p>Status: {task.status}</p>
            <button onClick={() => handleEditTask(task)}>Edit</button>
            <button onClick={() => handleDeleteTask(task.taskId)}>
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TaskList;
