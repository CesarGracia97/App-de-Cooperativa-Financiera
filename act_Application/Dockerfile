# Utiliza la imagen oficial de .NET 7 como punto de partida
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["act_Application/act_Application.csproj", "act_Application/"]
RUN dotnet restore "act_Application/act_Application.csproj"
COPY . .
WORKDIR "/src/act_Application"
RUN dotnet build "act_Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "act_Application.csproj" -c Release -o /app/publish

# Crea la imagen final del contenedor
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "act_Application.dll"]