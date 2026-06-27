# Etapa 1: Base de ejecución (Runtime ligera)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa 2: Compilación (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copiar el archivo del proyecto y restaurar dependencias
COPY ["custom-chat-backend.csproj", "./"]
RUN dotnet restore "custom-chat-backend.csproj"

# 2. Copiar absolutamente todo el resto del código (Api, Core, Infrastructure, etc.)
COPY . .

# 3. Compilar el proyecto principal
RUN dotnet build "custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 3: Publicación (Publish)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagen Final (Producción limpia)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 🛠️ VARIABLES DE ENTORNO NATIVAS Y BLINDADAS PARA CLOUD RUN
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "custom-chat-backend.dll"]