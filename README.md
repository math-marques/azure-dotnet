# DIO - Trilha .NET - Nuvem com Microsoft Azure

## Desafio de projeto
Para este desafio, usei conhecimentos adquiridos no módulo de Nuvem com Microsoft Azure, da trilha .NET da DIO.

## Contexto
Construi um sistema de RH, onde para essa versão inicial do sistema o usuário poderá cadastrar os funcionários de uma empresa. 

Essa cadastro tem um CRUD, obtendo registros ao criar, salvar e deletar. A sua aplicação também precisa armazenar logs de toda e qualquer alteração que venha a ocorrer com um funcionário.

## Premissas
Aplicação é do tipo Web API, Azure Functions e com MVC.

Aplicação implantada no Microsoft Azure, utilizando o App Service para a API, SQL Database para o banco relacional e Azure Table para armazenar os logs. Armazena logs de todas as alterações que venha a acontecer com o funcionário. Os logs são armazenados em uma Azure Table.

Classe principal, a classe Funcionario e a FuncionarioLog, são as seguintes:

![Diagrama da classe Funcionario](Imagens/diagrama_classe.png)

A classe FuncionarioLog é filha da classe Funcionario, pois o log terá as mesmas informações da Funcionario. Gerei uma migration para atualização no banco de dados.

**Swagger**


![Métodos Swagger](Imagens/swagger.png)


**Endpoints**


| Verbo  | Endpoint                | Parâmetro | Body               |
|--------|-------------------------|-----------|--------------------|
| GET    | /Funcionario/{id}       | id        | N/A                |
| PUT    | /Funcionario/{id}       | id        | Schema Funcionario |
| DELETE | /Funcionario/{id}       | id        | N/A                |
| POST   | /Funcionario            | N/A       | Schema Funcionario |

Esse é o schema (model) de Funcionario, utilizado para passar para os métodos que exigirem:

```json
{
  "nome": "Nome funcionario",
  "endereco": "Rua 1234",
  "ramal": "1234",
  "emailProfissional": "email@email.com",
  "departamento": "TI",
  "salario": 1000,
  "dataAdmissao": "2022-06-23T02:58:36.345Z"
}
```

## Ambiente
Este é um diagrama do ambiente que foi montado no Microsoft Azure, utilizando o App Service para a API, SQL Database para o banco relacional e Azure Table para armazenar os logs.

![Diagrama da classe Funcionario](Imagens/diagrama_api.png)
