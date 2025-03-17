namespace LadeskabLib;

public class DoorEventArgs : EventArgs
{
    public bool DoorOpen { get; set; }
}
public interface IDoor
{
    event EventHandler<DoorEventArgs> DoorStatusChanged;
    bool Locked { get; }
    bool Closed { get; }
    void LockDoor();
    void UnlockDoor();
    void OpenDoor();
    void CloseDoor();
}

