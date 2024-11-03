import React, { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { api } from '../../api';
import { AuthContext } from '../../context/AuthContext';
import ProjectForm from './ProjectForm';
import Navbar from '../Navbar';
import '../Styles/ProjectList.css';

const ProjectList = () => {
    const [projects, setProjects] = useState([]);
    const [editingProject, setEditingProject] = useState(null);
    const { user } = useContext(AuthContext);
    const navigate = useNavigate();

    const fetchProjects = async () => {
        try {
            const response = await api.get('/Projects');
            setProjects(response.data);
        } catch (error) {
            console.error('Error fetching projects:', error);
        }
    };

    useEffect(() => {
        fetchProjects();
    }, []);

    const handleEdit = (project) => {
        setEditingProject(project);
    };

    const handleDelete = async (projectId) => {
        try {
            await api.delete(`/Projects/${projectId}`);
            fetchProjects();
        } catch (error) {
            console.error('Error deleting project:', error);
        }
    };

    const handleViewTasks = (projectId) => {
        navigate(`/tasks/${projectId}`);
    };

    return (
        <div className="container">
            <Navbar />
    
            <h2 className="title">Projects</h2>
    
            <div className="main-content">
                <div className="project-list">
                    {projects.map((project) => (
                        <div key={project.projectId} className="project-card">
                            <strong className="project-name">{project.projectName}</strong>
                            <p className="project-description">{project.description}</p>
                            <div className="project-details">
                                <span>Start: {new Date(project.startDate).toLocaleDateString()}</span> |{' '}
                                <span>End: {new Date(project.endDate).toLocaleDateString()}</span> |{' '}
                                <span>Budget: ${project.budget.toFixed(2)}</span> |{' '}
                                <span>Owner: {project.owner}</span>
                            </div>
                            <div className="button-container">
                                <button
                                    className="button view-tasks"
                                    onClick={() => handleViewTasks(project.projectId)}
                                >
                                    View Tasks
                                </button>
                                {user && user.role === 'Manager' && (
                                    <>
                                        <button
                                            className="button edit"
                                            onClick={() => handleEdit(project)}
                                        >
                                            Edit
                                        </button>
                                        <button
                                            className="button delete"
                                            onClick={() => handleDelete(project.projectId)}
                                        >
                                            Delete
                                        </button>
                                    </>
                                )}
                            </div>
                        </div>
                    ))}
                </div>
    
                {user && user.role === 'Manager' && (
                    <div className="project-form-container">
                        <ProjectForm editingProject={editingProject} onFetch={fetchProjects} />
                    </div>
                )}
            </div>
        </div>
    );
};

export default ProjectList;
