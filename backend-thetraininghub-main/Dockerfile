FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish aa2-alberto-rodriguez-penalva.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 6873

ENTRYPOINT ["dotnet", "aa2-alberto-rodriguez-penalva.dll"]
