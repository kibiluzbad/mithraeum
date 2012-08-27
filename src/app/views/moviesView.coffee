define ['jquery','underscore','backbone','cs!views/movieView','text!templates/moviesViewTemplate.html'], ($,_,Backbone,MovieView,template) ->
  class MoviesView extends Backbone.View
    className: 'movies'
    template: $(template)
    tagName: 'ul'
    initialize: (options) ->
      @collection.bind 'change', @render, @
      @collection.bind 'reset', @render, @
    render: ->
      $(@el).empty()
      
      for movie in @collection.models
        $(@el).append new MovieView({model: movie, collection: @collection}).render().el 
      @