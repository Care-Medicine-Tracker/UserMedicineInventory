FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base

WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY "Care.UserMedicineInventory.Service.csproj" .
RUN dotnet restore "Care.UserMedicineInventory.Service.csproj"
# Copy everything into the docker directory and build
COPY . .
RUN dotnet build "Care.UserMedicineInventory.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Care.UserMedicineInventory.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Care.UserMedicineInventory.Service.dll"]
