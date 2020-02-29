# housing_apis :: lodging_dockerfile

##arguments
ARG DOTNET_VERSION=3.1

## stage - restore
FROM mcr.microsoft.com/dotnet/core/sdk:${DOTNET_VERSION} as restore
WORKDIR /src
COPY src/Revature.Lodging.Api/*.csproj Revature.Lodging.Api/
COPY src/Revature.Lodging.DataAccess/*.csproj Revature.Lodging.DataAccess/
COPY src/Revature.Lodging.Lib/*.csproj Revature.Lodging.Lib/
RUN dotnet resotre *.Api

## stage - publish
FROM restore as publish
COPY src/Revature.Lodging.Api/ Revature.Lodging.Api/
COPY src/Revature.Lodging.DataAccess/ Revature.Lodging.DataAccess/
COPY src/Revature.Lodging.Lib/ Revature.Lodging.Lib/
RUN dotnet publish *.Api --configuration Release --no-restore --output /src/dist

## stage -deploy
FROM mcr.microsoft.com/dotnet/core/aspnet:${DOTNET_VERSION} as deploy
WORKDIR /api
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=publish /src/dist/ ./
CMD dotnet *.Api.dll