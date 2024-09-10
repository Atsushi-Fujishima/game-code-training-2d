using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class Block
{
    private Vector2 pivots = new Vector2();

    

    public int PivotX 
    {
        get { return (int)pivots.X; }
        set { pivots.X = value; } 
    }

    public int PivotY
    {
        get { return (int)pivots.Y; }
        set { pivots.Y = value; }
    }

    private IMachine machine;

    // 1ブロックの大きさ = (squareLength * suareLength) * 4;
    // 1squareLength = 1ドット
    private readonly int squareLength = 8;

    // ドット描画座標
    private List<Vector2> dotPositions;

    // 隙間空ける補正値
    private int offset;

    public Block(IMachine machine)
    {
        this.machine = machine;
        offset = squareLength + 1;
        dotPositions = new List<Vector2>();
    }

    public void Draw(int pivotX, int pivotY)
    {
        PivotX = pivotX;
        PivotY = pivotY;
        DrawO(PivotX, PivotY);
    }

    public void DrawSquare(int pivotX, int pivotY, byte[] color)
    {
        for (int w = 0; w < squareLength; w++)
        {
            for (int h = 0; h < squareLength; h++)
            {
                // 描画
                Vector2 position = new Vector2(pivotX + w, pivotY + h);
                machine.Draw(pivotX + w, pivotY + h, color[0], color[1], color[2]);

                // 座標記憶
                dotPositions.Add(position);
            }
        }
    }

    public void Erase()
    {
        byte[] color = DotColor.color_Black;

        foreach (var dotPos in dotPositions)
        {
            machine.Draw((int)dotPos.X, (int)dotPos.Y, color[0], color[1], color[2]);
        }

        dotPositions.Clear();
    }

    public bool PermitMove(int powerX, int powerY)
    {
        // 移動先のドット
        Vector2[] futurePosition = new Vector2[dotPositions.Count];

        // 移動先のドットを全て取得
        for (int i = 0; i < futurePosition.Length; i++)
        {
            futurePosition[i].X = dotPositions[i].X + powerX;
            futurePosition[i].Y = dotPositions[i].Y + powerY;

            // 移動できなくなるため、移動前と移動先のドットが重複する場合は例外とする
            if (dotPositions.Contains(futurePosition[i]))
            {
                continue;
            }
            else
            {
                // ドットカラー取得用
                byte[] getColor = new byte[3];

                // 移動先のドットカラー取得
                machine.GetPixel(out getColor[0], out getColor[1], out getColor[2], (int)futurePosition[i].X, (int)futurePosition[i].Y);
                
                // 移動先のドットが黒でないなら移動不可
                if (getColor.SequenceEqual(DotColor.color_Black) == false)
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    private void DrawO(int pivotX, int pivotY)
    {
        byte[] setColor = DotColor.color_Blue;
        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY, setColor);
        DrawSquare(pivotX, pivotY + offset, setColor);
        DrawSquare(pivotX + offset, pivotY + offset, setColor);
    }
}
