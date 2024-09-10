
public class UpdateTime
{
    private float elapsedTime;
    private float previousTime;
    private readonly float oneFrameSecounds = 0.0167f;

    // 60fps�ł���Ƃ��@�\�͕ۏ؂����
    public bool IsOneSecoundLater()
    {
        UpdateElapsedTime(elapsedTime + oneFrameSecounds);

        // 1�b�o�߂��Ă��邩
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
