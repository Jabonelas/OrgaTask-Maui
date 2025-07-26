using Maui.DTOs.Tarefa;

namespace Maui.Interface
{
    public interface ITarefaService
    {
        Task<(bool Sucesso, string ErrorMessagem, List<TarefaConsultaDTO> ListaTarefaConsultaDTO, int totalCount)> ObterTarefasPaginadasAsync(int _pageNumber, int _pageSize, string _status);

        Task<(bool Sucesso, string ErrorMessagem)> DeletarTarefaAsync(int _id);

        Task<(bool Sucesso, string ErrorMessagem)> AlterarTarefaAsync(TarefaDTO _dadosTarefa);

        Task<(bool Sucesso, string ErrorMessagem)> CadastrarTarefaAsync(TarefaDTO _dadosTarefa);

        Task<(bool Sucesso, string ErrorMessagem, TarefaDTO TarefaAlterarDTO)> BuscarTarefaAsync(int _id);

        Task<(bool Sucesso, string ErrorMessagem, TarefaQtdStatusDTO TarefaQtdStatusDTO)> BuscarQtdStatusTarefaAsync();

        Task<(bool Sucesso, string ErrorMessagem, List<TarefaPrioridadeAltaDTO> ListaTarefaPrioridadeAltaDTO)> BuscarTarefasPrioridadeAltaAsync();
    }
}