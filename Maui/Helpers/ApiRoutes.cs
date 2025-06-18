namespace Maui.Helpers
{
    internal class ApiRoutes
    {
        public static string SetandoEndPoint(string _endPoint)
        {
            string endPoint = "";

#if DEBUG
            endPoint = $"https://localhost:7091/{_endPoint}";
#else
                endPoint = $"https://blazor-api.onrender.com{_endPoint}";
#endif
            return endPoint;
        }
    }
}
