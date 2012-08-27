define ['jquery','underscore','backbone','cs!collections/movies','cs!views/appView','cs!models/movie'], ($,_,Backbone,Movies,AppView,Movie ) ->
  class MithraeumRouter extends Backbone.Router
    routes:
      '':'index'    
    initialize: ->
      @movies = new Movies
      
    index: ->      
      @view = new AppView collection: @movies      
      movie = new Movie({title:'Matrix',year:1999})
      @movies.add(movie)