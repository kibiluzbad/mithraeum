define ['jquery','underscore','backbone','cs!models/movie','text!templates/movieViewTemplate'], ($,_,Backbone,Movie,template) ->
  class MovieView extends Backbone.View
    className: 'movie'
    template: $(template)
    initialize: (options) ->
      
    render: ->
      html = @template.tmpl @model.toJSON()
      ($ @el).html html
      @
  
  