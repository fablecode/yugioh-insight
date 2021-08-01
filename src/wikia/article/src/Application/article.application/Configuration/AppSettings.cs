namespace article.application.Configuration
{
    public record AppSettings
    {
        public string WikiaDomainUrl { get; init; }
        public string Category { get; init; }
        public int PageSize { get; init; }
    }
}