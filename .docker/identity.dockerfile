# housing_apis :: identity_dockerfile

## arguments
ARG DOTNET_VERSION=3.1

## stage - restore
FROM mcr.microsoft.com/dotnet/core/sdk:${DOTNET_VERSION} as restore
WORKDIR /src
COPY src/Revature.Identity.Api/*.csproj Revature.Identity.Api/
COPY src/Revature.Identity.DataAccess/*.csproj Revature.Identity.DataAccess/
COPY src/Revature.Identity.Lib/*.csproj Revature.Identity.Lib/
RUN dotnet restore *.Api

## stage - publish
FROM restore as publish
COPY src/Revature.Identity.Api/ Revature.Identity.Api/
COPY src/Revature.Identity.DataAccess/ Revature.Identity.DataAccess/
COPY src/Revature.Identity.Lib/ Revature.Identity.Lib/
RUN dotnet publish *.Api --configuration Release --no-restore --output /src/dist

## stage - deploy
FROM mcr.microsoft.com/dotnet/core/aspnet:${DOTNET_VERSION} as deploy
WORKDIR /api
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=publish /src/dist/ ./
CMD dotnet *.Api.dll
