using Domain;
using Microsoft.Data.SqlClient;

namespace DataAccess
{
    public class SqlDAO : IDAO
    {
        private string _connection;
        public SqlDAO(string connectionString)
        {
            _connection = connectionString;
        }

        public Task<Result> Execute(object req)
        {
            var tcs = new TaskCompletionSource<Result>();
            Result result = new Result();
            if (req.GetType() != typeof(string)) //Verifies if the parameter matches the acceptable string format type
            {
                result.ErrorMessage = "Error: input parameter for SqlDAO not of type string";
                result.IsSuccessful = false;
                tcs.SetResult(result);
                return tcs.Task;
            }
            using (SqlConnection connect = new SqlConnection(_connection.ToString()))
            {
                connect.Open();
                var resu = (new SqlCommand(req.ToString(), connect)).ExecuteNonQuery();
                result.Payload = resu;
            }
            if ((int)result.Payload == 1)
            {
                result.IsSuccessful = true;
            }
            else
            {
                result.IsSuccessful = false;
            }
            tcs.SetResult(result);
            return tcs.Task;
        }
    }
}