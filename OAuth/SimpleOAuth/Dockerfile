FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OAuth/SimpleOAuth/SimpleOAuth.csproj", "OAuth/SimpleOAuth/"]
RUN dotnet restore "OAuth/SimpleOAuth/SimpleOAuth.csproj"
COPY . .
WORKDIR "/src/OAuth/SimpleOAuth"
RUN dotnet build "SimpleOAuth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleOAuth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleOAuth.dll"]
