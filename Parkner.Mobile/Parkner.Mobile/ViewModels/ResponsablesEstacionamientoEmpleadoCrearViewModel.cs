using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Mobile.Services;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Image = SixLabors.ImageSharp.Image;
using Size = SixLabors.ImageSharp.Size;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class ResponsablesEstacionamientoEmpleadoCrearViewModel : BaseViewModel
    {
        public ResponsablesEstacionamientoEmpleadoCrearViewModel
        (
            IServicioEmpleados servicioEmpleados,
            string estacionamientoId
        )
        {
            this.ServicioEmpleados = servicioEmpleados;
            this.EstacionamientoId = estacionamientoId;
            this.RegistrarCommand = new Command(this.Crear);
            this.BuscarFotoCommand = new Command<Grid>(this.BuscarFoto);

            this.Habilitado = true;
        }

        public ResponsablesEstacionamientoEmpleadoCrearViewModel() { }

        private IServicioEmpleados ServicioEmpleados { get; }
        private string EstacionamientoId { get; }

        public bool Habilitado { get; set; }
        public Command RegistrarCommand { get; set; }
        public ICommand BuscarFotoCommand { get; set; }

        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string ClaveConfirmar { get; set; }
        private byte[] FotoBytes { get; set; }

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
                    a.Cancel = true;

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
                Debug.WriteLine(excepcion.Message);
            }
            finally
            {
                this.Habilitado = true;
            }
        }

        private async void Crear()
        {
            try
            {
                if
                (
                    this.Nombre.EsNulo() ||
                    this.Apellido.EsNulo() ||
                    this.Correo.EsNulo() ||
                    this.Clave.EsNulo() ||
                    this.ClaveConfirmar.EsNulo() ||
                    this.FotoBytes is null
                )
                {
                    Dependencia.Avisar("Debe llenar todos los campos");
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
                    Dependencia.Avisar("La contraseña y su confirmación deben coincidir");
                    return;
                }

                string id = Guid.NewGuid().ToString();
                string nombreFoto = $"{id}.png";

                this.Habilitado = false;
                this.Ocupado = true;

                Empleado empleado = new()
                {
                    Id = id,
                    Nombre = this.Nombre,
                    Apellido = this.Apellido,
                    Correo = this.Correo,
                    Clave = this.Clave.Encriptar(),
                    Foto = $"images/avatares/empleados/{nombreFoto}",
                    Rol = this.EstacionamientoId,
                    Creacion = DateTime.Now,
                    Disponible = true
                };

                await this.ServicioEmpleados.CrearAsync(empleado);

                if (await this.FotoBytes.SubirFotoAsync(nombreFoto, Roles.Empleado))
                {
                    await Dependencia.Navegacion.PopAsync();
                }
                else
                {
                    if (!(await this.ServicioEmpleados.ObtenerAsync(this.Correo)).Disponible)
                    {
                        await this.ServicioEmpleados.BorrarAsync(empleado.Id);

                        await this.ServicioEmpleados.QuitarDeAsync(new Empleado
                        {
                            Id = empleado.Id,
                            Rol = this.EstacionamientoId
                        });
                    }

                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear al usuario, intente nuevamente.", "Entendido");
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
            }
        }
    }
}
