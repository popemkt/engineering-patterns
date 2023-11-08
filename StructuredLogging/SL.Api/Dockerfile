FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StructuredLogging/SL.Api/SL.Api.csproj", "StructuredLogging/SL.Api/"]
RUN dotnet restore "StructuredLogging/SL.Api/SL.Api.csproj"
COPY . .
WORKDIR "/src/StructuredLogging/SL.Api"
RUN dotnet build "SL.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SL.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SL.Api.dll"]
