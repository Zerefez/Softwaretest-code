namespace LadeskabLib;

public class RFIDReader : IRFIDReader
{
    public event EventHandler<RFIDEventArgs> RFIDDetected;

    public void SimulateRFIDDetection(int id)
    {
        OnRFIDDetected(new RFIDEventArgs { RFID = id });
    }

    protected virtual void OnRFIDDetected(RFIDEventArgs e)
    {
        RFIDDetected?.Invoke(this, e);
    }
}