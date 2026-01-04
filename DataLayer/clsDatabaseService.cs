// Project: CodeGenerator.DataLayer
// Type: Class Library (.NET Framework 4.8)
// Namespace: CodeGenerator.DataLayer
// File: DatabaseService.cs

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace CodeGenerator.DataLayer
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }
    }

    public class ParameterInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Mode { get; set; } // IN, OUT, INOUT
    }

    public class clsDatabaseService
    {
        private const string QueryGetDatabases = "SELECT name FROM sys.databases WHERE database_id > 4 ORDER BY name;"; // Exclude system databases

        private const string QueryGetTables = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;";

        private const string QueryGetColumns = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName ORDER BY ORDINAL_POSITION;";

        private const string QueryGetStoredProcedureNames = "SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' ORDER BY ROUTINE_NAME;";

        private const string QueryGetStoredProcedureParameters = "SELECT PARAMETER_NAME, DATA_TYPE, PARAMETER_MODE FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME = @ProcName ORDER BY ORDINAL_POSITION;";

        /// <summary>
        /// Retrieves a list of all user databases from the SQL Server instance.
        /// </summary>
        /// <param name="masterConnectionString">Connection string to the master database.</param>
        /// <returns>List of database names.</returns>
        public List<string> GetAllDatabases(string masterConnectionString)
        {
            return ExecuteReader(masterConnectionString, QueryGetDatabases, null, reader =>
            {
                List<string> databases = new List<string>();
                while (reader.Read())
                {
                    databases.Add(reader["name"].ToString());
                }
                return databases;
            });
        }

        /// <summary>
        /// Retrieves a list of all table names in the specified database.
        /// </summary>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <returns>List of table names.</returns>
        public List<string> GetTables(string connectionString)
        {
            return ExecuteReader(connectionString, QueryGetTables, null, reader =>
            {
                List<string> tables = new List<string>();
                while (reader.Read())
                {
                    tables.Add(reader["TABLE_NAME"].ToString());
                }
                return tables;
            });
        }

        /// <summary>
        /// Retrieves column information for a specific table in the database.
        /// </summary>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>List of ColumnInfo objects.</returns>
        public List<ColumnInfo> GetColumns(string connectionString, string tableName)
        {
            var parameters = new Dictionary<string, object> { { "@TableName", tableName } };
            return ExecuteReader(connectionString, QueryGetColumns, parameters, reader =>
            {
                List<ColumnInfo> columns = new List<ColumnInfo>();
                while (reader.Read())
                {
                    columns.Add(new ColumnInfo
                    {
                        Name = reader["COLUMN_NAME"].ToString(),
                        DataType = reader["DATA_TYPE"].ToString()
                    });
                }
                return columns;
            });
        }

        /// <summary>
        /// Retrieves a list of all stored procedure names in the database.
        /// </summary>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <returns>List of stored procedure names.</returns>
        public List<string> GetStoredProcedureNames(string connectionString)
        {
            return ExecuteReader(connectionString, QueryGetStoredProcedureNames, null, reader =>
            {
                List<string> procedures = new List<string>();
                while (reader.Read())
                {
                    procedures.Add(reader["ROUTINE_NAME"].ToString());
                }
                return procedures;
            });
        }

        /// <summary>
        /// Retrieves parameter information for a specific stored procedure.
        /// </summary>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <param name="procName">Name of the stored procedure.</param>
        /// <returns>List of ParameterInfo objects.</returns>
        public List<ParameterInfo> GetStoredProcedureParameters(string connectionString, string procName)
        {
            var parameters = new Dictionary<string, object> { { "@ProcName", procName } };
            return ExecuteReader(connectionString, QueryGetStoredProcedureParameters, parameters, reader =>
            {
                List<ParameterInfo> paramsList = new List<ParameterInfo>();
                while (reader.Read())
                {
                    paramsList.Add(new ParameterInfo
                    {
                        Name = reader["PARAMETER_NAME"].ToString(),
                        DataType = reader["DATA_TYPE"].ToString(),
                        Mode = reader["PARAMETER_MODE"].ToString()
                    });
                }
                return paramsList;
            });
        }

        /// <summary>
        /// Generic method to execute a SQL query and process the results using a reader function.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="query">SQL query.</param>
        /// <param name="parameters">Dictionary of parameters (optional).</param>
        /// <param name="readerFunc">Function to process the SqlDataReader.</param>
        /// <returns>Result of type T.</returns>
        private T ExecuteReader<T>(string connectionString, string query, Dictionary<string, object> parameters, Func<SqlDataReader, T> readerFunc)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null) 
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return readerFunc(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In a real app, log the exception. For now, rethrow.
                throw new Exception($"Error executing query: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// جلب schema الأعمدة اللي بترجعها Stored Procedure (Result Set)
        /// </summary>
        public List<ColumnInfo> GetProcedureResultColumns(string connectionString, string procedureName)
        {
            string query = @"
        SET FMTONLY ON; 
        EXEC " + procedureName + ";  SET FMTONLY OFF; ";
        
    // طريقة أفضل وأدق باستخدام SqlCommand مع parameters فارغة
    // لكن FMTONLY قديم، هنستخدم طريقة أحدث وأكثر دقة

    var columns = new List<ColumnInfo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // أضف الباراميترات الفارغة عشان يقدر يعمل Derive Parameters
                    SqlCommandBuilder.DeriveParameters(cmd);

                    // إزالة الباراميترات عشان ما يشتكيش
                    cmd.Parameters.Clear();

                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        DataTable schema = reader.GetSchemaTable();
                        if (schema != null)
                        {
                            foreach (DataRow row in schema.Rows)
                            {
                                columns.Add(new ColumnInfo
                                {
                                    Name = row["ColumnName"].ToString(),
                                    DataType = row["DataTypeName"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return columns;
        }
    }
}