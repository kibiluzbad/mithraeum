define ['jquery','underscore','backbone','cs!views/movieView'], ($,_,Backbone,MovieView) ->
  class MoviesView extends Backbone.View
    className: 'movies'
    initialize: (options) ->
      
    render: ->
      $(@el).empty()
      
      for movie in @collection
        $(@el).append new MovieView({model: movie, collection: @collection}).render().el 
      @