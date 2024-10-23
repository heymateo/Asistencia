using Assistance.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistance.Services
{
#nullable enable
    public class CentroEducativoService
    {
        private readonly AssistanceDbContext _context;

        public CentroEducativoService(AssistanceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteCentroEducativoAsync()
        {
            return await _context.Centro_Educativo.AnyAsync();
        }

        public async Task<Centro_Educativo?> ObtenerCentroEducativoAsync()
        {
            return await _context.Centro_Educativo.FirstOrDefaultAsync();
        }

        public async Task GuardarCentroEducativoAsync(Centro_Educativo centro)
        {
            if (await ExisteCentroEducativoAsync())
            {
                throw new InvalidOperationException("Ya existe un centro educativo. No se puede crear más de uno.");
            }

            _context.Centro_Educativo.Add(centro);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCentroEducativoAsync(Centro_Educativo centro)
        {
            _context.Centro_Educativo.Update(centro);
            await _context.SaveChangesAsync();
        }

    }
}
