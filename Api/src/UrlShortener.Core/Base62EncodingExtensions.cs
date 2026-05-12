namespace UrlShortener.Core;

public static class Base62EncodingExtensions
{
    private const string AlphaNumeric =
        "0123456789" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz";

    extension(int number)
    {
        public string EncodeToBase62()
        {
            if (number == 0)
            {
                return AlphaNumeric[0].ToString();
            }

            var result = new Stack<char>();
            
            while (number > 0)
            {
                var index = number % 62;
                result.Push(AlphaNumeric[index]);
                number /= 62;
            }

            return new string(result.ToArray());
        }
    }
}