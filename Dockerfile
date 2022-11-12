FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PRY20220278.csproj", "./"]
RUN dotnet restore "PRY20220278.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "PRY20220278.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PRY20220278.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRY20220278.dll"]
