# 🚀 ColonyOS

ColonyOS is a full-stack, microservice-based simulation platform that models a remote off-world colony command system.

The application behaves like a mission control dashboard where a user manages critical systems such as oxygen, water, power, food, and crew operations in real time.

This project is intentionally designed to exercise real-world engineering patterns rather than serve as a traditional CRUD application.

---

## 🧠 Core Concept

The colony runs continuously in the background.

- Resources degrade over time  
- Crew members accumulate fatigue  
- Tasks take time to complete  
- System failures and incidents occur dynamically  

The user acts as a colony administrator, making decisions to keep the colony operational.

---

## 🏗️ Architecture Overview

ColonyOS follows a microservice architecture with clear domain boundaries.

### Services

#### 1. Colony State Service
Responsible for the simulation engine and system health.

- Resource tracking (oxygen, water, power, food, morale)  
- Background simulation loop  
- System degradation and recovery  
- Emits events when thresholds are breached  

---

#### 2. Crew Service
Manages all crew-related state.

- Crew members  
- Skills and roles  
- Fatigue and health  
- Task assignments  

---

#### 3. Task Scheduler Service
Handles all work execution.

- Task creation and lifecycle  
- Priority queues  
- Estimated completion tracking  
- Emits task completion events  

---

#### 4. Alert Service
Centralized system for warnings and incidents.

- Alert generation  
- Severity classification  
- Acknowledgement tracking  
- Event history  

---

#### 5. Gateway (BFF)
Single entry point for the frontend.

- Aggregates data across services  
- Exposes REST endpoints  
- Hosts SignalR for real-time updates  
- Simplifies frontend integration  

---

## ⚙️ Tech Stack

### Backend
- .NET 8  
- ASP.NET Core Web API  
- BackgroundService (simulation loops)  
- SignalR (real-time updates)  
- MassTransit + RabbitMQ (event-driven communication)  
- Entity Framework Core  
- SQL Server / PostgreSQL  

### Frontend
- Angular  
- Reactive Forms  
- RxJS  
- Angular Signals (optional)  
- Bootstrap / Angular Material (UI)  

---

## 📡 Key Features

### Real-Time Dashboard
- Live system metrics (oxygen %, power load, etc.)  
- Active alerts and incidents  
- Task activity overview  

### Task Management
- Create and manage tasks  
- Assign crew members  
- Track progress over time  

### Crew Management
- View crew status (health, fatigue)  
- Assign work based on availability and skills  

### Alert System
- Automatic alert generation  
- Severity levels (Warning, Critical)  
- Acknowledge and track resolution  

### Simulation Engine
Runs continuously via background services and models:

- Resource consumption  
- System degradation  
- Random incident generation  

---

## 🔄 Event-Driven Flow

ColonyOS uses asynchronous messaging between services.

**Example flow:**

1. Colony State detects low oxygen  
2. Emits `ResourceThresholdBreached` event  
3. Alert Service creates a critical alert  
4. Gateway pushes update via SignalR  
5. Task Scheduler creates a repair task  
6. Crew Service updates availability  

---

## 🧪 Example Feature Slice

**“Repair Failing Oxygen Generator”**

This single flow touches multiple services:

- Alert triggered from simulation  
- Task created in Task Scheduler  
- Crew assigned via frontend form  
- Background processing updates task status  
- Colony State updated on completion  
- Alert resolved automatically  

---
## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK  
- Node.js (v18+)  
- Angular CLI  
- Docker (optional, recommended)  

---

### Run Backend Services

```bash
dotnet build
dotnet run --project ColonyOS.Gateway
```

### Run Frontend
```bash
cd src/ColonyOS.Client
npm install
ng serve
```

### Access App
```
http://localhost:4200
```

---
## 🎯 Purpose of This Project
 - This project is designed to strengthen:
 - Microservice architecture design
 - Event-driven systems
 - Real-time UI updates
 - Angular reactive forms at scale
 - Background processing patterns in .NET
 - End-to-end feature ownership

---
## ⚠️ Notes
 - This is a simulation system, not a production-ready application
 - Architecture decisions prioritize learning and experimentation
 - Services are intentionally scoped to balance realism and complexity
