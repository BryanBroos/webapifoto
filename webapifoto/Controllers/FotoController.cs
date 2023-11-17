using Microsoft.AspNetCore.Mvc;
using webapifoto.Models;

namespace webapifoto.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class FotoController : Controller
    {
        public readonly IWebHostEnvironment _webHostEnviroment;

        public FotoController(IWebHostEnvironment webHostenviroment)
        {
            _webHostEnviroment = webHostenviroment;
        }

        [HttpPost]
        public async Task<ActionResult<Foto>> PostIndex([FromForm] Foto foto)
        {
            try
            {
                string webRootPath = _webHostEnviroment.WebRootPath;
                string rutaArchivos = Path.Combine(webRootPath, "files");
                if (foto.Archivo.Length > 0)
                {
                    if (!Directory.Exists(rutaArchivos))
                    {
                        Directory.CreateDirectory(rutaArchivos);
                    }
                    using (FileStream fileStream = System.IO.File.Create(Path.Combine(rutaArchivos, foto.Archivo.FileName)))
                    {
                        await foto.Archivo.CopyToAsync(fileStream);
                        fileStream.Flush();
                    }
                    foto.Url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/files/" + foto.Archivo.FileName;
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message + _webHostEnviroment.WebRootPath);
            }
            return CreatedAtAction(nameof(PostIndex), new { foto.Nombre, foto.Url });
        }
    }
}
