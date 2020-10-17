#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Dotnet
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MobileDataUsageReminder/MobileDataUsageReminder.csproj", "MobileDataUsageReminder/"]
RUN dotnet restore "MobileDataUsageReminder/MobileDataUsageReminder.csproj"
COPY . .
WORKDIR "/src/MobileDataUsageReminder"
RUN dotnet build "MobileDataUsageReminder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MobileDataUsageReminder.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls
RUN pwd
ENTRYPOINT ["dotnet", "MobileDataUsageReminder.dll"]
