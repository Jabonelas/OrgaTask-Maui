using Maui.Interface;
using Maui.Service;
using Maui.View.Tarefa;
using Maui.View.Usuario;
using Maui.ViewModel.Tarefa;
using Maui.ViewModel.Usuario;

namespace Maui.Extensions
{
    internal static class InjecaoDependencia
    {

        public static IServiceCollection AdicionarInjecoesDependencias(this IServiceCollection services)
        {
            // Infraestrutura
            services.AddSingleton<HttpClient>();

            //Notificacao
            services.AddSingleton<INotificacaoService, NotificacaoService>();

            // Serviços
            services.AddSingleton<IUsuarioService, UsuarioService>();
            services.AddSingleton<ITarefaService, TarefaService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<CadastrarUsuarioViewModel>();
            services.AddTransient<DashboardTarefasViewModel>();
            services.AddTransient<CadastrarTarefaViewModel>();
            services.AddTransient<VisualizarTarefaViewModel>();
            services.AddTransient<ExibirTarefasViewModel>();
            services.AddTransient<EditarTarefaViewModel>();

            // Views
            services.AddTransient<Login>();
            services.AddTransient<CadastrarUsuario>();
            services.AddTransient<DashboardTarefas>();
            services.AddTransient<CadastrarTarefa>();
            services.AddTransient<VisualizarTarefa>();
            services.AddTransient<ExibirTarefas>();
            services.AddTransient<EditarTarefa>();



            return services;
        }

    }
}
