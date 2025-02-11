using System;

namespace ECS.Lib
{
    public class EcsController
    {
        private int _threshold;
        private readonly TempSensor _tempSensor;
        private readonly Heater _heater;

        public EcsController(int thr)
        {
            SetThreshold(thr);
            _tempSensor = new TempSensor();
            _heater = new Heater();
        }

        public void Regulate()
        {
            var t = _tempSensor.GetTemp();
            Console.WriteLine($"Temperature measured was {t}");
            if (t < _threshold)
                _heater.TurnOn();
            else
                _heater.TurnOff();

        }

        public void SetThreshold(int thr)
        {
            _threshold = thr;
        }

        public int GetThreshold()
        {
            return _threshold;
        }

        public int GetCurTemp()
        {
            return _tempSensor.GetTemp();
        }

        public bool RunSelfTest()
        {
            return _tempSensor.RunSelfTest() && _heater.RunSelfTest();
        }
    }
}
