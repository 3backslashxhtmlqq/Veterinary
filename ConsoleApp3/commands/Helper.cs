namespace ConsoleApp3.commands;

public static class Helper
{
    public static bool IsAdmin(long userId) => File.Exists($"data/admins/{userId}.txt");
    public static bool IsClient(long userId) => File.Exists($"data/clients/{userId}.txt");
    public static bool IsDoctor(long userId) => File.Exists($"data/doctors/{userId}.txt");
}