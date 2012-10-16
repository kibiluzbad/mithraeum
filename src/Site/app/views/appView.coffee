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
    render: ->
      $(@el).empty()
      $(@el).html(@template())
      
      $content = $(@el).find('#content')
      $content.empty()
      
      $header = $(@el).find("#header")

      sw = new SearchView collection: @collection

      $header.append(sw.render().el)
      $header.append(new MenuView().render().el)
      
      $content.append subview.render().el for subview in @subviews
      @