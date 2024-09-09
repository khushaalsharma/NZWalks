using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [NotMapped] //This ensures that File attribute is not added to the SQL model
        public IFormFile File  { get; set; } //IFormFile represents a file sent from http request
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; } //in bytes
        public string FilePath { get; set; }
    }
}
