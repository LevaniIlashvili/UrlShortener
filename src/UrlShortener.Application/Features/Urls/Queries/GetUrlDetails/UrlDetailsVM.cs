using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrlDetails
{
    public class UrlDetailsVM
    {
        public string ShortCode { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long ClickCount { get; set; }
        public bool IsActive { get; set; }
        public List<ClickAnalytics> ClickAnalytics { get; set; }
    }
}
