namespace LadeskabLib;


public class ChargeControl : IChargeControl
{
    private readonly IUsbCharger _usbCharger;
    private readonly IDisplay _display;
    
    public bool IsCharging { get; private set; }
    
    public event EventHandler<ChargeEventArgs> ChargingStateChanged;

    public ChargeControl(IUsbCharger usbCharger, IDisplay display)
    {
        _usbCharger = usbCharger;
        _display = display;
        IsCharging = false;
        
        // Abonner på USB-laderens events
        _usbCharger.CurrentValueEvent += HandleCurrentValueChanged;
    }

    public bool IsConnected()
    {
        return _usbCharger.Connected;
    }

    public void StartCharge()
    {
        if (IsConnected())
        {
            _usbCharger.StartCharge();
            IsCharging = true;
            _display.ShowChargingMessage("Opladning er startet");
            OnChargingStateChanged(new ChargeEventArgs { Connected = true, Current = _usbCharger.CurrentValue, Message = "Opladning startet" });
        }
        else
        {
            _display.ShowChargingMessage("Ingen telefon tilsluttet");
        }
    }

    public void StopCharge()
    {
        _usbCharger.StopCharge();
        IsCharging = false;
        _display.ShowChargingMessage("Opladning er stoppet");
        OnChargingStateChanged(new ChargeEventArgs { Connected = IsConnected(), Current = 0.0, Message = "Opladning stoppet" });
    }

    private void HandleCurrentValueChanged(object sender, CurrentEventArgs e)
    {
        switch (e.Current)
        {
            case 0:
                if (IsCharging)
                {
                    _display.ShowChargingMessage("Ingen telefon tilsluttet eller opladning ikke startet");
                }
                break;
            
            case > 0 and <= 5:
                _display.ShowChargingMessage("Telefon fuldt opladet");
                IsCharging = false;
                OnChargingStateChanged(new ChargeEventArgs { Connected = true, Current = e.Current, Message = "Fuldt opladet" });
                break;
            
            case > 5 and <= 500:
                _display.ShowChargingMessage("Opladning i gang");
                break;
            
            case > 500:
                _display.ShowChargingMessage("FEJL: Strøm for høj! Afbryder opladning");
                StopCharge();
                OnChargingStateChanged(new ChargeEventArgs { Connected = true, Current = e.Current, Message = "Fejl: Overbelastning" });
                break;
        }
    }

    protected virtual void OnChargingStateChanged(ChargeEventArgs e)
    {
        ChargingStateChanged?.Invoke(this, e);
    }
}
