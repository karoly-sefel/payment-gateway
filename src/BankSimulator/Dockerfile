# syntax=docker/dockerfile:1.6

FROM mcr.microsoft.com/dotnet/aspnet:7.0.10-bookworm-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.400-bookworm-slim AS build
WORKDIR /src
COPY ["BankSimulator/BankSimulator.csproj", "BankSimulator/"]
RUN dotnet restore "BankSimulator/BankSimulator.csproj" --locked-mode
COPY . .
WORKDIR "/src/BankSimulator"
RUN dotnet build "BankSimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankSimulator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.BankSimulator.dll"]
