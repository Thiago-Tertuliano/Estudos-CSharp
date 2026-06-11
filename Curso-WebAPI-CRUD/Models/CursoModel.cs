namespace Curso.Models;

public class CursoModel
{
    public CursoModel(string nome)
    {
        Nome = nome;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; init; }
    public string Nome { get; private set; } = String.Empty;

    public void ChangeName(string nome)
    {
        Nome = nome;
    }
    
}