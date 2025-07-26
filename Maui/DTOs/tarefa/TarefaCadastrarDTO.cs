namespace Maui.DTOs.Tarefa
{
    public class TarefaCadastrarDTOAPI
    {
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public int? Prazo { get; set; }

        public static implicit operator TarefaCadastrarDTOAPI(TarefaDTO _tarefa)
        {
            return new TarefaCadastrarDTOAPI()
            {
                Titulo = _tarefa.Titulo,
                Descricao = _tarefa.Descricao,
                Prazo = _tarefa.Prazo
            };
        }
    }
}