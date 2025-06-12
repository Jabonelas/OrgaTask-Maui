﻿using Maui.DTOs.Tarefa;


namespace Maui.Interface
{
    public interface ITarefaService
    {


        Task<(bool success, string errorMessage, List<TarefaConsultaDTO> Items, int TotalCount)> ObterTarefasPaginadasAsync(int _pageNumber, int _pageSize, string _status);

        Task<(bool success, string errorMessage)> DeletarTarefaAsync(int _id);

        Task<(bool success, string errorMessage)> AlterarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool success, string errorMessage)> CadastrarTarefaAsync(TarefaAlterarDTO _dadosTarefa);

        Task<(bool success, string errorMessage, TarefaAlterarDTO)> BuscarTarefaAsync(int _id);

        Task<(bool success, string errorMessage, TarefaQtdStatusDTO)> BuscarQtdStatusTarefaAsync();

        Task<(bool success, string errorMessage, List<TarefaPrioridadeAltaDTO>)> BuscarTarefasPrioridadeAltaAsync();
    }
}
