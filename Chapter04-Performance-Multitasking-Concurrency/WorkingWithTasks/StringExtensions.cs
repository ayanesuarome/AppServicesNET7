using System.Text.RegularExpressions;

namespace WorkingWithTasks
{
    public static class StringExtensions
    {
        public static Task<bool> IsValidXmlTagAsync(this string input)
        {
            if (input == null)
            {
                return Task.FromException<bool>(
                    new ArgumentNullException($"Missing {nameof(input)} parameter"));
            }

            if(input.Length == 0)
            {
                return Task.FromException<bool>(
                    new ArgumentException($"{nameof(input)} parameter is empty."));
            }

            return Task.FromResult(
                Regex.IsMatch(
                    input,
                    @"^<([a-z]+)([^<]+)*(?:>(.*)<\/\1>|\s+\/>)$"));
        }

    }
}
