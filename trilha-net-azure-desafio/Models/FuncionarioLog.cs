using System.Text.Json;
using Azure;
using Azure.Data.Tables;

namespace TrilhaNetAzureDesafio.Models
{
    public class FuncionarioLog : Funcionario, ITableEntity
    {
    //construtor
        public FuncionarioLog() { }
//campo

        public FuncionarioLog(Funcionario funcionario, TipoAcao tipoAcao, string partitionKey, string rowKey)
        {
            base.Id = funcionario.Id;
            base.Nome = funcionario.Nome;
            base.Endereco = funcionario.Endereco;
            base.Ramal = funcionario.Ramal;
            base.EmailProfissional = funcionario.EmailProfissional;
            base.Departamento = funcionario.Departamento;
            base.Salario = funcionario.Salario;
            base.DataAdmissao = funcionario.DataAdmissao;
            TipoAcao = tipoAcao;
            JSON = JsonSerializer.Serialize(funcionario); //serialização e desserialização pra APIs
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
//propriedades
        public TipoAcao TipoAcao { get; set; }
        public string JSON { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
