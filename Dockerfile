# 使用官方 ASP.NET Core Runtime 作為基底映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# 使用官方 .NET SDK 建構映像
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 把專案檔複製進來
COPY DiaryApp/*.csproj DiaryApp/
RUN dotnet restore DiaryApp/DiaryApp.csproj

# 複製整個專案並建置
COPY . .
WORKDIR /src/DiaryApp
RUN dotnet publish -c Release -o /app/publish

# 最後建立正式執行映像
FROM base AS final
WORKDIR /app

# 將編譯好的檔案複製進來
COPY --from=build /app/publish .

# 🔥 如果本地有 .env，需要複製進容器（Render不需要，自己設好環境變數）
# COPY .env .env

ENTRYPOINT ["dotnet", "DiaryApp.dll"]
