using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using NUnit.Framework;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{   
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("User API")]
    public class ApiUserTests
    {
        private HttpClient _client;
        private TestConfig _config;

        [SetUp]
        public void SetUp()
        {
            _config = TestConfig.Load();
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri(_config.ApiBaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }

        [Test]
        [AllureTag("api", "smoke", "add-user")]           // <--- Теги для фильтрации
        [AllureSeverity(SeverityLevel.critical)]          // <--- Важность теста
        [AllureOwner("denis")]                            // <--- Автор теста (дополнительно)
        [AllureStory("Create user via API")]              // <--- Описание тест-сценария
        public async Task CanAddUserViaApi()
        {
            var user = new { name = "APIТест", surname = "Тестов", email = "apitest@test.ru" };
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/users", content);

            Assert.IsTrue(response.IsSuccessStatusCode, "Создание пользователя по API неуспешно: " + response.StatusCode);
            var responseData = await response.Content.ReadAsStringAsync();
        }
    }
}
