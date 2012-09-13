using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mithraeum.Api.Model;
using Mithraeum.Api.Modules;
using Moq;
using NUnit.Framework;
using Nancy.Json;
using Nancy.Testing;
using Raven.Client;
using Raven.Client.Linq;

namespace Mithraeum.Api.Tests
{
    [TestFixture]
    public class MoviesTests
    {
        [Test]
        public void Should_list_all_movies_when_calling_Api_Movies()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            fakeSession
                .Setup(c => c.Query<Movie>())
                .Returns(GetMovies());

            var bootstrapper = new FakeBootstrapper()
                                   {
                                       FakeSession = () => fakeSession.Object,
                                       FakeFinder = () => fakeFinder.Object
                                   };

            var browser = new Browser(bootstrapper);
            
            var response = browser
                .Get("/api/movies", 
                with => with.AjaxRequest());

            var serializer = new JavaScriptSerializer();

            var movies = serializer.DeserializeObject(response.Body.AsString()) as ICollection;

            Assert.That(movies.Count,
                Is.EqualTo(2));
            
        }

        [Test]
        public void Should_use_jsonp_when_passing_callback_parameter()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            fakeSession
                .Setup(c => c.Query<Movie>())
                .Returns(GetMovies());

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => fakeSession.Object,
                FakeFinder = () => fakeFinder.Object
            };

            var browser = new Browser(bootstrapper);

            var reponse = browser.Get("/api/movies", 
                with =>
            {
                with.AjaxRequest();
                with.Query("callback","cb");
            });

            var result = reponse.Body.AsString();

            Assert.That(Regex.IsMatch(result,"cb\\([^\\)]+\\)"),
                Is.True);

        }

        [Test]
        public void Should_return_movie_info_when_adding_new_movie_by_name()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            var matrixResult = new FinderOption {Imdbid = "tt0133093", Title = "Matrix"};

            fakeFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult
                             });

            fakeFinder
               .Setup(c => c.FindByImdbId(It.Is<FinderOption>(option => option.Imdbid == "tt0133093")))
               .Returns(GetMatrixMovieInfo());

            var bootstrapper = new FakeBootstrapper()
                                   {
                                       FakeSession = () => fakeSession.Object,
                                       FakeFinder = () => fakeFinder.Object
                                   };

            var browser = new Browser(bootstrapper);

            var reponse = browser.Post("/api/movies",
                                       with =>
                                           {
                                               with.AjaxRequest();
                                               with.FormValue("name", "The matrix");
                                           });

            var serializer = new JavaScriptSerializer();
            var movie = serializer.Deserialize<Movie>(reponse.Body.AsString());

            Assert.That(movie.Imdbid,
                        Is.EqualTo("tt0133093"));

        }

        [Test]
        public void Should_return_Ambiguous_when_finder_returns_more_than_one_record_for_movie_name()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            var matrixResult = new FinderOption {Imdbid = "tt0133093", Title = "Matrix"};

            var matrixReloadedResult = new FinderOption {Imdbid = "tt0133093", Title = "MatrixReloaded"};

            fakeFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult,
                                 matrixReloadedResult
                             });

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => fakeSession.Object,
                FakeFinder = () => fakeFinder.Object
            };

            var browser = new Browser(bootstrapper);

            var reponse = browser.Post("/api/movies",
                                       with =>
                                       {
                                           with.AjaxRequest();
                                           with.FormValue("name", "The matrix");
                                       });

            var serializer = new JavaScriptSerializer();
            var ambigous = serializer.Deserialize<Ambiguous>(reponse.Body.AsString());

            Assert.That(ambigous.Options.Count(),
                        Is.EqualTo(2));
        }

        [Test]
        public void Should_call_finder_FindByName_to_search_movies()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var mockFinder = new Mock<IMoviesFinder>();

            var matrixResult = new FinderOption {Imdbid = "tt0133093", Title = "Matrix"};

            var matrixReloadedResult = new FinderOption {Imdbid = "tt0133093", Title = "MatrixReloaded"};

            mockFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult,
                                 matrixReloadedResult
                             })
                             .Verifiable();

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => fakeSession.Object,
                FakeFinder = () => mockFinder.Object
            };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                                       with =>
                                       {
                                           with.AjaxRequest();
                                           with.FormValue("name", "The matrix");
                                       });

            mockFinder.Verify(c=>c.FindByName(It.IsAny<string>()),Times.Once());

        }

        [Test]
        public void Should_not_call_finder_FindByImdbId_when_FindByName_returns_more_than_one_result()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var mockFinder = new Mock<IMoviesFinder>();

            var matrixResult = new FinderOption {Imdbid = "tt0133093", Title = "Matrix"};

            var matrixReloadedResult = new FinderOption {Imdbid = "tt0133093", Title = "MatrixReloaded"};

            mockFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult,
                                 matrixReloadedResult
                             });

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => fakeSession.Object,
                FakeFinder = () => mockFinder.Object
            };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                                       with =>
                                       {
                                           with.AjaxRequest();
                                           with.FormValue("name", "The matrix");
                                       });

            mockFinder.Verify(c => c.FindByImdbId(It.IsAny<FinderOption>()), Times.Never());

        }

        [Test]
        public void Should_call_finder_FindByImdbId_when_FindByName_returns_only_one_result()
        {
            var fakeSession = new Mock<IDocumentSession>();
            var mockFinder = new Mock<IMoviesFinder>();

            var matrixResult = new FinderOption {Imdbid = "tt0133093", Title = "Matrix"};

            mockFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult
                             });

            mockFinder
               .Setup(c => c.FindByImdbId(It.Is<FinderOption>(options => options.Imdbid == "tt0133093")))
               .Returns(GetMatrixMovieInfo())
               .Verifiable();

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => fakeSession.Object,
                FakeFinder = () => mockFinder.Object
            };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                                       with =>
                                       {
                                           with.AjaxRequest();
                                           with.FormValue("name", "The matrix");
                                       });

            mockFinder.Verify(c => c.FindByImdbId(It.IsAny<FinderOption>()), Times.Once());

        }

        [Test]
        public void Should_call_session_Store_when_FindByName_returns_only_one_result()
        {
            var mockSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            mockSession
                .Setup(c => c.Store(It.IsAny<Movie>(), It.Is<string>(imdbid => imdbid == "tt0133093")))
                .Verifiable();

            var matrixResult = new FinderOption {Imdbid = "tt0133093", Title = "Matrix"};

            fakeFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult
                             });

            fakeFinder
                .Setup(c => c.FindByImdbId(It.Is<FinderOption>(options => options.Imdbid == "tt0133093")))
                .Returns(GetMatrixMovieInfo())
                .Verifiable();

            var bootstrapper = new FakeBootstrapper()
                                   {
                                       FakeSession = () => mockSession.Object,
                                       FakeFinder = () => fakeFinder.Object
                                   };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                         with =>
                             {
                                 with.AjaxRequest();
                                 with.FormValue("name", "The matrix");
                             });

            mockSession.Verify(c => c.Store(It.IsAny<Movie>(),
                                            It.Is<string>(imdbid => imdbid == "tt0133093")),
                               Times.Once());
        }

        [Test]
        public void Should_not_call_session_Store_without_id_when_storing_movies()
        {
            var mockSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            mockSession
                .Setup(c => c.Store(It.IsAny<Movie>(), It.Is<string>(imdbid => imdbid == "tt0133093")))
                .Verifiable();

            var matrixResult = new FinderOption { Imdbid = "tt0133093", Title = "Matrix" };

            fakeFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult
                             });

            fakeFinder
                .Setup(c => c.FindByImdbId(It.Is<FinderOption>(options => options.Imdbid == "tt0133093")))
                .Returns(GetMatrixMovieInfo())
                .Verifiable();

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => mockSession.Object,
                FakeFinder = () => fakeFinder.Object
            };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                         with =>
                         {
                             with.AjaxRequest();
                             with.FormValue("name", "The matrix");
                         });

            mockSession.Verify(c => c.Store(It.IsAny<Movie>()),
                               Times.Never());
        }

        [Test]
        public void Should_call_session_Store_when_storing_Ambiguous_result()
        {
            var mockSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            mockSession
                .Setup(c => c.Store(It.IsAny<Ambiguous>(),It.IsAny<string>()))
                .Verifiable();

            var matrixResult = new FinderOption { Imdbid = "tt0133093", Title = "Matrix" };

            var matrixReloadedResult = new FinderOption { Imdbid = "tt0133093", Title = "MatrixReloaded" };

            fakeFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult,
                                 matrixReloadedResult
                             });

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => mockSession.Object,
                FakeFinder = () => fakeFinder.Object
            };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                                       with =>
                                       {
                                           with.AjaxRequest();
                                           with.FormValue("name", "The matrix");
                                       });

            mockSession.Verify(c => c.Store(It.IsAny<Ambiguous>(), It.IsAny<string>()), Times.Once());

        }

        [Test]
        public void Should_not_call_session_Store_without_id_when_storing_Ambiguous_result()
        {
            var mockSession = new Mock<IDocumentSession>();
            var fakeFinder = new Mock<IMoviesFinder>();

            mockSession
                .Setup(c => c.Store(It.IsAny<Ambiguous>(), It.IsAny<string>()))
                .Verifiable();

            var matrixResult = new FinderOption { Imdbid = "tt0133093", Title = "Matrix" };

            var matrixReloadedResult = new FinderOption { Imdbid = "tt0133093", Title = "MatrixReloaded" };

            fakeFinder
                .Setup(c => c.FindByName(It.Is<string>(name => name == "The matrix")))
                .Returns(new[]
                             {
                                 matrixResult,
                                 matrixReloadedResult
                             });

            var bootstrapper = new FakeBootstrapper()
            {
                FakeSession = () => mockSession.Object,
                FakeFinder = () => fakeFinder.Object
            };

            var browser = new Browser(bootstrapper);

            browser.Post("/api/movies",
                                       with =>
                                       {
                                           with.AjaxRequest();
                                           with.FormValue("name", "The matrix");
                                       });

            mockSession.Verify(c => c.Store(It.IsAny<Ambiguous>()), Times.Never());

        }

        private IRavenQueryable<Movie> GetMovies()
        {
            const string json = "[{image:'http://ia.media-imdb.com/images/M/MV5BMjEzNjg1NTg2NV5BMl5BanBnXkFtZTYwNjY3MzQ5._V1._SY317_CR6,0,214,317_.jpg',title:'Matrix',year:1999,plot:'A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.',rating:8.7,genres:[\"Action\",\"Adventure\",\"Sci-Fi\"],imdbid:'tt0133093'},{image:'http://ia.media-imdb.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1._SY317_.jpg',title:'Inception',year:2010,plot:'In a world where technology exists to enter the human mind through dream invasion, a highly skilled thief is given a final chance at redemption which involves executing his toughest job to date: Inception.',rating:8.8,genres:[\"Action\",\"Adventure\",\"Mystery\"], imdbid:'tt1375666'}]";

            var serializer = new JavaScriptSerializer();

            var movies = serializer.Deserialize<IEnumerable<Movie>>(json);

            var fake = new Mock<IRavenQueryable<Movie>>();

            fake.Setup(c => c.GetEnumerator()).Returns(movies.GetEnumerator());
            fake.Setup(c => c.Customize(It.IsAny<Action<IDocumentQueryCustomization>>())).Returns(fake.Object);

            return fake.Object;
        }

        private Movie GetMatrixMovieInfo()
        {
            const string json = "{image:'http://ia.media-imdb.com/images/M/MV5BMjEzNjg1NTg2NV5BMl5BanBnXkFtZTYwNjY3MzQ5._V1._SY317_CR6,0,214,317_.jpg',title:'Matrix',year:1999,plot:'A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.',rating:8.7,genres:[\"Action\",\"Adventure\",\"Sci-Fi\"],imdbid:'tt0133093'}";

            var serializer = new JavaScriptSerializer();

            return serializer.Deserialize<Movie>(json);
        }
    }
}
