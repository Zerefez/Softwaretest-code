
namespace LadeskabLib;

public class ChargeEventArgs : EventArgs
{
    public bool Connected { get; set; }
    public double Current { get; set; }
    public string Message { get; set; }
}

public interface IChargeControl
{
    bool IsConnected();
    void StartCharge();
    void StopCharge();
    bool IsCharging { get; }
    event EventHandler<ChargeEventArgs> ChargingStateChanged;
}