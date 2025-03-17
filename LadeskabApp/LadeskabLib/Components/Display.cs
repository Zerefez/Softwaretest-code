namespace LadeskabLib;

public class Display : IDisplay
{
    public void ShowUserMessage(string message)
    {
        Console.WriteLine($"Bruger Message: {message}");
    }

    public void ShowChargingMessage(string message)
    {
        Console.WriteLine($"Opladning Status: {message}");
    }
}
