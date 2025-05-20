#include <iostream>
#include <string>
#include <sstream>
#include <curl/curl.h>
//#include <nlohmann/json.hpp> // 用於 JSON 處理（需安裝 json.hpp）
#include "json.hpp"

using json = nlohmann::json;
using namespace std;

const string BASE_URL = "https://localhost:5001/api/diaries";

// 回傳 body 資料用的 callback
static size_t WriteCallback(void* contents, size_t size, size_t nmemb, string* output) {
    size_t totalSize = size * nmemb;
    output->append((char*)contents, totalSize);
    return totalSize;
}

// 建立日誌
string createDiary() {
    CURL* curl = curl_easy_init();
    string responseStr;

    if (curl) {
        json data = {
            {"title", "C++ 測試日誌"},
            {"content", "這是從 C++ 傳送的內容。"},
            {"date", "2025-05-12T10:00:00"},
            {"tags", {"C++", "REST"}}
        };

        string jsonStr = data.dump();

        struct curl_slist* headers = nullptr;
        headers = curl_slist_append(headers, "Content-Type: application/json");

        curl_easy_setopt(curl, CURLOPT_URL, BASE_URL.c_str());
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, jsonStr.c_str());
        curl_easy_setopt(curl, CURLOPT_HTTPHEADER, headers);
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &responseStr);
        curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, 0L); // 忽略 HTTPS 憑證驗證（開發用）

        CURLcode res = curl_easy_perform(curl);
        if (res != CURLE_OK)
            cerr << "❌ curl_easy_perform() failed: " << curl_easy_strerror(res) << endl;

        curl_easy_cleanup(curl);
        curl_slist_free_all(headers);
    }

    return responseStr;
}

// 查詢所有日誌
string getAllDiaries() {
    CURL* curl = curl_easy_init();
    string responseStr;

    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, BASE_URL.c_str());
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &responseStr);
        curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, 0L); // 忽略 HTTPS 憑證驗證

        CURLcode res = curl_easy_perform(curl);
        if (res != CURLE_OK)
            cerr << "❌ curl_easy_perform() failed: " << curl_easy_strerror(res) << endl;

        curl_easy_cleanup(curl);
    }

    return responseStr;
}

int main() {
	CURL* curl = curl_easy_init();
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, "https://localhost:5001/api/diaries");

        // 加上這兩行讓 curl 接受本機的自簽憑證
        curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, 0L);
        curl_easy_setopt(curl, CURLOPT_SSL_VERIFYHOST, 0L);

        CURLcode res = curl_easy_perform(curl);

        if (res != CURLE_OK) {
            std::cerr << "❌ curl_easy_perform() failed: " << curl_easy_strerror(res) << std::endl;
        } else {
            std::cout << "✅ 成功連接 API！" << std::endl;
        }

        curl_easy_cleanup(curl);
    }
	/*
    cout << "📨 建立日誌中...\n";
    string createRes = createDiary();
    cout << "✅ 建立結果：" << createRes << "\n\n";

    cout << "📚 查詢所有日誌...\n";
    string allRes = getAllDiaries();
    cout << "✅ 所有日誌：" << allRes << "\n";
*/
    return 0;
}
