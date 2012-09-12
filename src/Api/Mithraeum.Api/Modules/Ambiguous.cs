using System.Collections.Generic;

namespace Mithraeum.Api.Modules
{
    public class Ambiguous 
    {
        public string Title { get; set; }
        public IEnumerable<FinderOption> Options { get; set; }
    }
}