//using BlazorInputFile;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;

namespace Parkner.Web.Shared
{
    public partial class InputPhoto
    {
        private MemoryStream _fotoMemoria;
        private string _value;

        private IBrowserFile Foto;

        private string FotoUri { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string MensajeBoton { get; set; }

        [Parameter]
        public string Value
        {
            get => this._value;
            set
            {
                if (this._value == value) return;
                this._value = value;
                this.ValueChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public MemoryStream FotoMemoria
        {
            get => this._fotoMemoria;
            set
            {
                if (this._fotoMemoria == value) return;
                this._fotoMemoria = value;
                this.FotoMemoriaChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<MemoryStream> FotoMemoriaChanged { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        private async void FotoSeleccionada(InputFileChangeEventArgs argumentos)
        {
            try
            {
                this.Foto = argumentos.GetMultipleFiles().FirstOrDefault();

                if
                (
                    (this.Foto != null) &&
                    (
                        this.Foto.ContentType.Equals("image/png") ||
                        this.Foto.ContentType.Equals("image/jpeg") ||
                        this.Foto.ContentType.Equals("image/gif")
                    )
                )
                {
                    using (Image fotoTemporal = await Image.LoadAsync(this.Foto.OpenReadStream(10000000)))
                    {
                        using Image clon = fotoTemporal.Clone(c => c.Resize(new ResizeOptions {Mode = ResizeMode.Crop, Size = new Size(256, 256), Position = AnchorPositionMode.Center}));

                        this.FotoMemoria = new MemoryStream();

                        await clon.SaveAsync(this.FotoMemoria, new PngEncoder {CompressionLevel = PngCompressionLevel.NoCompression, TransparentColorMode = PngTransparentColorMode.Preserve});

                        this.FotoUri = $"data:image/png;base64,{Convert.ToBase64String(this.FotoMemoria.ToArray())}";
                    }

                    this.Value = this.Foto.Name;

                    this.StateHasChanged();
                }
                else
                {
                    this.Foto = null;
                }
            }
            catch (Exception)
            {
                this.FotoMemoria = null;
                this.FotoUri = null;
                this.Value = null;
            }
        }
    }
}
