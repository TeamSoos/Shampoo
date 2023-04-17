FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["default/default.csproj", "default/"]
RUN dotnet restore "default/default.csproj"
COPY . .
WORKDIR "/src/default"
RUN dotnet build "default.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "default.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "default.dll"]
