namespace UrlShortener.Domain.Entities
{
    public class Url
    {
        public string ShortCode { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long ClickCount { get; set; }
        public bool IsActive { get; set; }
    }
}
