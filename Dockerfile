FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AirportControl.API/AirportControl.API.csproj", "AirportControl.API/"]
COPY ["AirportControl.Application/AirportControl.Application.csproj", "AirportControl.Application/"]
COPY ["AirportControl.CteleportClient/AirportControl.CteleportClient.csproj", "AirportControl.CteleportClient/"]
COPY ["AirportControl.CacheClient/AirportControl.CacheClient.csproj", "AirportControl.CacheClient/"]
RUN dotnet restore "AirportControl.API/AirportControl.API.csproj"
COPY . .
WORKDIR "/src/AirportControl.API"
RUN dotnet build "AirportControl.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AirportControl.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirportControl.API.dll"]
