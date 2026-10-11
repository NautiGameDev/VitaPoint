# VitaPoint
## Overview
VitaPoint is a full stack, client-based patient portal created for the Computer Science Capstone course at CSU Global. It helps patients and medical offices streamline patient management tasks, and allows patients to manage their patient profiles, view lab results, request refills on their prescriptions, schedule and manage appointments, and message medical staff. It prioritizes several security features such as validation of the patient prior to account creation and data integrity to mitigate BOLA and HIPAA law violations. Being a healthcare app, protecting sensitive user data was a top priority while building this project.

## Tech Stack
* **Backend:** ASP.NET Core
* **Frontend:** React
* **Database:** Microsoft SQL Server
* **Unit Tests:** xUnit

## Languages
* C# for backend API development
* HTML,CSS, and Javascript for frontend React SPA

## Directory
* VitaPoint.Server - Backend API
* VitaPoint.client - React frontend single-page application
* VitaPoint.Server.Tests - Unit tests with xUnit

## Architecture
VitaPoint uses a three-tiered full-stack architecture which includes a web application for frontend, client-side functions, API middleware that handles system business logic, data processing, and system security features, and database for data storage. Additionally, the front and backend applications follow a layered architecture pattern to cleanly organize logic and functionality of objects in the system.

### Backend Pattern
* **Controller:** Handles HTTP requests, routing, and authorization, and returns HTTP status codes with system messages for clear communication.
* **Service:** Business logic layer that turns raw incoming data into models for data storage, and validates data to prevent system errors
* **Repository:** Data access layer that handles Database queries via LINQ and EF Core.

### Frontend Pattern
* **Pages/Components:** View layer that handles UI rendering, user input, and client state management.
* **Service Layer:** Manages domain-specific client actions, connects view layer to the API client layer, and directs the API client layer to API endpoints.
* **API Client:** Handles HTTP requests to the API, validates responses, and handles refresh tokens on the client side.
