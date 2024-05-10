# Define a stage for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy the entire project directory
COPY CollectionManagement/. .

# Restore dependencies as separate layers
RUN dotnet restore

# Build the application for release configuration and publish the output to /out directory
RUN dotnet publish CollectionManagement.Presentation/CollectionManagement.Presentation.csproj -c Release -o out

# Define the runtime image stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory
WORKDIR /App

# Copy the published artifacts from the build stage
COPY --from=build-env /App/out .

# Expose port 5000
EXPOSE 5000

# Set the environment variable for ASP.NET Core to listen on port 5000
ENV ASPNETCORE_URLS=http://+:5000

# Entrypoint to run the application
ENTRYPOINT ["dotnet", "CollectionManagement.Presentation.dll"]

