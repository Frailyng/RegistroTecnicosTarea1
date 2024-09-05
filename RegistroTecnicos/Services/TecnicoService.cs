﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Models;

namespace RegistroTecnicos.Services;

public class TecnicoService
{
    private readonly Contexto _context;
    public TecnicoService(Contexto contexto)
    {
        _context = contexto;
    }

    public async Task<bool> Guardar(Tecnicos Tecnicos)
    {
        if (!await Existe(Tecnicos.TecnicoId))
            return await Insertar(Tecnicos);
        else
            return await Modificar(Tecnicos);
    }

    public async Task<bool> Insertar(Tecnicos Tecnicos)
    {
        _context.Tecnicos.Add(Tecnicos);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Tecnicos Tecnicos)
    {
        _context.Update(Tecnicos);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> Existe(int TecnicoId)
    {
        return await _context.Tecnicos
            .AnyAsync(p => p.TecnicoId == TecnicoId);
    }

    public async Task<bool> Existe(string? descripcion, int? tecnicoId = null)
    {
        return await _context.Tecnicos
            .AnyAsync(p => p.Descripcion.Equals(descripcion));
    }

    public async Task<bool> Existe(int tecnicoId, string? descripcion)
    {
        //TODO: Unir los dos existe en uno solo para reducir duplicidad de codigo.
        return await _context.Tecnicos
        .AnyAsync(p => p.TecnicoId != tecnicoId && p.Descripcion.Equals(descripcion));


    }

    public async Task<bool> Eliminar(int id)
    {
        var tecnicos = await _context.Tecnicos
            .Where(p => p.TecnicoId == id)
            .ExecuteDeleteAsync();
        return tecnicos > 0;

    }

    public async Task<Tecnicos?> Buscar(int id)
    {
        return await _context.Tecnicos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.TecnicoId == id);

    }

    public async Task<List<Tecnicos>> Listar(Expression<Func<Tecnicos, bool>> criterio)
    {
        return await _context.Tecnicos
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }
}
