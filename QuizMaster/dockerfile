# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY ["QuizMaster/QuizMaster.csproj", "QuizMaster/"]
RUN dotnet restore

COPY . .
WORKDIR "/src/QuizMaster"
RUN dotnet publish -c Release -o /app/publish

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "QuizMaster.dll"]