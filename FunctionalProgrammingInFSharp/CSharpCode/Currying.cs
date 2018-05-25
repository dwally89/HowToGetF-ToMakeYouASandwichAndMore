

using System;

public class Currying
{
    public static Func<int, int> Add(int x) => y => x + y;
    public static Func<int, int> Multiply(int x) => y => x + y;

    public static int Add5(int y) => Add(5)(y);
    public static int Multiply7(int y) => Multiply(7)(y);

    public static int Add5ThenMultiplyBy7(int y) => Multiply7(Add5(y));

    public static Func<string, Func<int, Action<int>>> PrintResult(int x) =>
        op =>
            y =>
                result =>
                    Console.WriteLine($"{x} {op} {y} = {result}");

    public static Func<int, Action<int>> PrintAddResult(int x) => 
        y => result => PrintResult(x)("+")(y)(result);

    public static Action<int> AddAndPrintResult(int x) =>
        y => PrintAddResult(x)(y)(Add(x)(y));

    public static void Example()
    {
        var result1 = Add5ThenMultiplyBy7(15);
        AddAndPrintResult(3)(9);
    }
}

