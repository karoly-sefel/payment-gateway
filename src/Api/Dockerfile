# syntax=docker/dockerfile:1.6

FROM mcr.microsoft.com/dotnet/aspnet:7.0.10-bookworm-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.400-bookworm-slim AS build
WORKDIR /src
COPY --link Directory.Build.props global.json ./
COPY --link Api/Api.csproj ./Api/
COPY --link Application/Application.csproj ./Application/
COPY --link Domain/Domain.csproj ./Domain/
COPY --link Infrastructure/Infrastructure.csproj ./Infrastructure/
RUN dotnet restore "Api/Api.csproj" --locked-mode

COPY Api ./Api
COPY Application ./Application
COPY Domain ./Domain
COPY Infrastructure ./Infrastructure
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build --no-restore


FROM build as test-runner
COPY tests /src/tests/
WORKDIR /src/tests/Api.Specs
COPY scripts/run-tests.sh ./
RUN dotnet restore "Api.Specs.csproj" --locked-mode
RUN dotnet build "Api.Specs.csproj" -c Release --no-restore
CMD ["bash", "-c", "./run-tests.sh"]


FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.Api.dll"]
