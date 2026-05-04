# AIDOGOVOR

Notification microservice built with DDD and event-driven architecture using RabbitMQ

# Overview

A notification microservice built to async processes events from the main service via RabbitMQ

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

AIDogovor/
├── Domain/              # Entities, Value Objects, Domain Events
├── Application/         # Use cases, DTOs, Interfaces
├── Infrastructure/      # EF Core, RabbitMQ consumer, External services
├── Presentation/        # Controllers
└── AIDogovor.Tests/     # Unit tests (xUnit + Moq)

## Getting started

Prerequisites
.NET 8 SDK
Docker & Docker Compose

Setting up:
1) Clone the repo
   
   git clone https://github.com/ssanmaxi/AIDogovor.git
   
   cd AIDogovor
3) Configure environment variables:
   
   cp .env.example .env
   
5) Start dependencies (PostgreSQL + RabbitMQ)
   
   docker-compose up -d
   
7) Run the service:
   
   dotnet run --project AIDogovor


