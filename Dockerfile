#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# EXPOSE 80
# #EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY ["marauderserver.csproj", "."]
# RUN dotnet restore "marauderserver.csproj"
# COPY . .
# WORKDIR "/src"
# RUN dotnet build "marauderserver.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "marauderserver.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "marauderserver.dll"]

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY marauderserver/*.csproj ./marauderserver/
RUN dotnet restore

# copy everything else and build app
COPY marauderserver/. ./marauderserver/
WORKDIR /source/marauderserver
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "marauderserver.dll"]