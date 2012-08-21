define ['jquery','underscore','backbone','cs!views/moviesView'], ($,_,Backbone,MoviesView) ->
  class AppView extends Backbone.View
    el: '#wrap'
    initialize: (options) ->
      @subviews = [
        new MoviesView collection: @collection        
        ]
      @collection.bind 'reset', @render, @
    render: ->
      $(@el).empty()
      $(@el).append subview.render().el for subview in @subviews
      @