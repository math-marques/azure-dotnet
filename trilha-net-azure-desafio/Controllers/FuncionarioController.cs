//os usings servem para implementar as funcionalidades extermas à aplicação da pagina abaixo. Classes, pacotes, etc
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using TrilhaNetAzureDesafio.Context;
using TrilhaNetAzureDesafio.Models;

//serve pra agrupar as classes a uma mesma funcionalidade, um mesmo objetivo num conjunto de classes
namespace TrilhaNetAzureDesafio.Controllers;

// defindindo a rota da API no Controller (aquele que é mediador entre a view/Models
[ApiController]
[Route("[controller]")]

//classe FuncionarioController que herda de controllrebase com seus respectivos CAMPOS PARAMETRIZADOS, voltados pra
//conexao com banco, contexto, entidade
public class FuncionarioController : ControllerBase
{
    private readonly RHContext _context;
    private readonly string _connectionString;
    private readonly string _tableName;

//metodo da classe
    public FuncionarioController(RHContext context, IConfiguration configuration)
    {
        _context = context;
        //a config ao banco está pegando uma lista de strings pra conectar com o repositorio
        _connectionString = configuration
        .GetValue<string>("ConnectionStrings:SAConnectionString");
        //se conecta c os serviços da azure tbm
        _tableName = configuration.GetValue<string>("ConnectionStrings:AzureTableName");
    }
//metodo privado. ou seja, n podera ser instanciado nos objetos
    private TableClient GetTableClient()
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient(_tableName);

        tableClient.CreateIfNotExists();
        return tableClient;
    }
//pega informação através do ID
    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
        var funcionario = _context.Funcionarios.Find(id);

        if (funcionario == null)
            return NotFound();

        return Ok(funcionario);
    }

    [HttpPost]
    public IActionResult Criar(Funcionario funcionario)
    {
        // Adiciona o funcionário no banco SQL
        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges(); // Salva as alterações no banco SQL

        // Cria o log de inclusão no Azure Table
        var tableClient = GetTableClient();
        var funcionarioLog = new FuncionarioLog(funcionario, TipoAcao.Inclusao, funcionario.Departamento, Guid.NewGuid().ToString());
        tableClient.UpsertEntity(funcionarioLog); // Salva o log no Azure Table

        return CreatedAtAction(nameof(ObterPorId), new { id = funcionario.Id }, funcionario);
    }
// insere
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Funcionario funcionario)
    {
        var funcionarioBanco = _context.Funcionarios.Find(id);

        if (funcionarioBanco == null)
            return NotFound();

        // Atualiza as propriedades do funcionário
        funcionarioBanco.Nome = funcionario.Nome;
        funcionarioBanco.Endereco = funcionario.Endereco;
        funcionarioBanco.Departamento = funcionario.Departamento;
        funcionarioBanco.DataAdmissao = funcionario.DataAdmissao;

        _context.Funcionarios.Update(funcionarioBanco);
        _context.SaveChanges(); // Salva as alterações no banco SQL

        // Cria o log de atualização no Azure Table
        var tableClient = GetTableClient();
        var funcionarioLog = new FuncionarioLog(funcionarioBanco, TipoAcao.Atualizacao, funcionarioBanco.Departamento, Guid.NewGuid().ToString());
        tableClient.UpsertEntity(funcionarioLog); // Salva o log no Azure Table

        return Ok(funcionarioBanco);
    }

    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var funcionarioBanco = _context.Funcionarios.Find(id);

        if (funcionarioBanco == null)
            return NotFound();

        // Remove o funcionário do banco SQL
        _context.Funcionarios.Remove(funcionarioBanco);
        _context.SaveChanges(); // Salva as alterações no banco SQL

        // Cria o log de remoção no Azure Table
        var tableClient = GetTableClient();
        var funcionarioLog = new FuncionarioLog(funcionarioBanco, TipoAcao.Remocao, funcionarioBanco.Departamento, Guid.NewGuid().ToString());
        tableClient.UpsertEntity(funcionarioLog); // Salva o log no Azure Table

        return NoContent();
    }
}
