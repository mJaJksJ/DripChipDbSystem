#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DripChipDbSystem/DripChipDbSystem.csproj", "DripChipDbSystem/"]
COPY ["Startup/Startup.csproj", "Startup/"]
RUN dotnet restore "DripChipDbSystem/DripChipDbSystem.csproj"
COPY . .
WORKDIR "/src/DripChipDbSystem"
RUN dotnet build "DripChipDbSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DripChipDbSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DripChipDbSystem.dll"]