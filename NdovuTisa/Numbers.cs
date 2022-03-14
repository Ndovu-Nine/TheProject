using System;


namespace NdovuTisa
{
    [Obsolete]
    public static class Numbers
    {
        public static bool IsMultiple(this double a, double b)
        {
            var ck = a % b;
            return !(ck > 0);
        }
        public static bool IsMultiple(this int a, int b)
        {
            var ck = a % b;
            return !(ck > 0);
        }

        public static bool IsNumber(this string number)
        {
            try
            {
                var i = int.Parse(number);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static int ToInt(this double number)
        {
            return (int) Math.Round(number,MidpointRounding.AwayFromZero);
        }
    }
}
