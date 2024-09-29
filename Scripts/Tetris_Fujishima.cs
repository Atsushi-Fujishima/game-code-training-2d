
using System;
using System.Numerics;

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
    private int generateBlockPositionOffsetY = 298;

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
                // ���R����
                FallingBlock(-moveVertical, machine);
            }

            // �v���C���[�ɂ��u���b�N����
            PlayerControl();
        }
    }

    // ��ʂɐV�����u���b�N��`��
    private void GenerateBlock(IMachine machine)
    {
        // ����ׂɃu���b�N�^�C�v�����߂�
        currentBlock = new Block(machine, RandomBlockType());

        // TODO: �u���b�N�̎�ނ��Œ�ɂ��ăe�X�g��
        //currentBlock = new Block(machine, BlockType.O);

        currentBlock.Draw(generateBlockPositionOffsetX, generateBlockPositionOffsetY);
    }

    private BlockType RandomBlockType()
    {
        // �^������
        Random random = new Random();
        int r = random.Next(0, Enum.GetValues(typeof(BlockType)).Length);
        Array blockTypeValues = Enum.GetValues(typeof(BlockType));
        BlockType setType = (BlockType)blockTypeValues.GetValue(r);
        return setType;
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
        else if (playerInputControl.UpKeyWasPressed())
        {
            currentBlock.Test_Rotattion_9();
            // currentBlock.FakeRotation();
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

    private void FallingBlock(int power, IMachine machine)
    {
        if (currentBlock.PermitMove(0, power))
        {
            currentBlock.Move(new Vector2(0, power));
        }
        else
        {
            // TODO: �@�\�e�X�g��
            blockField.EraseLine();

            // ���݂̃u���b�N�̐���������A�V�����u���b�N�𐶐�����
            GenerateBlock(machine);
        }
    }

    private void MoveVerticalBlock(int power)
    {
        if (currentBlock.PermitMove(0, power))
        {
            currentBlock.Move(new Vector2(0, power));
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
            currentBlock.Move(new Vector2(power, 0));
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
