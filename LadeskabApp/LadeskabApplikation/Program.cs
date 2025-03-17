using LadeskabLib;

namespace LadeskabApplikation
{
    public class Program
{
    public static void Main(string[] args)
    {
        // Create components
        IDoor door = new Door();
        IDisplay display = new Display();
        IRFIDReader rfidReader = new RFIDReader();
        IUsbCharger usbCharger = new UsbChargerSimulator();
        IChargeControl chargeControl = new ChargeControl(usbCharger, display);
        ILogger logger = new Logger();

        // Create station control
        StationControl stationControl = new StationControl(door, display, rfidReader, chargeControl, logger);

        bool running = true;
        display.ShowUserMessage("Ladeskabssystem er startet");
        display.ShowUserMessage("Tryk på O for at åbne døren, L for at lukke døren, S for at scanne RFID, Q for at afslutte");

        do
        {
            string input = Console.ReadLine().ToUpper();
            switch (input)
            {
                case "O":
                    door.OpenDoor();
                    break;
                    
                case "L":
                    door.CloseDoor();
                    break;
                    
                case "S":
                    display.ShowUserMessage("Indtast RFID-id:");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        rfidReader.SimulateRFIDDetection(id);
                    }
                    else
                    {
                        display.ShowUserMessage("Ugyldigt RFID-id");
                    }
                    break;
                    
                case "Q":
                    running = false;
                    break;
                    
                default:
                    display.ShowUserMessage("Ukendt kommando");
                    break;
            }
        } while (running);
    }
}
}
