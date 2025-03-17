
using ECS.Lib.MockData;

namespace  ECS.Lib
{
    public class StubHeater : IHeater
    {
        private bool _result;

        public StubHeater(bool initialResult)
        {
            _result = initialResult;
        }

        public void SetSelfTestResult(bool result)
        {
            _result = result;
        }

        public void TurnOn()
        {
            Console.WriteLine("StubHeater: TurnOn called");
        }

        public void TurnOff()
        {
            Console.WriteLine("StubHeater: TurnOff called");
        }

        public bool RunSelfTest()
        {
            return _result;
        }
    }

}

