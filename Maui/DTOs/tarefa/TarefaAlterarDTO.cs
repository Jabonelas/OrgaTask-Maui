namespace Maui.DTOs.Tarefa
{
    public class TarefaAlterarDTOAPI
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public int? Prazo { get; set; }

        public static implicit operator TarefaAlterarDTOAPI(TarefaDTO _tarefa)
        {
            return new TarefaAlterarDTOAPI()
            {
                Id = _tarefa.Id,
                Titulo = _tarefa.Titulo,
                Descricao = _tarefa.Descricao,
                Prazo = _tarefa.Prazo
            };
        }
    }
}