namespace System;



public static class StringExtensions
{
    public static bool IsPresent(this string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    public static string FirstLetterToUpper(this string str)
    {
        return str.Length switch
        {
            0 => str,
            1 => char.ToUpper(str[0]).ToString(),
            _ => char.ToUpper(str[0]) + str[1..]
        };
    }
}