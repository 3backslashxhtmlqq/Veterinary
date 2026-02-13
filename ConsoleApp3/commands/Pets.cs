using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp3.commands;

public static class Pets
{
    public static async Task UpdaterHandler(ITelegramBotClient bot, Message msg)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("Добавить питомца"), new KeyboardButton("Удалить питомца") },
            new[] { new KeyboardButton("Список питомцев"), new KeyboardButton("Назад") }
        })
        { ResizeKeyboard = true };

        await bot.SendMessage(msg.Chat.Id, "Меню питомцев:", replyMarkup: keyboard);
    }

    public static async Task Add(ITelegramBotClient bot, Message msg)
    {
        if (!Helper.IsClient(msg.From!.Id))
        {
            await bot.SendMessage(msg.Chat.Id, "Вы не авторизованы для добавления питомцев.");
            return;
        }

        Directory.CreateDirectory("data/pets");
        string path = $"data/pets/{msg.From.Id}_{DateTime.Now.Ticks}.txt";
        File.WriteAllText(path, msg.Text!);

        await bot.SendMessage(msg.Chat.Id, "Карточка питомца добавлена.");
    }

    public static async Task Delete(ITelegramBotClient bot, Message msg)
    {
        var files = Directory.GetFiles("data/pets")
            .Where(f => f.Contains(msg.From!.Id.ToString()));

        int count = 0;
        foreach (var file in files)
        {
            File.Delete(file);
            count++;
        }

        string response = count > 0 ? "Все карточки питомцев удалены." : "У вас нет питомцев для удаления.";
        await bot.SendMessage(msg.Chat.Id, response);
    }

    public static async Task List(ITelegramBotClient bot, Message msg)
    {
        var files = Directory.GetFiles("data/pets")
            .Where(f => f.Contains(msg.From!.Id.ToString()))
            .ToArray();

        if (files.Length == 0)
        {
            await bot.SendMessage(msg.Chat.Id, "У вас нет питомцев.");
            return;
        }

        string text = "Ваши питомцы:\n" +
            string.Join("\n", files.Select(f => Path.GetFileNameWithoutExtension(f)));

        await bot.SendMessage(msg.Chat.Id, text);
    }
}