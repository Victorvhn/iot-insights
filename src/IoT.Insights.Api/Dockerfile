FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IoT.Insights.Api/IoT.Insights.Api.csproj", "IoT.Insights.Api/"]
RUN dotnet restore "IoT.Insights.Api/IoT.Insights.Api.csproj"
COPY . .
WORKDIR "/src/IoT.Insights.Api"
RUN dotnet build "IoT.Insights.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IoT.Insights.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoT.Insights.Api.dll"]
