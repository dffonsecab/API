using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiVentas.Custom;
using WebApiVentas.Models;
using WebApiVentas.Models.DTO.Producto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using System.Linq.Expressions;

namespace WebApiVentas.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        private readonly DbPruebaContext _dbPruebaContext;
 


        public ProductoController(DbPruebaContext dbPruebaContext)
        {
            _dbPruebaContext = dbPruebaContext;
          
        }


        /* Listar Productos */

        /**
 
        public async Task<List<Producto>> lista()
        {

            var listas = await _dbPruebaContext.Productos.ToListAsync();

            return listas;

        }
        **/

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> lista()
        {

            var lista = _dbPruebaContext.Productos.ToListAsync();

            return StatusCode(StatusCodes.Status200OK, new { data = lista });


        }
        


        /* --------------- Adicionar Productos ----------------------------- */

        [HttpPost]
        [Route("agregarProducto")]

        public async Task<IActionResult> AgregaProducto(ProductoRequestDto model)
        {

            Producto nuevo = new Producto
            {
                Nombre = model.Nombre,
                IdCategoria = model.IdCategoria,
                Precio = model.Precio,
                Marca= model.Marca,
         

            };
          
           await _dbPruebaContext.Productos.AddAsync(nuevo);
           await _dbPruebaContext.SaveChangesAsync();

           
            
          return StatusCode(StatusCodes.Status201Created, new {Rpta="Creado",data=nuevo});
       

        }



        /* ------------------------- Eliminar Producto --------------------------------- */

        [HttpDelete]
        [Route("delete/{id}")]

        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var productoEncontrado=await _dbPruebaContext.Productos.FirstOrDefaultAsync(x=>x.IdProducto==id);
                if (productoEncontrado != null)
                {

                   var eliminar=_dbPruebaContext.Productos.Remove(productoEncontrado);
                   await _dbPruebaContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new { resul = "Producto Eliminado" });

                }


            }
            catch (Exception ex)
            {

                throw new TaskCanceledException($"Imposible realizar la eliminacion {ex.Message} ");


            }

            return StatusCode(StatusCodes.Status404NotFound, new { reslt = "Imposible eliminar el producto" });

        }



        /* --------------- Actualizar Producto ------------------------------- */

        [HttpPut]
        [Route("actualizar")]


        public async Task<IActionResult> actualizar(ProductoRequestDto producto)
        {

            Producto buscarProducto = await _dbPruebaContext.Productos.FirstAsync(x => x.IdProducto == producto.Id);


            try {

                if (buscarProducto != null)
                {


                    buscarProducto.Nombre = producto.Nombre;
                    buscarProducto.IdCategoria = producto.IdCategoria;
                    buscarProducto.Marca = producto.Marca;
                    buscarProducto.Precio=producto.Precio;
                    
                  

                    _dbPruebaContext.Productos.Update(buscarProducto);
                    _dbPruebaContext.SaveChanges();

                }

             }catch{

                throw new TaskCanceledException( "Imposible actualizar el producto");

            }




            return StatusCode(StatusCodes.Status201Created, new { reult = "ok" });

        }


        /* ----------------- Obtener Producto ------------- */

        [HttpGet]
        [Route("obtener/{id}")]

        public async Task<IActionResult> obtener(int id)
        {
            try
            {
                Producto obtener = await _dbPruebaContext.Productos.FirstAsync(x => x.IdProducto == id);

                if (obtener != null)
                {

                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        data = obtener

                    });

                }
                
            }
            catch
            {

                throw;

            }

            return StatusCode(StatusCodes.Status200OK, new { data = "Sin Contenido" });

        }
    }
}
