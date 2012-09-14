using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mithraeum.Api.Infra;
using Mithraeum.Api.Model;
using Mithraeum.Api.Modules;
using NUnit.Framework;

namespace Mithraeum.Api.Tests
{
    [TestFixture]
    public class ThothTests
    {
        [Test]
        public void Should_return_some_results_when_searching_for_The_Matrix()
        {
            var finder = new Thoth();
            IEnumerable<FinderOption> results = finder.FindByName("The Matrix");

            Assert.That(results.Count(),
                Is.GreaterThan(0));

        }

        [Test]
        public void Should_return_movie_info_when_searching_by_imdbid()
        {
            var finder = new Thoth();
            Movie movie = finder.FindByImdbId(new FinderOption(){ Imdbid = "tt0133093"});

            Assert.That(movie.Title,
                Is.EqualTo("Matrix"));

        }
    }
}
