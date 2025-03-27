## Running the Infrastructure

1. Start the infrastructure services:

```bash
docker-compose up -d
```

2. Verify services are running:

- PostgreSQL: localhost:5432
- Kafka: localhost:9092
- Kafka UI: http://localhost:8080

3. Run the application:

```bash
dotnet run --project PhoneDirectory.API
```

The application will be available at:

- HTTP: http://localhost:5015
- HTTPS: https://localhost:7241
- Swagger UI: http://localhost:5015
