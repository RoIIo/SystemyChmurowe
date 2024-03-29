#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# To pozwala odpalić swaggerka
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chmura/Chmura.csproj", "Chmura/"]

RUN dotnet restore "./Chmura/./Chmura.csproj"
COPY . .
WORKDIR "/src/Chmura"
RUN dotnet build "./Chmura.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Chmura.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY "Chmura/Data/" "Data/"
ENV ConnectionStrings__DefaultConnection "Server=host.docker.internal;Database=CHMURA;Port=5432;Username=CHMURAUser;Password=CHMURAPassword;"
ENTRYPOINT ["dotnet", "Chmura.dll", "--launch-profile", "Docker"]