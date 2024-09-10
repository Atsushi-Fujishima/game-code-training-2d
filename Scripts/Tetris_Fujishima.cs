
public class Tetris_Fujishima : UserApplication
{
    private UpdateTime time = new UpdateTime();

    // ���ݑ���\�ȃu���b�N
    private Block currentBlock = null;

    // ���͊Ǘ�
    PlayerInputControl playerInputControl = null;

    // �u���b�N�ړ��͈�
    BlockField blockField = null;

    // �Q�[���v���C�J�n�t���O
    private bool isGameStart = false;

    // �u���b�N�����␳�ʒu
    private int generateBlockPositionOffsetX = 90;
    private int generateBlockPositionOffsetY = 10;

    // �u���b�N�ړ�
    // move = Block��squareLength
    private int moveVertical = 9;
    private int moveHorizontal = 9;

    public override void Update(IMachine machine)
    {
        if (machine.Space && isGameStart == false)
        {
            GameStart(machine);
            GenerateBlock(machine);
        }

        if (currentBlock != null && isGameStart)
        {
            // 1�b���ɗ���
            if (time.IsOneSecoundLater())
            {
                // TODO: �ǂ������ɂȂ�����g�p
                // ���R����
                // MoveVerticalBlock(-moveVertical);
            }

            // �v���C���[�ɂ��u���b�N����
            PlayerControl();
        }
    }

    // ��ʂɐV�����u���b�N��`��
    private void GenerateBlock(IMachine machine)
    {
        int generatePositionX = generateBlockPositionOffsetX;
        int generatePositionY = machine.Height - generateBlockPositionOffsetY;
        currentBlock = new Block(machine);
        currentBlock.Draw(generatePositionX, generatePositionY);
    }

    private void PlayerControl()
    {
        if (playerInputControl.LeftKeyWasPressed())
        {
            MoveHorizontalBlock(-moveHorizontal);
        }
        else if (playerInputControl.RightKeyWasPressed())
        {
            MoveHorizontalBlock(moveHorizontal);
        }
        else if (playerInputControl.DownKeyWasPressed())
        {
            MoveVerticalBlock(-moveVertical);
        }

        if (playerInputControl.LeftKey())
        {
            MoveHorizontalBlock(-moveHorizontal);
        }
        else if (playerInputControl.RightKey())
        {
            MoveHorizontalBlock(moveHorizontal);
        }
        else if (playerInputControl.DownKey())
        {
            MoveVerticalBlock(-moveVertical);
        }
    }

    private void MoveVerticalBlock(int power)
    {
        if (currentBlock.PermitMove(0, power))
        {
            currentBlock.Erase();
            currentBlock.Draw(currentBlock.PivotX, currentBlock.PivotY + power);
        }
        else
        {
            return;
        }
    }

    private void MoveHorizontalBlock(int power)
    {
        if (currentBlock.PermitMove(power, 0))
        {
            currentBlock.Erase();
            currentBlock.Draw(currentBlock.PivotX + power, currentBlock.PivotY);
        }
        else
        {
            return;
        }
    }

    private void GameStart(IMachine machine)
    {
        isGameStart = true;
        playerInputControl = new PlayerInputControl(machine);

        blockField = new BlockField(machine);
        blockField.GenerateFrame();

        machine.Log("Game Start : {0}", isGameStart);
    }

    private void GameEnd(IMachine machine)
    {
        isGameStart = false;
        machine.Log("Game End : {0}", isGameStart);
    }
}
