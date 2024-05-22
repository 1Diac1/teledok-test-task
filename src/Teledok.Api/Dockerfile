FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Teledok.Api/Teledok.Api.csproj", "src/Teledok.Api/"]
COPY ["src/Teledok.Core/Teledok.Core.csproj", "src/Teledok.Core/"]
COPY ["src/Teledok.Domain/Teledok.Domain.csproj", "src/Teledok.Domain/"]
COPY ["src/Teledok.Api.AspNetCore/Teledok.Api.AspNetCore.csproj", "src/Teledok.Api.AspNetCore/"]
COPY ["src/Teledok.Infrastructure.EntityFrameworkCore.Postgresql/Teledok.Infrastructure.EntityFrameworkCore.Postgresql.csproj", "src/Teledok.Infrastructure.EntityFrameworkCore.Postgresql/"]
COPY ["src/Teledok.Infrastructure.EntityFrameworkCore/Teledok.Infrastructure.EntityFrameworkCore.csproj", "src/Teledok.Infrastructure.EntityFrameworkCore/"]
COPY ["src/Teledok.Infrastructure.Abstractions/Teledok.Infrastructure.Abstractions.csproj", "src/Teledok.Infrastructure.Abstractions/"]
COPY ["src/Teledok.Application/Teledok.Application.csproj", "src/Teledok.Application/"]
RUN dotnet restore "src/Teledok.Api/Teledok.Api.csproj"
COPY . .
WORKDIR "/src/src/Teledok.Api"
RUN dotnet build "Teledok.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teledok.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teledok.Api.dll"]
