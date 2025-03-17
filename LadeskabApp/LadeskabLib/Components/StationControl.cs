namespace LadeskabLib;

public class StationControl
{
    // States
    private enum LadeskabState
    {
        Available,
        Locked,
        DoorOpen
    }

    // Dependencies
    private readonly IDoor _door;
    private readonly IDisplay _display;
    private readonly IRFIDReader _rfidReader;
    private readonly IChargeControl _chargeControl;
    private readonly ILogger _logger;

    // State variables
    private LadeskabState _state;
    private int _oldId;

    public StationControl(
        IDoor door, 
        IDisplay display, 
        IRFIDReader rfidReader, 
        IChargeControl chargeControl, 
        ILogger logger)
    {
        _door = door;
        _display = display;
        _rfidReader = rfidReader;
        _chargeControl = chargeControl;
        _logger = logger;
        
        // Initial state
        _state = LadeskabState.Available;
        
        // Register event handlers
        _door.DoorStatusChanged += HandleDoorStatusChanged;
        _rfidReader.RFIDDetected += HandleRFIDDetected;
    }

    private void HandleDoorStatusChanged(object sender, DoorEventArgs e)
    {
        if (e.DoorOpen)
        {
            if (_state == LadeskabState.Available)
            {
                _state = LadeskabState.DoorOpen;
                _display.ShowUserMessage("Tilslut din telefon til opladeren");
            }
        }
        else // Door closed
        {
            if (_state == LadeskabState.DoorOpen)
            {
                _state = LadeskabState.Available;
                _display.ShowUserMessage("Scan dit RFID-tag for at låse skabet");
            }
        }
    }

    private void HandleRFIDDetected(object sender, RFIDEventArgs e)
    {
        switch (_state)
        {
            case LadeskabState.Available:
                // Only lock if phone is connected
                if (_chargeControl.IsConnected())
                {
                    _door.LockDoor();
                    _chargeControl.StartCharge();
                    _oldId = e.RFID;
                    _logger.LogDoorLocked(e.RFID);
                    _display.ShowUserMessage("Skab låst og oplader. Brug dit RFID-tag til at låse op.");
                    _state = LadeskabState.Locked;
                }
                else
                {
                    _display.ShowUserMessage("Din telefon er ikke korrekt tilsluttet. Prøv igen.");
                }
                break;
                
            case LadeskabState.DoorOpen:
                _display.ShowUserMessage("Luk døren, før du låser skabet.");
                break;
                
            case LadeskabState.Locked:
                // Check for correct RFID
                if (e.RFID == _oldId)
                {
                    _chargeControl.StopCharge();
                    _door.UnlockDoor();
                    _logger.LogDoorUnlocked(e.RFID);
                    _display.ShowUserMessage("Tag din telefon nu og luk døren.");
                    _state = LadeskabState.Available;
                }
                else
                {
                    _display.ShowUserMessage("Forkert RFID-tag.");
                }
                break;
        }
    }
}