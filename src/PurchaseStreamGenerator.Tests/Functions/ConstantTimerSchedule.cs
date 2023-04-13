using Microsoft.Azure.WebJobs.Extensions.Timers;

public class ConstantTimerSchedule : TimerSchedule
{
    private readonly TimeSpan _interval;

    public ConstantTimerSchedule(TimeSpan interval)
    {
        _interval = interval;
    }

    public override DateTime GetNextOccurrence(DateTime now)
    {
        return now.Add(_interval);
    }

    public override bool AdjustForDST
    {
        get { return false; }
    }
}