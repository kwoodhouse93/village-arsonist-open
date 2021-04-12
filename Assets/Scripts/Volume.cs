public class Volume
{
    const float HIGH_LEVEL = 0f;
    const float MEDIUM_LEVEL = -10f;
    const float LOW_LEVEL = -20f;
    const float OFF_LEVEL = -80f;

    public enum Level
    {
        Off,
        Low,
        Medium,
        High,
    }

    public Level level;

    public Volume()
    {
        level = Level.Low;
    }

    public Volume(int startLevel)
    {
        switch (startLevel)
        {
            case 0:
                level = Level.Off;
                return;
            case 1:
                level = Level.Low;
                return;
            case 2:
                level = Level.Medium;
                return;
            case 3:
                level = Level.High;
                return;
            default:
                level = Level.Medium;
                return;
        }
    }

    public void Increment()
    {
        switch (level)
        {
            case Level.Off:
                level = Level.High;
                break;
            case Level.Low:
                level = Level.Off;
                break;
            case Level.Medium:
                level = Level.Low;
                break;
            case Level.High:
                level = Level.Medium;
                break;
            default:
                level = Level.Off;
                break;
        }
    }

    public float ToFloat()
    {
        switch (level)
        {
            case Level.Off:
                return OFF_LEVEL;
            case Level.Low:
                return LOW_LEVEL;
            case Level.Medium:
                return MEDIUM_LEVEL;
            case Level.High:
                return HIGH_LEVEL;
            default:
                return -80f;
        }
    }

    public new string ToString()
    {
        switch (level)
        {
            case Level.Off:
                return "OFF";
            case Level.Low:
                return "LOW";
            case Level.Medium:
                return "MED";
            case Level.High:
                return "HI";
            default:
                return "---";
        }
    }

    public int ToInt()
    {
        switch (level)
        {
            case Level.Off:
                return 0;
            case Level.Low:
                return 1;
            case Level.Medium:
                return 2;
            case Level.High:
                return 3;
            default:
                return 0;
        }
    }
}
