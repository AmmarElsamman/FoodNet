FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Ordering.API/Ordering.API.csproj", "Ordering.API/"]
COPY ["FoodNet.Contracts/FoodNet.Contracts.csproj", "FoodNet.Contracts/"]
RUN dotnet restore "Ordering.API/Ordering.API.csproj"

COPY . .
RUN dotnet publish "Ordering.API/Ordering.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet" , "Ordering.API.dll" ]