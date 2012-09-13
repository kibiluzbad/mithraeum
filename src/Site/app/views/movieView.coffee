define ['jquery','underscore','backbone','raty','cs!models/movie','text!templates/movieViewTemplate.html'], ($,_,Backbone,Raty,Movie,template) ->
  class MovieView extends Backbone.View
    className: 'movie'
    template: _.template(template)
    tagName: 'li'
    events:
      "click .show-details":"showDetails"    
    initialize: (options) ->
    
    showDetails: =>
    	Backbone.history.navigate('movie/'+@model.id,true)
    
    render: ->
      data = @model.toJSON()
      html = @template(data)
      ($ @el).html html
      ($ @el).find('.rating').raty(
        readOnly: true
        score: @model.get("Rating")
        starOn: 'star-on-big.png',
        starOff: 'star-off-big.png'
        starHalf  : 'star-half-big.png',
        path: '/img'
        width: '300px'
        number: 10
        half: true
      )
      @
  
  