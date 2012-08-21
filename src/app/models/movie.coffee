define ['jquery','underscore','backbone'], ($,_,Backbone) ->
  class Movie extends Backbone.Model
    idAttribute: 'imdbid'
    initialize: ->
      