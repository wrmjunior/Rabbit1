#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PageData.WebApi/PageData.WebApi.csproj", "PageData.WebApi/"]
COPY ["PageData.QueueServices/PageData.QueueServices.csproj", "PageData.QueueServices/"]
COPY ["PageData.DataServices/PageData.DataServices.csproj", "PageData.DataServices/"]
COPY ["PageData.Repositories/PageData.Repositories.csproj", "PageData.Repositories/"]
RUN dotnet restore "PageData.WebApi/PageData.WebApi.csproj"
COPY . .
WORKDIR "/src/PageData.WebApi"
RUN dotnet build "PageData.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PageData.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PageData.WebApi.dll"]