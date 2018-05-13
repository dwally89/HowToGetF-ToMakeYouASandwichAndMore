

public class FibLoop
{
    public static int Fib(int n)
    {
        var a = 0;
        var b = 1;
        while (n > 1)
        {
            n = n - 1;
            var temp = a;
            a = b;
            b = temp + b;
        }

        return n == 0 ? a : b;
    }
}