using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FavoriteComic
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int ComicId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public User? User { get; set; }
    }
}
