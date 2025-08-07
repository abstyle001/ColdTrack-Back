namespace ColdTrack_Back.Utils;

public static class FeelTheBaseUtil
{
    /*
     * 三十六进制(string)转换十进制(int)
     */
    public static int ThirtyHexadecimalToDecimal(string value)
    {
        var result = 0;
        var mule = 1;
        for (int i = value.Length - 1; i >= 0; i--)
        {
            result += CharToInt(value[i]) * mule;
            mule *= 36;
        }

        return result;
    }

    /*
     * 十进制(int)转十六进制(string)
     */
    public static string DecimalToThirtyHexadecimal(int value)
    {
        if (value == 0) return "00";
        var result = "";
        while (value > 0)
        {
            var remainder = value % 36;
            var str = IntToString(remainder);
            result = string.Concat(str, result);
            value /= 36;
        }
        // 特殊需要，低于两位要补零，与系统规定seq一致
        if (result.Length < 2)
        {
            result = string.Concat("0", result);
        }
        return result;
    }

    /*
     * 单个36进制字符转十进制
     */
    private static int CharToInt(char value)
    {
        return value - '0' <= 9 ? value - '0' : value - 'A' + 10;
    }

    /*
     * 单个十进制转36进制字符
     */
    private static string IntToString(int value)
    {
        return value < 10 ? value.ToString() : ((char)('A' + value - 10)).ToString();
    }
}