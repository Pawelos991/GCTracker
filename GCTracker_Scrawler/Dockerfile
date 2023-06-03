FROM ubuntu:20.04 AS base

# Install .NET
RUN apt-get update && apt-get install -y \
    wget \
    apt-transport-https \
    software-properties-common

RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get update; \
    apt-get install -y apt-transport-https && \
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb

RUN add-apt-repository universe && \
    apt-get update && \
    apt-get install -y dotnet-sdk-7.0

WORKDIR /app

# Prepare environment for Selenium
RUN apt-get update && \
    apt-get install -y xvfb wget gnupg2 unzip curl && \
    wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - && \
    echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list && \
    apt-get update && \
    apt-get install -y google-chrome-stable && \
    wget https://chromedriver.storage.googleapis.com/2.41/chromedriver_linux64.zip && \
    unzip chromedriver_linux64.zip && \
    mv chromedriver /usr/bin/chromedriver && \
    chown root:root /usr/bin/chromedriver && \
    chmod +x /usr/bin/chromedriver

# Install necessary libraries
RUN apt-get install -y libglib2.0-0 libnss3 libx11-6

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GCTracker_Scrawler/GCTracker_Scrawler.csproj", "GCTracker_Scrawler/"]
COPY ["GC_Tracker_Datalayer/GC_Tracker_Datalayer.csproj", "GC_Tracker_Datalayer/"]
RUN dotnet restore "GCTracker_Scrawler/GCTracker_Scrawler.csproj"
COPY . .
WORKDIR "/src/GCTracker_Scrawler"
RUN dotnet build "GCTracker_Scrawler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GCTracker_Scrawler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GCTracker_Scrawler.dll"]
