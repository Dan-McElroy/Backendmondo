FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Backendmondo.API/Backendmondo.API.csproj", "Backendmondo.API/"]
COPY ["Backendmondo.Tests/Backendmondo.Tests.csproj", "Backendmondo.Tests/"]
RUN dotnet restore "Backendmondo.API/Backendmondo.API.csproj"
COPY . .
WORKDIR "/src/Backendmondo.API"
RUN dotnet build "Backendmondo.API.csproj" -c Release -o /app/build

FROM build AS testing
WORKDIR "/src/Backendmondo.Tests"
RUN dotnet test "Backendmondo.Tests.csproj" -c Release -o /app/testing

FROM build AS publish
RUN dotnet publish "Backendmondo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Backendmondo.API.dll

#COPY ["entrypoint.sh", "entrypoint.sh"]
#RUN chmod +x ./entrypoint.sh
#CMD ASPNETCORE_URLS=http://*:$PORT ./entrypoint.sh