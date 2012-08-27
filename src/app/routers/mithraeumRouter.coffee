define ['jquery','underscore','backbone','cs!collections/movies','cs!views/appView','cs!models/movie'], ($,_,Backbone,Movies,AppView,Movie ) ->
  class MithraeumRouter extends Backbone.Router
    routes:
      '':'index'    
    initialize: ->
      @movies = new Movies
      
    index: ->      
      @view = new AppView collection: @movies      
      movie1 = new Movie({image:'http://ia.media-imdb.com/images/M/MV5BMjEzNjg1NTg2NV5BMl5BanBnXkFtZTYwNjY3MzQ5._V1._SY317_CR6,0,214,317_.jpg',title:'Matrix',year:1999,plot:'A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.',rate:8.7,tags:["Action","Adventure","Sci-Fi"]})
      movie2 = new Movie({image:'http://ia.media-imdb.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1._SY317_.jpg',title:'Inception',year:2010,plot:'In a world where technology exists to enter the human mind through dream invasion, a highly skilled thief is given a final chance at redemption which involves executing his toughest job to date: Inception.',rate:8.8,tags:["Action","Adventure","Mystery"]})
      @movies.add(movie1)
      @movies.add(movie2)