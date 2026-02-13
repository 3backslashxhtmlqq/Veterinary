using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConsoleApp3.commands;

public static class DoctorHandle
{
    public static async Task UpdaterHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (!Helper.IsClient(update.Message.From!.Id))
        {
            return;
        }
        
        string password = File.ReadAllText("data/password/doctor_password.txt").Trim();
        if (update.Message.Text == password)
        {
            foreach (var admin in Directory.GetFiles("data/admins"))
            {
                var adminId = Path.GetFileNameWithoutExtension(admin);
                await botClient.SendMessage(
                    adminId,
                    $"Пользователь {update.Message.From.Id} хочет получить роль Врач.\n" +
                    $"Введите: /acceptdoc {update.Message.From.Id}",
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
}