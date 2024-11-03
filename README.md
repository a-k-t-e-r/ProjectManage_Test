
# Project Management Application Setup Guide

This guide provides detailed instructions on setting up and running the Project Management App, including the backend and frontend environments. Follow the steps below to get started.

---

## Project URLs

### Backend
- **.NET CLI URL:** `http://localhost:5048/api`
- **IIS URL:** `http://localhost:39160/api`

> **Note:** Modify the API URL in `api.js` based on your chosen running environment.

### Frontend
- **URL:** `http://localhost:3000/`

---

## Prerequisites

- Ensure you have the .NET SDK installed for backend operations.
- Make sure Node.js and npm are installed for frontend setup.
- Database migration tools must be available for successful backend configuration.
- Docker support is currently not implemented. If you encounter exceptions, ensure all required package versions are installed correctly.

---

## Backend Setup

1. **Run Database Migration:**
   ```bash
   dotnet ef database update

2. **Build and Run the Project:**
   ```bash
   dotnet build
   dotnet run

---
## Frontend Setup

1. **Install and start Project Dependencies:**
   ```bash
   npm i
   npm start

> **Note:** Verify package versions in your project to match your local environment, as this can prevent potential exceptions.

---
## Role-Based Access Setup

1. Once both backend and frontend services are running, create a new user in the application.
2. Assign the user role as **Manager** to test role-based access functionality.

> **Note:** For any issues, refer to the project documentation or check package compatibility..
