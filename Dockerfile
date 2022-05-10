#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Tellbal/Tellbal.csproj", "Tellbal/"]
COPY ["WebFramework/WebFramework.csproj", "WebFramework/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "Tellbal/Tellbal.csproj"
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/* 
COPY . .
WORKDIR "/src/Tellbal"
RUN dotnet build "Tellbal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tellbal.csproj" -c Release -o /app/publish
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/* 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
ENTRYPOINT ["dotnet", "Tellbal.dll"]
