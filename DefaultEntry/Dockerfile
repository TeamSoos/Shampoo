FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DefaultEntry.csproj", "."]
RUN dotnet restore "DefaultEntry.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "DefaultEntry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DefaultEntry.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DefaultEntry.dll"]
