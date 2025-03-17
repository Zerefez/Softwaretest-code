using System;
namespace LadeskabLib;
public class UsbChargerSimulator : IUsbCharger
{
    // Constants
    private const double ChargeRate = 500.0; // mA
    private const double FullyChargedCurrent = 5.0; // mA
    private const int OverloadCurrent = 750; // mA
    private const int ChargeTimeMinutes = 20; // minutes
    private const int CurrentTickInterval = 250; // ms

    // Properties
    public event EventHandler<CurrentEventArgs> CurrentValueEvent;
    public double CurrentValue { get; private set; }
    public bool Connected { get; private set; }
    public bool Overload { get; private set; }

    // Private fields
    private bool _charging;
    private System.Timers.Timer _timer;
    private int _ticksSinceStart;

    public UsbChargerSimulator()
    {
        CurrentValue = 0.0;
        Connected = true;
        _charging = false;
        Overload = false;
        _timer = new System.Timers.Timer();
        _timer.Interval = CurrentTickInterval;
        _timer.Elapsed += OnTimerElapsed;
    }

    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // Only process if both connected and charging
        if (Connected && _charging)
        {
            _ticksSinceStart++;

            // Calculate remaining charge
            double maxRemainingCharge = (ChargeTimeMinutes * 60 * 1000 - _ticksSinceStart * CurrentTickInterval) / (ChargeTimeMinutes * 60 * 1000);

            if (Overload)
            {
                CurrentValue = OverloadCurrent;
            }
            else if (maxRemainingCharge <= 0.0)
            {
                CurrentValue = FullyChargedCurrent;
                _charging = false;
                _timer.Stop();
            }
            else
            {
                double newValue = maxRemainingCharge * ChargeRate;
                CurrentValue = Math.Max(newValue, FullyChargedCurrent);
            }

            OnNewCurrent();
        }
    }

    private void OnNewCurrent()
    {
        CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = CurrentValue });
    }

    public void StartCharge()
    {
        if (Connected && !Overload)
        {
            _ticksSinceStart = 0;
            _charging = true;
            _timer.Start();
        }
    }

    public void StopCharge()
    {
        _timer.Stop();
        _charging = false;
        CurrentValue = 0.0;
        OnNewCurrent();
    }

    public void SimulateConnected(bool connected)
    {
        Connected = connected;
        if (!connected && _charging)
        {
            StopCharge();
        }
    }

    public void SimulateOverload(bool overload)
    {
        Overload = overload;
        if (overload)
        {
            CurrentValue = OverloadCurrent;
            OnNewCurrent();
            if (_charging)
            {
                _timer.Stop();
                _charging = false;
            }
        }
    }
}