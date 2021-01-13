using Microsoft.EntityFrameworkCore;
using Parkner.Api.Models;
using Parkner.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Parkner.Api.Repositories
{
    public interface IRepositorio<TEntidad> where TEntidad : class
    {
        Task<int> GuardarAsync();
        Task<TEntidad> ObtenerPorIdAsync(params object[] id);
        Task<TEntidad> ObtenerPorIdConAsociacionesAsync(string id, params Expression<Func<TEntidad, object>>[] incluidos);
        Task<List<TEntidad>> ObtenerPrimerosAsync(int cantidad);
        Task<TEntidad> ObtenerDondeAsync(Expression<Func<TEntidad, bool>> condicion);
        Task CrearAsync(TEntidad entidad);
        void Borrar(TEntidad entidad);
        void Actualizar(TEntidad entidad);
        Task<TEntidad> ObtenerDondeConAsociacionesAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] incluidos);
        Task<List<TEntidad>> ObtenerVariosDondeAsync(Expression<Func<TEntidad, bool>> condicion);
        IQueryable<TEntidad> ObtenerTodoComoConsuntable();
        Task<List<TEntidad>> ObtenerVariosDondeConAsociacionesAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] incluidos);
        Task<List<TEntidad>> ObtenerTodoAsync();
        Task<List<TEntidad>> ObtenerTodoConAsociacionesAsync(params Expression<Func<TEntidad, object>>[] incluidos);
        Task CrearVariosAsync(List<TEntidad> entidades);
        Task CrearVariosAsync(params TEntidad[] entidades);
        void BorrarVarios(List<TEntidad> entidades);
        void BorrarVarios(params TEntidad[] entidades);
        void ActualizarVarios(List<TEntidad> entidades);
        void ActualizarVarios(params TEntidad[] entidades);
        Task<bool> AlgunoDondeAsync(Expression<Func<TEntidad, bool>> condicion);
    }

    public class Repositorio<TEntidad> : IRepositorio<TEntidad> where TEntidad : Base
    {
        public Repositorio(Contexto contexto) => this.Contexto = contexto;
        public Contexto Contexto { get; }

        public async Task<int> GuardarAsync() => await this.Contexto.SaveChangesAsync();

        public async Task<TEntidad> ObtenerPorIdAsync(params object[] id) => await this.Contexto.Set<TEntidad>().FindAsync(id);

        public async Task<TEntidad> ObtenerPorIdConAsociacionesAsync(string id, params Expression<Func<TEntidad, object>>[] incluidos)
        {
            IQueryable<TEntidad> consulta = this.Contexto.Set<TEntidad>().AsQueryable();
            consulta = incluidos.Aggregate(consulta, (a, i) => a.Include(i));
            consulta = consulta.Where(c => c.Id.Equals(id));
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<TEntidad>> ObtenerPrimerosAsync(int cantidad) => await this.Contexto.Set<TEntidad>().Take(cantidad).ToListAsync();

        public async Task<TEntidad> ObtenerDondeAsync(Expression<Func<TEntidad, bool>> condicion) => await this.Contexto.Set<TEntidad>().Where(condicion).FirstOrDefaultAsync();

        public async Task CrearAsync(TEntidad entidad) => await this.Contexto.Set<TEntidad>().AddAsync(entidad);

        public void Borrar(TEntidad entidad) => this.Contexto.Set<TEntidad>().Remove(entidad);

        public void Actualizar(TEntidad entidad) => this.Contexto.Set<TEntidad>().Update(entidad);

        public async Task<TEntidad> ObtenerDondeConAsociacionesAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] incluidos)
        {
            IQueryable<TEntidad> consulta = this.Contexto.Set<TEntidad>().AsQueryable();
            consulta = incluidos.Aggregate(consulta, (a, i) => a.Include(i));
            consulta = consulta.Where(condicion);
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<List<TEntidad>> ObtenerVariosDondeAsync(Expression<Func<TEntidad, bool>> condicion) => await this.Contexto.Set<TEntidad>().Where(condicion).ToListAsync();

        public IQueryable<TEntidad> ObtenerTodoComoConsuntable() => this.Contexto.Set<TEntidad>().AsQueryable();

        public async Task<List<TEntidad>> ObtenerVariosDondeConAsociacionesAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] incluidos)
        {
            IQueryable<TEntidad> consulta = this.Contexto.Set<TEntidad>().AsQueryable();
            consulta = incluidos.Aggregate(consulta, (a, i) => a.Include(i));
            consulta = consulta.Where(condicion);
            return await consulta.ToListAsync();
        }

        public async Task<List<TEntidad>> ObtenerTodoAsync() => await this.Contexto.Set<TEntidad>().ToListAsync();

        public async Task<List<TEntidad>> ObtenerTodoConAsociacionesAsync(params Expression<Func<TEntidad, object>>[] incluidos)
        {
            IQueryable<TEntidad> consulta = this.Contexto.Set<TEntidad>().AsQueryable();
            consulta = incluidos.Aggregate(consulta, (a, i) => a.Include(i));
            return await consulta.ToListAsync();
        }

        public async Task CrearVariosAsync(List<TEntidad> entidades) => await this.Contexto.Set<TEntidad>().AddRangeAsync(entidades);

        public async Task CrearVariosAsync(params TEntidad[] entidades) => await this.Contexto.Set<TEntidad>().AddRangeAsync(entidades);

        public void BorrarVarios(List<TEntidad> entidades) => this.Contexto.Set<TEntidad>().RemoveRange(entidades);

        public void BorrarVarios(params TEntidad[] entidades) => this.Contexto.Set<TEntidad>().RemoveRange(entidades);

        public void ActualizarVarios(List<TEntidad> entidades) => this.Contexto.Set<TEntidad>().UpdateRange(entidades);

        public void ActualizarVarios(params TEntidad[] entidades) => this.Contexto.Set<TEntidad>().UpdateRange(entidades.ToList());

        public async Task<bool> AlgunoDondeAsync(Expression<Func<TEntidad, bool>> condicion) => await this.Contexto.Set<TEntidad>().AnyAsync(condicion);
    }
}
