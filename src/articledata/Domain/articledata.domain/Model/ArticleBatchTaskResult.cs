namespace articledata.domain.Model
{
    public class ArticleBatchTaskResult
    {
        public string Category { get; set; }
        public int Processed { get; set; }
        public int Failed { get; set; }
    }
}