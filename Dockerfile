# Etapa 1: Base de ejecución (Runtime ligera)
# NOTA: Si usas .NET 8 o 9, cambia el "10.0" por "8.0" o "9.0" según corresponda.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# Un solo EXPOSE. Cloud Run requiere estrictamente el 8080 por defecto.
EXPOSE 8080

# Etapa 2: Compilación (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copiar y restaurar primero (aprovecha el caché de capas de Docker)
COPY ["custom-chat-backend.csproj", "./"]
RUN dotnet restore "custom-chat-backend.csproj"

# 2. Copiar el resto del código y compilar
COPY . .
RUN dotnet build "custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 3: Publicación (Publish)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagen Final (Producción limpia)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Obligamos a ASP.NET Core a escuchar en el puerto correcto usando variables de entorno nativas
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "custom-chat-backend.dll"]