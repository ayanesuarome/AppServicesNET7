using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringApp
{
    public partial class Program
    {
        public static void SectionTitle(string title)
        {
            ConsoleColor previousColor = ForegroundColor;
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("*");
            WriteLine($"* {title}");
            WriteLine("*");
            ForegroundColor = previousColor;
        }
    }
}
