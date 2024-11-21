#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DesignRepositoryService.Web/DesignRepositoryService.Web.csproj", "DesignRepositoryService.Web/"]
RUN dotnet restore "DesignRepositoryService.Web/DesignRepositoryService.Web.csproj"
COPY . .
WORKDIR "/src/DesignRepositoryService.Web"
RUN dotnet build -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DesignRepositoryService.Web.dll"]


