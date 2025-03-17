namespace LadeskabLib;

public class Logger : ILogger
{
    private readonly string _logFile;

    public Logger(string logFile = "logfile.txt")
    {
        _logFile = logFile;
    }

    public void LogDoorLocked(int id)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string logMessage = $"{timestamp}: Skab låst med RFID: {id}";
        
        using (StreamWriter writer = File.AppendText(_logFile))
        {
            writer.WriteLine(logMessage);
        }
    }

    public void LogDoorUnlocked(int id)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string logMessage = $"{timestamp}: Skab låst op med RFID: {id}";
        
        using (StreamWriter writer = File.AppendText(_logFile))
        {
            writer.WriteLine(logMessage);
        }
    }
}
