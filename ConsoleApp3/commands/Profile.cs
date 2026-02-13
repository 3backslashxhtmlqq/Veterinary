using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class Profile
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        string client = $"data/clients/{update.Message.From!.Id}.txt";
        string doctor = $"data/doctors/{update.Message.From.Id}.txt";

        if (File.Exists(client))
        {
            await botClient.SendMessage(
                update.Message.Chat.Id,
                File.ReadAllText(client),
                cancellationToken: cancellationToken
            );
        } else if (File.Exists(doctor))
        {
            await botClient.SendMessage(
                update.Message.Chat.Id,
                File.ReadAllText(doctor),
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await botClient.SendMessage(
                update.Message.Chat.Id,
                "Профиль не найден.",
                cancellationToken: cancellationToken
            );
        }
    }
}