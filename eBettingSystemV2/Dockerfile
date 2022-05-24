#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eBettingSystemV2/eBettingSystemV2.csproj", "eBettingSystemV2/"]
RUN dotnet restore "eBettingSystemV2/eBettingSystemV2.csproj"
COPY . .
WORKDIR "/src/eBettingSystemV2"
RUN dotnet build "eBettingSystemV2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eBettingSystemV2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eBettingSystemV2.dll"]