using System.Collections.Concurrent;
using ConsoleApp3.commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp3;

public class Program
{
    private static readonly string Token = "8233498544:AAE298pKcE6VwdzbqXoM93UJJ5IVmMo3tj4";
    private static TelegramBotClient _botClient;
    
    private static ConcurrentDictionary<long, string> userStates = new();

    public static async Task Main(string[] args)
    {
        _botClient = new TelegramBotClient(Token);
        using var cts = new CancellationTokenSource();
        _botClient.StartReceiving(
            UpdaterHandler,
            ErrorHandler,
            cancellationToken: cts.Token);
        
        Console.WriteLine("Бот запущен");
        Console.ReadLine();
        cts.Cancel();
    }

    static async Task UpdaterHandler(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
            return;

        var msg = update.Message;
        var text = msg.Text;

        var mainKeyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Регистрация"), new KeyboardButton("Авторизация") },
            new[] { new KeyboardButton("Профиль"), new KeyboardButton("Питомцы") },
            new[] { new KeyboardButton("Запись на прием"), new KeyboardButton("Услуги") },
            new[] { new KeyboardButton("Выход") }
        })
        { ResizeKeyboard = true };
        
        if (userStates.TryGetValue(msg.From!.Id, out var state))
        {
            if (state == "adding_pet")
            {
                await Pets.Add(botClient, msg);
                userStates.TryRemove(msg.From!.Id, out _);
                return;
            }

            if (state == "registering")
            {
                await Registration.UpdaterHandler(botClient, update, ct);
                userStates.TryRemove(msg.From!.Id, out _);
                return;
            }

            if (state == "auth")
            {
                await Authorization.UpdaterHandler(botClient, update, ct);
                userStates.TryRemove(msg.From!.Id, out _);
                return;
            }
        }

        if (text.StartsWith("/accept") || text.StartsWith("/decline"))
        {
            await Administration.UpdaterHandler(botClient, update, ct);
            return;
        }

        switch (text)
        {
            case "/start":
                await botClient.SendMessage(msg.Chat.Id, "Главное меню", replyMarkup: mainKeyboard);
                break;

            case "Регистрация":
            case "/register":
                userStates[msg.From!.Id] = "registering";
                await Registration.UpdaterHandler(botClient, update, ct);
                break;

            case "Авторизация":
            case "/login":
                userStates[msg.From!.Id] = "login";
                await Authorization.UpdaterHandler(botClient, update, ct);
                break;

            case "Профиль":
            case "/profile":
                await Profile.UpdaterHandler(botClient, update, ct);
                break;

            case "Питомцы":
            case "/pets":
                await Pets.UpdaterHandler(botClient, msg);
                break;

            case "Добавить питомца":
                userStates[msg.From!.Id] = "adding_pet";
                await botClient.SendMessage(msg.Chat.Id, "Отправьте данные о питомце:");
                break;

            case "Удалить питомца":
                await Pets.Delete(botClient, msg);
                break;

            case "Список питомцев":
                await Pets.List(botClient, msg);
                break;

            case "Назад":
                await botClient.SendMessage(msg.Chat.Id, "Главное меню", replyMarkup: mainKeyboard);
                break;

            case "Запись на прием":
            case "/zapis":
                await Appointment.UpdaterHandler(botClient, update, ct);
                break;

            case "Услуги":
            case "/services":
                await Services.UpdaterHandler(botClient, update, ct);
                break;

            case "Выход":
            case "/logout":
                await Logout.UpdaterHandler(botClient, update, ct);
                break;

            default:
                await botClient.SendMessage(msg.Chat.Id, "Неизвестная команда. Выберите из меню.");
                break;
        }
    }

    public static Task ErrorHandler(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);
        return Task.CompletedTask;
    }
}