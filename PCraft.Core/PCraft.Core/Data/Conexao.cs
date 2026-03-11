using Npgsql;

namespace PCraft.Core.Data
{
    public class Conexao
    {
        public static NpgsqlConnection GetConexao()
        {
            string connString =
            "Host=db.tnxeoyuyagopikdolzqk.supabase.co;" +
            "Port=5432;" +
            "Database=postgres;" +
            "Username=postgres;" +
            "Password=Pc_craft12@@;" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true";

            return new NpgsqlConnection(connString);
        }
    }
}