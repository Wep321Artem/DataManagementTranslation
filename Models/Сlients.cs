using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataManagementTranslation.Models
{
    public class Clients
    {
        [Key]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
       
        public int? CardCode{ get; set;}

        [MaxLength(1_000)]
        public string? LastName { get; set; }

        [MaxLength(1_000)]
        public string? FirstName{ get; set;}

        [MaxLength(1_000)]
        public string? SurName { get; set; }

        [MaxLength(1_000)]
        public string? PhoneMobile { get; set;}

        [MaxLength(1_000)]
        public string? Email { get; set;}

        [MaxLength(1_000)]
        public string? GenderId { get; set;}

        public DateTime? Birthday { get; set;}

        [MaxLength(1_000)]
        public string? City { get; set;}

        
        public int? Pincode { get; set;}

        
        public int? Bonus { get; set;}

       
       public int? Turnover { get; set;}


        
    }
}
