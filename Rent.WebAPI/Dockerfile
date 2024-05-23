FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rent.WebAPI/Rent.WebAPI.csproj", "Rent.WebAPI/"]
COPY ["Rent.BLL/Rent.BLL.csproj", "Rent.BLL/"]
COPY ["Rent.DAL/Rent.DAL.csproj", "Rent.DAL/"]
COPY ["Rent.Dtos.Library/Rent.DTOs.Library.csproj", "Rent.Dtos.Library/"]
COPY ["Rent.Model.Library/Rent.Model.Library.csproj", "Rent.Model.Library/"]
COPY ["Rent.ExceptionLibrary/Rent.ExceptionLibrary.csproj", "Rent.ExceptionLibrary/"]
COPY ["Rent.ResponseAndRequestLibrary/Rent.ResponseAndRequestLibrary.csproj", "Rent.ResponseAndRequestLibrary/"]
RUN dotnet restore "./Rent.WebAPI/Rent.WebAPI.csproj"
COPY . .
WORKDIR "/src/Rent.WebAPI"
RUN dotnet build "./Rent.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rent.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rent.WebAPI.dll"]