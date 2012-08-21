define ['jquery','underscore','backbone','cs!models/movie'], ($,_,Backbone,Movie) ->
  @app = window.app ? {}

  class Movies extends Backbone.Collection
    model: Movie
    url: 'api/movies'
    initialize: ->
      
  