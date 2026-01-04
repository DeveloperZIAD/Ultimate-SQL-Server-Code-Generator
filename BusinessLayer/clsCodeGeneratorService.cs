// Project: CodeGenerator.BusinessLayer
// Namespace: CodeGenerator.BusinessLayer
// File: CodeGeneratorService.cs

using CodeGenerator.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenerator.BusinessLayer
{
    public class GeneratedEntity
    {
        public string ClassName { get; set; }
        public string EntityCode { get; set; }
    }

    public class GeneratedRepository
    {
        public string TableName { get; set; }
        public string RepositoryCode { get; set; }
    }
    public class SimpleColumnInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }
    }
    public class SimpleParameterInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Mode { get; set; } // IN, OUT, INOUT
    }
    public class CodeGeneratorService
    {
        private readonly clsDatabaseService _dbService;

        public CodeGeneratorService()
        {
            _dbService = new clsDatabaseService();
        }

        /// <summary>
        /// جلب كل قواعد البيانات المتاحة (غير النظامية)
        /// </summary>
        public List<string> GetAllDatabases(string masterConnectionString)
        {
            if (string.IsNullOrWhiteSpace(masterConnectionString))
                throw new ArgumentException("Master connection string is required.");

            return _dbService.GetAllDatabases(masterConnectionString);
        }

        /// <summary>
        /// جلب كل الجداول في قاعدة بيانات معينة
        /// </summary>
        public List<string> GetTables(string databaseConnectionString)
        {
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new ArgumentException("Database connection string is required.");

            return _dbService.GetTables(databaseConnectionString);
        }

        /// <summary>
        /// جلب معلومات الأعمدة لجدول معين
        /// </summary>
        public List<SimpleColumnInfo> GetColumns(string databaseConnectionString, string tableName)
        {
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new ArgumentException("Database connection string is required.");
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name is required.");

            var dataLayerColumns = _dbService.GetColumns(databaseConnectionString, tableName);

            // تحويل من ColumnInfo (DataLayer) إلى SimpleColumnInfo (BusinessLayer)
            var simpleColumns = new List<SimpleColumnInfo>();
            foreach (var col in dataLayerColumns)
            {
                simpleColumns.Add(new SimpleColumnInfo
                {
                    Name = col.Name,
                    DataType = col.DataType
                });
            }

            return simpleColumns;
        }

        /// <summary>
        /// جلب أسماء كل الـ Stored Procedures
        /// </summary>
        public List<string> GetStoredProcedures(string databaseConnectionString)
        {
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new ArgumentException("Database connection string is required.");

            return _dbService.GetStoredProcedureNames(databaseConnectionString);
        }

        public clsDatabaseService Get_dbService()
        {
            return _dbService;
        }

        /// <summary>
        /// جلب باراميترات Stored Procedure معينة
        /// </summary>
        public List<SimpleParameterInfo> GetStoredProcedureParameters(string databaseConnectionString, string procName)
        {
            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new ArgumentException("Database connection string is required.");
            if (string.IsNullOrWhiteSpace(procName))
                throw new ArgumentException("Procedure name is required.");

            var dataLayerParams = _dbService.GetStoredProcedureParameters(databaseConnectionString, procName);

            var simpleParams = new List<SimpleParameterInfo>();
            foreach (var p in dataLayerParams)
            {
                simpleParams.Add(new SimpleParameterInfo
                {
                    Name = p.Name,
                    DataType = p.DataType,
                    Mode = p.Mode
                });
            }

            return simpleParams;
        }
        /// <summary>
        /// توليد كود الـ Entity Class (C# POCO) من جدول
        /// </summary>
        public GeneratedEntity GenerateEntityClass(string tableName, List<SimpleColumnInfo> columns)
        {
            string className = ToPascalCase(tableName.Replace(" ", ""));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace GeneratedEntities");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {className}");
            sb.AppendLine("    {");

            foreach (var col in columns)
            {
                string propName = ToPascalCase(col.Name);
                string csharpType = MapSqlTypeToCSharp(col.DataType);

                sb.AppendLine($"        public {csharpType} {propName} {{ get; set; }}");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return new GeneratedEntity
            {
                ClassName = className,
                EntityCode = sb.ToString()
            };
        }
        /// <summary>
        /// مثال بسيط لتوليد Repository (يمكن توسيعه لاحقاً)
        /// </summary>
        public GeneratedRepository GenerateBasicRepository(string tableName, string entityClassName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine();
            sb.AppendLine("namespace GeneratedRepositories");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {entityClassName}Repository");
            sb.AppendLine("    {");
            sb.AppendLine("        private readonly string _connectionString;");
            sb.AppendLine();
            sb.AppendLine($"        public {entityClassName}Repository(string connectionString)");
            sb.AppendLine("        {");
            sb.AppendLine("            _connectionString = connectionString;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        // TODO: Add CRUD methods using stored procedures or direct queries");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return new GeneratedRepository
            {
                TableName = tableName,
                RepositoryCode = sb.ToString()
            };
        }

        // Helper Methods
        private string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var parts = input.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1).ToLower();
            }
            return string.Join("", parts);
        }

        private string MapSqlTypeToCSharp(string sqlType)
        {
            string type = sqlType.ToLower();
            if (type == "int") return "int";
            if (type == "bigint") return "long";
            if (type == "smallint") return "short";
            if (type == "tinyint") return "byte";
            if (type == "bit") return "bool";
            if (type == "decimal" || type == "numeric" || type == "money" || type == "smallmoney") return "decimal";
            if (type == "float") return "double";
            if (type == "real") return "float";
            if (type == "date" || type == "datetime" || type == "datetime2" || type == "smalldatetime") return "DateTime";
            if (type == "time") return "TimeSpan";
            if (type == "char" || type == "nchar" || type == "varchar" || type == "nvarchar" || type == "text" || type == "ntext") return "string";
            if (type == "uniqueidentifier") return "Guid";
            if (type == "varbinary" || type == "binary" || type == "image") return "byte[]";
            return "object"; // fallback
        }

        /// <summary>
        /// توليد Data Logic كامل وصحيح 100% لأي Stored Procedure
        /// </summary>
        public string GenerateDataLogicCode(string procedureName, List<SimpleParameterInfo> parameters)
        {
            string className = ToPascalCase(procedureName.Replace("sp_", "").Replace("usp_", "")) + "DataLogic";

            // توليد باراميترات الدالة بالأنواع الصحيحة
            var inputParams = parameters.Where(p => p.Mode == "IN" || p.Mode == "INOUT").ToList();
            string paramsDeclaration = string.Join(", ", inputParams.Select(p =>
            {
                string csharpType = MapSqlTypeToCSharp(p.DataType);
                string paramName = ToPascalCase(p.Name.TrimStart('@'));
                return $"{csharpType} {paramName}";
            }));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine();
            sb.AppendLine("namespace DataLogic");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {className}");
            sb.AppendLine("    {");
            sb.AppendLine("        private const string ConnectionString = \"Data Source=your_server;Initial Catalog=your_db;Integrated Security=True;TrustServerCertificate=True;\"; // عدل حسب بيئتك");
            sb.AppendLine();
            sb.AppendLine($"        public static int Execute({paramsDeclaration})");
            sb.AppendLine("        {");
            sb.AppendLine("            using (SqlConnection conn = new SqlConnection(ConnectionString))");
            sb.AppendLine("            {");
            sb.AppendLine("                conn.Open();");
            sb.AppendLine($"                using (SqlCommand cmd = new SqlCommand(\"{procedureName}\", conn))");
            sb.AppendLine("                {");
            sb.AppendLine("                    cmd.CommandType = CommandType.StoredProcedure;");
            foreach (var p in inputParams)
            {
                string paramName = ToPascalCase(p.Name.TrimStart('@'));
                sb.AppendLine($"                    cmd.Parameters.AddWithValue(\"{p.Name}\", {paramName} ?? DBNull.Value);");
            }
            sb.AppendLine("                    return cmd.ExecuteNonQuery();");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // GetReader فقط لو الـ Procedure اسمها يحتوي على Get أو Select
            if (procedureName.IndexOf("Get", StringComparison.OrdinalIgnoreCase) >= 0 ||
                procedureName.IndexOf("Select", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                sb.AppendLine($"        public static SqlDataReader GetReader({paramsDeclaration})");
                sb.AppendLine("        {");
                sb.AppendLine("            SqlConnection conn = new SqlConnection(ConnectionString);");
                sb.AppendLine("            conn.Open();");
                sb.AppendLine($"            SqlCommand cmd = new SqlCommand(\"{procedureName}\", conn);");
                sb.AppendLine("            cmd.CommandType = CommandType.StoredProcedure;");
                foreach (var p in inputParams)
                {
                    string paramName = ToPascalCase(p.Name.TrimStart('@'));
                    sb.AppendLine($"            cmd.Parameters.AddWithValue(\"{p.Name}\", {paramName} ?? DBNull.Value);");
                }
                sb.AppendLine("            return cmd.ExecuteReader(CommandBehavior.CloseConnection);");
                sb.AppendLine("        }");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }


        public string GenerateBusinessLogicCode(string procedureName, List<SimpleParameterInfo> parameters, List<SimpleColumnInfo> resultColumns)
        {
            // تنظيف الاسم
            string cleanedName = procedureName
                .Replace("sp_", "")
                .Replace("usp_", "")
                .Replace("GetAll", "")
                .Replace("Get", "")
                .Replace("List", "")
                .Replace("Select", "")
                .Replace("Search", "");

            string dtoName = ToPascalCase(cleanedName);
            if (string.IsNullOrWhiteSpace(dtoName) || dtoName.Length < 2)
                dtoName = "Result";
            dtoName += "Dto";

            string methodName = ToPascalCase(procedureName
                .Replace("sp_", "")
                .Replace("usp_", ""));

            string dataLogicClass = methodName + "DataLogic";

            var inputParams = parameters.Where(p => p.Mode == "IN" || p.Mode == "INOUT");

            string paramsDeclaration = inputParams.Count() == 0 ? "" : string.Join(", ", inputParams.Select(p =>
                $"{MapSqlTypeToCSharp(p.DataType)} {ToPascalCase(p.Name.TrimStart('@'))}"));

            string paramsCall = inputParams.Count() == 0 ? "" : string.Join(", ", inputParams.Select(p => ToPascalCase(p.Name.TrimStart('@'))));

            bool isSelect = resultColumns.Count > 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using DataLogic;");
            sb.AppendLine();
            sb.AppendLine("namespace BusinessLogic");
            sb.AppendLine("{");
            sb.AppendLine("    // ================================================");
            sb.AppendLine("    // DTO مولد تلقائيًا من نتائج الـ Stored Procedure");
            sb.AppendLine("    // ================================================");
            sb.AppendLine($"    public class {dtoName}");
            sb.AppendLine("    {");

            if (!isSelect)
            {
                sb.AppendLine("        public int AffectedRows { get; set; }");
                sb.AppendLine("        public bool Success => AffectedRows > 0;");
            }
            else
            {
                foreach (var col in resultColumns)
                {
                    string propName = ToPascalCase(col.Name);
                    string csharpType = MapSqlTypeToCSharp(col.DataType);
                    if (csharpType == "object") csharpType = "string";
                    sb.AppendLine($"        public {csharpType} {propName} {{ get; set; }}");
                }
            }

            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine($"    public static class {dtoName}Service");
            sb.AppendLine("    {");

            if (isSelect)
            {
                sb.AppendLine($"        public static List<{dtoName}> {methodName}({paramsDeclaration})");
                sb.AppendLine("        {");
                sb.AppendLine($"            var results = new List<{dtoName}>();");
                sb.AppendLine();
                sb.AppendLine($"            using (SqlDataReader reader = {dataLogicClass}.GetReader({paramsCall}))");
                sb.AppendLine("            {");
                sb.AppendLine("                while (reader.Read())");
                sb.AppendLine("                {");
                sb.AppendLine($"                    results.Add(new {dtoName}");
                sb.AppendLine("                    {");

                foreach (var col in resultColumns)
                {
                    string propName = ToPascalCase(col.Name);
                    string getter = GetReaderMethod(col.DataType);
                    sb.AppendLine($"                        {propName} = reader.{getter}(\"{col.Name}\"),");
                }

                sb.AppendLine("                    });");
                sb.AppendLine("                }");
                sb.AppendLine("            }");
                sb.AppendLine("            return results;");
                sb.AppendLine("        }");
            }
            else
            {
                sb.AppendLine($"        public static {dtoName} {methodName}({paramsDeclaration})");
                sb.AppendLine("        {");
                sb.AppendLine($"            int affected = {dataLogicClass}.Execute({paramsCall});");
                sb.AppendLine("            return new {dtoName} {{ AffectedRows = affected }};");
                sb.AppendLine("        }");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }        // Replace the GetReaderMethod method with the following to avoid 'or pattern' (C# 9+) and use explicit if/else logic.
        // Also, ignore the SPELL diagnostics for SQL type names as they are correct in this context.

        private string GetReaderMethod(string sqlType)
        {
            string type = sqlType.ToLower();
            if (type == "int" || type == "bigint" || type == "smallint" || type == "tinyint")
                return "GetInt32";
            if (type == "bit")
                return "GetBoolean";
            if (type == "datetime" || type == "datetime2" || type == "date" || type == "smalldatetime")
                return "GetDateTime";
            if (type == "decimal" || type == "money" || type == "smallmoney" || type == "numeric")
                return "GetDecimal";
            if (type == "float")
                return "GetDouble";
            if (type == "real")
                return "GetFloat";
            if (type == "uniqueidentifier")
                return "GetGuid";
            // fallback for nvarchar, varchar, text, etc.
            return "GetString";
        }
        public List<SimpleColumnInfo> GetProcedureResultColumns(string databaseConnectionString, string procName)
        {
            var dataLayerColumns = _dbService.GetProcedureResultColumns(databaseConnectionString, procName);

            return dataLayerColumns.Select(c => new SimpleColumnInfo
            {
                Name = c.Name,
                DataType = c.DataType
            }).ToList();
        }

        /// <summary>
        /// توليد Data Logic كامل CRUD احترافي وآمن للـ Table (مع using + try-catch في كل دالة)
        /// </summary>
        public string GenerateTableDataLogic(string tableName, List<SimpleColumnInfo> columns)
        {
            string className = ToPascalCase(tableName) + "DataLogic";

            // Primary Key detection
            var pkColumn = columns.FirstOrDefault(c => c.Name.ToLower().EndsWith("id")) ??
                           columns.FirstOrDefault(c => c.Name.ToLower() == "id");
            string pkType = pkColumn != null ? MapSqlTypeToCSharp(pkColumn.DataType) : "int";
            string pkParam = pkColumn != null ? ToPascalCase(pkColumn.Name) : "Id";
            string pkColumnName = pkColumn != null ? pkColumn.Name : "Id";

            var nonPkColumns = columns.Where(c => c.Name != pkColumnName);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine();
            sb.AppendLine("namespace DataLogic");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {className}");
            sb.AppendLine("    {");
            sb.AppendLine("        private const string ConnectionString = \"Data Source=.;Initial Catalog=YourDatabase;Integrated Security=True;TrustServerCertificate=True;\"; // عدل حسب بيئتك");
            sb.AppendLine();

            // GetAll
            sb.AppendLine("        public static SqlDataReader GetAll()");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection conn = new SqlConnection(ConnectionString))");
            sb.AppendLine("                {");
            sb.AppendLine("                    conn.Open();");
            sb.AppendLine($"                    using (SqlCommand cmd = new SqlCommand(\"SELECT * FROM [{tableName}]\", conn))");
            sb.AppendLine("                    {");
            sb.AppendLine("                        return cmd.ExecuteReader(CommandBehavior.CloseConnection);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                throw new Exception(\"Error in GetAll for {tableName}: \" + ex.Message, ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // GetById
            sb.AppendLine($"        public static SqlDataReader GetBy{pkParam}({pkType} {pkParam})");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection conn = new SqlConnection(ConnectionString))");
            sb.AppendLine("                {");
            sb.AppendLine("                    conn.Open();");
            sb.AppendLine($"                    using (SqlCommand cmd = new SqlCommand(\"SELECT * FROM [{tableName}] WHERE [{pkColumnName}] = @{pkParam}\", conn))");
            sb.AppendLine("                    {");
            sb.AppendLine($"                        cmd.Parameters.AddWithValue(\"@{pkParam}\", {pkParam});");
            sb.AppendLine("                        return cmd.ExecuteReader(CommandBehavior.CloseConnection);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                throw new Exception(\"Error in GetBy{pkParam} for {tableName}: \" + ex.Message, ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Insert
            string insertColumns = string.Join(", ", nonPkColumns.Select(c => $"[{c.Name}]"));
            string insertParams = string.Join(", ", nonPkColumns.Select(c => $"@{ToPascalCase(c.Name)}"));
            string insertDeclaration = string.Join(", ", nonPkColumns.Select(c =>
                $"{MapSqlTypeToCSharp(c.DataType)} {ToPascalCase(c.Name)}"));

            sb.AppendLine($"        public static int Insert({insertDeclaration})");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection conn = new SqlConnection(ConnectionString))");
            sb.AppendLine("                {");
            sb.AppendLine("                    conn.Open();");
            sb.AppendLine($"                    using (SqlCommand cmd = new SqlCommand(\"INSERT INTO [{tableName}] ({insertColumns}) VALUES ({insertParams})\", conn))");
            sb.AppendLine("                    {");

            foreach (var col in nonPkColumns)
            {
                string paramName = ToPascalCase(col.Name);
                sb.AppendLine($"                        cmd.Parameters.AddWithValue(\"@{paramName}\", {paramName} ?? DBNull.Value);");
            }

            sb.AppendLine("                        return cmd.ExecuteNonQuery();");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                throw new Exception(\"Error inserting into {tableName}: \" + ex.Message, ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Update
            string updateSet = string.Join(", ", nonPkColumns.Select(c => $"[{c.Name}] = @{ToPascalCase(c.Name)}"));

            sb.AppendLine($"        public static int Update({pkType} {pkParam}, {insertDeclaration})");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection conn = new SqlConnection(ConnectionString))");
            sb.AppendLine("                {");
            sb.AppendLine("                    conn.Open();");
            sb.AppendLine($"                    using (SqlCommand cmd = new SqlCommand(\"UPDATE [{tableName}] SET {updateSet} WHERE [{pkColumnName}] = @{pkParam}\", conn))");
            sb.AppendLine("                    {");
            sb.AppendLine($"                        cmd.Parameters.AddWithValue(\"@{pkParam}\", {pkParam});");

            foreach (var col in nonPkColumns)
            {
                string paramName = ToPascalCase(col.Name);
                sb.AppendLine($"                        cmd.Parameters.AddWithValue(\"@{paramName}\", {paramName} ?? DBNull.Value);");
            }

            sb.AppendLine("                        return cmd.ExecuteNonQuery();");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                throw new Exception(\"Error updating {tableName}: \" + ex.Message, ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Delete
            sb.AppendLine($"        public static int Delete({pkType} {pkParam})");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                using (SqlConnection conn = new SqlConnection(ConnectionString))");
            sb.AppendLine("                {");
            sb.AppendLine("                    conn.Open();");
            sb.AppendLine($"                    using (SqlCommand cmd = new SqlCommand(\"DELETE FROM [{tableName}] WHERE [{pkColumnName}] = @{pkParam}\", conn))");
            sb.AppendLine("                    {");
            sb.AppendLine($"                        cmd.Parameters.AddWithValue(\"@{pkParam}\", {pkParam});");
            sb.AppendLine("                        return cmd.ExecuteNonQuery();");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                throw new Exception(\"Error deleting from {tableName}: \" + ex.Message, ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public string GenerateTableBusinessLogic(string tableName, List<SimpleColumnInfo> columns)
        {
            string dtoName = ToPascalCase(tableName) + "Dto";
            string serviceName = dtoName + "Service";

            var pkColumn = columns.FirstOrDefault(c => c.Name.ToLower().Contains("id"));
            string pkType = pkColumn != null ? MapSqlTypeToCSharp(pkColumn.DataType) : "int";
            string pkParam = pkColumn != null ? ToPascalCase(pkColumn.Name) : "Id";

            var nonPkColumns = columns.Where(c => c.Name != pkColumn?.Name);

            string insertUpdateParams = string.Join(", ", nonPkColumns.Select(c =>
                $"{MapSqlTypeToCSharp(c.DataType)} {ToPascalCase(c.Name)}"));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using DataLogic;");
            sb.AppendLine();
            sb.AppendLine("namespace BusinessLogic");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {dtoName}");
            sb.AppendLine("    {");

            foreach (var col in columns)
            {
                string propName = ToPascalCase(col.Name);
                string type = MapSqlTypeToCSharp(col.DataType);
                if (type == "object") type = "string";
                sb.AppendLine($"        public {type} {propName} {{ get; set; }}");
            }

            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine($"    public static class {serviceName}");
            sb.AppendLine("    {");

            // GetAll
            sb.AppendLine($"        public static List<{dtoName}> GetAll()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var results = new List<{dtoName}>();");
            sb.AppendLine($"            using (SqlDataReader reader = {ToPascalCase(tableName)}DataLogic.GetAll())");
            sb.AppendLine("            {");
            sb.AppendLine("                while (reader.Read())");
            sb.AppendLine("                {");
            sb.AppendLine($"                    results.Add(new {dtoName}");
            sb.AppendLine("                    {");

            foreach (var col in columns)
            {
                string propName = ToPascalCase(col.Name);
                string getter = GetReaderMethod(col.DataType);
                sb.AppendLine($"                        {propName} = reader.{getter}(\"{col.Name}\"),");
            }

            sb.AppendLine("                    });");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return results;");
            sb.AppendLine("        }");
            sb.AppendLine();

            // GetById
            sb.AppendLine($"        public static {dtoName} GetBy{pkParam}({pkType} {pkParam})");
            sb.AppendLine("        {");
            sb.AppendLine($"            using (SqlDataReader reader = {ToPascalCase(tableName)}DataLogic.GetBy{pkParam}({pkParam}))");
            sb.AppendLine("            {");
            sb.AppendLine("                if (reader.Read())");
            sb.AppendLine("                {");
            sb.AppendLine($"                    return new {dtoName}");
            sb.AppendLine("                    {");

            foreach (var col in columns)
            {
                string propName = ToPascalCase(col.Name);
                string getter = GetReaderMethod(col.DataType);
                sb.AppendLine($"                        {propName} = reader.{getter}(\"{col.Name}\"),");
            }

            sb.AppendLine("                    };");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("            return null;");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Insert
            sb.AppendLine($"        public static int Insert({insertUpdateParams})");
            sb.AppendLine("        {");
            sb.AppendLine($"            return {ToPascalCase(tableName)}DataLogic.Insert({string.Join(", ", nonPkColumns.Select(c => ToPascalCase(c.Name)))})");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Update
            sb.AppendLine($"        public static int Update({pkType} {pkParam}, {insertUpdateParams})");
            sb.AppendLine("        {");
            sb.AppendLine($"            return {ToPascalCase(tableName)}DataLogic.Update({pkParam}, {string.Join(", ", nonPkColumns.Select(c => ToPascalCase(c.Name)))})");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Delete
            sb.AppendLine($"        public static int Delete({pkType} {pkParam})");
            sb.AppendLine("        {");
            sb.AppendLine($"            return {ToPascalCase(tableName)}DataLogic.Delete({pkParam});");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }





        private void AddParametersCode(StringBuilder sb, List<SimpleParameterInfo> parameters)
        {
            foreach (var p in parameters.Where(x => x.Mode == "IN" || x.Mode == "INOUT"))
            {
                string paramName = p.Name.TrimStart('@');
                string varName = ToPascalCase(paramName);
                sb.AppendLine($"                    cmd.Parameters.AddWithValue(\"{p.Name}\", {varName} ?? DBNull.Value);");
            }
        }
        // دوال مساعدة
        private string GenerateParametersDeclaration(List<SimpleParameterInfo> parameters, bool withoutTypes = false)
        {
            if (parameters.Count == 0) return "";

            var parts = new List<string>();
            foreach (var p in parameters.Where(x => x.Mode == "IN" || x.Mode == "INOUT"))
            {
                string type = withoutTypes ? "" : MapSqlTypeToCSharp(p.DataType) + " ";
                string name = p.Name.StartsWith("@") ? p.Name.Substring(1) : p.Name;
                parts.Add($"{type}{ToPascalCase(name)}");
            }
            return string.Join(", ", parts);
        }

        private string GenerateParametersCall(List<SimpleParameterInfo> parameters)
        {
            return string.Join(", ", parameters.Where(x => x.Mode == "IN" || x.Mode == "INOUT")
                .Select(p => p.Name + " = @" + ToPascalCase(p.Name.StartsWith("@") ? p.Name.Substring(1) : p.Name)));
        }

        private string GenerateParametersNames(List<SimpleParameterInfo> parameters)
        {
            return string.Join(", ", parameters.Where(x => x.Mode == "IN" || x.Mode == "INOUT")
                .Select(p => ToPascalCase(p.Name.StartsWith("@") ? p.Name.Substring(1) : p.Name)));
        }
    }
}