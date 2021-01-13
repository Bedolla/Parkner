using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using Parkner.Mobile.Views;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Image = SixLabors.ImageSharp.Image;
using Size = SixLabors.ImageSharp.Size;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class PerfilViewModel : BaseViewModel
    {
        private string _actualizar;
        private string _apellido;
        private ICommand _buscarFotoCommand;
        private string _clave;
        private string _claveConfirmar;
        private string _correo;
        private Command _editarCommand;
        private bool _habilitado;
        private string _nombre;

        public PerfilViewModel
        (
            IServicioClientes servicioClientes,
            IServicioEmpleados servicioEmpleados,
            IServicioResponsables servicioResponsables
        )
        {
            this.ServicioClientes = servicioClientes;
            this.ServicioEmpleados = servicioEmpleados;
            this.ServicioResponsables = servicioResponsables;

            this.Obtener();
        }

        public PerfilViewModel() { }

        private IServicioClientes ServicioClientes { get; }
        private IServicioEmpleados ServicioEmpleados { get; }
        private IServicioResponsables ServicioResponsables { get; }

        public Command EditarCommand
        {
            get => this._editarCommand ??= new Command(this.Editar);
            set => this._editarCommand = value;
        }

        public ICommand BuscarFotoCommand
        {
            get => this._buscarFotoCommand ??= new Command<Grid>(this.BuscarFoto);
            set => this._buscarFotoCommand = value;
        }

        public string Nombre
        {
            get => this._nombre;
            set
            {
                this._nombre = value;
                this.OnPropertyChanged();
            }
        }

        public string Apellido
        {
            get => this._apellido;
            set
            {
                this._apellido = value;
                this.OnPropertyChanged();
            }
        }

        public string Correo
        {
            get => this._correo;
            set
            {
                this._correo = value;
                this.OnPropertyChanged();
            }
        }

        public string Clave
        {
            get => this._clave;
            set
            {
                this._clave = value;
                this.OnPropertyChanged();
            }
        }

        public string ClaveConfirmar
        {
            get => this._claveConfirmar;
            set
            {
                this._claveConfirmar = value;
                this.OnPropertyChanged();
            }
        }

        private byte[] FotoBytes { get; set; }

        public bool Habilitado
        {
            get => this._habilitado;
            set
            {
                this._habilitado = value;
                this.OnPropertyChanged();
            }
        }

        public string Actualizar
        {
            get => this._actualizar;
            set
            {
                this._actualizar = value;
                this.OnPropertyChanged();
            }
        }

        private async void Obtener()
        {
            this.Habilitado = true;
            this.Actualizar = "Actualizar";

            switch (Application.Current.Properties[Propiedades.Rol].ToString())
            {
                case Roles.Cliente:
                {
                    Cliente cliente = await this.ServicioClientes.ObtenerAsync(Application.Current.Properties[Propiedades.Id].ToString());
                    this.Nombre = cliente.Nombre;
                    this.Apellido = cliente.Apellido;
                    this.Correo = cliente.Correo;
                    break;
                }
                case Roles.Empleado:
                {
                    Empleado empleado = await this.ServicioEmpleados.ObtenerAsync(Application.Current.Properties[Propiedades.Id].ToString());
                    this.Nombre = empleado.Nombre;
                    this.Apellido = empleado.Apellido;
                    this.Correo = empleado.Correo;
                    break;
                }
                case Roles.Responsable:
                {
                    Responsable responsable = await this.ServicioResponsables.ObtenerAsync(Application.Current.Properties[Propiedades.Id].ToString());
                    this.Nombre = responsable.Nombre;
                    this.Apellido = responsable.Apellido;
                    this.Correo = responsable.Correo;
                    break;
                }
            }
        }

        private async void BuscarFoto(Grid contendorEditor)
        {
            try
            {
                this.Habilitado = false;

                Stream fotoPorSubir = await DependencyService.Get<IPhotoPickerService>().ObtnerImagenAsync();

                if (fotoPorSubir == null) return;

                SfImageEditor editor = new();
                editor.ImageSaving += async (_, a) =>
                {
                    try
                    {
                        a.Cancel = true;
                        this.Ocupado = true;

                        using Image fotoTemporal = await Image.LoadAsync(a.Stream);
                        using Image clon = fotoTemporal.Clone
                        (
                            c => c.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.BoxPad,
                                Size = new Size(256, 256),
                                Position = AnchorPositionMode.Center
                            })
                        );
                        MemoryStream fotoMemoria = new();
                        await clon.SaveAsync(fotoMemoria, new PngEncoder {CompressionLevel = PngCompressionLevel.NoCompression, TransparentColorMode = PngTransparentColorMode.Preserve});
                        //string fotoBase64String = $"data:image/png;base64,{Convert.ToBase64String(fotoMemoria.ToArray())}";
                        //ImageSource imagen = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(fotoBase64String)));
                        this.FotoBytes = fotoMemoria.ToArray();

                        contendorEditor.HeightRequest = 128;
                        contendorEditor.VerticalOptions = LayoutOptions.Start;
                        if (Device.RuntimePlatform == Device.UWP)
                            editor.IsVisible = false;
                        else
                            contendorEditor.Children.Clear();
                        contendorEditor.Children.Add(new Xamarin.Forms.Image {WidthRequest = 128, HeightRequest = 128, Source = ImageSource.FromStream(() => new MemoryStream(this.FotoBytes))});

                        this.Habilitado = true;
                    }
                    catch (Exception excepcion)
                    {
                        await Application.Current.MainPage.DisplayAlert("Aviso", excepcion.Message, "Entendido");
                    }
                    finally
                    {
                        this.Ocupado = false;
                    }
                };
                editor.Source = ImageSource.FromStream(() => fotoPorSubir);
                editor.SetToolbarItemVisibility("Undo,Redo,Text,Shape,Path,free,original,3:1,3:2,4:3,5:4,16:9,Effects", false);
                editor.ToolbarSettings.ToolbarItems.ForEach(i =>
                    i.Text = i.Name switch
                    {
                        "Save" => "Guardar",
                        "Reset" => "Deshacer",
                        _ => i.Text
                    }
                );

                contendorEditor.IsVisible = true;
                contendorEditor.HeightRequest = 512;
                contendorEditor.VerticalOptions = LayoutOptions.Start;
                contendorEditor.Children.Clear();
                contendorEditor.Children.Add(editor);
            }
            catch (Exception excepcion)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", excepcion.Message, "Entendido");
            }
        }

        private async void Editar(object obj)
        {
            try
            {
                if
                (
                    this.Nombre.EsNulo() ||
                    this.Apellido.EsNulo() ||
                    this.Correo.EsNulo()
                )
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Debe llenar todos los campos", "Entendido");
                    return;
                }

                if
                (
                    (
                        this.Clave.NoEsNulo() &&
                        this.ClaveConfirmar.EsNulo()
                    )
                    ||
                    (
                        this.Clave.EsNulo() &&
                        this.ClaveConfirmar.NoEsNulo()
                    )
                )
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Debe llenar la contraseña y su confirmación", "Entendido");
                    return;
                }

                if
                    (this.Clave != this.ClaveConfirmar)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "La contraseña y su confirmación deben coincidir", "Entendido");
                    return;
                }

                this.Habilitado = false;
                this.Ocupado = true;
                this.Actualizar = "Espere...";

                switch (Application.Current.Properties[Propiedades.Rol].ToString())
                {
                    case Roles.Cliente:
                    {
                        Cliente cliente = await this.ServicioClientes.ObtenerAsync(this.Correo);

                        if (cliente.Id == Application.Current.Properties[Propiedades.Id].ToString())
                        {
                            string nombreFoto = $"{cliente.Id}.png";

                            cliente.Nombre = this.Nombre;
                            cliente.Apellido = this.Apellido;
                            cliente.Correo = this.Correo;
                            cliente.Foto = $"images/avatares/clientes/{nombreFoto}";
                            if (this.Clave.NoEsNulo()) cliente.Clave = this.Clave.Encriptar();

                            await this.ServicioClientes.EditarAsync(cliente);

                            if (this.FotoBytes is not null) await this.FotoBytes.SubirFotoAsync(nombreFoto);

                            Application.Current.Properties[Propiedades.Nombre] = cliente.Nombre;
                            Application.Current.Properties[Propiedades.Apellido] = cliente.Apellido;
                            Application.Current.Properties[Propiedades.Correo] = cliente.Correo;
                            Application.Current.Properties[Propiedades.Foto] = cliente.Foto;

                            this.Ocupado = false;

                            await Dependencia.Navegacion.PopAsync();
                            Dependencia.Inicio = new InicioPage();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Duplicado", "Ya hay un usuario con ese Correo.", "Entendido");
                        }
                        break;
                    }
                    case Roles.Empleado:
                    {
                        Empleado empleado = await this.ServicioEmpleados.ObtenerAsync(this.Correo);

                        if (empleado.Id == Application.Current.Properties[Propiedades.Id].ToString())
                        {
                            string nombreFoto = $"{empleado.Id}.png";

                            empleado.Nombre = this.Nombre;
                            empleado.Apellido = this.Apellido;
                            empleado.Correo = this.Correo;
                            empleado.Foto = $"images/avatares/empleados/{nombreFoto}";
                                if (this.Clave.NoEsNulo()) empleado.Clave = this.Clave.Encriptar();

                            await this.ServicioEmpleados.EditarAsync(empleado);

                            if (this.FotoBytes is not null) await this.FotoBytes.SubirFotoAsync(nombreFoto, Roles.Empleado);

                            Application.Current.Properties[Propiedades.Nombre] = empleado.Nombre;
                            Application.Current.Properties[Propiedades.Apellido] = empleado.Apellido;
                            Application.Current.Properties[Propiedades.Correo] = empleado.Correo;
                            Application.Current.Properties[Propiedades.Foto] = empleado.Foto;

                            this.Ocupado = false;

                            await Dependencia.Navegacion.PopAsync();
                            Dependencia.Inicio = new InicioPage();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Duplicado", "Ya hay un usuario con ese Correo.", "Entendido");
                        }
                        break;
                    }
                    case Roles.Responsable:
                    {
                        Responsable responsable = await this.ServicioResponsables.ObtenerAsync(this.Correo);

                        if (responsable.Id == Application.Current.Properties[Propiedades.Id].ToString())
                        {
                            string nombreFoto = $"{responsable.Id}.png";

                            responsable.Nombre = this.Nombre;
                            responsable.Apellido = this.Apellido;
                            responsable.Correo = this.Correo;
                            responsable.Foto = $"images/avatares/responsables/{nombreFoto}";
                                if (this.Clave.NoEsNulo()) responsable.Clave = this.Clave.Encriptar();

                            await this.ServicioResponsables.EditarAsync(responsable);

                            if (this.FotoBytes is not null) await this.FotoBytes.SubirFotoAsync(nombreFoto, Roles.Responsable);

                            Application.Current.Properties[Propiedades.Nombre] = responsable.Nombre;
                            Application.Current.Properties[Propiedades.Apellido] = responsable.Apellido;
                            Application.Current.Properties[Propiedades.Correo] = responsable.Correo;
                            Application.Current.Properties[Propiedades.Foto] = responsable.Foto;

                            this.Ocupado = false;

                            await Dependencia.Navegacion.PopAsync();
                            Dependencia.Inicio = new InicioPage();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Duplicado", "Ya hay un usuario con ese Correo.", "Entendido");
                        }
                        break;
                        }
                }
            }
            catch (Exception excepcion)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", excepcion.Message, "Entendido");
            }
            finally
            {
                this.Ocupado = false;
                this.Habilitado = true;
                this.Actualizar = "Actualizar";
            }
        }
    }
}
