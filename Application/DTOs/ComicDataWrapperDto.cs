using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ComicDataWrapperDto
    {
        public ComicDataContainer data { get; set; }
    }

    public class ComicDataContainer
    {
        public List<ComicResultDto> results { get; set; }
    }
}
