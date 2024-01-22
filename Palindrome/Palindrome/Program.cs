// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

Console.WriteLine("Hello, World!");
string input = Console.ReadLine().ToString();

Console.WriteLine(IsPalindrome(input).ToString());

bool IsPalindrome(string value)
{
    // a b c b a
    // 0, 1, 2
    for (int i = 0; i < value.Length / 2; i++)
    {
        Console.WriteLine(i);
        if (value[i] != value[value.Length - i - 1])
        {
            return false;
        }
    }

    return true;
}

//for (int i = 0; i < value.Length; i++)
//{
//    if (reverse[i] != value[i])
//    {
//        return false;
//    }
//}