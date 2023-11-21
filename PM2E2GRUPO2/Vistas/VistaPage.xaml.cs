using PM2E2GRUPO2.Modelos;
namespace PM2E2GRUPO2.Vistas;

[QueryProperty(nameof(Detalle),"Detalle")]

public partial class VistaPage : ContentPage
{
	private double Latitud;
    private double Longitud;
	Empleado detalle;
	public Empleado Detalle {
		get => detalle;
		set
		{
			detalle = value;
			OnPropertyChanged();
        }

	}
	public VistaPage()
	{
		InitializeComponent();
		BindingContext = this;
	}


}