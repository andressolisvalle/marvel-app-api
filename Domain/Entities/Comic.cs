using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comic
    {
        public int Id { get; set; } // ID de Marvel API
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
