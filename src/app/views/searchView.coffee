define [
  'jquery'
  'underscore'
  'backbone'
  'text!templates/searchViewTemplate.html'
  ], ($,_,Backbone,template) ->
  class SearchView extends Backbone.View
    className: 'search'    
    template: _.template(template)
    initialize: (options) ->
      
    render: ->
      html = @template()
      ($ @el).html html      
      @
  