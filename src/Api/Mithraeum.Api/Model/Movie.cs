using System.Collections.Generic;

namespace Mithraeum.Api.Model
{
    public class Movie
    {
        public string Imdbid { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Plot { get; set; }
        public decimal Rate { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}