# housing_apis :: room_dockerfile

## arguments
ARG DOTNET_VERSION=3.1

## stage - restore
FROM mcr.microsoft.com/dotnet/core/sdk:${DOTNET_VERSION} as restore
WORKDIR /src
COPY src/Revature.Room.Api/*.csproj Revature.Room.Api/
COPY src/Revature.Room.DataAccess/*.csproj Revature.Room.DataAccess/
COPY src/Revature.Room.Lib/*.csproj Revature.Room.Lib/
RUN dotnet restore *.Api

## stage - publish
FROM restore as publish
COPY src/Revature.Room.Api/ Revature.Room.Api/
COPY src/Revature.Room.DataAccess/ Revature.Room.DataAccess/
COPY src/Revature.Room.Lib/ Revature.Room.Lib/
RUN dotnet publish *.Api --configuration Release --no-restore --output /src/dist

## stage - deploy
FROM mcr.microsoft.com/dotnet/core/aspnet:${DOTNET_VERSION} as deploy
WORKDIR /api
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=publish /src/dist/ ./
CMD dotnet *.Api.dll
