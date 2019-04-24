namespace T4EntitySupportLib
{
    public interface ISqlDataType
    {
        string ToCSharpType(string sqlDataType, bool isNullable);
    }
}
