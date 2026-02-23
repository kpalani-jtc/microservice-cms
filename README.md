# Microservice CMS (ASP.NET Web API + Entra ID + Gateway)

ASP.NET microservices solution implementing:
- Lead Service (CRUD)
- Complaints Service (CRUD)
- Master Service (CRUD)
- Service Appointments Service (CRUD)
- API Gateway with YARP

## Hexagonal Architecture (Ports and Adapters)

Each service follows the same layered structure:

- `Domain` → core entities and business data.
- `Application/Ports` → inbound and outbound interfaces (use cases + repository/integration ports).
- `Application/UseCases` → business workflows and orchestration.
- `Infrastructure` → adapters for Postgres repositories and RabbitMQ integration.
- `Api` → HTTP adapters/controllers, auth middleware, DI composition.

## Security

- **Authentication**: Microsoft Entra ID using `Microsoft.Identity.Web` JWT validation.
- **Authorization**: local Postgres RBAC (`auth` schema) with custom permission policy handler.

## Inter-service Communication

- Gateway (YARP) for synchronous HTTP routing.
- RabbitMQ topic exchange (`cms.events`) for asynchronous events (notifications/email/sms workflows).

## Observability

- Serilog configured in gateway and all services.

## Run

```bash
docker compose up --build
```

## Database

`database/001_init.sql` includes:
- RBAC schema (`auth.*`) for roles, permissions, assignments, policies, audit.
- Service schemas/tables for CRUD persistence:
  - `lead.leads`
  - `complaints.complaints`
  - `master.master_items`
  - `appointments.service_appointments`

## Important

Set actual Entra values in each `appsettings.json` before deployment.
