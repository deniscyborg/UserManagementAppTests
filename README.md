UserManagementApp Автотесты
1. Структура проекта автотестов
text
tests/
├── PageObjects/         # Page Object-модели для UI-страниц (пример: UserGridPage)
├── Tests/               # Тестовые классы (UI и API)
├── appsettings.json     # Конфиг с переменными окружения для тестов
├── TestConfig.cs        # Загрузка env-переменных и конфиг
├── README.md            # Описание и инструкции запуска
├── tests.csproj         # Проект автотестов (C#)
PageObjects/ — содержит классы, реализующие работу с конкретными страницами.

Tests/ — классы автотестов: API и UI.

appsettings.json — факультативный: хранит значения BASE_URL, BROWSER, REMOTE.

TestConfig.cs — логика загрузки переменных окружения/config.

README.md — краткая документация.

tests.csproj — C# проект автотестов.

2. Page Object — минимальный пример

// PageObjects/UserGridPage.cs
using Microsoft.Playwright;

public class UserGridPage
{
    private readonly IPage Page;

    public UserGridPage(IPage page) => Page = page;

    public async Task OpenAsync() => await Page.GotoAsync("users");

    public async Task<string[]> GetUserNamesAsync()
    {
        // Селектор фиксируй по реальному HTML!
        return await Page.Locator(".user-row .user-name").AllInnerTextsAsync();
    }
}


3. Инструкции запуска
	1. Установить зависимости:

.NET 8 SDK

Playwright (Автоматически ставится при первом запуске тестов: playwright install)

	2. Требуемые переменные окружения:

BASE_URL — адрес Frontend-сервера, например: http://localhost:5173

BROWSER — браузер для запуска тестов (chromium / firefox / webkit)

REMOTE — (опционально) URL Selenium Grid/удалённого раннера

Можно задать значения через appsettings.json или через переменные окружения.

3. Запуск тестов:

dotnet test

Для отдельных тестов:


dotnet test --filter "FullyQualifiedName~Tests.ApiUserTests"

4. Краткий план автоматизации

В первую очередь покрываются:

API-тесты: создание, чтение, изменение и удаление пользователя (CRUD).

UI-тесты: добавление пользователя через форму, проверка отображения в таблице.

Базовые метрики:

Процент прохождения тестов

Кол-во ошибок по категориям (функционал/валидация/UI/интеграция)

Allure Report или аналогичный отчет (можно интегрировать с CI/CD)

Автоматизация позволит оперативно выявлять ошибки, проверять работу пользовательской формы и API, отслеживать стабильность приложения на различных конфигурациях.