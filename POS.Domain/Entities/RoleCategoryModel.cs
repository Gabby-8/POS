using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entities
{
    public class RoleCategoryModel
    {
        [Key]
        public int ID { get; set; }
        public string? RoleName { get; set; }
    }
}
