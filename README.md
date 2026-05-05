# AIDOGOVOR

Notification microservice built with DDD and event-driven architecture using RabbitMQ

# Overview

AIDogovor is a notification microservice for a contract-generation platform. The main application publishes domain events to RabbitMQ whenever something noteworthy happens — a contract is created, a share link is opened, a password is changed — and this C# service consumes those events, builds in-app notifications, and serves them to the frontend over a REST API.

Splitting notifications into a separate service keeps the core contract flow fast and resilient: the main backend never waits on notification logic, and if this service goes down, events safely queue up in RabbitMQ until it's back.

## Technical stack

Backend: C#, .NET 8, ASP.NET Core

Db: PostgreSQL, EF Core

Messaging: RabbitMQ

Auth: JWT

Logging: Serilog

Architecture: Domain-Driven Design (DDD)

Testing: xUnit, Moq

## Features

- Async notification processing via RabbitMQ consumer
- Cron task: checking expiring messages
- Structured logging with Serilog

## Project Structure

```
AIDogovor/
├── Domain/              # Entities, Value Objects, Domain Events
├── Application/         # Use cases, DTOs, Interfaces
├── Infrastructure/      # EF Core, RabbitMQ consumer, External services
├── Presentation/        # Controllers
└── AIDogovor.Tests/     # Unit tests (xUnit + Moq)
```

## Getting started

Prerequisites
.NET 8 SDK
Docker & Docker Compose

Setting up:
1) Clone the repo
   ```
   git clone https://github.com/ssanmaxi/AIDogovor.git
   
   cd AIDogovor
   ```
3) Configure environment variables:
   ```
   cp .env.example .env
   ```
   
5) Start dependencies (PostgreSQL + RabbitMQ)
   ```
   docker-compose up -d
   
7) Run the service:
   ```
   dotnet run --project AIDogovor


