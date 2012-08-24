define ['jquery','underscore','backbone','cs!views/movieView','text!templates/moviesViewTemplate'], ($,_,Backbone,MovieView,template) ->
  class MoviesView extends Backbone.View
    className: 'movies'
    template: $(template)
    initialize: (options) ->
      
    render: ->
      $(@el).empty()
      
      for movie in @collection
        $(@el).append new MovieView({model: movie, collection: @collection}).render().el 
      @