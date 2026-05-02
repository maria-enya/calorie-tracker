# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY CalorieTracker/CalorieTracker.csproj CalorieTracker/
RUN dotnet restore CalorieTracker/CalorieTracker.csproj
COPY CalorieTracker/ CalorieTracker/
WORKDIR /src/CalorieTracker
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Create folder for SQLite database
RUN mkdir -p /data

ENV DATA_PATH=/data
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "CalorieTracker.dll"]