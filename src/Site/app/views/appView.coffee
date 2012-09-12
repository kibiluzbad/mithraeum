define [
  'jquery'
  'underscore'
  'backbone'
  'cs!views/moviesView'
  'text!templates/appViewTemplate.html'
  'cs!views/searchView'
  'cs!views/menuView'
  ], ($,_,Backbone,MoviesView,template,SearchView,MenuView) ->
  class AppView extends Backbone.View
    el: '#wrap'
    template: _.template(template)
    initialize: (options) ->
      @subviews = [
        new MoviesView collection: @collection        
        ]
      @collection.bind 'reset', @render, @
      @collection.bind 'add', @render, @
    render: ->
      $(@el).empty()
      $(@el).html(@template())
      
      $content = $(@el).find('#content')
      $content.empty()
      
      $header = $(@el).find("#header")
      
      $header.append(new SearchView().render().el)
      $header.append(new MenuView().render().el)
      
      $content.append subview.render().el for subview in @subviews
      @