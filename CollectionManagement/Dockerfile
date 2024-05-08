FROM mcr.microsoft.com/dotnet/sdk:8.0  AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
RUN dotnet restore /Collection.Presentation/Collection.Presentation.csproj
# Build and publish a release
RUN dotnet publish -c Release -o out
RUN dotnet publish /Collection.Presentation/Collection.Presentation.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "Collection.Presentation.dll"]
