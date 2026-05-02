# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj from root (no subfolder)
COPY CalorieTracker.csproj .
RUN dotnet restore CalorieTracker.csproj

# Copy everything else
COPY . .
RUN dotnet publish CalorieTracker.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p /data
ENV DATA_PATH=/data
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "CalorieTracker.dll"]