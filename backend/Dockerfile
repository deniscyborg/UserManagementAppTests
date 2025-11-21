# Используем официальный .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем проект (*.csproj) и восстанавливаем зависимости
COPY backend.csproj ./
RUN dotnet restore

# Копируем все остальные исходники и публикуем
COPY . ./
RUN dotnet publish -c Release -o out

# Используем рантайм для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "backend.dll"]
