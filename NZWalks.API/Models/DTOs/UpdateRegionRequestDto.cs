using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code must be of 3 characters only")]
        [MaxLength(3, ErrorMessage = "Code must be of 3 characters only")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
