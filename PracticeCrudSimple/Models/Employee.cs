using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeCrudSimple.Models
{
    public class Employee
    {

        [Key]
        public int Id { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(150)]
        public string FullName { get; set; }
        [MaxLength(250)]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public Byte Age { get; set; }
        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public Position   Position { get; set; }
    }
}
