namespace AnResh.HelperFunctions
{
    public class Math
    {
        public static int Ceil(int dividend, int divisor)
        {
            var result = dividend / divisor;

            if (dividend % divisor != 0)
                result++;

            return result;
        }
    }
}