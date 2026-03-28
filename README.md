# Calendar App - Training Management System

A multi-layered C# console application designed for managing gym coaches and training sessions.

## Technologies Used
* **Language:** C# (.NET 8)
* **ORM:** Entity Framework Core
* **Database:** MS SQL Server (LocalDB)
* **Architecture:** Clean Architecture (separated into Domain, Application, Infrastructure, and Presentation layers)
* **Dependency Injection:** Microsoft Extensions Dependency Injection
* **Testing:** Unit tests included

## Key Features
* Full CRUD operations for **Coaches** and **Training sessions**.
* Automatic database migration and seeding on startup.
* Decoupled architecture for high maintainability and testability.
* Interactive UI: Console-based daily view with navigation between schedule days.
* Capacity Management: Automatically warns or blocks entries based on room type (Small vs. Large) and maximum participant count.
* Event-Driven Architecture: Uses C# Events to notify users about:
    * Successful scheduling.
    * Capacity warnings (e.g., room size vs. maximum attendance).
