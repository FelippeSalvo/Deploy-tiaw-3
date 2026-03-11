using PCraft.Core.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultControllerRoute();

try
{
    using (var conn = Conexao.GetConexao())
    {
        conn.Open();
        Console.WriteLine("conexao realizada!");
    }
}
catch (Exception ex)
{
    Console.WriteLine("erro na conexao: " + ex.Message);
}

app.Run();