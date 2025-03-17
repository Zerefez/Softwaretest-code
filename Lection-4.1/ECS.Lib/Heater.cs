namespace ECS.Lib
{
    public class Heater : IHeater
    {
        
        public bool SelfTestResult { get; set; } = true;

        public void TurnOn()
        {
            System.Console.WriteLine("Heater is on");
        }

        public void TurnOff()
        {
            System.Console.WriteLine("Heater is off");
        }

        public bool RunSelfTest()
        {
            return  SelfTestResult;
        }

    }
}