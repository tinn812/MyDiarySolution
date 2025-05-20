import requests
import json
from datetime import datetime

# API 伺服器網址（依實際狀況使用 http 或 https）
BASE_URL = "https://localhost:5001/api/diaries"
# 如果使用 http://localhost:5000 請改 BASE_URL
HEADERS = {
    "Content-Type": "application/json"
}

# 建立一篇新的日誌
def create_diary():
    diary_data = {
        "title": "從 Python 建立的日誌",
        "content": "這是來自 Python 的內容。",
        "date": datetime.now().isoformat(),
        "tags": ["Python", "API", "測試"]
    }

    try:
        response = requests.post(BASE_URL, data=json.dumps(diary_data), headers=HEADERS, verify=False) # 如果使用自簽憑證，跳過 HTTPS 驗證（僅限開發環境） verify=False 是為了繞過 HTTPS 自簽名憑證錯誤（開發時可以用，正式環境請改成 verify=True 或提供 CA 憑證）。
        if response.status_code in (200, 201):
            print("✅ 成功建立日誌！")
            print("🔎 回傳內容：", response.json())
            return response.json()["id"]  # 回傳 ID 以便查詢
        else:
            print("❌ 建立失敗，狀態碼：", response.status_code)
            print(response.text)
            return None
    except Exception as e:
        print("⚠️ 發生錯誤：", e)
        return None

# 查詢特定 ID 的日誌
def get_diary(diary_id):
    try:
        response = requests.get(f"{BASE_URL}/{diary_id}", headers=HEADERS, verify=False)
        if response.status_code == 200:
            diary = response.json()
            print("📘 查詢成功：")
            print("標題：", diary["title"])
            print("日期：", diary["date"])
            print("內容：", diary["content"])
            print("標籤：", ", ".join(diary["tags"]))
        else:
            print("❌ 查詢失敗，狀態碼：", response.status_code)
            print(response.text)
    except Exception as e:
        print("⚠️ 發生錯誤：", e)

# 查詢所有日誌
def get_all_diaries():
    try:
        response = requests.get(BASE_URL, headers=HEADERS, verify=False)
        if response.status_code == 200:
            diaries = response.json()
            print(f"\n📚 所有日誌（共 {len(diaries)} 筆）：")
            for d in diaries:
                print(f"- [{d['date']}] {d['title']} - 標籤：{', '.join(d['tags'])}")
        else:
            print("❌ 查詢失敗：", response.status_code)
            print(response.text)
    except Exception as e:
        print("⚠️ 發生錯誤：", e)

# 測試流程：建立、查詢單篇、查詢所有

# 測試流程：先建立再查詢
if __name__ == "__main__":
    new_id = create_diary()
    if new_id:
        get_diary(new_id)
    get_all_diaries()