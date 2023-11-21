using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
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
            if (empleado.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
            {
                // Eliminar el elemento de la lista local
                var empleadoToDelete = Lista.FirstOrDefault(e => e.Id == empleado.Object.Id);
                if (empleadoToDelete != null)
                {
                    Lista.Remove(empleadoToDelete);
                }
            }
            else if (empleado.Object != null)
            {
                // Agregar o actualizar el elemento en la lista local
                var existingEmpleado = Lista.FirstOrDefault(e => e.Id == empleado.Object.Id);
                if (existingEmpleado != null)
                {
                    // Actualizar el elemento existente
                    Lista.Remove(existingEmpleado);
                }

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
        if (sender is Button button && button.BindingContext is Empleado empleado)
        {
            // Llamada a la función para eliminar el registro de Firebase
            await EliminarRegistroFirebase(empleado.Id);
        }
    }

    private async Task EliminarRegistroFirebase(string empleadoId)
    {
        try
        {
            var empleadoToDelete = Lista.FirstOrDefault(e => e.Id == empleadoId);

            if (empleadoToDelete != null)
            {
                // Eliminar el registro de Firebase
                await client.Child("Empleados").Child(empleadoId).DeleteAsync();

                // Eliminar el registro de la lista local
                Lista.Remove(empleadoToDelete);
            }
        }
        catch (Exception ex)
        {
            // Manejar cualquier error que pueda ocurrir durante la eliminación
            Console.WriteLine($"Error al eliminar el registro: {ex.Message}");
        }
    }

    private async void Actualizar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Empleado empleado)
        {
            // Obtener la información del empleado seleccionado
            var empleadoToUpdate = Lista.FirstOrDefault(emp => emp.Id == empleado.Id);

            // Redirigir a la página de actualización pasando los datos del empleado
            await Shell.Current.GoToAsync($"//UpdatePage?id={empleadoToUpdate.Id}&descripcion={empleadoToUpdate.descripcion}&latitud={empleadoToUpdate.latitud}&longitud={empleadoToUpdate.longitud}");
        }
    }



}