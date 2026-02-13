using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class Authorization
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var userId = update.Message.From!.Id;
        string temp = $"data/temp_users/{userId}.txt";

        if (!File.Exists(temp))
        {
            await botClient.SendMessage(
                update.Message.Chat.Id,
                "Вы не зарегистрированы.",
                cancellationToken: cancellationToken
            );
            return;
        }
        
        string adminsPath = "data/admins.txt";

        if (!File.Exists(adminsPath))
        {
            await botClient.SendMessage(
                update.Message.Chat.Id,
                "Список администраторов отсутствует.",
                cancellationToken: cancellationToken
            );
            return;
        }
        
        var adminIds = File.ReadAllLines(adminsPath)
            .Select(line => long.TryParse(line, out var id) ? id : 0)
            .Where(id => id != 0)
            .ToArray();

        foreach (var adminId in adminIds)
        {
            await botClient.SendMessage(
                adminId,
                $"Пользователь {userId} запрашивает роль Клиент.\n" +
                $"Введите: /accept {userId} или /decline {userId}",
                cancellationToken: cancellationToken
            );
        }

        await botClient.SendMessage(
            update.Message.Chat.Id,
            "Запрос отправлен администратору.",
            cancellationToken: cancellationToken
        );
    }
}