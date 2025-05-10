![.NET Core CI](https://github.com/tinn812/MyDiarySolution/actions/workflows/ci.yml/badge.svg)
這是一個 ASP.NET Core 製作的日誌系統，支援 PostgreSQL、SQLite、多標籤與圖片上傳...開發中(此為公開版)

# DIARYAPP
前往 http://localhost:5034/Diaries/Index 啟動網站

或 https://mydiarysolution.onrender.com 測試部署版本

測試API：

在瀏覽器輸入：`https://localhost:5001/api/diaries`

或 Swagger UI ：`https://localhost:5001/swagger`
查看所有 API 的路徑、方法以及測試

- 專案結構（簡略）

```
MyDiarySolution/
├── .github/workflows/ci.yml   # GitHub Actions 的設定檔
├── DiaryApp           # 主網站專案
│   ├── DiaryApp.csproj
│   └── ...
├── DiaryApp.Test/     # 測試專案
│   ├── DiaryApp.Tests.csproj
│   └── ...
├── MyDiarySolution.sln    # Solution
├── .env              # DATABASE_URL 讀取資料庫連線字串，可使用Render提供之PostgreSQL
├── Dockerfile        # Render 用 Dockerfile 來部署
├── .dockerignore
└── README.md       # 待補
```


.env 中 DATABASE_URL 決定使用資料庫

切換資料庫重建Migrations
```
dotnet ef migrations add InitialCreate
```
用 Local 的 SQLite 需
```
 dotnet ef database update
```
！Render 的 sql 不執行此行


pass
## Description 
pass


## Model 規劃

五個 `Models` 建立日誌系統的**資料結構與資料處理邏輯**

### 1. **AppDbContext.cs**
跟資料庫溝通的主要橋樑。

- 管理資料表的 CRUD 操作。
- 定義有哪些資料表（像 Diaries、Tags 等）。
- 是 Entity Framework Core 的 `DbContext` 衍生類別。


### 2. **Diary.cs**
**日誌的主資料模型（資料表）**  
每一篇日誌都對應到這個類別。

- 包含：標題、內容、日期、圖片路徑等等。
- 是資料庫的主資料表之一。
- 跟 Tag 有多對多關係（透過 DiaryTag）。


### 3. **DiaryTag.cs**
**日誌與標籤的「關聯表」**  
這是多對多關係的橋樑表。

- 一篇日誌可以有多個標籤。
- 一個標籤也可以被多篇日誌共用。
- 這個表記錄每篇日誌和每個標籤的配對。


### 4. **Tag.cs**
**標籤的資料模型**  
每個標籤（如「心情」、「工作」）都對應這個表。

- 記錄標籤名稱、使用次數等。
- 跟 Diary 有多對多關係。


### 5. **DiaryViewModel.cs**
**前端編輯畫面的專用 ViewModel**  
不是資料表，而是**給畫面（View）用的資料結構**。

- 用在像編輯頁或建立頁。
- 會整合 Diary 的基本資訊 + 傳上來的圖片檔案。
- 通常還會做一些格式處理或驗證。


---

### 總結表 

| Model             | 用途                           | 是否對應資料表 | 額外說明                     |
|------------------|--------------------------------|----------------|------------------------------|
| `AppDbContext.cs` | 資料庫上下文（橋樑）            | ❌             | 提供資料存取 API             |
| `Diary.cs`        | 日誌主資料                      | ✅             | 有標題、內容、圖片等欄位     |
| `Tag.cs`          | 標籤資料                        | ✅             | 每個標籤只出現一次           |
| `DiaryTag.cs`     | 日誌與標籤的中介表              | ✅             | 建立多對多關聯               |
| `DiaryViewModel.cs` | 畫面用的模型（表單處理、上傳圖） | ❌             | 不會存到資料庫               |

---
