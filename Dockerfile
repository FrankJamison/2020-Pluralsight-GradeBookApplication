FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project file(s) first for better layer caching
COPY GradeBook/GradeBook.csproj GradeBook/
RUN dotnet restore GradeBook/GradeBook.csproj

# Copy the rest of the source and publish
COPY . .
RUN dotnet publish GradeBook/GradeBook.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish ./

# Run as non-root
RUN useradd -m appuser && chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "GradeBook.dll"]
