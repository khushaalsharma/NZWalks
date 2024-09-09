using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddRegionRequestDto //DTO to get the data from the client
    {
        [Required] //this says that following property is non-nullable
        [MinLength(3, ErrorMessage = "Code should be of 3 characters")] //code has to be 3 character long
        [MaxLength(3, ErrorMessage = "Code should be of 3 characters")]

        //[StringLength(3, ErrorMessage = "Code should be 3 characters long")] this will set max length to 3 instead min and max to be 3
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
