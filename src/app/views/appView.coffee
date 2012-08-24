define ['jquery','underscore','backbone','cs!views/moviesView','text!templates/appViewTemplate'], ($,_,Backbone,MoviesView,template) ->
  class AppView extends Backbone.View
    el: '#wrap'
    template: $(template)
    initialize: (options) ->
      @subviews = [
        new MoviesView collection: @collection        
        ]
      @collection.bind 'reset', @render, @
    render: ->
      $(@el).empty()
      $(@el).append subview.render().el for subview in @subviews
      @