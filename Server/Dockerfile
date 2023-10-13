FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/CalculatorApp.Server.csproj", "Server/"]
COPY ["Shared/CalculatorApp.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/CalculatorApp.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "CalculatorApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalculatorApp.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalculatorApp.Server.dll"]