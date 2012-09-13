using System.Collections.Generic;

namespace Mithraeum.Api.Model
{
    public class Ambiguous 
    {
        public string Title { get; set; }
        public IEnumerable<FinderOption> Options { get; set; }
    }
}