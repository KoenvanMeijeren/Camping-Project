namespace SystemCore
{
    public static class Validation
    {
        public static bool IsInputFilled(string input)
        {
            return (!string.IsNullOrEmpty(input) && input.Length != 0);
        }
    }
}