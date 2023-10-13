FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Client/CalculatorApp.Client.csproj", "Client/"]
COPY ["Shared/CalculatorApp.Shared.csproj", "Shared/"]
RUN dotnet restore "Client/CalculatorApp.Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "CalculatorApp.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CalculatorApp.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#Přidán nginx server
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot/ .