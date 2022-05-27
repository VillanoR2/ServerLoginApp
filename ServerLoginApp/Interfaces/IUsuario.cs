using ServerLoginApp.Modelos.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerLoginApp.Interfaces
{
    public interface IUsuario
    {
        Task<List<UsuarioDto>> GetUsuarios();
        Task<UsuarioDto> GetUsuarioById (int id);
        Task<UsuarioDto> CreateUpdate (UsuarioDto usuarioDto);
        Task<bool> DeleteUsuario (int id);

    }
}
