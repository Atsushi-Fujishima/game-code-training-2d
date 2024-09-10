
public class UpdateTime
{
    private float elapsedTime;
    private float previousTime;
    private readonly float oneFrameSecounds = 0.0167f;

    // 60fps‚Å‚ ‚é‚Æ‚«‹@”\‚Í•ÛØ‚³‚ê‚é
    public bool IsOneSecoundLater()
    {
        UpdateElapsedTime(elapsedTime + oneFrameSecounds);

        // 1•bŒo‰ß‚µ‚Ä‚¢‚é‚©
        if (elapsedTime > previousTime + 1.0f)
        {
            UpdatePrevousTime(elapsedTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateElapsedTime(float secound)
    {
        elapsedTime = secound;
    }

    public void UpdatePrevousTime(float secound)
    {
        previousTime = secound;
    }

    public void InitializeTime()
    {
        elapsedTime = 0;
        previousTime = 0;
    }
}
