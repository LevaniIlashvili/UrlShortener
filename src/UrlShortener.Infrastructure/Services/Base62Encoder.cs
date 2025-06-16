using System.Text;
using UrlShortener.Application.Contracts.Infrastructure;

namespace UrlShortener.Infrastructure.Services
{
    public class Base62Encoder : IBase62Encoder
    {
        private const string Base62Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string Encode(long number)
        {
            var result = new StringBuilder();

            while (number > 0)
            {
                result.Insert(0, Base62Chars[(int)(number % 62)]);
                number /= 62;
            }

            return result.ToString();
        }
    }
}
