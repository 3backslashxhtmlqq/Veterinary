using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class Administration
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var msg = update.Message!;
        var adminId = msg.From!.Id;
        
        if (!Helper.IsAdmin(adminId))
            return;

        var parts = msg.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) return;

        string command = parts[0].ToLower();
        if (!long.TryParse(parts[1], out long userId)) return;

        string tempFile = $"data/temp_users/{userId}.txt";
        string clientFile = $"data/clients/{userId}.txt";
        Directory.CreateDirectory("data/clients");

        switch (command)
        {
            case "/accept":
                if (File.Exists(tempFile))
                {
                    File.Move(tempFile, clientFile, true);
                    await botClient.SendMessage(
                        userId,
                        "Администратор подтвердил ваш запрос. Вам выдана роль клиента.",
                        cancellationToken: cancellationToken
                    );
                }
                break;

            case "/decline":
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    await botClient.SendMessage(
                        userId,
                        "Ваш запрос отклонен администратором.",
                        cancellationToken: cancellationToken
                    );
                }
                break;
        }
        
        await botClient.SendMessage(
            adminId,
            $"Команда {command} для пользователя {userId} выполнена.",
            cancellationToken: cancellationToken
        );
    }
}