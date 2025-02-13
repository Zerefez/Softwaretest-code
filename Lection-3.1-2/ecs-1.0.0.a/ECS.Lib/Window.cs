namespace ECS.Lib;

public class Window: IWindow
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