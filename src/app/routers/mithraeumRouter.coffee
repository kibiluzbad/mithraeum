define ['jquery','underscore','backbone','cs!collections/movies','cs!views/appView'], ($,_,Backbone,Movies,AppView ) ->
  class MithraeumRouter extends Backbone.Router
    routes:
      '':'index'    
    initialize: ->
      @movies = new Movies
      @movies.fetch
    index: ->      
      @view = new AppView collection: @movies