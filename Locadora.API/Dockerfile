#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Locadora.API/Locadora.API.csproj", "Locadora.API/"]
COPY ["Locadora.Repository/Locadora.Repository.csproj", "Locadora.Repository/"]
COPY ["Locadora.Domain/Locadora.Domain.csproj", "Locadora.Domain/"]
RUN dotnet restore "Locadora.API/Locadora.API.csproj"
COPY . .
WORKDIR "/src/Locadora.API"
RUN dotnet build "Locadora.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Locadora.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Locadora.API.dll"]
