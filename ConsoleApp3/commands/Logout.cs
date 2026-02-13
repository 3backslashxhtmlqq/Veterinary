using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class Logout
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        await botClient.SendMessage(
            update.Message.Chat.Id,
            "Вы вышли из аккаунта.",
            cancellationToken: token
        );
    }
}