using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.SQLDataAccess.Abstractions;

namespace TeamBigData.Utification.SQLDataAccess
{

    public class SqlDAOFactory
    {
        /*private readonly SqlDAO _sqlDAO;
        public SqlDAOFactory(SqlDAO sqlDAO) 
        { 
            _sqlDAO = sqlDAO;
        }*/

        public DbContextOptions<SqlDAO> CreateDBInserter(String connection)
        {
            DbContextOptionsBuilder<SqlDAO> options = new DbContextOptionsBuilder<SqlDAO>();
            options.UseSqlServer(connection);
            IDBInserter inserter = new SqlDAO(options.Options);
            return options.Options;
        }

        public DbContextOptions<SqlDAO> CreateDBSelecter(String connection)
        {
            DbContextOptionsBuilder<SqlDAO> options = new DbContextOptionsBuilder<SqlDAO>();
            options.UseSqlServer(connection);
            IDBSelecter selecter = new SqlDAO(options.Options);
            return options.Options;
        }
    }
}
