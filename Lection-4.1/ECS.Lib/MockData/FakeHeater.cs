namespace ECS.Lib.MockData;

public class FakeHeater : IHeater
{
    public void TurnOn()
        {
            System.Console.WriteLine("Heater is on");
        }

    public void TurnOff()
    {
        System.Console.WriteLine("Heater is off");
    }
}