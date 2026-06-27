# Etapa 1: Base de ejecución (Runtime)
# Utiliza la imagen oficial de ASP.NET Core (cambia a 8.0 o 9.0 si no usas la preview de .NET 10)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# Cloud Run asigna automáticamente un puerto en la variable de entorno $PORT, 
# ASP.NET Core lee la variable PORT por defecto en versiones recientes.
EXPOSE 8080
EXPOSE 80

# Etapa 2: Compilación (Build)
# Utiliza el SDK completo para restaurar y compilar el código fuente
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia el archivo .csproj directamente desde tu raíz actual
COPY ["custom-chat-backend.csproj", "./"]
RUN dotnet restore "custom-chat-backend.csproj"

# Copia todo el resto de los archivos del repositorio
COPY . .
WORKDIR "/src"
RUN dotnet build "custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 3: Publicación (Publish)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagen Final (Producción)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "custom-chat-backend.dll"]