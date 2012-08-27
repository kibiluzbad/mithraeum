define ['jquery','underscore','backbone','cs!models/movie','text!templates/movieViewTemplate.html'], ($,_,Backbone,Movie,template) ->
  class MovieView extends Backbone.View
    className: 'movie'
    template: _.template(template)
    tagName: 'li'
    initialize: (options) ->
      
    render: ->
      data = @model.toJSON()
      html = @template(data)
      ($ @el).html html
      @
  
  