using Firebase.Database;
using Firebase.Database.Query;
using PM2E2GRUPO2.Modelos;
using System.Collections.ObjectModel;
namespace PM2E2GRUPO2.Vistas;

public partial class MainPage : ContentPage
{
	FirebaseClient client = new FirebaseClient("https://basegrupo2-default-rtdb.firebaseio.com/");
	public ObservableCollection<Empleado> Lista{  get; set; }= new ObservableCollection<Empleado>();
	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
		CargarLista();
	}
	public void CargarLista()
	{
		client.Child("Empleados")
			.AsObservable<Empleado>()
			.Subscribe((empleado) =>
			{
				if(empleado.Object != null)
				{
					Lista.Add(empleado.Object);
				}
			});
	}
	private async void nuevoReg_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(CreateEmpl));
	}
	private void filtroEntry_TextChanged(object sender, EventArgs e)
	{
		string filtro = filtroEntry.Text.ToLower();
		if(filtro.Length > 0 )
		{
			listaCollection.ItemsSource = Lista.Where(x => x.descripcion.ToLower().Contains(filtro));
		}
		else
		{
			listaCollection.ItemsSource = Lista;
		}
	}

	private async void listaCollection_SelectionChanged (object sender, SelectionChangedEventArgs e)
	{
		Empleado empleado = e.CurrentSelection.FirstOrDefault() as Empleado;
		var parametro = new Dictionary<string, object> {
			["Detalle"]=empleado
		};
		await Shell.Current.GoToAsync(nameof(VistaPage), parametro);
	}

    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
        // Obtener el contexto del elemento de la lista al que se le hizo clic en el botón
        var button = sender as Button;
        var empleado = button?.BindingContext as Empleado;

        if (empleado != null)
        {
            // Realizar la eliminación en Firebase
            await client.Child("Empleados").Child(empleado.Id.ToString()).DeleteAsync();

            // Eliminar el elemento de la lista localmente
            Lista.Remove(empleado);

            Console.WriteLine($"Intentando eliminar registro con Id: {empleado.Id}");

            try
            {
                await client.Child("Empleados").Child(empleado.Id).DeleteAsync();
                Lista.Remove(empleado);
                Console.WriteLine("Registro eliminado correctamente en Firebase.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar registro en Firebase: {ex.Message}");
            }
        }
    }

}