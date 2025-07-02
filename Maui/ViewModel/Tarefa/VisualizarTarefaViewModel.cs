using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.Interface;
using Maui.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maui.DTOs.Tarefa;

namespace Maui.ViewModel.Tarefa
{

    [QueryProperty(nameof(IdTarefa), "idTarefa")]

    [QueryProperty(nameof(Origin), "origin")]
    public partial class VisualizarTarefaViewModel : ObservableObject
    {

        private readonly ITarefaService iTarefaService;

        [ObservableProperty]
        private TarefaAlterarDTO tarefaAlterarDTO;

        [ObservableProperty]
        private int idTarefa;

        [ObservableProperty]
        private string origin;

        public VisualizarTarefaViewModel(ITarefaService _iTarefaService)
        {

            iTarefaService = _iTarefaService;

            TarefaAlterarDTO = new TarefaAlterarDTO();

        }

        partial void OnIdTarefaChanged(int value)
        {
            CarregarTarefaAsync(value);
        }

        private async Task CarregarTarefaAsync(int idTarefa)
        {
            try
            {
                (bool Sucesso, string ErrorMessagem, TarefaAlterarDTO TarefaAlterarDTO) = await iTarefaService.BuscarTarefaAsync(IdTarefa);

                if (Sucesso)
                {
                    this.TarefaAlterarDTO = TarefaAlterarDTO;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Atenção!", ErrorMessagem, "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao visualizar dados da tarefa: {ex.Message}");

                await Application.Current.MainPage.DisplayAlert("Atenção!",
                    "Ocorreu um erro interno. Nossa equipe já foi notificada.", "OK");
            }
        }
        
        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync($"//{Origin}"); 
        }
    }
}
