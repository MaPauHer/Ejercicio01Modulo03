using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EjercicioClase1Modulo3.Controllers
{
    [Route("v1/libros")] 
    [ApiController]
    public class BookController : ControllerBase
    {
        //Books contiene una lista de libros. Esta información viene del archivo libros.json ubicado dentro de la carpeta Data.
        public List<Book> Books { get; set; }

        //filePath contiene la ubicación del archivo libros.json. No mover el archivo libros.json de esa carpeta.
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\libros.json");

        public BookController()
        {
            //Instanciación e inicialización de la lista de libros deserializando el archivo libros.json
            Books = JsonSerializer.Deserialize<List<Book>>(System.IO.File.ReadAllText(filePath));
        }

        #region Ejercicio 1
        /*
        Completar y modificar el método siguiente para crear un endpoint que liste todos los libros y tenga la siguiente estructura:
        [GET] v1/libros
        */


        [HttpGet]
        [Route(" ")] //Prueba --> https://localhost:7165/v1/libros
        public IActionResult GetBooks()
        {

            return Ok(Books);

        }

        #endregion

        #region Ejercicio 2
        /*
         Crear un endpoint para Obtener un libro por su número de id usando route parameters que tenga la siguiente estructura:
        [GET] v1/libros/{id}
        Ejemplo: v1/libros/8 (devuelve toda la información del libro cuyo id es 8. Es decir: El diario de Ana Frank)
        */

        [HttpGet]
        [Route("{id}")] //Prueba --> https://localhost:7165/v1/libros/8
        public IActionResult GetBookById([FromRoute] int id)
        {

            var book = Books.FirstOrDefault(w => w.id == id);
            if (id <= 0)
            {
                return BadRequest("El Id debe ser mayor a 0.");
            }
            else
            {
                if (book == null)
                {
                    //return BadRequest("No se encontro el libro solicitado.");
                    return Ok("No se encontro el libro solicitado.");
                }
                else
                {
                    return Ok(book);
                }
            }

        }

        #endregion

        #region Ejercicio 3
        /*
         Crear un endpoint para listar todos libros de un género en particular usando route parameters que tenga la siguiente estructura:
        [GET] v1/libros/genero/{genero} 
        Ejemplo: v1/libros/genero/fantasía (devuelve una lista de todos los libros del género fantasía)
         */

        [HttpGet]
        [Route("genero/{genero}")] //Prueba --> https://localhost:7165/v1/libros/genero/Autoayuda
        public IActionResult GetBooksByGenero([FromRoute] string genero)
        {

            var booksPorGenero = Books.Where(w => w.genero == genero).ToList();
            int cantGeneros = booksPorGenero.Count;
            if (cantGeneros > 0)
            {
                return Ok(booksPorGenero);
            }
            else
            {
                //return BadRequest("No existen libros del genero solicitado.");
                return Ok("No existen libros del genero solicitado.");
            }

        }

        #endregion

        #region Ejercicio 4
        /*
        Crear un endpoint para Listar todos los libros de un autor usando query parameters que tenga la siguiente estructura:
        [GET] v1/libros?autor={autor}
        Ejemplo: v1/libros?autor=Paulo Coelho (devuelve una lista de todos los libros del autor Paulo Coelho)
         */

        [HttpGet]
        [Route("autor")] //Prueba --> https://localhost:7165/v1/libros/autor?autor=Paulo%20Coelho
        public IActionResult GetBooksByAutor([FromQuery] string autor)
        {

            var booksPorAutor = Books.Where(w => w.autor == autor).ToList();
            int cantLibrosAutor = booksPorAutor.Count;
            if (cantLibrosAutor > 0)
            {
                return Ok(booksPorAutor);
            }
            else
            {
                //return BadRequest("No existen libros del autor solicitado.");
                return Ok("No existen libros del autor solicitado.");
            }

        }

        #endregion

        #region Ejercicio 5
        /*
        Crear un endpoint para Listar unicamente todos los géneros de libros disponibles que tenga la siguiene estructura:
        [GET] v1/libros/generos
        Idealmente, el listado de géneros que retorne el endpoint, no debe contener repetidos.
         */

        [HttpGet]
        [Route("generos")] //Prueba --> https://localhost:7165/v1/libros/generos
        public IActionResult GetGeneros()
        {

            var generos = Books.Select(s => s.genero).Distinct().ToList();
            return Ok(generos);

        }

        #endregion

        #region Ejercicio 6
        /*
        Crear un endpoint para listar todos los libros implementando paginación usando route parameters con la siguiente estructura:
        [GET] v1/libros?pagina={numero-pagina}&cantidad={cantidad-por-pagina}
        Ejemplos: 
        v1/libros?pagina=1&cantidad=10 (devuelve una lista de los primeros diez libros)
        v1/libros?pagina=2&cantidad=10 (devuelve una lista de diez libros, salteando los primeros 10)
        v1/libros?pagina=3&cantidad=10 (devuelve una lista de diez libros, salteando los primeros 20)
         */

        [HttpGet]
        [Route("")] //Prueba --> https://localhost:7165/v1/libros?pagina=1&cantidad=10
        public IActionResult GetPorPagina([FromQuery] int pagina, [FromQuery] int cantidad)
        {

            if (pagina < 1)
            {
                return BadRequest("Numero de página debe ser mayor a cero.");
            }
            else
            {
                if (cantidad < 1)
                {
                    return BadRequest("Cantidad por página deber ser mayor a cero.");
                }
                else
                {

                    int cantLibros = Books.Count;
                    int cantSaltar = (pagina - 1) * cantidad;
                    Console.WriteLine($"Cantidad Libros {cantLibros}");
                    Console.WriteLine($"Cantidad Saltar {cantSaltar}");

                    if (cantSaltar < cantLibros)
                    {
                        var booksByPage = Books.Skip(cantSaltar).Take(cantidad).ToList();
                        return Ok(booksByPage);
                    }
                    else
                    {
                        //return BadRequest("No quedan libros por mostrar.");
                        return Ok("No quedan libros por mostrar.");
                    }

                }
            }

        }

        #endregion
    }
}
