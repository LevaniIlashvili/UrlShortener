namespace UrlShortener.Domain.Entities
{
    public class ClickAnalytics
    {
        public string ShortCode { get; set; }
        public DateTime ClickDate { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
    }
}
