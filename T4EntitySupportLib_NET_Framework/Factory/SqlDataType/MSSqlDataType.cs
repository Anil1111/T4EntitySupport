namespace T4EntitySupportLib
{
    public class MSSqlDataType : ISqlDataType
    {
        public string ToCSharpType(string sqlDataType, bool isNullable)
        {
            switch (sqlDataType)
            {
                case "int": return isNullable ? "int?" : "int";
                case "nvarchar": return "string";
                case "char": return "string";
                case "datetime": return isNullable ? "DateTime?" : "DateTime";
                case "uniqueidentifier": return isNullable ? "Guid?" : "Guid";
                case "decimal": return isNullable ? "decimal?" : "decimal";
                case "float": return isNullable ? "float?" : "float";
                default:
                    return "string";
            }
        }
    }
}
