using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace T4EntitySupportLib
{
    public class TableRepository
    {
        private string _connectionString = null;

        public TableRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<string> GetTables()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = string.Format(@"
                    SELECT IT.TABLE_NAME
                    FROM INFORMATION_SCHEMA.TABLES IT
                    WHERE TABLE_TYPE ='BASE TABLE'
                    order by IT.TABLE_NAME
                ");
                return conn.Query<string>(sql);
            }
        }

        public IEnumerable<Table> GetTableProperties(string TABLE_NAME)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = string.Format(@"
                    SELECT c.data_type,c.column_name,CASE WHEN pk.COLUMN_NAME IS NOT NULL THEN 'PRIMARY KEY' ELSE '' END AS key_type, c.is_nullable 
                    FROM INFORMATION_SCHEMA.COLUMNS c
                    LEFT JOIN (
                                SELECT ku.TABLE_CATALOG,ku.TABLE_SCHEMA,ku.TABLE_NAME,ku.COLUMN_NAME
                                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
                                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS ku
                                    ON tc.CONSTRAINT_TYPE = 'PRIMARY KEY' 
                                    AND tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
                             )   pk 
                    ON  c.TABLE_CATALOG = pk.TABLE_CATALOG
                                AND c.TABLE_SCHEMA = pk.TABLE_SCHEMA
                                AND c.TABLE_NAME = pk.TABLE_NAME
                                AND c.COLUMN_NAME = pk.COLUMN_NAME
                    WHERE c.TABLE_NAME = @TABLE_NAME
                    ORDER BY c.TABLE_SCHEMA,c.TABLE_NAME, c.ORDINAL_POSITION 
                ");
                return conn.Query<Table>(sql, new { TABLE_NAME });
            }
        }
    }
}
