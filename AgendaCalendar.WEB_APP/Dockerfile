#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AgendaCalendar.WEB_APP/AgendaCalendar.WEB_APP.csproj", "AgendaCalendar.WEB_APP/"]
RUN dotnet restore "./AgendaCalendar.WEB_APP/AgendaCalendar.WEB_APP.csproj"
COPY . .
WORKDIR "/src/AgendaCalendar.WEB_APP"
RUN dotnet build "./AgendaCalendar.WEB_APP.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AgendaCalendar.WEB_APP.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgendaCalendar.WEB_APP.dll"]