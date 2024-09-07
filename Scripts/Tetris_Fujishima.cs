
// システム実行

using System;

public class Tetris_Fujishima : UserApplication
{
    private UpdateTime time = new UpdateTime();
    private UpdateTime castTime = new UpdateTime();

    private Block currentBlock = null;

    // ゲームプレイ開始フラグ
    private bool isGameStart = false;

    // ブロック生成補正位置
    private int generateBlockPositionOffsetY = 10;

    // ブロック移動
    // move = BlockのsquareLength
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
            // 1秒毎に落下
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
                // ブロックを操作する度にクールタイム発生
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
