namespace webapifoto.Models
{
    public class Foto
    {
        public IFormFile Archivo { get; set; }

        public string Nombre { get; set; }

        public string? Url { get; set; }
    }
}
