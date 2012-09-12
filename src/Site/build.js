({
    appDir: '.',
    baseUrl: 'app',

    //Uncomment to turn off uglify minification.
    optimize: 'none',
    dir: '../build',

    //Stub out the cs module after a build since
    //it will not be needed.
    stubModules: ['cs'],

    paths: {
        'cs' :'../../src/tools/cs',
        'coffee-script': '../../src/tools/coffee-script',
        'jquery': '../lib/jquery',
		'backbone': '../lib/backbone',
		'underscore': '../lib/underscore'		
    },
    
    shim: {
		'backbone': {
			// These script dependencies should be loaded 
			// before loading backbone.js
			deps: ['underscore', 'jquery'],
			// Once loaded, use the global 'Backbone' 
			// as the module value.
			attach: function(_,$) {
		        // If you plan on loading any plugins for Backbone, do not use this
		        // approach of removing it from the global scope.  They will be
		        // unable to find Backbone.
		        return Backbone;
		      }
		},
		'underscore': {
			// Use the global '_' as the module value.
			 attach: "_"
		}
	},

    modules: [
        {
            name: 'main',
            //The optimization will load CoffeeScript to convert
            //the CoffeeScript files to plain JS. Use the exclude
            //directive so that the coffee-script module is not included
            //in the built file.
            exclude: ['coffee-script','cs']
        }
    ]
})