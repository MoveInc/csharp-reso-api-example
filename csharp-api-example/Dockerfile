#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["csharp-api-example/csharp-api-example.csproj", "csharp-api-example/"]
RUN dotnet restore "csharp-api-example/csharp-api-example.csproj"
COPY . .
WORKDIR "/src/csharp-api-example"
RUN dotnet build "csharp-api-example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "csharp-api-example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "csharp-api-example.dll"]
