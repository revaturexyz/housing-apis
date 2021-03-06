# housing_apis :: address_dockerfile

## arguments
ARG DOTNET_VERSION=3.1

## stage - restore
FROM mcr.microsoft.com/dotnet/core/sdk:${DOTNET_VERSION} as restore
WORKDIR /src
COPY src/Revature.Address.Api/*.csproj Revature.Address.Api/
COPY src/Revature.Address.DataAccess/*.csproj Revature.Address.DataAccess/
COPY src/Revature.Address.Lib/*.csproj Revature.Address.Lib/
RUN dotnet restore *.Api

## stage - publish
FROM restore as publish
COPY src/Revature.Address.Api/ Revature.Address.Api/
COPY src/Revature.Address.DataAccess/ Revature.Address.DataAccess/
COPY src/Revature.Address.Lib/ Revature.Address.Lib/
RUN dotnet publish *.Api --configuration Release --no-restore --output /src/dist

## stage - deploy
FROM mcr.microsoft.com/dotnet/core/aspnet:${DOTNET_VERSION} as deploy
WORKDIR /api
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=publish /src/dist/ ./
CMD dotnet *.Api.dll
