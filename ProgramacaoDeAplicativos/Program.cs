using System.Reflection;
using static System.Console;
using System.Data;
using System.Data.SqlClient;



ClientTable client = new ClientTable();
var assembly = Assembly.GetExecutingAssembly();

foreach (var type in assembly.GetTypes())
{

    if (type.GetCustomAttribute<TableAttribute>() != null)
    {
        string comando = "";

        comando = "CREATE TABLE " + type.Name + " (";
        List<string> collums = new List<string>();
        List<string> propriedades = new List<string>();

        foreach (var prop in type.GetProperties())
        {
            comando += " " + prop.Name;
            var typeInt = prop.GetCustomAttribute<IntAttribute>();
            if (typeInt != null) comando += " int";


            var typeatt = prop.GetCustomAttribute<VarCharAttribute>();
            if (typeatt != null)
            {
                comando += " VARCHAR(" + typeatt.Size + ")";
            }

            var typeNotNull = prop.GetCustomAttribute<NotNullAttribute>();
            if (typeNotNull != null)
            {
                comando += " NOT NULL";
            }
            var typePrimaryKey = prop.GetCustomAttribute<PrimaryKeyAttribute>();
            if (typePrimaryKey != null)
            {
                comando += " PRIMARY KEY";
            }
            var typeIdentity = prop.GetCustomAttribute<IdentityAttribute>();
            if (typeIdentity != null)
            {
                comando += " IDENTITY(1, 1)";
            }


            comando += ",";
        }
        comando = comando.Substring(0, comando.Length - 1);
        comando += ")";
        CoenctaBanco(comando);


    }
}

static void CoenctaBanco(string comandoSql)
{

    string conexao = @"server=CCH01LABF103\TEW_SQLEXPRESS;database=testedatabase;trusted_connection=true;";
    SqlConnection conn = new SqlConnection(conexao);
    try
    {
        conn.Open();
        SqlCommand command = new SqlCommand(comandoSql, conn);
        command.ExecuteNonQuery();
        WriteLine(comandoSql + "\nExecutada com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        conn.Close();
    }

}


public class TableAttribute : Attribute
{

}
public class PrimaryKeyAttribute : Attribute
{

}
public class IdentityAttribute : Attribute
{
    public IdentityAttribute(int initial, int increment)
    {
        Initial = initial;
        Increment = increment;
    }

    public int Initial { get; set; }
    public int Increment { get; set; }
}
public class NotNullAttribute : Attribute
{

}
public class VarCharAttribute : Attribute
{
    public VarCharAttribute(int size)
    {
        Size = size;
    }

    public int Size { get; set; }
}
public class IntAttribute : Attribute
{

}

[Table]
public class ClientTable
{
    [PrimaryKey]
    [Identity(1, 1)]
    [Int]
    public int Id { get; set; }


    [VarChar(50)]
    [NotNull]
    public string Name { get; set; }


}



[Table]
public class VendedorTable
{
    [PrimaryKey]
    [Identity(1, 1)]
    [Int]
    public int Id { get; set; }


    [VarChar(50)]
    [NotNull]
    public string Name { get; set; }


}

