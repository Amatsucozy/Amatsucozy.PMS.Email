FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Amatsucozy.PMS.Email.Messaging/Amatsucozy.PMS.Email.Messaging.csproj", "Amatsucozy.PMS.Email.Messaging/"]
COPY ["Amatsucozy.PMS.Email.Contracts/Amatsucozy.PMS.Email.Contracts.csproj", "Amatsucozy.PMS.Email.Contracts/"]
COPY ["Amatsucozy.PMS.Email.Core/Amatsucozy.PMS.Email.Core.csproj", "Amatsucozy.PMS.Email.Core/"]
COPY ["Amatsucozy.PMS.Email.Infrastructure/Amatsucozy.PMS.Email.Infrastructure.csproj", "Amatsucozy.PMS.Email.Infrastructure/"]
COPY ["nuget.config", "nuget.config"]
RUN dotnet restore "Amatsucozy.PMS.Email.Messaging/Amatsucozy.PMS.Email.Messaging.csproj"
COPY . .
WORKDIR "/src/Amatsucozy.PMS.Email.Messaging"
RUN dotnet build "Amatsucozy.PMS.Email.Messaging.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Amatsucozy.PMS.Email.Messaging.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Amatsucozy.PMS.Email.Messaging.dll"]
