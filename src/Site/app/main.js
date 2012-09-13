require.config({
	'paths': {
		'jquery': '../lib/jquery',
		'underscore': '../lib/underscore',
		'backbone': '../lib/backbone',
		'raty': '../lib/jquery.raty',
		'lazyLoad': '../lib/jquery.lazyload',		
		'cs': '../tools/cs',
    	'coffee-script': '../tools/coffee-script'   ,
    	'text': '../lib/text',
    	'templates': 'templates'
	}, 
	shim: {
		'backbone': {
			// These script dependencies should be loaded 
			// before loading backbone.js
			deps: ['underscore', 'jquery'],
			// Once loaded, use the global 'Backbone' 
			// as the module value.
			exports: 'Backbone'		      
		},
		'underscore': {
			// Use the global '_' as the module value.
			 exports: '_'
		}
	}
});
 
require(['underscore','jquery','backbone','cs!csmain']);