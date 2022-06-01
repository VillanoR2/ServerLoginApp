using Microsoft.AspNetCore.Mvc;
using ServerLoginApp.Interfaces;
using ServerLoginApp.Modelos;
using ServerLoginApp.Modelos.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServerLoginApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuario _usuario;
        protected ResponseDto _response;

        public UsuariosController(IUsuario usuario)
        {
            _usuario = usuario;
            _response = new ResponseDto();
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                var lista = await _usuario.GetUsuarios();
                _response.Result = lista;
                _response.DisplayMessage = "Lista de Usuarios";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return Ok(_response);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuario.GetUsuarioById(id);
            if (usuario == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Usuario no existe";
                return NotFound(_response);
            }

            _response.Result = usuario;
            _response.DisplayMessage = "Informacion de Usuario";
            return Ok(_response);
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDto usuarioDto)
        {
            try
            {
                UsuarioDto model = await _usuario.CreateUpdate(usuarioDto);
                _response.Result = model;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error Al Actualizar el Registro";
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ActionName("register")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostRegisterUsuario(string authCode, [FromBody] UsuarioDto usuarioDto)
        {

            BindIdManager bindIdManager = new BindIdManager();
            BindID bindID = await bindIdManager.PostRequestGetUserToken(authCode);

            BindIdManager bindIdManager2 = new BindIdManager();

            usuarioDto.BindAlias = usuarioDto.Nombre_Usuario;

            try
            {
                UsuarioDto model = await _usuario.CreateUpdate(usuarioDto);
                _response.Result = model;

                HttpResponseMessage responseMessage = await bindIdManager2.PostSendBindAliasIsNewUser(bindID.AccessToken, usuarioDto.BindAlias);

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return CreatedAtAction("GetUsuario", new { id = model.Id }, _response);
                }
                else
                {
                    bool estaEliminado = await _usuario.DeleteUsuario(usuarioDto.Id);
                    if (estaEliminado)
                    {
                        _response.Result = estaEliminado;
                        _response.DisplayMessage = "Error al registrar en Transmit, Usuario eliminado con exito";
                        return BadRequest(_response);
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.DisplayMessage = "Error al registrar en Transmit, Error al eliminar el Registro";
                        return BadRequest(_response);
                    }
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error Al Actualizar el Registro";
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }



        }

        [ActionName("login")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostLoginUsuario(string authCode)
        {
            //postrequestGetUser
            //Si bindalias -> autenticar
            //dtousuario.BuscarBindAlias

            //sino -> registrar
            //PostSendBindAlias
            //si ok -> dtoUsuario.CreateUpdate
            //sino -> Error Al Actualizar el Registro;

            BindIdManager bindIdManager = new BindIdManager();
            BindID bindID = await bindIdManager.PostRequestGetUserToken(authCode);
            var stream = bindID.IdToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var bindAlias = tokenS.Claims.First(claim => claim.Type == "bindid_alias").Value;

            if (bindAlias == null)
            {
                _response.DisplayMessage = "Este usuario no esta registrado";
                return NotFound(_response);
            }
            else
            {
                var usuario = await _usuario.GetUsuarioByBindAlias(bindAlias);

                if (bindAlias == usuario.BindAlias)
                {
                    _response.Result = usuario;
                    _response.DisplayMessage = "Usuario logeado exitosamente";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Error al logear al usuario, *ERROR INTERNO*";
                    return BadRequest(_response);
                }
            }

        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                bool estaEliminado = await _usuario.DeleteUsuario(id);
                if (estaEliminado)
                {
                    _response.Result = estaEliminado;
                    _response.DisplayMessage = "Usuario eliminado con exito";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Error al eliminar el Registro";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
