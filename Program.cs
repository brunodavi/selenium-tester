using OpenQA.Selenium;
using SeleniumTester;

var driverManager = new FirefoxDriverManager();
IWebDriver driver;

try
{
    driver = driverManager.Initialize();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("----------------------------------------------------------");
    Console.WriteLine(ex.Message);
    Console.WriteLine("----------------------------------------------------------");
    Console.ReadKey();
    return;
}

try
{
    Console.WriteLine("Navegando para o Google...");
    driver.Navigate().GoToUrl("https://www.google.com");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao navegar: {ex.Message}");
}

Console.WriteLine("\nPressione qualquer tecla para fechar o navegador...");
Console.ReadKey();
driverManager.Quit();