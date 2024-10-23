using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiVentas.Custom;
using WebApiVentas.Models;
using WebApiVentas.Models.DTO.Usuario;
using Microsoft.AspNetCore.Authorization;

namespace WebApiVentas.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous] //Permite el acceso al api sin necesidad de autorizacion
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly DbPruebaContext _dbContext;
        private readonly Utilidades _utilidades;


        public AccesoController(DbPruebaContext dbContext, Utilidades utilidades)
        {

            _dbContext = dbContext;
            _utilidades = utilidades;
 
        }


        /* Registrar Usuario */

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult>Registrarse(UsuarioRequestDto objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                Clave = _utilidades.encriptarSha256(objeto.Clave!)
            };

            /* Guardar en la Base de Datos */
            await _dbContext.Usuarios.AddAsync(modeloUsuario);
            await _dbContext.SaveChangesAsync();

            if (modeloUsuario.IdUsuario != 0)
                
                return StatusCode(StatusCodes.Status201Created, new { isSuccess=true });
            else

                return StatusCode(StatusCodes.Status201Created, new { isSuccess =false });

        }

        /*---- Login de Usuario */

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto objeto)
        {

            var UsuarioEncontrado = await _dbContext.Usuarios.Where(x => 

                x.Correo == objeto.Correo && x.Clave == _utilidades.encriptarSha256(objeto.Clave)
                
                ).FirstOrDefaultAsync();


            if (UsuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    isSucces = false,
                    token = ""
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    isSucces = true,
                    token = _utilidades.generarJWT(UsuarioEncontrado)

                });

            }



        }











    }
}
