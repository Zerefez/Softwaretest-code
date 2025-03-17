namespace LadeskabLib;

public class Door : IDoor
{
    public event EventHandler<DoorEventArgs> DoorStatusChanged;
    
    public bool Locked { get; private set; }
    public bool Closed { get; private set; }

    public Door()
    {
        Locked = false;
        Closed = true;
    }

    public void LockDoor()
    {
        if (Closed && !Locked)
        {
            Locked = true;
        }
    }

    public void UnlockDoor()
    {
        if (Locked)
        {
            Locked = false;
        }
    }

    public void OpenDoor()
    {
        if (!Locked && Closed)
        {
            Closed = false;
            OnDoorStatusChanged(new DoorEventArgs { DoorOpen = true });
        }
    }

    public void CloseDoor()
    {
        if (!Closed)
        {
            Closed = true;
            OnDoorStatusChanged(new DoorEventArgs { DoorOpen = false });
        }
    }

    protected virtual void OnDoorStatusChanged(DoorEventArgs e)
    {
        DoorStatusChanged?.Invoke(this, e);
    }
}
