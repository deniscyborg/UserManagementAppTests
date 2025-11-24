
# UserManagementApp Автотесты

![Build Status](https://github.com/deniscyborg/UserManagementAppTests/actions/workflows/dotnet-allure.yml/badge.svg)

**Приложение**: [http://usermanagementapp.com/](http://usermanagementapp.com/)  
**Отчет Allure**: [http://report.usermanagementapp.com/](http://report.usermanagementapp.com/)

## 1. Структура проекта

<img width="723" height="629" alt="image" src="https://github.com/user-attachments/assets/89e0feb9-d53a-4e1c-82b9-a18a1889a61b" />


**PageObjects/** — классы для управления UI (Playwright)  
**Tests/** — классы тестов (UI и API)  
**appsettings.json** — хранит BASE_URL, BROWSER, REMOTE  
**TestConfig.cs** — загрузка переменных окружения  
**README.md** — краткая документация  
**Tests.csproj** — проект автотестов
---
## 2. Page Object — минимальный пример

// tests/PageObjects/UserGridPage.cs
using Microsoft.Playwright;

public class UserGridPage
{
private readonly IPage Page;

public UserGridPage(IPage page) => Page = page;
public async Task OpenAsync() => await Page.GotoAsync("users");
public async Task<string[]> GetUserNamesAsync()
{
    // Используй актуальный селектор для таблицы
    return await Page.Locator(".user-row .user-name").AllInnerTextsAsync();
}
}


---

## 3. Инструкции запуска

- **Зависимости:**
  - .NET 8 SDK
  - Playwright (`playwright install` автоматически)

- **Переменные окружения:**
  - `BASE_URL` — адрес фронта (например, http://localhost:5173)
  - `BROWSER` — browser: chromium / firefox / webkit
  - `REMOTE` — адрес Selenium Grid (опционально)

  _Рекомендуется задавать через `appsettings.json` или ENV._

- **Запуск тестов:**
dotnet test
Для отдельного теста:
dotnet test --filter "FullyQualifiedName~Tests.ApiUserTests"


---

## 4. Краткий план автоматизации

- API-тесты: создание, чтение, изменение, удаление пользователя (CRUD)
- UI-тесты: добавление пользователя через форму, проверка отображения в таблице

---

## 5. Базовые метрики

- Процент прохождения тестов
- Количество ошибок по категориям (функционал, валидация, UI, интеграция)
- Отчёты Allure (интеграция с CI/CD)

_Автоматизация позволяет быстро выявлять ошибки, отслеживать работу форм и API, контролировать стабильность на всех конфигурациях._

---

