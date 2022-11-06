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

        public Result Execute(object req)
        {
            Result result = new Result();
            if (req.GetType() != typeof(string)) //Verifies if the parameter matches the acceptable string format type
            {
                result.ErrorMessage = "Error: input parameter for SqlDAO not of type string";
                result.IsSuccessful = false;
                return result;
            }
            Console.WriteLine("SqlDAO.Excute functions");
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
            return result;
        }
    }
}