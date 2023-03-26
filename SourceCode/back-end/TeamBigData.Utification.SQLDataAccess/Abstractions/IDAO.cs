using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess
{
    public interface IDAO
    {
        Task<Response> Execute(object req);
    }
}
