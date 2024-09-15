using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class BlockField
{
    private IMachine machine;

    // �z�u��
    private int underSideSquareNumber = 10;
    private int bothSideSquareNumber = 45;

    // �u���b�N�ړ���
    private int moveValue = 9;

    private int squareNum = 10;

    // �g�̋N�_
    private Vector2 starttingPoint = new Vector2(54, 10);

    // �t���[���̐F
    private readonly byte[] fieldColor = DotColor.color_White;

    // �͈͓��̋N�_���W
    private List<Vector2> inRangePivots = new List<Vector2>();

    // �`��p
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
        // �����̋N�_
        Vector2 underStarttingPoint = starttingPoint;

        // �����̋N�_
        Vector2 leftSideStarttingPoint = new Vector2((int)underStarttingPoint.X - moveValue, underStarttingPoint.Y);

        // �E���̋N�_
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

        // �g��
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
        // �������s�̍ŏ��l
        int minLine = 100;

        // �����F
        byte[] eraseColor = DotColor.color_Black;

        // Y���X�N�G�A���J��Ԃ�
        for (int i = 0; i < bothSideSquareNumber; i++)
        {
            // �s
            int gyou = (int)starttingPoint.Y + i * moveValue + moveValue;
            List<Vector2> vector2s = new List<Vector2>();

            // X���X�N�G�A���J��Ԃ�
            for (int n = 0; n < squareNum; n++)
            {
                // ��
                int retu = (int)starttingPoint.X + n * moveValue;

                // ���W�J���[�擾
                byte[] getColor = new byte[3];
                machine.GetPixel(out getColor[0], out getColor[1], out getColor[2], retu, gyou);
                
                // ���Ɣ��ȊO�Ȃ����
                if (DotColor.IsCompareColorAwithB(getColor, fieldColor) == false && 
                    DotColor.IsCompareColorAwithB(getColor, eraseColor) == false)
                {
                    vector2s.Add(new Vector2(retu, gyou));
                }
            }

            // i�s�̑S�Ă̗񂪍����ȊO�Ȃ����
            if (vector2s.Count == squareNum)
            {
                foreach (Vector2 vector2 in vector2s)
                {
                    // ����
                    block.DrawSquare((int)vector2.X, (int)vector2.Y, eraseColor);
                }

                // �������s�̍ŏ��l���X�V����
                minLine = (minLine > i) ? i : minLine;
                machine.Log("minLine{0}", minLine);
            }
        }

        //TODO : ��ŏC��
        // �������s�̍ŏ��l���傫���s�̑S�Ẵh�b�g������Ƃ�
        if (minLine != 100)
        {
            foreach (Vector2 v2 in inRangePivots)
            {
                // ���W�J���[�擾
                byte[] getColor = new byte[3];
                machine.GetPixel(out getColor[0], out getColor[1], out getColor[2], (int)v2.X, (int)v2.Y);

                block.DrawSquare((int)v2.X, (int)v2.Y -1, getColor);
            }
        }
    }
}
