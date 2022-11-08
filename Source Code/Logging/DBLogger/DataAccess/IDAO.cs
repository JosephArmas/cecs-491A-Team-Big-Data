using Domain;

namespace DataAccess
{
    public interface IDAO
    {
        Task<Result> Execute(object req);
    }
}
