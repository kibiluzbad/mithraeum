define ['jquery','underscore','backbone','cs!models/movie'], ($,_,Backbone,Movie) ->
  class MovieView extends Backbone.View
    className: 'movie'
    initialize: (options) ->
      
    render: ->
      html = @template.tmpl @model.toJSON()
      ($ @el).html html
      @
  
  