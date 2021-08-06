namespace carddata.application.Configuration
{
    public record QueuesSetting
    {
        public string CardArticleQueue { get; init; }
        public string SemanticArticleQueue { get; init; }
    }
}