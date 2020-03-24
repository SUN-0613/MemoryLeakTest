using System;

/// <summary>乱数生成</summary>
public static class RandomValue
{

    /// <summary>乱数ジェネレータの取得</summary>
    /// <param name="seed">乱数既定値</param>
    /// <returns>乱数ジェネレータ</returns>
    public static Random GetValue(int seed = -1)
    {

        if (!seed.Equals(-1))
        {
            return new Random(seed);
        }
        else if (int.TryParse(DateTime.Now.Ticks.ToString(), out var num))
        {
            return new Random(num);
        }
        else
        {
            return new Random(DateTime.Now.Millisecond);
        }

    }

}
