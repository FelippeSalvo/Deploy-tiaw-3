
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PCraft.Core/PCraft.Core/PCraft.Core.csproj", "PCraft.Core/PCraft.Core/"]
RUN dotnet restore "PCraft.Core/PCraft.Core/PCraft.Core.csproj"

COPY . .

WORKDIR "/src/PCraft.Core/PCraft.Core"
RUN dotnet build "PCraft.Core.csproj" -c Release -o /app/build
RUN dotnet publish "PCraft.Core.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PCraft.Core.dll"]
