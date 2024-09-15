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
    // ブロックの種類
    private BlockType blockType = BlockType.O;

    // ブロックの回転基準座標
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

    // 1ブロックの大きさ = (squareLength * suareLength) * 4;
    // 1squareLength = 1ドット
    private readonly int squareLength = 8;

    // ドット描画座標
    private List<Vector2> dotPositions;

    // 隙間空ける補正値
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
        foreach (var dotPos in dotPositions)
        {
            machine.Draw((int)dotPos.X, (int)dotPos.Y, clearColor[0], clearColor[1], clearColor[2]);
        }
    }

    public bool PermitMove(int x, int y)
    {
        // 移動先のドット
        Vector2[] futurePosition = new Vector2[dotPositions.Count];

        // 移動先のドットを全て取得
        for (int i = 0; i < futurePosition.Length; i++)
        {
            futurePosition[i].X = dotPositions[i].X + x;
            futurePosition[i].Y = dotPositions[i].Y + y;

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

    public void Move(Vector2 vector)
    {
        // ドットを一度消す
        Erase();

        // ドットをvector分動かす
        for (int i = 0; i < dotPositions.Count; i++)
        {
            int newX = (int)dotPositions[i].X + (int)vector.X;
            int newY = (int)dotPositions[i].Y + (int)vector.Y;

            machine.Draw(newX, newY, setColor[0], setColor[1], setColor[2]);
            UpdateDotPositions(new Vector2(newX, newY), i);
        }

        // 基準座標を更新する
        PivotX = (int)dotPositions[0].X;
        PivotY = (int)dotPositions[0].Y;
    }

    // ドット配列の更新
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
        // ドットを一度消す
        Erase();

        for (int i = 0; i < dotPositions.Count; i++)
        {
            Vector2 dot = dotPositions[i];

            // 相対座標に変換
            float relativeX = dot.X - pivots.X;
            float relativeY = dot.Y - pivots.Y;

            // 回転行列適用（90度回転）
            float newX = relativeY;
            float newY = relativeX;

            // 元の座標に戻す
            newX += pivots.X;
            newY += pivots.Y;

            // 新しい座標にドットを描画
            machine.Draw((int)newX, (int)newY, setColor[0], setColor[1], setColor[2]);

            UpdateDotPositions(new Vector2(newX, newY), i);
        }

        // 基準座標を更新する
        PivotX = (int)dotPositions[0].X;
        PivotY = (int)dotPositions[0].Y;
    }

    // 実験3
    public void Test_Rotattion_9()
    {
        // 回転後の座標配列
        Vector2[] rotationDots = dotPositions.ToArray();

        // ドットを一度消す
        Erase();

        // 回転処理
        for (int i = 0; i < rotationDots.Length; i++)
        {
            Vector2 dot = rotationDots[i];

            // 相対座標に変換
            float relativeX = dot.X - PivotX;
            float relativeY = dot.Y - PivotY;

            // 回転行列適用（90度回転）
            float newX = -relativeY;
            float newY = relativeX;

            // 元の座標に戻す
            newX += PivotX;
            newY += PivotY;

            rotationDots[i] = new Vector2(newX, newY);
        }

        // リスト更新
        dotPositions.Clear();
        dotPositions.AddRange(rotationDots);

        // pivot更新
        pivots = dotPositions[0];

        // offsetを考慮した位置に移動
        machine.Log("aa {0}, {1}", (int)pivots.X, (int)pivots.Y);

        int offsetX = FindCloseOffsetPosition(PivotX) - PivotX;
        int offsetY = FindCloseOffsetPosition(PivotY) - PivotY;


        for (int i = 0; i < dotPositions.Count; i++)
        {
            dotPositions[i] = new Vector2(dotPositions[i].X + offsetX, dotPositions[i].Y + offsetY);
            machine.Draw((int)dotPositions[i].X, (int)dotPositions[i].Y, setColor[0], setColor[1], setColor[2]);
        }

        // pivot更新
        pivots = dotPositions[0];
    }

    private int FindCloseOffsetPosition(int i)
    {
        int closeOffset = (int)MathF.Round(i / offset) * offset;
        return closeOffset;
    }


    // 実験2
    public void Test_Rotation_center()
    {
        // 回転後の座標配列
        Vector2[] rotationDots = dotPositions.ToArray();

        // 現在のブロックの中心座標
        Vector2 originalCenter = CalculateCenter(dotPositions.ToArray());

        // ドットを一度消す
        Erase();

        // 回転処理
        for (int i = 0; i < rotationDots.Length; i++)
        {
            Vector2 dot = rotationDots[i];

            // 相対座標に変換
            float relativeX = dot.X - originalCenter.X;
            float relativeY = dot.Y - originalCenter.Y;

            // 回転行列適用（90度回転）
            float newX = -relativeY;
            float newY = relativeX;

            // 元の座標に戻す
            newX += originalCenter.X;
            newY += originalCenter.Y;

            rotationDots[i] = new Vector2(newX, newY);
        }

        // 回転後の中心座標
        Vector2 newCenter = CalculateCenter(rotationDots);

        //中心座標差分計算
        float deltaX = originalCenter.X - newCenter.X;
        float deltaY = originalCenter.Y - newCenter.Y;

        // リスト更新
        dotPositions.Clear();
        dotPositions.AddRange(rotationDots);

        // ブロック全体を平行移動して中心座標にて補正
        for (int i = 0; i < dotPositions.Count; i++)
        {
            int x = (int)dotPositions[i].X + (int)deltaX;
            int y = (int)dotPositions[i].Y + (int)deltaY;
            dotPositions[i] = new Vector2(x, y);
        }

        // pivot更新
        pivots = dotPositions[0];

        // offsetを考慮した位置に移動
        machine.Log("aa {0}, {1}", (int)pivots.X, (int)pivots.Y);

        int offsetX = FindCloseOffsetPosition(PivotX) - PivotX;
        int offsetY = FindCloseOffsetPosition(PivotY) - PivotY;


        for (int i = 0; i < dotPositions.Count; i++)
        {
            dotPositions[i] = new Vector2(dotPositions[i].X + offsetX, dotPositions[i].Y + offsetY);
            machine.Draw((int)dotPositions[i].X, (int)dotPositions[i].Y, setColor[0], setColor[1], setColor[2]);
        }

        // pivot更新
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

    // 実験１
    public void Test_Rotation()
    {
        // ドットを一度消す
        Erase();

        for (int i = 0; i < dotPositions.Count; i++)
        {
            Vector2 dot = dotPositions[i];

            // 相対座標に変換
            float relativeX = dot.X - pivots.X;
            float relativeY = dot.Y - pivots.Y;

            // 回転行列適用（90度回転）
            float newX = -relativeY;
            float newY = relativeX;

            // 元の座標に戻す
            newX += pivots.X;
            newY += pivots.Y;

            // 新しい座標にドットを描画
            machine.Draw((int)newX, (int)newY, setColor[0], setColor[1], setColor[2]);

            UpdateDotPositions(new Vector2(newX, newY), i);
        }

        // 基準座標を更新する
        PivotX = (int)dotPositions[0].X;
        PivotY = (int)dotPositions[0].Y;
    }
}
