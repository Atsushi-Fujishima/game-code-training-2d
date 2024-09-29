
using System;
using System.Numerics;

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
    private int generateBlockPositionOffsetY = 298;

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
                // 自由落下
                FallingBlock(-moveVertical, machine);
            }

            // プレイヤーによるブロック操作
            PlayerControl();
        }
    }

    // 画面に新しいブロックを描画
    private void GenerateBlock(IMachine machine)
    {
        // 無作為にブロックタイプを決める
        currentBlock = new Block(machine, RandomBlockType());

        // TODO: ブロックの種類を固定にしてテスト中
        //currentBlock = new Block(machine, BlockType.O);

        currentBlock.Draw(generateBlockPositionOffsetX, generateBlockPositionOffsetY);
    }

    private BlockType RandomBlockType()
    {
        // 疑似乱数
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
            // TODO: 機能テスト中
            blockField.EraseLine();

            // 現在のブロックの制御を失い、新しいブロックを生成する
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
