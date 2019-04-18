namespace articledata.domain.Contracts
{
    public interface ISubmitArticle
    {
        int Id { get; set; }
        string Title { get; set; }
        string Url { get; set; }
    }

    public class SubmitArticle : ISubmitArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}