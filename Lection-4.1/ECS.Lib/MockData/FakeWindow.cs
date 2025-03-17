namespace ECS.Lib.MockData;

public class FakeWindow : IWindow
{
    public void Open()
    {
        Console.WriteLine("Window is open");
    }

    public void Close()
    {
        Console.WriteLine("Window is closed");
    }
}