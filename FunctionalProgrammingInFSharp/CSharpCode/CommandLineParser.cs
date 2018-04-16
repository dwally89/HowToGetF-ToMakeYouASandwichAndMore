using System;

namespace Parser
{
    public class CommandLineParser
    {
        public static T GetArgument<T>(string prompt, Func<string, T> parser)
        {
            Console.Write(prompt);
            return parser(Console.ReadLine());
        }

        public static int GetInteger()
        {
            return GetArgument("Enter an integer: ", int.Parse);
        }
    }
}