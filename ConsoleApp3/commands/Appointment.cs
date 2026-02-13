using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class Appointment
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (!Helper.IsClient(update.Message.From!.Id))
        {
            return;
        }
        
        Directory.CreateDirectory("data/appointments");
        string path = $"data/appointments/{update.Message.From.Id}_{DateTime.Now.Ticks}.txt";
        File.WriteAllLines(path, new[]
        {
            $"Клиент: {update.Message.From.Id}",
            $"Статус: Ожидание",
            $"Описание: {update.Message.Text}"
        });

        await botClient.SendMessage(
            update.Message.Chat.Id,
            "Запись создана.",
            cancellationToken: cancellationToken
        );
    }
}