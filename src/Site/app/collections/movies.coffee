define ['jquery','underscore','backbone','cs!models/movie'], ($,_,Backbone,Movie) ->
  @app = window.app ? {}

  class Movies extends Backbone.Collection
    model: Movie
    url: 'http://localhost:18724/api/movies'
    sync: (method, model, options) ->
      options.timeout = 10000
      options.dataType = "jsonp"
      Backbone.sync(method, model, options)
    initialize: ->
      
  