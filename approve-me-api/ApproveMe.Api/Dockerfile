FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApproveMe.Api/ApproveMe.Api.csproj", "ApproveMe.Api/"]
COPY ["ApproveMe.Core/ApproveMe.Core.csproj", "ApproveMe.Core/"]
COPY ["ApproveMe.Repository/ApproveMe.Repository.csproj", "ApproveMe.Repository/"]

RUN dotnet restore "ApproveMe.Api/ApproveMe.Api.csproj"
RUN dotnet restore "ApproveMe.Core/ApproveMe.Core.csproj"
RUN dotnet restore "ApproveMe.Repository/ApproveMe.Repository.csproj"

COPY . .
WORKDIR "/src/ApproveMe.Api"
RUN dotnet build "ApproveMe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApproveMe.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApproveMe.Api.dll"]
    