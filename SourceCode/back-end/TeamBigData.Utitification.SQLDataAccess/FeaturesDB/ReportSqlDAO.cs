using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB
{
    public class ReportsSqlDAO : DbContext, IReportsDBInserter, IReportsDBSelecter, IReportsDBUpdater, IReportsDBDeleter
    {
        private readonly string _connectionString;
        public ReportsSqlDAO(DbContextOptions<ReportsSqlDAO> options) : base(options)
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public ReportsSqlDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Response> DeleteUserReport()
        {
            Response result = new Response();
            return result;
        }
        public async Task<Response> UpdateFeedback()
        {
            Response result = new Response();
            return result;
        }

        public async Task<DataResponse<DataSet>> SelectUserReportsAsync(int user)
        {
            DataResponse<DataSet> result = new DataResponse<DataSet>();

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet set = new DataSet();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "PartitionSelectUserReports";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@reportedUser", user);

                        adapter.SelectCommand = command;

                        adapter.Fill(set, "dbo.Reports");

                    }
                    result.IsSuccessful = true;
                    result.Data = set;
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
            }
            return result;
        }

        public async Task<DataResponse<Tuple<double, int>>> SelectNewReputationAsync(Report report)
        {
            DataResponse<Tuple<double,int>> result = new DataResponse<Tuple<double,int>>();
            double newReputation = report.Rating;
            int numberOfReports = 1;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "NumberOfUserReports";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@reportedUser", report.ReportedUser);

                        using (SqlDataReader execute = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            while (await execute.ReadAsync())
                            {
                                var rating = execute.GetOrdinal("rating");
                                newReputation += Decimal.ToDouble(execute.GetDecimal(rating));

                                numberOfReports++;
                            }
                        }
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.Message;
                }
            }

            Tuple<double, int> updateReputation = new Tuple<double, int>(newReputation, numberOfReports);
            result.IsSuccessful = true;
            result.Data = updateReputation;

            return result;
        }

        public async Task<Response> InsertUserReportAsync(Report report)
        {
            Response result = new Response();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.AddWithValue("@rating", (Decimal)report.Rating);
                        command.Parameters.AddWithValue("@reportedUser", report.ReportedUser);
                        command.Parameters.AddWithValue("@reportingUser", report.ReportingUser);
                        command.Parameters.AddWithValue("@feedback", report.Feedback);
                        command.Parameters.AddWithValue("@createDate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@updateDate", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@lastModifierUser", report.ReportingUser);

                        command.Connection = connection;
                        command.CommandText = "InsertUserReport";
                        command.CommandType = CommandType.StoredProcedure;

                        int execute = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                        if (execute == 1)
                        {
                            result.IsSuccessful = true;
                        }
                    }
                }
                catch (SqlException s)
                {
                    result.ErrorMessage = s.Message;
                }
            }
            return result;
        }
    }
}
