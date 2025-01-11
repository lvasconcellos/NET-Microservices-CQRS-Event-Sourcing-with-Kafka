# NET-Microservices-CQRS-Event-Sourcing-with-Kafka

## Overview

This project is based on a course by Sean Campbell available on Udemy, designed to teach concepts such as microservices architecture, CQRS (Command Query Responsibility Segregation), and Event Sourcing. It implements a Social Media API leveraging Docker, Kafka, MongoDB, and PostgreSQL to provide a scalable, reliable, and efficient system. As a bonus, the API includes a service to migrate the CRUD database to a different type of database using the event history.

## Features

- **Microservices Architecture**: Modular and scalable service design.
- **CQRS and Event Sourcing**: Ensures separation of read and write operations and provides reliable event logging.
- **Technologies**:
  - Docker: Containerization of services.
  - Kafka: Event streaming platform.
  - MongoDB: Event storage for sourcing.
  - PostgreSQL: CRUD database operations.
- **Migration Service**: Migrate CRUD data to another database using event history.

## Prerequisites

- Docker and Docker Compose
- .NET 7.0 SDK
- Kafka
- MongoDB
- PostgreSQL

Refer to `Installing+Prerequisites.txt` for detailed installation instructions.

## Project Structure

```
📦NET-Microservices-CQRS-Event-Sourcing-with-Kafka
 ┣ 📂CQRS-ES
 ┃ ┗ 📂CQRS.Core            # Core libraries for CQRS and Event Sourcing
 ┣ 📂SM-Post
 ┃ ┣ 📂Post.Cmd            # Command side microservice
 ┃ ┃ ┣ 📂Post.Cmd.Api      # API for handling commands
 ┃ ┃ ┣ 📂Post.Cmd.Domain   # Domain models and logic
 ┃ ┃ ┗ 📂Post.Cmd.Infrastructure # Infrastructure for command handling
 ┃ ┣ 📂Post.Common         # Shared DTOs and event definitions
 ┃ ┣ 📂Post.Query          # Query side microservice
 ┃ ┃ ┣ 📂Post.Query.Api    # API for handling queries
 ┃ ┃ ┣ 📂Post.Query.Domain # Domain models for queries
 ┃ ┃ ┗ 📂Post.Query.Infrastructure # Infrastructure for query handling
 ┗ 📜README.md             # Project documentation
```

## How to Run

1. **Clone the Repository**:

   ````bash
   export KAFKA_TOPIC=<your-kafka-topic>
   ```bash
   git clone <repository-url>
   cd NET-Microservices-CQRS-Event-Sourcing-with-Kafka
   ````

2. **Build and Run with Docker**:

   ```bash
   docker-compose up --build
   ```

3. **Access the Services**:

   - Command API: `http://localhost:<cmd-port>`
   - Query API: `http://localhost:<query-port>`

4. **Test the APIs**:
   Use tools like Postman or Curl to send requests to the endpoints.

## API Endpoints

### Command API

- **Create Post**: `POST /api/v1/NewPost/`
- **Edit Message**: `PUT /api/v1/EditMessage/{id}`
- **Delete Post**: `DELETE /api/v1/DeletePost/{id}`
- **Add Comment**: `PUT /api/v1/AddComment/{id}`
- **Like Post**: `PUT /api/v1/LikePost/{id}`
- **Remove Comment**: `DELETE /api/v1/RemoveComment/{id}`
- **Restore Read Database**: `POST /api/v1/RestoreReadDb`

### Query API

- **Get All Posts**: `GET /api/v1/PostLookup`
- **Get Post By ID**: `GET /api/v1/PostLookup/byId/{postId}`
- **Get Posts By Author**: `GET /api/v1/PostLookup/byAuthor/{author}`
- **Get Posts With Comments**: `GET /api/v1/PostLookup/withComments`
- **Get Posts With Likes**: `GET /api/v1/PostLookup/withLikes/{numberoflikes}`

## Technologies Used

- **Backend**: .NET 7.0, C#
- **Event Streaming**: Kafka
- **Databases**: MongoDB (Event Store), PostgreSQL (CRUD)
- **Containerization**: Docker
- **Hosting**: Docker Compose for multi-container orchestration

## Bonus Migration Service

The project includes a bonus service that allows you to migrate CRUD data to another database type using the event history stored in MongoDB. The key function, `RepublishEventsAsync`, plays a central role in restoring events to any database by replaying events from the event store and producing them to Kafka topics.

## Future Enhancements

- Integration with additional databases for CRUD operations.
- Adding unit tests to ensure the reliability and maintainability of the application.
- Adding comments or improving clarity in the `RepublishEventsAsync` function to make it more beginner-friendly.
- Improved documentation for migration services.
- Advanced query capabilities and analytics.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
