using System.Numerics;

public class BlockField
{
    private IMachine machine;

    // フレームの色
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
        // 枠の起点
        Vector2 starttingPoint = new Vector2(54, 10);

        // 配置数
        int underSideSquareNumber = 10;
        int bothSideSquareNumber = 45;

        // ブロック移動量
        int moveValue = 9;

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
