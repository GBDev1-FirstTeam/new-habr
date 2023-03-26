#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["01_Presentation/NewHabr.WebApi/NewHabr.WebApi.csproj", "01_Presentation/NewHabr.WebApi/"]
COPY ["03_DAL/NewHabr.MSSQL/NewHabr.MSSQL.csproj", "03_DAL/NewHabr.MSSQL/"]
COPY ["03_DAL/NewHabr.DAL/NewHabr.DAL.csproj", "03_DAL/NewHabr.DAL/"]
COPY ["02_Business/NewHabr.Domain/NewHabr.Domain.csproj", "02_Business/NewHabr.Domain/"]
COPY ["03_DAL/NewHabr.PostgreSQL/NewHabr.PostgreSQL.csproj", "03_DAL/NewHabr.PostgreSQL/"]
COPY ["02_Business/NewHabr.Business/NewHabr.Business.csproj", "02_Business/NewHabr.Business/"]
RUN dotnet restore "01_Presentation/NewHabr.WebApi/NewHabr.WebApi.csproj"
COPY . .
WORKDIR "/src/01_Presentation/NewHabr.WebApi"
RUN dotnet build "NewHabr.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewHabr.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewHabr.WebApi.dll"]
