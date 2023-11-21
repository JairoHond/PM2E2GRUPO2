using Microsoft.Extensions.Logging;
using Firebase.Database;
using Firebase.Database.Query;
using PM2E2GRUPO2.Modelos;
using Plugin.Maui.Audio;
using PM2E2GRUPO2.Vistas;
namespace PM2E2GRUPO2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<CreateEmpl>();


#if DEBUG
            builder.Logging.AddDebug();
#endif
            Registro();
            return builder.Build();
        }
        public static void Registro()
        {
            FirebaseClient client = new FirebaseClient("https://basegrupo2-default-rtdb.firebaseio.com/");
            var ubic = client.Child("Registros").OnceAsync<Ubicacion>();
            if(ubic.Result.Count==0) 
            {
                client.Child("Registros").PostAsync(new Ubicacion { desc="En Atlantida"});
                client.Child("Registros").PostAsync(new Ubicacion { desc = "En Atlantida" });
                client.Child("Registros").PostAsync(new Ubicacion { desc = "En Olancho" });
                client.Child("Registros").PostAsync(new Ubicacion { desc = "En Villanueva" });
            }
        }
    }
}
