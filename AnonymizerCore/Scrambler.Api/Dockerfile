FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Scrambler.Api/Scrambler.Api.csproj", "Scrambler.Api/"]
RUN dotnet restore "Scrambler.Api/Scrambler.Api.csproj"
COPY . .
WORKDIR "/src/Scrambler.Api"
RUN dotnet build "Scrambler.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Scrambler.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Scrambler.Api.dll"]