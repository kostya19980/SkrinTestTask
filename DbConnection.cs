using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SkrinTestTask
{
    public class DbConnection
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        public DbConnection(string connetionString)
        {
            _connection = new SqlConnection(connetionString);
            _connection.Open();
        }
        public int InsertData(string queryString, SqlParameter[] values)
        {
            using (_command = new SqlCommand(queryString, _connection))
            {
                _command.Parameters.AddRange(values);
                var id = _command.ExecuteScalar();
                if (id == null)
                {
                    return 0;
                }
                return (int)id;
            }
        }
    }
}
