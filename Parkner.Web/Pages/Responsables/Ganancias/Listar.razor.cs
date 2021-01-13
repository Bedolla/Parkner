using Blazorise.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.Shared.Tablas;
using Parkner.Web.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Ganancias
{
    [Authorize]
    public partial class Listar
    {
        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private IServicioGanancias ServicioGanancias { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private ILogger<Listar> Registro { get; set; }

        private List<Ganancia> Ganancias { get; set; }
        private List<OrdenarOpciones> Opciones { get; set; }
        private PaginacionPeticion PaginacionPeticion { get; } = new PaginacionPeticion();
        private PaginacionMetaData PaginacionMetaData { get; set; } = new PaginacionMetaData();
        private Chart<decimal> Grafica { get; set; } = new Chart<decimal>();
        private ChartOptions GraficaOpciones { get; } = new ChartOptions
        {
            Scales = new Scales
            {
                XAxes = new List<Axis>
                {
                    new Axis
                    {
                        Display = true,
                        ScaleLabel = new AxisScaleLabel {LabelString = "Meses", Display = true, FontColor = "#6658dd", FontSize = 20},
                        Ticks = new AxisTicks {BeginAtZero = true, Display = true},
                        GridLines = new AxisGridLines {Display = true}
                    }
                },
                YAxes = new List<Axis>
                {
                    new Axis
                    {
                        Display = true,
                        ScaleLabel = new AxisScaleLabel {LabelString = "Dinero", Display = true, FontColor = "#02a8b5", FontSize = 20},
                        Ticks = new AxisTicks {BeginAtZero = true, Display = true},
                        GridLines = new AxisGridLines {Display = true}
                    }
                }
            },
            Legend = new Legend {Display = true, FullWidth = true},
            Responsive = true,
            Tooltips = new Tooltips {Enabled = true}
        };

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            this.PaginacionPeticion.OrdenarPor = "Fecha";

            this.Opciones = new List<OrdenarOpciones>
            {
                new OrdenarOpciones {Valor = "Fecha", Texto = "Fecha ↑"},
                new OrdenarOpciones {Valor = "Fecha desc", Texto = "Fecha ↓"},
                new OrdenarOpciones {Valor = "Cantidad", Texto = "Cantidad ↑"},
                new OrdenarOpciones {Valor = "Cantidad desc", Texto = "Cantidad ↓"}
            };

            this.PaginacionPeticion.CantidadPorPagina = 5;
            await this.GraficaActualizar();
            await this.RecibirAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) => await this.GraficaActualizar();

        private async Task PaginaSeleccionada(int pagina)
        {
            this.PaginacionPeticion.NumeroPagina = pagina;
            await this.RecibirAsync();
        }

        private async Task BusquedaCambio(string terminoBuscado)
        {
            this.PaginacionPeticion.NumeroPagina = 1;
            this.PaginacionPeticion.TerminoBuscado = terminoBuscado;
            await this.RecibirAsync();
        }

        private async Task OrdenCambio(string ordenarPor)
        {
            this.PaginacionPeticion.OrdenarPor = ordenarPor;
            await this.RecibirAsync();
        }

        private async Task CantidadCambio(int tamano)
        {
            this.PaginacionPeticion.CantidadPorPagina = tamano;
            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            try
            {
                this.PaginacionPeticion.Id = this.ResponsableState.Id;
                ListaPaginada<Ganancia> gananciasPaginadas = await this.ServicioGanancias.ObtenerDeAsync(this.PaginacionPeticion);
                this.Ganancias = gananciasPaginadas.Lista;
                this.PaginacionMetaData = gananciasPaginadas.MetaData;
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error al obtener ganancias paginados /responsables/ver: {excepcion.Message}");
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Crear() => this.Navegacion.NavigateTo("/responsables/ganancias/crear");

        private async Task GraficaActualizar()
        {
            await this.Grafica.Clear();

            await this.Grafica.AddLabelsDatasetsAndUpdate(new List<string>
            {
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            }, new ChartDataset<decimal>
            {
                Label = "Ganancias",
                Data = await this.GraficaObtenerDatos(),
                BackgroundColor = new List<string>
                {
                    ChartColor.FromRgba(255, 99, 132, 0.2f),
                    ChartColor.FromRgba(54, 162, 235, 0.2f),
                    ChartColor.FromRgba(255, 206, 86, 0.2f),
                    ChartColor.FromRgba(75, 192, 192, 0.2f),
                    ChartColor.FromRgba(153, 102, 255, 0.2f),
                    ChartColor.FromRgba(255, 159, 64, 0.2f),
                    ChartColor.FromRgba(255, 99, 132, 0.2f),
                    ChartColor.FromRgba(54, 162, 235, 0.2f),
                    ChartColor.FromRgba(255, 206, 86, 0.2f),
                    ChartColor.FromRgba(75, 192, 192, 0.2f),
                    ChartColor.FromRgba(153, 102, 255, 0.2f),
                    ChartColor.FromRgba(255, 159, 64, 0.2f)
                },
                BorderColor = new List<string>
                {
                    ChartColor.FromRgba(255, 99, 132, 1f),
                    ChartColor.FromRgba(54, 162, 235, 1f),
                    ChartColor.FromRgba(255, 206, 86, 1f),
                    ChartColor.FromRgba(75, 192, 192, 1f),
                    ChartColor.FromRgba(153, 102, 255, 1f),
                    ChartColor.FromRgba(255, 159, 64, 1f),
                    ChartColor.FromRgba(255, 99, 132, 1f),
                    ChartColor.FromRgba(54, 162, 235, 1f),
                    ChartColor.FromRgba(255, 206, 86, 1f),
                    ChartColor.FromRgba(75, 192, 192, 1f),
                    ChartColor.FromRgba(153, 102, 255, 1f),
                    ChartColor.FromRgba(255, 159, 64, 1f)
                },
                BorderWidth = 5
            });
        }

        private async Task<List<decimal>> GraficaObtenerDatos() =>
            (await this.ServicioGanancias.ObtenerDeAsync(new PaginacionPeticion
            {
                Id = this.ResponsableState.Id,
                CantidadPorPagina = 12,
                NumeroPagina = 1,
                OrdenarPor = "Fecha"
            })).Lista.Select(ganancia => ganancia.Cantidad).ToList();
    }
}
