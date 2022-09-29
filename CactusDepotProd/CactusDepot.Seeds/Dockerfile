#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443
EXPOSE 9091

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CactusDepot.Seeds/CactusDepot.Seeds.csproj", "CactusDepot.Seeds/"]
COPY ["CactusDepot.Common.DataContext/CactusDepot.Common.DataContext.csproj", "CactusDepot.Common.DataContext/"]
COPY ["CactusDepot.Common.Models/CactusDepot.Common.Models.csproj", "CactusDepot.Common.Models/"]
RUN dotnet restore "CactusDepot.Seeds/CactusDepot.Seeds.csproj"
COPY . .
WORKDIR "/src/CactusDepot.Seeds"
RUN dotnet build "CactusDepot.Seeds.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CactusDepot.Seeds.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CactusDepot.Seeds.dll"]