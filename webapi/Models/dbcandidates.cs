using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Models
{
    public class dbcandidates
    {
        [Key]
        public int id { get; set; }
        
        [Column(TypeName ="nvarchar(100)")]
        public string fullname { get; set; }
        
        [Column(TypeName = "nvarchar(10)")]
        public string mobile { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string email { get; set; }

        public int age { get; set; }

        [Column(TypeName = "nvarchar(3)")]
        public string bloodgroup { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string address { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string sex { get; set; }

    }
}
