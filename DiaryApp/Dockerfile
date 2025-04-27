# 使用官方的 .NET 8.0 SDK 做 build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# 複製 csproj 檔案並還原依賴
COPY *.csproj ./
RUN dotnet restore

# 複製其他全部檔案
COPY . ./

# build 出 release 版
RUN dotnet publish -c Release -o out

# ----------------------------------------------------------

# 使用官方的 .NET 8.0 Runtime 映像檔跑 App
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# 從 build 階段複製 publish 出來的成果
COPY --from=build /app/out .

# 預設執行這個應用程式
ENTRYPOINT ["dotnet", "DiaryApp.dll"]
