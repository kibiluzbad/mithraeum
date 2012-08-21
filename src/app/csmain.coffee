require ['jquery','underscore','backbone','cs!routers/mithraeumRouter'], ($,_,Backbone,MithraeumRouter) ->
  m = new MithraeumRouter()
  Backbone.history.start({pushState:true})