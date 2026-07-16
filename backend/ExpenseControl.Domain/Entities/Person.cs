namespace ExpenseControl.Domain.Entities;

public class Person
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int Age { get; private set; }

    // Uma pessoa pode possuir várias transações associadas.
    // Esse relacionamento permite consultar e remover movimentações
    // financeiras vinculadas ao cadastro da pessoa.
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Person()
    {
        // Construtor utilizado pelo Entity Framework Core
        // durante a criação das entidades a partir do banco.
    }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}