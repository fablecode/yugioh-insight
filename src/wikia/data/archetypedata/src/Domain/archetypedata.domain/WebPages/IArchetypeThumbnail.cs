using System.Threading.Tasks;

namespace archetypedata.domain.WebPages
{
    public interface IArchetypeThumbnail
    {
        Task<string> FromArticleId(int articleId);
        string FromWebPage(string url);
    }
}