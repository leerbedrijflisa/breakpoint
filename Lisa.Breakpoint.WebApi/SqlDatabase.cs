using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lisa.Breakpoint.WebApi
{
    public class SqlDatabase
    {
        private SqlConnection _conn;

        private const string ConnString = "Data Source=(localdb)\\ProjectsV12;Initial Catalog=BreakPoint;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private readonly string _selectSql = "SELECT * FROM dbo.bugs;";


        public IList<Bug> GetAllBugs()
        {
            var bugs = new List<Bug>();
            try
            {
                _conn = new SqlConnection(ConnString);
                _conn.Open();

                var cmd = new SqlCommand(_selectSql, _conn);
                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var bug = new Bug()
                    {
                        Id = rdr.IsDBNull(rdr.GetOrdinal("Id")) ? -1 : rdr.GetInt32(rdr.GetOrdinal("Id")),
                        Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? null : rdr.GetString(rdr.GetOrdinal("Title")),
                        Description = rdr.IsDBNull(rdr.GetOrdinal("Description")) ? null : rdr.GetString(rdr.GetOrdinal("Description")),
                        Status = rdr.IsDBNull(rdr.GetOrdinal("Status")) ? null : rdr.GetString(rdr.GetOrdinal("Status"))
                    };
                    bugs.Add(bug);
                }
            }
            finally
            {
                _conn?.Close();
            }
            return bugs;
        }

        public Bug GetBugById(int id)
        {
            Bug bug = null;
            try
            {
                _conn = new SqlConnection(ConnString);
                _conn.Open();

                const string sql = "SELECT * FROM dbo.bugs WHERE [Id] = @Id;";

                var cmd = new SqlCommand(sql, _conn);

                var paramId = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = id
                };
                cmd.Parameters.Add(paramId);

                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    bug = new Bug
                    {
                        Id = rdr.IsDBNull(rdr.GetOrdinal("Id")) ? -1 : rdr.GetInt32(rdr.GetOrdinal("Id")),
                        Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? null : rdr.GetString(rdr.GetOrdinal("Title")),
                        Description = rdr.IsDBNull(rdr.GetOrdinal("Description")) ? null : rdr.GetString(rdr.GetOrdinal("Description")),
                        Status = rdr.IsDBNull(rdr.GetOrdinal("Status")) ? null : rdr.GetString(rdr.GetOrdinal("Status"))
                    };
                }
            }
            finally
            {
                _conn?.Close();
            }
            return bug;
        }

        public long insertBug(Bug bug)
        {
            try
            {
                _conn = new SqlConnection(ConnString);

                var cmd = _conn.CreateCommand();

                cmd.CommandText =
                    @"INSERT INTO[dbo].[Bugs] (Title, Description, Status) 
                    VALUES(@Title, @Description,  @Status);SELECT CAST(scope_identity() AS int)";

                cmd.Parameters.Add("@Title", SqlDbType.VarChar);
                cmd.Parameters["@Title"].Value = bug.Title;

                cmd.Parameters.Add("@Description", SqlDbType.VarChar);
                cmd.Parameters["@Description"].Value = bug.Description;

                cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                cmd.Parameters["@Status"].Value = bug.Status;

                _conn.Open();

                return long.Parse(cmd.ExecuteScalar().ToString());
            }
            finally
            {
                _conn?.Close();
            }
        }


        public void updateBug(int id, Bug bug)
        {
            var bugToUpdate = GetBugById(id);
            if (bugToUpdate == null)
            {
                throw new Exception("Bug does not exist in database");
            }

            try
            {
                _conn = new SqlConnection(ConnString);

                var cmd = _conn.CreateCommand();
                cmd.CommandText = @"UPDATE Bugs SET [Title]=@paramTitle, 
                                                    [Description]=@paramDescription, 
                                                    [Status]=@paramStatus
                                                    WHERE Id=@Id";

                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;

                cmd.Parameters.Add("@paramTitle", SqlDbType.VarChar);
                cmd.Parameters["@paramTitle"].Value = bug.Title;

                cmd.Parameters.Add("@paramDescription", SqlDbType.VarChar);
                cmd.Parameters["@paramDescription"].Value = bug.Description;

                cmd.Parameters.Add("@paramStatus", SqlDbType.VarChar);
                cmd.Parameters["@paramStatus"].Value = bug.Status;

                _conn.Open();

                var number = cmd.ExecuteNonQuery();

                if (number != 1)
                {
                    throw new Exception($"No Bugs were updated with Id: {id}");
                }
            }
            finally
            {
                _conn?.Close();
            }
        }


        public void deleteBug(int id)
        {
            _conn = new SqlConnection(ConnString);

            var sqlComm = _conn.CreateCommand();
            sqlComm.CommandText = @"DELETE FROM bugs WHERE [ID] = @Id;";
            sqlComm.Parameters.Add("@Id", SqlDbType.Int);
            sqlComm.Parameters["@Id"].Value = id;

            _conn.Open();

            var rowsAffected = sqlComm.ExecuteNonQuery();

            _conn.Close();

            if (rowsAffected < 1)
            {
                throw new Exception("Entity has not been deleted!");
            }
        }
    }
}
