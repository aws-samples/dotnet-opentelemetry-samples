FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["SampleApi.csproj", "./"]
RUN dotnet restore "SampleApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SampleApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SampleApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
RUN apt-get update
RUN apt-get install curl -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleApi.dll"]
