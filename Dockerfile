FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restaura primeiro só com os .csproj para aproveitar o cache de camadas.
COPY SmartWallet.Domain/SmartWallet.Domain.csproj SmartWallet.Domain/
COPY SmartWallet.Application/SmartWallet.Application.csproj SmartWallet.Application/
COPY SmartWallet.Infrastructure/SmartWallet.Infrastructure.csproj SmartWallet.Infrastructure/
COPY SmartWallet.Web/SmartWallet.Web.csproj SmartWallet.Web/
RUN dotnet restore SmartWallet.Web/SmartWallet.Web.csproj

COPY SmartWallet.Domain/ SmartWallet.Domain/
COPY SmartWallet.Application/ SmartWallet.Application/
COPY SmartWallet.Infrastructure/ SmartWallet.Infrastructure/
COPY SmartWallet.Web/ SmartWallet.Web/
RUN dotnet publish SmartWallet.Web/SmartWallet.Web.csproj --configuration Release --no-restore --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

USER $APP_UID
EXPOSE 8080

ENTRYPOINT ["dotnet", "SmartWallet.Web.dll"]
