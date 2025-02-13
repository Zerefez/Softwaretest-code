using System;

namespace ECS.Lib
{
   public class EcsController
   {
      private int _windowThreshold;
      private int _threshold;
      private readonly ITempSensor _tempSensor;
      private readonly IHeater _heater;
      private readonly IWindow _window;

      public EcsController(int thr, int windowThr ,ITempSensor tempSensor, IHeater heater, IWindow window)
      {
         SetThreshold(thr);
         _tempSensor = tempSensor;
         _heater = heater;
         _windowThreshold = windowThr;
         _window = window;
      }

      public void Regulate()
      {
         var t = _tempSensor.GetTemp();
         Console.WriteLine($"Temperature measured was {t}");
         if (t < _threshold)
            _heater.TurnOn();
         else
            _heater.TurnOff();
         
         if (t > _windowThreshold)
            _window.Open();
         else
            _window.Close();
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
   }
}
