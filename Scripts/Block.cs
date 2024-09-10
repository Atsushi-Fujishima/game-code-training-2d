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

    // 1�u���b�N�̑傫�� = (squareLength * suareLength) * 4;
    // 1squareLength = 1�h�b�g
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

    public void DrawSquare(int pivotX, int pivotY, byte[] color)
    {
        for (int w = 0; w < squareLength; w++)
        {
            for (int h = 0; h < squareLength; h++)
            {
                // �`��
                Vector2 position = new Vector2(pivotX + w, pivotY + h);
                machine.Draw(pivotX + w, pivotY + h, color[0], color[1], color[2]);

                // ���W�L��
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
        // �ړ���̃h�b�g
        Vector2[] futurePosition = new Vector2[dotPositions.Count];

        // �ړ���̃h�b�g��S�Ď擾
        for (int i = 0; i < futurePosition.Length; i++)
        {
            futurePosition[i].X = dotPositions[i].X + powerX;
            futurePosition[i].Y = dotPositions[i].Y + powerY;

            // �ړ��ł��Ȃ��Ȃ邽�߁A�ړ��O�ƈړ���̃h�b�g���d������ꍇ�͗�O�Ƃ���
            if (dotPositions.Contains(futurePosition[i]))
            {
                continue;
            }
            else
            {
                // �h�b�g�J���[�擾�p
                byte[] getColor = new byte[3];

                // �ړ���̃h�b�g�J���[�擾
                machine.GetPixel(out getColor[0], out getColor[1], out getColor[2], (int)futurePosition[i].X, (int)futurePosition[i].Y);
                
                // �ړ���̃h�b�g�����łȂ��Ȃ�ړ��s��
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
