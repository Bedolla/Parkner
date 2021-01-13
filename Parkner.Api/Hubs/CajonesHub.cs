using Microsoft.AspNetCore.SignalR;
using Parkner.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Api.Hubs
{
    public class CajonesHub : Hub
    {
        public async Task AcepatarDatos(List<CajonDto> cajones) => await this.Clients.All.SendAsync("ExchangeChartData", cajones);
    }
}
