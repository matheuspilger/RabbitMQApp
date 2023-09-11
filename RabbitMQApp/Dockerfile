FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RabbitMQApp/RabbitMQApp.csproj", "RabbitMQApp/"]
RUN dotnet restore "RabbitMQApp/RabbitMQApp.csproj"
COPY . .
WORKDIR "/src/RabbitMQApp"
RUN dotnet build "RabbitMQApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQApp.dll"]