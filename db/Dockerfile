FROM postgres:latest

# Custom configuration can be added here if needed
COPY db/init.sql /docker-entrypoint-initdb.d/

# Set default environment variables
ENV POSTGRES_DB=$POSTGRES_DB
ENV POSTGRES_USER=$POSTGRES_USER
ENV POSTGRES_PASSWORD=$POSTGRES_PASSWORD

# Expose the PostgreSQL port
EXPOSE 5432
