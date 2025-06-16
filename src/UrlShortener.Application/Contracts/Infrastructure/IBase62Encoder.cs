namespace UrlShortener.Application.Contracts.Infrastructure
{
    public interface IBase62Encoder
    {
        string Encode(long number);
    }
}
