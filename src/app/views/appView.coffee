define ['jquery','underscore','backbone','cs!views/moviesView','text!templates/appViewTemplate.html'], ($,_,Backbone,MoviesView,template) ->
  class AppView extends Backbone.View
    el: '#wrap'
    template: $(template)
    initialize: (options) ->
      @subviews = [
        new MoviesView collection: @collection        
        ]
      @collection.bind 'reset', @render, @
      @collection.bind 'add', @render, @
    render: ->
      $(@el).empty()
      $(@el).append subview.render().el for subview in @subviews
      @