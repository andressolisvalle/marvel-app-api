using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ComicResultDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnail thumbnail { get; set; }
    }

    public class Thumbnail
    {
        public string path { get; set; }
        public string extension { get; set; }
    }
}
