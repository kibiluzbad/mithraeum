using System.Collections.Generic;

namespace Mithraeum.Api.Model
{
    public class Movie
    {
        public string Imdbid { get; set; }
        public string Picture_Path { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Plot { get; set; }
        public decimal Rating { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public IEnumerable<Person> Directors { get; set; }
        public IEnumerable<Person> Writers { get; set; }
        public IEnumerable<Character> Cast { get; set; }
    }

    public class Person
    {
        public string Imdbid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Character : Person
    {
        public string Picture_Path { get; set; }
        public string Actor { get; set; }
    }
}