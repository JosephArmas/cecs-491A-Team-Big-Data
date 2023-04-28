using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB.Abstractions.Reports;
using TeamBigData.Utification.ErrorResponse;

namespace TeamBigData.Utification.SQLDataAccess.FeaturesDB
{
    public class ReportsSqlDAO : DbContext, IReportsDBInserter, IReportsDBSelecter
    {
        private readonly string _connectionString;
        public ReportsSqlDAO(DbContextOptions<ReportsSqlDAO> options) : base(options) 
        {
            _connectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public async Task<Response> SelectUserReportsAsync(UserProfile userProfile)
        {
            Response result = new Response();

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
                        command.Parameters.AddWithValue("@reportedUser", userProfile._userID);

                        adapter.SelectCommand = command;

                        adapter.Fill(set, "dbo.Reports");

                    }
                    result.isSuccessful = true;
                    result.data = set;
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
            }
            return result;
        }

        public async Task<Response> SelectNewReputationAsync(Report report)
        {
            Response result = new Response();
            double newReputation = report._rating;
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
                        command.Parameters.AddWithValue("@reportedUser", report._reportedUser);

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
                    result.errorMessage = s.Message;
                }
                catch (Exception e)
                {
                    result.errorMessage = e.Message;
                }
            }

            Tuple<double, int> updateReputation = new Tuple<double, int>(newReputation, numberOfReports);
            result.isSuccessful = true;
            result.data = updateReputation;

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
                        command.Parameters.AddWithValue("@rating", (Decimal)report._rating);
                        command.Parameters.AddWithValue("@reportedUser", report._reportedUser);
                        command.Parameters.AddWithValue("@reportingUser", report._reportingUser);
                        command.Parameters.AddWithValue("@feedback", report._feedback);
                        command.Parameters.AddWithValue("@createDate", DateTime.Now);
                        command.Parameters.AddWithValue("@updateDate", DateTime.Now);
                        command.Parameters.AddWithValue("@lastModifierUser", report._reportingUser);

                        command.Connection = connection;
                        command.CommandText = "InsertUserReport";
                        command.CommandType = CommandType.StoredProcedure;

                        int execute = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                        if (execute == 1)
                        {
                            result.data = 1;
                            result.isSuccessful = true;
                        }
                    }
                }
                catch (SqlException s)
                {
                    result.errorMessage = s.Message;
                }
            }
            return result;
        }
    }
}
