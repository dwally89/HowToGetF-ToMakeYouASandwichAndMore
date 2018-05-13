

using System;
using System.Linq;

public class IfStatement
{
    public static bool IsInTheBeatles(string name) => 
        new[] { "John", "Paul", "George", "Ringo" }.Contains(name);

    public static bool MoreComplicatedFunction(string name) => false;

    public static void AnalyseDrummer(string drummerName)
    {
        bool isTheGreatest;
        if (drummerName == "Ringo")
        {
            if (IsInTheBeatles(drummerName))
            {
                isTheGreatest = false;
            }
            else
            {
                isTheGreatest = MoreComplicatedFunction(drummerName);
            }
        }
        else
        {
            isTheGreatest = MoreComplicatedFunction(drummerName);
        }

        if (isTheGreatest)
        {
            Console.WriteLine($"{drummerName} is the greatest drummer");
        }
    }
}
