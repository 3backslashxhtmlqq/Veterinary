using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class Services
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        string path = "data/services/services.txt";
        if (!File.Exists(path))
        {
            Directory.CreateDirectory("data/services");
            File.WriteAllText(path,
                "1. Осмотр\n2. Вакцинация\n3. Чипирование");
        }

        await botClient.SendMessage(
            update.Message.Chat.Id,
            File.ReadAllText(path),
            cancellationToken: cancellationToken
        );
    }
}