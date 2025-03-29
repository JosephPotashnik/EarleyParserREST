# Use the official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /out

# Copy the Grammars and Vocs directories into the container (add this line)
COPY EarleyParserREST/Grammars /app/Grammars
COPY EarleyParserREST/Vocs /app/Vocs

# Use a runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /out .

# Copy the Grammars and Vocs directories into the runtime container as well
COPY EarleyParserREST/Grammars /app/Grammars
COPY EarleyParserREST/Vocs /app/Vocs

# Expose the application port (8080 is commonly used for containerized .NET apps)
EXPOSE 8080

# Set the environment variables if needed for production
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
CMD ["dotnet", "EarleyParserREST.dll"]