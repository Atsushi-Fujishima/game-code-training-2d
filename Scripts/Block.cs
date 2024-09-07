using System.Collections.Generic;
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

    // 1�u���b�N�̑傫�� = (squareLength * suareLength) * 4;
    // 1squareLength = 1dot
    private readonly int squareLength = 8;

    // �h�b�g�`����W
    private List<Vector2> dotPositions;

    // ���ԋ󂯂�␳�l
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

    private void DrawO(int pivotX, int pivotY)
    {
        DrawSquare(pivotX, pivotY);
        DrawSquare(pivotX + offset, pivotY);
        DrawSquare(pivotX, pivotY + offset);
        DrawSquare(pivotX + offset, pivotY + offset);
    }

    private void DrawSquare(int pivotX, int pivotY)
    {
        for (int w = 0; w < squareLength; w++)
        {
            for (int h = 0; h < squareLength; h++)
            {
                // �`��
                byte[] color = DotColor.color_Blue;
                Vector2 position = new Vector2(pivotX + w, pivotY + h);
                machine.Draw(pivotX + w, pivotY + h, color[0], color[1], color[2]);

                // ���W�}��
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
    }
}
