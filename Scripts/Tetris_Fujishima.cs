
public class Tetris_Fujishima : UserApplication
{
    private UpdateTime time = new UpdateTime();

    // 現在操作可能なブロック
    private Block currentBlock = null;

    // 入力管理
    PlayerInputControl playerInputControl = null;

    // ブロック移動範囲
    BlockField blockField = null;

    // ゲームプレイ開始フラグ
    private bool isGameStart = false;

    // ブロック生成補正位置
    private int generateBlockPositionOffsetX = 90;
    private int generateBlockPositionOffsetY = 10;

    // ブロック移動
    // move = BlockのsquareLength
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
            // 1秒毎に落下
            if (time.IsOneSecoundLater())
            {
                // TODO: 良い感じになったら使用
                // 自由落下
                // MoveVerticalBlock(-moveVertical);
            }

            // プレイヤーによるブロック操作
            PlayerControl();
        }
    }

    // 画面に新しいブロックを描画
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
