# CHAT

#prueba

## Local setup

You need [.NET 10](https://dotnet.microsoft.com/download), [Docker](https://docs.docker.com/get-docker/),
and the EF Core CLI:

```bash
dotnet tool install --global dotnet-ef
```

If `dotnet ef` is not found afterwards, add the tools folder to your PATH:

```bash
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc && source ~/.bashrc
```

### Running it

The database runs in Docker. The credentials in `docker-compose.yml` already match
`ConnectionStrings:DefaultConnection` in `appsettings.json`, so there is nothing to configure.

```bash
docker compose up -d --wait   # start PostgreSQL, wait until it accepts connections
dotnet ef database update     # apply the migrations
dotnet run                    # http://localhost:8080
```

The API reference (Scalar) is at `/scalar/v1` in Development.

### Everyday commands

```bash
docker compose ps             # is it running?
docker compose logs -f postgres
docker compose down           # stop the database, keep the data
docker compose down -v        # stop it and DELETE the data (start clean)
```

Open a SQL console against the local database:

```bash
docker exec -it dinocord-postgres psql -U chat_user -d custom_chat_db
```

Useful once inside: `\dt` lists the tables, `\d "Users"` describes one (the quotes matter —
the table names are PascalCase), `\q` quits.

### Starting over

If your local database ends up in a weird state, throw it away.

```bash
docker compose down -v && docker compose up -d --wait
dotnet ef database update
```

## Migrations

Migrations are **generated, not written by hand**. EF compares the entities in `Core/Domain`
and their configurations in `Infrastructure/Persistence/Configurations` against
`Migrations/ApplicationDbContextModelSnapshot.cs`, and writes the difference.

```bash
dotnet ef migrations add SomeDescriptiveName
dotnet ef database update
dotnet ef migrations remove          # undo the last one, if not applied yet
dotnet ef migrations has-pending-model-changes   # do the entities match the migrations?
```

## Documentation

- [ADR 0001 — Relational model migration](docs/adr/0001-relational-model-migration.md)
