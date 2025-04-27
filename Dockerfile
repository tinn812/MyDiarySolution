# 使用官方的 .NET 8 SDK映像，來build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 複製csproj並還原相依套件
COPY DiaryApp/*.csproj ./DiaryApp/
RUN dotnet restore DiaryApp/DiaryApp.csproj

# 複製剩下的所有檔案
COPY . .

# build成發布版本
RUN dotnet publish DiaryApp/DiaryApp.csproj -c Release -o /app/publish

# 只留下小巧的runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 對外開放Port（很重要）
EXPOSE 8080

# 預設啟動
ENTRYPOINT ["dotnet", "DiaryApp.dll"]
