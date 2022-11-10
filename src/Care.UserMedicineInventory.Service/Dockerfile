FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

WORKDIR /app

# copy project csproj file and restore it in docker directory
COPY ./*.csproj ./
RUN dotnet restore

# Copy everything into the docker directory and build
COPY . .
RUN dotnet publish -c Release -o out

EXPOSE 5005

# Build runtime final image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Care.UserMedicineInventory.Service.dll"]

# FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
# WORKDIR /src
# COPY ["/Care.UserMedicineInventory.Service.csproj", "Care.UserMedicineInventory.Service/"]
# RUN dotnet restore "Care.UserMedicineInventory.Service/Care.UserMedicineInventory.Service.csproj"
# COPY . .
# WORKDIR "/src"
# RUN dotnet build "Care.UserMedicineInventory.Service.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "Care.UserMedicineInventory.Service.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Care.UserMedicineInventory.Service.dll"]