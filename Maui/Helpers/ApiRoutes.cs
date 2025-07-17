namespace Maui.Helpers
{
    internal class ApiRoutes
    {
        public static string SetandoEndPoint(string _endPoint)
        {
            string endPoint = "";

#if DEBUG
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // Endereço especial para emulador Android
                endPoint = $"https://10.0.2.2:7091/{_endPoint}";
            }
            else
            {
                // Localhost para Windows/iOS/Mac/etc
                endPoint = $"https://localhost:7091/{_endPoint}";
            }
#else
            // Configuração de produção
            endPoint = $"https://blazor-api.onrender.com/{_endPoint}";
#endif

            return endPoint;
        }
    }
}