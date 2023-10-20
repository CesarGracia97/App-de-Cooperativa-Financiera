FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /act_Application
COPY ["act_Application/act_Application.csproj", ""]
RUN dotnet restore "./act_Application.csproj"
COPY . .
WORKDIR "/act_Application/."
RUN dotnet build "act_Application/act_Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "act_Application/act_Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "act_Application.dll"]