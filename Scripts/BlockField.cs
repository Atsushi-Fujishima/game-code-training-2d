using System.Numerics;

public class BlockField
{
    private IMachine machine;

    // �t���[���̐F
    private readonly byte[] fieldColor = DotColor.color_White;

    public byte[] GetFieldColor()
    {
        return fieldColor;
    }

    public BlockField(IMachine machine)
    {
        this.machine = machine;
    }

    public void GenerateFrame()
    {
        // �g�̋N�_
        Vector2 starttingPoint = new Vector2(54, 10);

        // �z�u��
        int underSideSquareNumber = 10;
        int bothSideSquareNumber = 45;

        // �u���b�N�ړ���
        int moveValue = 9;

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
            Block block = new Block(machine);
            block.DrawSquare(x, (int)underStarttingPoint.Y, fieldColor);
        }

        // LeftSide
        for (int i = 0; i < bothSideSquareNumber; i++)
        {
            int y = (int)leftSideStarttingPoint.Y + (i * moveValue);
            Block block = new Block(machine);
            block.DrawSquare((int)leftSideStarttingPoint.X, y, fieldColor);
        }

        // RightSide
        for (int i = 0; i < bothSideSquareNumber; i++)
        {
            int y = (int)rightSideStarttingPoint.Y + (i * moveValue);
            Block block = new Block(machine);
            block.DrawSquare((int)rightSideStarttingPoint.X, y, fieldColor);
        }
    }
}
