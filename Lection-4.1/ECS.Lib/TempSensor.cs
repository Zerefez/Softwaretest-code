using System;

namespace ECS.Lib
{
    public class TempSensor: ITempSensor
    {
        private Random gen = new Random();

        public int GetTemp()
        {
            return gen.Next(-5, 45);
        }
    }
}