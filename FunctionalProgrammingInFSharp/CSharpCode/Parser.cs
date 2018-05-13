

using System;

namespace CSharp
{
    public class Parser
    {
        public static bool TryParseInt(string value, out int i)
        {
            i = 0;

            try
            {
                i = int.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ReadInt()
        {
            var line = Console.ReadLine();
            if (TryParseInt(line, out var i))
            {
                Console.WriteLine($"Successfully read integer: {i}");
            }
            else
            {
                Console.WriteLine("Failed to read integer");
            }
        }
    }
}