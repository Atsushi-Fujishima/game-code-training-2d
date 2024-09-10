
public class PlayerInputControl
{
    private IMachine machine;
    private readonly int keyPressHoldCount = 20; 

    private bool isLeftKeySet = false;
    private int leftKeyCount = 0;

    private bool isRightKeySet = false;
    private int rightKeyCount = 0;

    private bool isDownKeySet = false;
    private int downKeyCount = 0;

    public PlayerInputControl(IMachine machine)
    {
        this.machine = machine;
    }

    // ‰Ÿ‚µ‘±‚¯‚Ä‚¢‚é
    public bool LeftKey()
    {
        if (machine.Left)
        {
            if (leftKeyCount > keyPressHoldCount)
            {
                return true;
            }
            else
            {
                leftKeyCount++;
                return false;
            }
        }
        else
        {
            leftKeyCount = 0;
            return false;
        }
    }

    public bool RightKey()
    {
        if (machine.Right)
        {
            if (rightKeyCount > keyPressHoldCount)
            {
                return true;
            }
            else
            {
                rightKeyCount++;
                return false;
            }
        }
        else
        {
            rightKeyCount = 0;
            return false;
        }
    }

    public bool DownKey()
    {
        if (machine.Down)
        {
            if (downKeyCount > keyPressHoldCount)
            {
                return true;
            }
            else
            {
                downKeyCount++;
                return false;
            }
        }
        else
        {
            downKeyCount = 0;
            return false;
        }
    }

    // ‚Û‚¿‚Á‚Æ‰Ÿ‚µ
    public bool LeftKeyWasPressed()
    {
        if (machine.Left)
        {
            // ‰Ÿ‚µ‚½Œã
            if (isLeftKeySet)
            {
                return false;
            }

            isLeftKeySet = true;
            return true;
        }
        else
        {
            // ‰Ÿ‚µ‚Ä‚È‚¢
            isLeftKeySet = false;
            return false;
        }
    }

    public bool RightKeyWasPressed()
    {
        if (machine.Right)
        {
            if (isRightKeySet)
            {
                return false;
            }

            isRightKeySet = true;
            return true;
        }
        else
        {
            isRightKeySet = false;
            return false;
        }
    }

    public bool DownKeyWasPressed()
    {
        if (machine.Down)
        {
            if (isDownKeySet)
            {
                return false;
            }

            isDownKeySet = true;
            return true;
        }
        else
        {
            isDownKeySet = false;
            return false;
        }
    }
}
