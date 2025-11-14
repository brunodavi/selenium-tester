# Selenium Tester

Aplicação console em C# para testes automatizados usando Selenium WebDriver com Firefox.

## Descrição

Este projeto facilita a execução de testes manuais em páginas web utilizando o Selenium. A aplicação carrega automaticamente o perfil padrão do Firefox do usuário, mantendo cookies, sessões e extensões.

## Requisitos

- .NET 9.0
- Firefox instalado
- Selenium.WebDriver (instalado via NuGet)

## Como usar

1. **Importante**: Feche todas as instâncias do Firefox antes de executar
2. Execute o projeto: `dotnet run`
3. A aplicação abrirá o Firefox com seu perfil padrão
4. Pressione qualquer tecla para finalizar

## Estrutura

- `Program.cs` - Ponto de entrada da aplicação e execução dos testes
- `FirefoxDriverManager.cs` - Classe responsável pela inicialização e gerenciamento do WebDriver

## Observações

O perfil do Firefox só pode ser usado por uma instância por vez. Certifique-se de fechar o navegador pelo Gerenciador de Tarefas se necessário.
