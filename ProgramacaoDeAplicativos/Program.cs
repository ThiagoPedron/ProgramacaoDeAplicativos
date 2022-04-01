using System.Reflection;
using static System.Console;
using System.Data;
using System.Data.SqlClient;

//definindo o server e a database que serão usadas
const string Server = "CTPC3616";
const string DataBase = "ConectaSLQ";



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
            //adicionando a coluna criada
            comando += " " + prop.Name;
            //verificando suas propriedades
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

            //adiciona , no final da linha
            comando += ",";
        }
        //retira a ultima , do comando sql
        comando = comando.Substring(0, comando.Length - 1);
        comando += ")";
        CoenctaBanco(comando);
    }
}

// função que recebe o comando SQL e executa no banco de dados 
static void CoenctaBanco(string comandoSql)
{


    string conexao = @"server="+Server+";database="+DataBase+";trusted_connection=true;";
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

// definindo uma tabela
public class TableAttribute : Attribute
{

}
// definindo uma chave primária
public class PrimaryKeyAttribute : Attribute
{

}
//setando uma teg para um atributo com auto increment 
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
//setando uma "tag" para o atributo tipo não nulo
public class NotNullAttribute : Attribute
{

}
//setando uma "tag" para o atributo tipo varChar
public class VarCharAttribute : Attribute
{
    public VarCharAttribute(int size)
    {
        Size = size;
    }

    public int Size { get; set; }
}
//setando uma "tag" para o atributo tipo inteiro
public class IntAttribute : Attribute
{

}
/*TABELA CLIENTE*/
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
/*TABELA VENDEDOR*/
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

