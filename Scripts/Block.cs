using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public enum BlockType
{
    I, O, S, Z, J, L, T
}

public class Block
{
    // �u���b�N�̎��
    private BlockType blockType = BlockType.O;

    // �u���b�N�̉�]����W
    private Vector2 pivots = new Vector2();

    private byte[] setColor = null;
    private byte[] clearColor = DotColor.color_Black;

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

    public Block(IMachine machine, BlockType blockType)
    {
        this.machine = machine;
        this.blockType = blockType;
        offset = squareLength + 1;
        dotPositions = new List<Vector2>();
    }

    public void Draw(int pivotX, int pivotY)
    {
        PivotX = pivotX;
        PivotY = pivotY;

        switch (blockType)
        {
            case BlockType.I: DrawI(pivotX, pivotY); break;
            case BlockType.O: DrawO(pivotX, pivotY); break;
            case BlockType.S: DrawS(pivotX, pivotY); break;
            case BlockType.Z: DrawZ(pivotX, pivotY); break;
            case BlockType.J: DrawJ(pivotX, pivotY); break;
            case BlockType.L: DrawL(pivotX, pivotY); break;
            case BlockType.T: DrawT(pivotX, pivotY); break;
        }
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
        foreach (var dotPos in dotPositions)
        {
            machine.Draw((int)dotPos.X, (int)dotPos.Y, clearColor[0], clearColor[1], clearColor[2]);
        }
    }

    public bool PermitMove(int x, int y)
    {
        // �ړ���̃h�b�g
        Vector2[] futurePosition = new Vector2[dotPositions.Count];

        // �ړ���̃h�b�g��S�Ď擾
        for (int i = 0; i < futurePosition.Length; i++)
        {
            futurePosition[i].X = dotPositions[i].X + x;
            futurePosition[i].Y = dotPositions[i].Y + y;

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

    public void Move(Vector2 vector)
    {
        // �h�b�g����x����
        Erase();

        // �h�b�g��vector��������
        for (int i = 0; i < dotPositions.Count; i++)
        {
            int newX = (int)dotPositions[i].X + (int)vector.X;
            int newY = (int)dotPositions[i].Y + (int)vector.Y;

            machine.Draw(newX, newY, setColor[0], setColor[1], setColor[2]);
            UpdateDotPositions(new Vector2(newX, newY), i);
        }

        // ����W���X�V����
        PivotX = (int)dotPositions[0].X;
        PivotY = (int)dotPositions[0].Y;
    }

    // �h�b�g�z��̍X�V
    private void UpdateDotPositions(Vector2 position, int index)
    {
        dotPositions[index] = position;
    }

    private void DrawI(int pivotX,  int pivotY)
    {
        setColor = DotColor.color_LightBlue;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX, pivotY + offset, setColor);
        DrawSquare(pivotX, pivotY + offset + offset, setColor);
        DrawSquare(pivotX, pivotY + offset + offset + offset, setColor);
    }

    private void DrawO(int pivotX, int pivotY)
    {
        setColor = DotColor.color_Yeloow;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY, setColor);
        DrawSquare(pivotX, pivotY + offset, setColor);
        DrawSquare(pivotX + offset, pivotY + offset, setColor);
    }

    private void DrawS(int pivotX, int pivotY)
    {
        setColor = DotColor.color_Green;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY + offset, setColor);
        DrawSquare(pivotX + offset + offset, pivotY + offset, setColor);
    }

    private void DrawZ(int pivotX, int pivotY)
    {
        setColor = DotColor.color_Red;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY, setColor);
        DrawSquare(pivotX, pivotY + offset, setColor);
        DrawSquare(pivotX - offset, pivotY + offset, setColor);
    }

    private void DrawJ(int pivotX, int pivotY)
    {
        setColor = DotColor.color_Blue;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY + offset, setColor);
        DrawSquare(pivotX + offset, pivotY + offset + offset, setColor);
    }

    private void DrawL(int pivotX, int pivotY)
    {
        setColor = DotColor.color_Orange;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX + offset, pivotY, setColor);
        DrawSquare(pivotX, pivotY + offset, setColor);
        DrawSquare(pivotX, pivotY + offset + offset, setColor);
    }

    private void DrawT(int pivotX, int pivotY)
    {
        setColor = DotColor.color_Purple;

        DrawSquare(pivotX, pivotY, setColor);
        DrawSquare(pivotX, pivotY + offset, setColor);
        DrawSquare(pivotX - offset, pivotY + offset, setColor);
        DrawSquare(pivotX + offset, pivotY + offset, setColor);
    }

    public void FakeRotation()
    {
        // �h�b�g����x����
        Erase();

        for (int i = 0; i < dotPositions.Count; i++)
        {
            Vector2 dot = dotPositions[i];

            // ���΍��W�ɕϊ�
            float relativeX = dot.X - pivots.X;
            float relativeY = dot.Y - pivots.Y;

            // ��]�s��K�p�i90�x��]�j
            float newX = relativeY;
            float newY = relativeX;

            // ���̍��W�ɖ߂�
            newX += pivots.X;
            newY += pivots.Y;

            // �V�������W�Ƀh�b�g��`��
            machine.Draw((int)newX, (int)newY, setColor[0], setColor[1], setColor[2]);

            UpdateDotPositions(new Vector2(newX, newY), i);
        }

        // ����W���X�V����
        PivotX = (int)dotPositions[0].X;
        PivotY = (int)dotPositions[0].Y;
    }

    // ����3
    public void Test_Rotattion_9()
    {
        // ��]��̍��W�z��
        Vector2[] rotationDots = dotPositions.ToArray();

        // �h�b�g����x����
        Erase();

        // ��]����
        for (int i = 0; i < rotationDots.Length; i++)
        {
            Vector2 dot = rotationDots[i];

            // ���΍��W�ɕϊ�
            float relativeX = dot.X - PivotX;
            float relativeY = dot.Y - PivotY;

            // ��]�s��K�p�i90�x��]�j
            float newX = -relativeY;
            float newY = relativeX;

            // ���̍��W�ɖ߂�
            newX += PivotX;
            newY += PivotY;

            rotationDots[i] = new Vector2(newX, newY);
        }

        // ���X�g�X�V
        dotPositions.Clear();
        dotPositions.AddRange(rotationDots);

        // pivot�X�V
        pivots = dotPositions[0];

        // offset���l�������ʒu�Ɉړ�
        machine.Log("aa {0}, {1}", (int)pivots.X, (int)pivots.Y);

        int offsetX = FindCloseOffsetPosition(PivotX) - PivotX;
        int offsetY = FindCloseOffsetPosition(PivotY) - PivotY;


        for (int i = 0; i < dotPositions.Count; i++)
        {
            dotPositions[i] = new Vector2(dotPositions[i].X + offsetX, dotPositions[i].Y + offsetY);
            machine.Draw((int)dotPositions[i].X, (int)dotPositions[i].Y, setColor[0], setColor[1], setColor[2]);
        }

        // pivot�X�V
        pivots = dotPositions[0];
    }

    private int FindCloseOffsetPosition(int i)
    {
        int closeOffset = (int)MathF.Round(i / offset) * offset;
        return closeOffset;
    }


    // ����2
    public void Test_Rotation_center()
    {
        // ��]��̍��W�z��
        Vector2[] rotationDots = dotPositions.ToArray();

        // ���݂̃u���b�N�̒��S���W
        Vector2 originalCenter = CalculateCenter(dotPositions.ToArray());

        // �h�b�g����x����
        Erase();

        // ��]����
        for (int i = 0; i < rotationDots.Length; i++)
        {
            Vector2 dot = rotationDots[i];

            // ���΍��W�ɕϊ�
            float relativeX = dot.X - originalCenter.X;
            float relativeY = dot.Y - originalCenter.Y;

            // ��]�s��K�p�i90�x��]�j
            float newX = -relativeY;
            float newY = relativeX;

            // ���̍��W�ɖ߂�
            newX += originalCenter.X;
            newY += originalCenter.Y;

            rotationDots[i] = new Vector2(newX, newY);
        }

        // ��]��̒��S���W
        Vector2 newCenter = CalculateCenter(rotationDots);

        //���S���W�����v�Z
        float deltaX = originalCenter.X - newCenter.X;
        float deltaY = originalCenter.Y - newCenter.Y;

        // ���X�g�X�V
        dotPositions.Clear();
        dotPositions.AddRange(rotationDots);

        // �u���b�N�S�̂𕽍s�ړ����Ē��S���W�ɂĕ␳
        for (int i = 0; i < dotPositions.Count; i++)
        {
            int x = (int)dotPositions[i].X + (int)deltaX;
            int y = (int)dotPositions[i].Y + (int)deltaY;
            dotPositions[i] = new Vector2(x, y);
        }

        // pivot�X�V
        pivots = dotPositions[0];

        // offset���l�������ʒu�Ɉړ�
        machine.Log("aa {0}, {1}", (int)pivots.X, (int)pivots.Y);

        int offsetX = FindCloseOffsetPosition(PivotX) - PivotX;
        int offsetY = FindCloseOffsetPosition(PivotY) - PivotY;


        for (int i = 0; i < dotPositions.Count; i++)
        {
            dotPositions[i] = new Vector2(dotPositions[i].X + offsetX, dotPositions[i].Y + offsetY);
            machine.Draw((int)dotPositions[i].X, (int)dotPositions[i].Y, setColor[0], setColor[1], setColor[2]);
        }

        // pivot�X�V
        pivots = dotPositions[0];
    }

    private Vector2 CalculateCenter(Vector2[] dotPositions)
    {
        float sumX = 0;
        float sumY = 0;

        foreach (Vector2 point in dotPositions)
        {
            sumX += point.X;
            sumY += point.Y;
        }

        float centerX = sumX / dotPositions.Length;
        float centerY = sumY / dotPositions.Length;

        return new Vector2(centerX, centerY);
    }

    // �����P
    public void Test_Rotation()
    {
        // �h�b�g����x����
        Erase();

        for (int i = 0; i < dotPositions.Count; i++)
        {
            Vector2 dot = dotPositions[i];

            // ���΍��W�ɕϊ�
            float relativeX = dot.X - pivots.X;
            float relativeY = dot.Y - pivots.Y;

            // ��]�s��K�p�i90�x��]�j
            float newX = -relativeY;
            float newY = relativeX;

            // ���̍��W�ɖ߂�
            newX += pivots.X;
            newY += pivots.Y;

            // �V�������W�Ƀh�b�g��`��
            machine.Draw((int)newX, (int)newY, setColor[0], setColor[1], setColor[2]);

            UpdateDotPositions(new Vector2(newX, newY), i);
        }

        // ����W���X�V����
        PivotX = (int)dotPositions[0].X;
        PivotY = (int)dotPositions[0].Y;
    }
}
