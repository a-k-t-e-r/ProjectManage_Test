import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { api } from '../../api';
import Navbar from '../Navbar';
// import '../Styles/TaskList.css';

const TaskList = () => {
    const [tasks, setTasks] = useState([]);
    const { projectId } = useParams();
    const navigate = useNavigate();

    // Function to fetch tasks by projectId
    const fetchTasks = async () => {
        try {
            const response = await api.get(`/Task?projectId=${projectId}`);
            setTasks(response.data);
        } catch (error) {
            console.error('Error fetching tasks:', error);
        }
    };

    useEffect(() => {
        fetchTasks();
    }, [projectId]);

    // Function to handle task editing
    const handleEdit = async (task) => {
        try {
            const updatedTask = { ...task, taskName: 'Updated Task Name' }; // Modify task details as needed
            await api.put(`/Task/${task.taskId}`, updatedTask);
            fetchTasks();
        } catch (error) {
            console.error('Error updating task:', error);
        }
    };

    // Function to handle task deletion
    const handleDelete = async (taskId) => {
        try {
            await api.delete(`/Task/${taskId}`);
            fetchTasks();
        } catch (error) {
            console.error('Error deleting task:', error);
        }
    };

    return (
        <div className="container">
            <Navbar />
            <h2 className="title">Tasks</h2>
            <div className="task-list">
                {tasks.map((task) => (
                    <div key={task.taskId} className="task-card">
                        <strong className="task-name">{task.taskName}</strong>
                        <p className="task-description">{task.description}</p>
                        <div className="task-details">
                            <span>Assigned To: {task.assignedTo}</span> |{' '}
                            <span>Priority: {task.priority}</span> |{' '}
                            <span>Status: {task.status}</span> |{' '}
                            <span>
                                Start: {new Date(task.startDate).toLocaleDateString()}
                            </span> |{' '}
                            <span>End: {new Date(task.endDate).toLocaleDateString()}</span>
                        </div>
                        <div className="button-container">
                            <button
                                className="button edit"
                                onClick={() => handleEdit(task)}
                            >
                                Edit
                            </button>
                            <button
                                className="button delete"
                                onClick={() => handleDelete(task.taskId)}
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default TaskList;
