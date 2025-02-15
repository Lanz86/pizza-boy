
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernel.PizzaBoy.Plugins;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Assistant> Prenota la tua pizza! ");

var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

string credential = configuration["OpenAI:Credential"]!;

var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion("gpt-4o", credential);
builder.Services.AddLogging(service => service.AddConsole().SetMinimumLevel(LogLevel.Trace));

Kernel kernel = builder.Build();
kernel.Plugins.AddFromType<PizzaPlugin>("Pizzas");
var executionSettings = new OpenAIPromptExecutionSettings {
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
};
var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
string? userInput = string.Empty;
do {
    Console.Write("User> ");
    userInput = Console.ReadLine();
    if(userInput != null) {
        history.AddUserMessage(userInput);
        var response = await chatCompletion.GetChatMessageContentAsync(
            history, 
            executionSettings,
            kernel: kernel
        );

        Console.WriteLine($"Assistant > {response}");
        history.AddMessage(response.Role, response.InnerContent!.ToString() ?? string.Empty);

    }

} while(userInput is not null);