
// �V�X�e�����s

using System;

public class Tetris_Fujishima : UserApplication
{
    private UpdateTime time = new UpdateTime();
    private UpdateTime castTime = new UpdateTime();

    private Block currentBlock = null;

    // �Q�[���v���C�J�n�t���O
    private bool isGameStart = false;

    // �u���b�N�����␳�ʒu
    private int generateBlockPositionOffsetY = 10;

    // �u���b�N�ړ�
    // move = Block��squareLength
    private int moveVertical = 8;
    private int moveHorizontal = 8;
    private bool isMoving = false;

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
                FallBlock(-moveVertical);
            }

            
            if (isMoving == false)
            {
                PlayerControl(machine);
            }
            else
            {
                // �u���b�N�𑀍삷��x�ɃN�[���^�C������
                if (castTime.IsOneSecoundLater())
                {
                    isMoving = false;
                }
            }
        }
    }

    private void GenerateBlock(IMachine machine)
    {
        int generatePositionX = (int)(machine.Width * 0.5);
        int generatePositionY = machine.Height - generateBlockPositionOffsetY;
        currentBlock = new Block(machine);
        currentBlock.Draw(generatePositionX, generatePositionY);
    }

    private void PlayerControl(IMachine machine)
    {
        if (machine.Down)
        {
            MoveVerticalBlock(-moveVertical);
        }
        else if (machine.Left)
        {
            MoveHorizontalBlock(-moveHorizontal);
        }
        else if (machine.Right)
        {
            MoveHorizontalBlock(moveHorizontal);
        }
        else
        {
            return;
        }
    }

    private void FallBlock(int power)
    {
        currentBlock.Erase();
        currentBlock.Draw(currentBlock.PivotX, currentBlock.PivotY + power);
    }

    private void MoveVerticalBlock(int power)
    {
        currentBlock.Erase();
        currentBlock.Draw(currentBlock.PivotX, currentBlock.PivotY + power);
        isMoving = true;
        castTime.UpdatePrevousTime(0);
        castTime.UpdatePrevousTime(0);
    }

    private void MoveHorizontalBlock(int power)
    {
        currentBlock.Erase();
        currentBlock.Draw(currentBlock.PivotX + power, currentBlock.PivotY);
        isMoving = true;
        castTime.UpdatePrevousTime(0);
        castTime.UpdatePrevousTime(0);
    }

    private void GameStart(IMachine machine)
    {
        isGameStart = true;
        machine.Log("Game Start : {0}", isGameStart);
    }

    private void GameEnd(IMachine machine)
    {
        isGameStart = false;
        machine.Log("Game End : {0}", isGameStart);
    }
}
