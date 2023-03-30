FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Amatsucozy.PMS.Email.API/Amatsucozy.PMS.Email.API.csproj", "Amatsucozy.PMS.Email.API/"]
COPY ["Amatsucozy.PMS.Email.Contracts/Amatsucozy.PMS.Email.Contracts.csproj", "Amatsucozy.PMS.Email.Contracts/"]
COPY ["Amatsucozy.PMS.Email.Core/Amatsucozy.PMS.Email.Core.csproj", "Amatsucozy.PMS.Email.Core/"]
COPY ["Amatsucozy.PMS.Email.Infrastructure/Amatsucozy.PMS.Email.Infrastructure.csproj", "Amatsucozy.PMS.Email.Infrastructure/"]
COPY ["nuget.config", "nuget.config"]
RUN dotnet restore "Amatsucozy.PMS.Email.API/Amatsucozy.PMS.Email.API.csproj" -
COPY . .
WORKDIR "/src/Amatsucozy.PMS.Email.API"
RUN dotnet build "Amatsucozy.PMS.Email.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Amatsucozy.PMS.Email.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Amatsucozy.PMS.Email.API.dll"]
