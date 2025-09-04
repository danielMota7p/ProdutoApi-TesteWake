# ====== Build stage ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie apenas os csproj (cache de restore)
COPY src/ProductApi/ProductApi.csproj src/ProductApi/
COPY src/ProductApplication/ProductApplication.csproj src/ProductApplication/
COPY src/ProductDomain/ProductDomain.csproj src/ProductDomain/
COPY src/ProductInfrastructure/ProductInfrastructure.csproj src/ProductInfrastructure/

# Restaure pelo projeto da API (não precisa dos testes)
RUN dotnet restore src/ProductApi/ProductApi.csproj

# Agora copie todo o código
COPY . .

# Publicar
RUN dotnet publish src/ProductApi/ProductApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# ====== Runtime stage ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductApi.dll"]
