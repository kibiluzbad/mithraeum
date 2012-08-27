define [
  'jquery'
  'underscore'
  'backbone'
  'text!templates/menuViewTemplate.html'
  ], ($,_,Backbone,template) ->
  class MenuView extends Backbone.View
    className: 'menu'    
    template: _.template(template)
    initialize: (options) ->
      
    render: ->
      html = @template()
      ($ @el).html html      
      @
  