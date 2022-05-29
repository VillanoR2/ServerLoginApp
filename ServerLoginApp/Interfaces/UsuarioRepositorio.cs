using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServerLoginApp.Data;
using ServerLoginApp.Modelos;
using ServerLoginApp.Modelos.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerLoginApp.Interfaces
{
    public class UsuarioRepositorio : IUsuario
    {

        private readonly ApplicationDbContext _db;

        private IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<UsuarioDto> CreateUpdate(UsuarioDto usuarioDto)
        {
            Usuario usuario = _mapper.Map<UsuarioDto, Usuario>(usuarioDto);
            if(usuario.Id > 0)
            {
                _db.Usuarios.Update(usuario);
            }
            else
            {
                await _db.Usuarios.AddAsync(usuario);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Usuario, UsuarioDto>(usuario);
        }



        public async Task<bool> DeleteUsuario(int id)
        {
            try
            {
                Usuario usuario = await _db.Usuarios.FindAsync(id);
                if(usuario == null)
                {
                    return false;
                }
                _db.Usuarios.Remove(usuario);
                await _db.SaveChangesAsync();

                return true;

            }catch (Exception)
            {
                return false;
            }
        }

        public async Task<UsuarioDto> GetUsuarioById(int id)
        {
            Usuario usuario = await _db.Usuarios.FindAsync(id);

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<List<UsuarioDto>> GetUsuarios()
        {
            List<Usuario> usuarioList = await _db.Usuarios.ToListAsync();

            return _mapper.Map<List<UsuarioDto>>(usuarioList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindAlias"></param>
        /// <returns><see cref="Usuario"/> por <paramref name="bindAlias"/>.
        /// Regresa NULL si no existe. </returns>
        public async Task<UsuarioDto> GetUsuarioByBindAlias(string bindAlias)
        {
            Usuario usuario = await _db.Usuarios.FirstOrDefaultAsync(usuario => usuario.BindAlias == bindAlias);

            return _mapper.Map<UsuarioDto>(usuario);
        }
    }
}
