FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TaskManagementSystem.Api.Consumer/TaskManagementSystem.Api.Consumer.csproj", "TaskManagementSystem.Api.Consumer/"]
RUN dotnet restore "TaskManagementSystem.Api.Consumer/TaskManagementSystem.Api.Consumer.csproj"
COPY . .
WORKDIR "/src/TaskManagementSystem.Api.Consumer"
RUN dotnet build "TaskManagementSystem.Api.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementSystem.Api.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementSystem.Api.Consumer.dll"]
