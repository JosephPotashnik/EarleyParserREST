# Use the official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /out

# Copy the Grammars and Vocs directories into the container (add this line)
COPY Grammars /app/Grammars
COPY Vocs /app/Vocs

# Use a runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Expose the application port
EXPOSE 8080

# Run the application
CMD ["dotnet", "EarleyParserREST.dll"]
