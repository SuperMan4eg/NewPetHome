FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/NewPetHome.API/NewPetHome.API.csproj", "Src/NewPetHome.API/"]
COPY ["Src/NewPetHome.Applications/NewPetHome.Applications.csproj", "Src/NewPetHome.Applications/"]
COPY ["Src/NewPetHome.Domain/NewPetHome.Domain.csproj", "Src/NewPetHome.Domain/"]
COPY ["Src/NewPetHome.Infrastructure/NewPetHome.Infrastructure.csproj", "Src/NewPetHome.Infrastructure/"]
RUN dotnet restore "Src/NewPetHome.API/NewPetHome.API.csproj"
COPY . .
WORKDIR "/src/Src/NewPetHome.API"
RUN dotnet build "NewPetHome.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NewPetHome.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewPetHome.API.dll"]
