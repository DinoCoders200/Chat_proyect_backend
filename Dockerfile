FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
# Cloud Run solo necesita el puerto 8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["custom-chat-backend/custom-chat-backend.csproj", "custom-chat-backend/"]
RUN dotnet restore "custom-chat-backend/custom-chat-backend.csproj"
COPY . .
WORKDIR "/src/custom-chat-backend"
RUN dotnet build "./custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./custom-chat-backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "custom-chat-backend.dll"]