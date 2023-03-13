#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5003
EXPOSE 443

ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.csproj ./

RUN dotnet restore "IquraSchool.csproj"
COPY . /src
RUN dotnet build "IquraSchool.csproj" -c Release -o /app/build

COPY Directory.Build.props .

FROM build AS publish
RUN dotnet publish "IquraSchool.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build as final
WORKDIR /src/IquraSchool
ENTRYPOINT [ "dotnet", "watch", "run" ]

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "IquraSchool.dll"]