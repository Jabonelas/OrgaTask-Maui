namespace Maui.DTOs.Tarefa
{
    public class TarefaDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Prioridade { get; set; }

        public int? Prazo { get; set; }

        public string Status { get; set; }
    }
}