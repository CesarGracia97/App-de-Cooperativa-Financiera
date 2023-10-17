# Utiliza la imagen oficial de .NET 7 como punto de partida
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /act_Application

EXPOSE 80
EXPOSE 5024

COPY ./*.csproj ./
RUN dotnet restore 

COPY . .
RUN dotnet publish -c Release -o out

#Imagen
FROM mcr.microsoft.com/dotnet/sdk:7.0 
WORKDIR /act_Application
COPY --from=build /act_Application/out .   
ENTRYPOINT ["dotnet", "act_Application.dll"]
