using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

// 1. Obter a variável de ambiente APPDATA (que aponta para a pasta Roaming)
string? appDataPath = Environment.GetEnvironmentVariable("APPDATA");

if (string.IsNullOrEmpty(appDataPath))
{
    Console.WriteLine("ERRO CRÍTICO: Não foi possível encontrar a variável de ambiente APPDATA.");
    return; // Encerra o programa
}

// 2. Montar o caminho para a pasta de perfis do Firefox
string profilesDirectory = Path.Combine(appDataPath, @"Mozilla\Firefox\Profiles");
string? profilePath; // Variável para armazenar o caminho que encontrarmos

if (!Directory.Exists(profilesDirectory))
{
    Console.WriteLine($"ERRO: A pasta de perfis do Firefox não foi encontrada em: {profilesDirectory}");
    return;
}

try
{
    // 3. Tentar encontrar o perfil ".default-release" automaticamente
    var releaseProfiles = Directory.GetDirectories(profilesDirectory, "*.default-release");

    if (releaseProfiles.Length == 1)
    {
        profilePath = releaseProfiles[0]; // Encontramos!
        Console.WriteLine("Perfil '.default-release' encontrado automaticamente.");
    }
    else if (releaseProfiles.Length > 1)
    {
        Console.WriteLine("ERRO: Múltiplos perfis '.default-release' encontrados. Não foi possível decidir qual usar.");
        return;
    }
    else
    {
        // Se não achar o 'default-release', tenta o '.default' (mais antigo)
        var defaultProfiles = Directory.GetDirectories(profilesDirectory, "*.default");
        if (defaultProfiles.Length == 1)
        {
            profilePath = defaultProfiles[0];
            Console.WriteLine("Perfil '.default' (antigo) encontrado automaticamente.");
        }
        else
        {
            Console.WriteLine($"ERRO: Não foi possível encontrar um perfil '*default-release' ou '*.default' em {profilesDirectory}");
            Console.WriteLine("Por favor, verifique sua pasta de perfis.");
            return;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO ao acessar a pasta de perfis: {ex.Message}");
    return;
}


// --- O restante do script ---

Console.WriteLine($"Usando perfil em: {profilePath}");
Console.WriteLine("Iniciando o Firefox... (Lembre-se de fechar todas as instâncias!)");

// 4. Crie as opções do Firefox
var options = new FirefoxOptions();

// 5. Adiciona os argumentos para carregar o perfil "snapshot"
options.AddArgument("-profile");
options.AddArgument(profilePath); // Usa o caminho que encontramos

IWebDriver driver;
try
{
    // 6. Inicie o driver do Firefox
    // Lembre-se: Feche o Firefox no Gerenciador de Tarefas!
    driver = new FirefoxDriver(options);
}
catch (WebDriverException ex)
{
    Console.WriteLine("----------------------------------------------------------");
    Console.WriteLine("ERRO AO INICIAR O DRIVER: " + ex.Message);
    Console.WriteLine("\nLembre-se: Você DEVE fechar TODAS as janelas e processos do Firefox (firefox.exe) no Gerenciador de Tarefas antes de rodar.");
    Console.WriteLine("----------------------------------------------------------");
    Console.ReadKey(); // Pausa para ler o erro
    return;
}

// 7. (BOA PRÁTICA) Adiciona uma espera implícita
driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

try
{
    // 8. Navega para a página
    Console.WriteLine("Driver iniciado com sucesso! Navegando para o Google...");
    driver.Navigate().GoToUrl("https://www.google.com");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao navegar: {ex.Message}");
}

Console.WriteLine("Pressione qualquer tecla para fechar o navegador...");
Console.ReadKey(); // Pausa o console para você ver o navegador
driver.Quit(); // Fecha o navegador e o geckodriver