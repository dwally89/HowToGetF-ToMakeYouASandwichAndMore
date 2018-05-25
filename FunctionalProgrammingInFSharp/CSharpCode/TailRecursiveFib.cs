

public class TailRecursiveFib
{
    public static int Fib(int n, int a = 0, int b = 1)
    {
        switch (n)
        {
            case 0:
                return a;
            case 1:
                return b;
            default:
                return Fib(n - 1, b, a + b);
        }
    }
}