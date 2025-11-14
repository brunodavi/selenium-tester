using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTester;

public class FirefoxDriverManager
{
    private IWebDriver? _driver;

    /// <summary>
    /// Inicializa o Firefox WebDriver com o perfil padrão do usuário
    /// </summary>
    /// <returns>Instância do WebDriver configurada</returns>
    /// <exception cref="InvalidOperationException">Quando não é possível encontrar ou inicializar o perfil</exception>
    public IWebDriver Initialize()
    {
        string profilePath = GetFirefoxProfilePath();
        
        Console.WriteLine($"Usando perfil em: {profilePath}");
        Console.WriteLine("Iniciando o Firefox... (Lembre-se de fechar todas as instâncias!)");

        var options = new FirefoxOptions();
        options.AddArgument("-profile");
        options.AddArgument(profilePath);

        try
        {
            _driver = new FirefoxDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Console.WriteLine("Driver iniciado com sucesso!");
            return _driver;
        }
        catch (WebDriverException ex)
        {
            throw new InvalidOperationException(
                "ERRO AO INICIAR O DRIVER: " + ex.Message +
                "\n\nLembre-se: Você DEVE fechar TODAS as janelas e processos do Firefox (firefox.exe) no Gerenciador de Tarefas antes de rodar.",
                ex
            );
        }
    }

    /// <summary>
    /// Obtém o caminho do perfil padrão do Firefox
    /// </summary>
    private string GetFirefoxProfilePath()
    {
        string? appDataPath = Environment.GetEnvironmentVariable("APPDATA");

        if (string.IsNullOrEmpty(appDataPath))
        {
            throw new InvalidOperationException("ERRO CRÍTICO: Não foi possível encontrar a variável de ambiente APPDATA.");
        }

        string profilesDirectory = Path.Combine(appDataPath, @"Mozilla\Firefox\Profiles");

        if (!Directory.Exists(profilesDirectory))
        {
            throw new InvalidOperationException($"ERRO: A pasta de perfis do Firefox não foi encontrada em: {profilesDirectory}");
        }

        try
        {
            // Tenta encontrar o perfil ".default-release"
            var releaseProfiles = Directory.GetDirectories(profilesDirectory, "*.default-release");

            if (releaseProfiles.Length == 1)
            {
                Console.WriteLine("Perfil '.default-release' encontrado automaticamente.");
                return releaseProfiles[0];
            }

            if (releaseProfiles.Length > 1)
            {
                throw new InvalidOperationException("ERRO: Múltiplos perfis '.default-release' encontrados. Não foi possível decidir qual usar.");
            }

            // Se não encontrar 'default-release', tenta o '.default'
            var defaultProfiles = Directory.GetDirectories(profilesDirectory, "*.default");
            if (defaultProfiles.Length == 1)
            {
                Console.WriteLine("Perfil '.default' (antigo) encontrado automaticamente.");
                return defaultProfiles[0];
            }

            throw new InvalidOperationException(
                $"ERRO: Não foi possível encontrar um perfil '*default-release' ou '*.default' em {profilesDirectory}\n" +
                "Por favor, verifique sua pasta de perfis."
            );
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            throw new InvalidOperationException($"ERRO ao acessar a pasta de perfis: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Finaliza o driver e fecha o navegador
    /// </summary>
    public void Quit()
    {
        _driver?.Quit();
        _driver = null;
    }
}
