#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GCTracker_Backend/GCTracker_Backend.csproj", "GCTracker_Backend/"]
COPY ["GC_Tracker_Datalayer/GC_Tracker_Datalayer.csproj", "GC_Tracker_Datalayer/"]
COPY ["GC_Tracker_Logic/GC_Tracker_Logic.csproj", "GC_Tracker_Logic/"]
RUN dotnet restore "GCTracker_Backend/GCTracker_Backend.csproj"
COPY . .
WORKDIR "/src/GCTracker_Backend"
RUN dotnet build "GCTracker_Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GCTracker_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GCTracker_Backend.dll"]