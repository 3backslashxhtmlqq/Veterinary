using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp3.commands;

public static class Registration
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
        {
            return;
        }
        
        var message = update.Message;
        var userId = message.From!.Id;

        Directory.CreateDirectory("data/temp_users");
        string path = $"data/temp_users/{userId}.txt";

        if (File.Exists(path))
        {
            await botClient.SendMessage(
                message.Chat.Id,
                text: "Вы уже зарегистрировались! Вам осталось только авторизоваться",
                cancellationToken: cancellationToken
                );
            return;
        }

        await botClient.SendMessage(
            message.Chat.Id,
            text: "Введите ФИО:",
            cancellationToken: cancellationToken
        );
        
        File.WriteAllLines(path, new[]
        {
            $"ФИО: Ожидается",
            $"Роль: Не подтверждена",
            $"Дата регистрации: {DateTime.Now:dd.MM.yyyy}",
            $"Количество записей: 0",
            $"Услуги: Нет",
            $"Карточки питомцев: 0"
        });

    }
}