FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DMSPortal.BackendServer/DMSPortal.BackendServer.csproj", "DMSPortal.BackendServer/"]
COPY ["DMSPortal.Models/DMSPortal.Models.csproj", "DMSPortal.Models/"]
RUN dotnet restore "DMSPortal.BackendServer/DMSPortal.BackendServer.csproj"
COPY . .
WORKDIR "/src/DMSPortal.BackendServer"
RUN dotnet build "DMSPortal.BackendServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DMSPortal.BackendServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DMSPortal.BackendServer.dll"]