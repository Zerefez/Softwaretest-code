namespace LadeskabLib;

public interface ILogger
{
    void LogDoorLocked(int id);
    void LogDoorUnlocked(int id);
}