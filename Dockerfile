FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TaskManagementSystem/TaskManagementSystem.csproj", "TaskManagementSystem/"]
RUN dotnet restore "TaskManagementSystem/TaskManagementSystem.csproj"
COPY . .
WORKDIR "/src/TaskManagementSystem"
RUN dotnet build "TaskManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementSystem.dll"]
