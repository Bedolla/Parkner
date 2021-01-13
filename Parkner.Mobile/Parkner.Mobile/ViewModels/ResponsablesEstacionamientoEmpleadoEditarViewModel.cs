using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
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
    public class ResponsablesEstacionamientoEmpleadoEditarViewModel : BaseViewModel
    {
        private string _actualizar;
        private string _apellido;
        private ICommand _borrarCommand;
        private ICommand _buscarFotoCommand;
        private string _clave;
        private string _claveConfirmar;
        private string _correo;
        private Command _editarCommand;
        private bool _habilitado;
        private string _nombre;

        public ResponsablesEstacionamientoEmpleadoEditarViewModel
        (
            IServicioEmpleados servicioEmpleados,
            string empleadoId
        )
        {
            this.ServicioEmpleados = servicioEmpleados;
            this.EmpleadoId = empleadoId;

            this.Obtener();
        }

        public ResponsablesEstacionamientoEmpleadoEditarViewModel() { }

        private IServicioEmpleados ServicioEmpleados { get; }
        private string EmpleadoId { get; }

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

        public ICommand BorrarCommand
        {
            get => this._borrarCommand ??= new Command(this.Borrar);
            set => this._borrarCommand = value;
        }

        private async void Borrar()
        {
            try
            {
                await this.ServicioEmpleados.BorrarAsync(this.EmpleadoId);

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                Dependencia.Avisar(excepcion.Message);
            }
        }

        private async void Obtener()
        {
            this.Habilitado = true;
            this.Actualizar = "Editar";

            Empleado empleado = await this.ServicioEmpleados.ObtenerAsync(this.EmpleadoId);
            this.Nombre = empleado.Nombre;
            this.Apellido = empleado.Apellido;
            this.Correo = empleado.Correo;
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

        private async void Editar()
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

                Empleado empleado = await this.ServicioEmpleados.ObtenerAsync(this.EmpleadoId);

                string nombreFoto = $"{empleado.Id}.png";

                empleado.Nombre = this.Nombre;
                empleado.Apellido = this.Apellido;
                empleado.Correo = this.Correo;
                empleado.Foto = $"images/avatares/empleados/{nombreFoto}";
                if (this.Clave.NoEsNulo()) empleado.Clave = this.Clave.Encriptar();

                await this.ServicioEmpleados.EditarAsync(empleado);

                if (this.FotoBytes is not null) await this.FotoBytes.SubirFotoAsync(nombreFoto, Roles.Empleado);

                this.Ocupado = false;

                await Dependencia.Navegacion.PopAsync();
            }
            catch (Exception excepcion)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", excepcion.Message, "Entendido");
            }
            finally
            {
                this.Ocupado = false;
                this.Habilitado = true;
                this.Actualizar = "Editar";
            }
        }
    }
}
