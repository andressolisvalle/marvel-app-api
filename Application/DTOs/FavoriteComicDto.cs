﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FavoriteComicDto
    {
        public Guid Id { get; set; }
        public int ComicId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
