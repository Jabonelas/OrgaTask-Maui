using Maui.DTOs.Tarefa;


namespace Maui.Interface
{
    public interface ITarefaService
    {


        Task<(bool Sucesso, string errorMessage, List<TarefaConsultaDTO> listaTarefaConsultaDTO, int totalCount)> ObterTarefasPaginadasAsync(int _pageNumber, int _pageSize, string _status);

        Task<(bool Sucesso, string ErrorMessagem)> DeletarTarefaAsync(int _id);

        Task<(bool Sucesso, string ErrorMessagem)> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool Sucesso, string ErrorMessagem)> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool Sucesso, string ErrorMessagem, TarefaAlterarDTO TarefaAlterarDTO)> BuscarTarefaAsync(int _id);

        Task<(bool Sucesso, string ErrorMessagem, TarefaQtdStatusDTO TarefaQtdStatusDTO)> BuscarQtdStatusTarefaAsync();

        Task<(bool Sucesso, string ErrorMessagem, List<TarefaPrioridadeAltaDTO> ListaTarefaPrioridadeAltaDTO)> BuscarTarefasPrioridadeAltaAsync();
    }
}
