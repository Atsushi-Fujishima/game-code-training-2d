using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class BlockField
{
    private IMachine machine;

    // 配置数
    private int underSideSquareNumber = 10;
    private int bothSideSquareNumber = 45;

    // ブロック移動量
    private int moveValue = 9;

    private int squareNum = 10;

    // 枠の起点
    private Vector2 starttingPoint = new Vector2(54, 10);

    // フレームの色
    private readonly byte[] fieldColor = DotColor.color_White;

    // 範囲内の起点座標
    private List<Vector2> inRangePivots = new List<Vector2>();

    // 描画用
    private Block block;

    public byte[] GetFieldColor()
    {
        return fieldColor;
    }

    public BlockField(IMachine machine)
    {
        this.machine = machine;
        block = new Block(machine, BlockType.O);
        GetInRangePivots();
    }

    public void GenerateFrame()
    {
        // 下線の起点
        Vector2 underStarttingPoint = starttingPoint;

        // 左線の起点
        Vector2 leftSideStarttingPoint = new Vector2((int)underStarttingPoint.X - moveValue, underStarttingPoint.Y);

        // 右線の起点
        Vector2 rightSideStarttingPoint = new Vector2((int)underStarttingPoint.X + (moveValue * underSideSquareNumber), underStarttingPoint.Y);

        // UnderSide
        for (int i = 0; i < underSideSquareNumber; i++)
        {
            int x = (int)underStarttingPoint.X + (i * moveValue);
            block.DrawSquare(x, (int)underStarttingPoint.Y, fieldColor);
        }

        // LeftSide
        for (int i = 0; i < bothSideSquareNumber; i++)
        {
            int y = (int)leftSideStarttingPoint.Y + (i * moveValue);
            block.DrawSquare((int)leftSideStarttingPoint.X, y, fieldColor);
        }

        // RightSide
        for (int i = 0; i < bothSideSquareNumber; i++)
        {
            int y = (int)rightSideStarttingPoint.Y + (i * moveValue);
            block.DrawSquare((int)rightSideStarttingPoint.X, y, fieldColor);
        }
    }

    private void GetInRangePivots()
    {
        Block block = new Block(machine, BlockType.O);

        // 枠内
        for (int i = 0; i < squareNum; i++)
        {
            int x = (int)starttingPoint.X + i * moveValue;

            for (int h = 0; h < bothSideSquareNumber; h++)
            {
                int y = (int)starttingPoint.Y + h * moveValue + moveValue;
                inRangePivots.Add(new Vector2(x, y));

                
                block.DrawSquare(x, y, DotColor.color_Cyan);
            }
        }
    }

    public void EraseLine()
    {
        // 消した行の最小値
        int minLine = 100;

        // 消す色
        byte[] eraseColor = DotColor.color_Black;

        // Y軸スクエア分繰り返す
        for (int i = 0; i < bothSideSquareNumber; i++)
        {
            // 行
            int gyou = (int)starttingPoint.Y + i * moveValue + moveValue;
            List<Vector2> vector2s = new List<Vector2>();

            // X軸スクエア分繰り返す
            for (int n = 0; n < squareNum; n++)
            {
                // 列
                int retu = (int)starttingPoint.X + n * moveValue;

                // 座標カラー取得
                byte[] getColor = new byte[3];
                machine.GetPixel(out getColor[0], out getColor[1], out getColor[2], retu, gyou);
                
                // 黒と白以外なら消す
                if (DotColor.IsCompareColorAwithB(getColor, fieldColor) == false && 
                    DotColor.IsCompareColorAwithB(getColor, eraseColor) == false)
                {
                    vector2s.Add(new Vector2(retu, gyou));
                }
            }

            // i行の全ての列が黒白以外なら消す
            if (vector2s.Count == squareNum)
            {
                foreach (Vector2 vector2 in vector2s)
                {
                    // 消す
                    block.DrawSquare((int)vector2.X, (int)vector2.Y, eraseColor);
                }

                // 消した行の最小値を更新する
                minLine = (minLine > i) ? i : minLine;
                machine.Log("minLine{0}", minLine);
            }
        }

        //TODO : 後で修正
        // 消した行の最小値より大きい行の全てのドットを一つ落とす
        if (minLine != 100)
        {
            foreach (Vector2 v2 in inRangePivots)
            {
                // 座標カラー取得
                byte[] getColor = new byte[3];
                machine.GetPixel(out getColor[0], out getColor[1], out getColor[2], (int)v2.X, (int)v2.Y);

                block.DrawSquare((int)v2.X, (int)v2.Y -1, getColor);
            }
        }
    }
}
