namespace LadeskabLib;

public class RFIDEventArgs : EventArgs
{
    public int RFID { get; set; }
}

public interface IRFIDReader
{
    event EventHandler<RFIDEventArgs> RFIDDetected;
    void SimulateRFIDDetection(int id);
}