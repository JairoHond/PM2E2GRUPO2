using Firebase.Database;
using Firebase.Database.Query;
using PM2E2GRUPO2.Modelos;
using Firebase.Storage;
using Plugin.Maui.Audio;
namespace PM2E2GRUPO2.Vistas;

public partial class CreateEmpl : ContentPage
{
	FirebaseClient client = new FirebaseClient("https://basegrupo2-default-rtdb.firebaseio.com/");
    private string Urlfoto {  get; set; }
    readonly IAudioManager _audioManager;
    readonly IAudioRecorder _audioRecorder;
    private Empleado alumnosService;
    public CreateEmpl(IAudioManager audioManager)
	{
		InitializeComponent();
        _audioManager = audioManager;
        _audioRecorder = audioManager.CreateRecorder();
        BindingContext = this;
        alumnosService = new Empleado();
        GenerarNumero();

    }


	private async void Guardar_Clicked(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(LatitudeEntry.Text) || string.IsNullOrEmpty(LongitudeEntry.Text) || string.IsNullOrEmpty(txtDesc.Text))
        {
            alertN();
        }
        else
        {
            int currentCounter = await alumnosService.GetCounterAsync();
            int newId = currentCounter + 1;


            await client.Child("Empleados").Child(newId.ToString()).PutAsync(new Empleado
            {
                Id = newId.ToString(),
                descripcion = txtDesc.Text,
                latitud = LatitudeEntry.Text,
                longitud = LongitudeEntry.Text,
                Urlfoto=Urlfoto
            });
            await alumnosService.UpdateCounterAsync(newId);
            await Shell.Current.GoToAsync("..");
            alertP();
       
        
        }
	}

    private void GenerarNumero()
    {
        Random random = new Random();
        int numeroAleatorio = random.Next(1, 100); // Genera un número aleatorio entre 1 y 100

        // Asigna el número aleatorio al Entry
        entryNumero.Text = numeroAleatorio.ToString();
    }

    private async void ObtenerUbicacion_Clicked(object sender, EventArgs e)
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Default);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                double latitud = location.Latitude;
                double longitud = location.Longitude;

                // Mostrar la latitud y longitud en los Entry
                LatitudeEntry.Text = latitud.ToString();
                LongitudeEntry.Text = longitud.ToString();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicación", "OK");
            }
        }
        catch (FeatureNotSupportedException)
        {
            // El dispositivo no admite la geolocalización
            await DisplayAlert("Error", "La geolocalización no es compatible en este dispositivo", "OK");
        }
        catch (PermissionException)
        {
            // Permiso denegado por el usuario
            await DisplayAlert("Error", "Se denegó el permiso de ubicación", "OK");
        }
        catch (Exception ex)
        {
            // Otro error
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void Guardar_Audio(object sender, EventArgs e)
    {
        if(await Permissions.RequestAsync<Permissions.Microphone>()!=PermissionStatus.Granted)
        {
            return;
        }
        if (!_audioRecorder.IsRecording)
        {
            await _audioRecorder.StartAsync();
        }
        else
        {
            var recorderdAudio=await _audioRecorder.StopAsync();
            var player = AudioManager.Current.CreatePlayer(recorderdAudio.GetAudioStream());
            player.Play();
        }
    }

    private async void alertP()
    {
        await DisplayAlert("Registro añadido Correctamente", "Correctamente", "OK");
    }

    private async void alertN()
    {
        await DisplayAlert("Registro no Realizado", "Rellene todo los campos", "OK");
    }

    private async void SeleccionarImagen_Clicked(object sender, EventArgs e)
    {
        var foto = await MediaPicker.PickPhotoAsync();
        if (foto != null)
        {
            var stream = await foto.OpenReadAsync();
            Urlfoto = await new FirebaseStorage("basegrupo2.appspot.com")
                .Child("Fotos")
                .Child(DateTime.Now.ToString("ddMMyyyyhhmmss") + foto.FileName)
                .PutAsync(stream);
            fotoImage.Source = Urlfoto;
        }
    }

}