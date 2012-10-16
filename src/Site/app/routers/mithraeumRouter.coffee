define ['jquery','underscore','backbone','cs!collections/movies','cs!views/appView','cs!models/movie'], ($,_,Backbone,Movies,AppView,Movie ) ->
  class MithraeumRouter extends Backbone.Router
    routes:
      '':'index'
      'movie/:id':'movieDetails'
    initialize: ->
      @movies = new Movies
      
    index: ->      
      @view = new AppView collection: @movies      
      @movies.fetch()
      @view.render()
    movieDetails: (id) ->
      alert "Detalhes"