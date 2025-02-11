namespace ECS.Lib.MockData;

public class FakeTempSensor : ITempSensor
{
    private Random gen = new Random();

        public int GetTemp()
        {
            return gen.Next(-5, 45);
        }
}