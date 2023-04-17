FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GUI.csproj", "."]
RUN dotnet restore "GUI.csproj"
COPY . .
WORKDIR "/src/"
# dotnet libraries
RUN dotnet build "GUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update
RUN apt-get install -y libx11-6 libice-dev libsm6 libfontconfig1

ENTRYPOINT ["dotnet", "GUI.dll"]
