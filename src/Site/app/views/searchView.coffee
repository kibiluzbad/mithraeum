define ['jquery','underscore','backbone','text!templates/searchViewTemplate.html'], ($,_,Backbone,template) ->
  class SearchView extends Backbone.View
    className: 'search'    
    template: _.template(template)
    events:
      "submit #search-form":"doSearch"	
    initialize: (options) ->
      
    render: ->
      html = @template()
      ($ @el).html html      
      @
    doSearch:(event) =>
      event.preventDefault()
      term = $(@el).find("#query").val()
      $.getJSON "http://localhost:18724/api/movies/?jsoncallback=?&term=" + term, (json) =>
        @collection.reset(json)
  