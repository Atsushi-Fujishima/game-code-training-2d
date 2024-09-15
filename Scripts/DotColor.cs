// ドット(ピクセル)の色
// 気軽に使ってね

using System.Linq;

public static class DotColor
{
    public static readonly byte[] color_Red = { 255, 0, 0 };
    public static readonly byte[] color_Green = { 0, 255, 0 };
    public static readonly byte[] color_Blue = { 0, 0, 255 };
    public static readonly byte[] color_LightBlue = { 175, 223, 228 };
    public static readonly byte[] color_Yeloow = { 255, 255, 0 };
    public static readonly byte[] color_Orange = { 255, 153, 51 };
    public static readonly byte[] color_Purple = { 204, 0, 204 };
    public static readonly byte[] color_Cyan = { 0, 174, 239 };
    public static readonly byte[] color_White = { 255, 255, 255 };
    public static readonly byte[] color_Black = { 0, 0, 0 };

    public static bool IsCompareColorAwithB(byte[] colorA, byte[] colorB)
    {
        if (colorA.SequenceEqual(colorB))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
