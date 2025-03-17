namespace ECS.Lib;

public interface IHeater
{
    public void TurnOn();
    public void TurnOff();
    public bool RunSelfTest();
}