jQuery.noConflict()(function($){
    "use strict";

var $wwidth;
var item;
var gallery;
var sum;

	$('.post_gallery').flickity({
		cellSelector: '.gallery_cell',
		cellAlign: 'left',
		contain: true,
		resize: true,
		setGallerySize: false,	
		adaptiveHeight: true,	
		bgLazyLoad: 1,
	});
	
		$(function() {
		  $.fn.almComplete = function(alm){  
		    	$('.post_gallery').flickity({
					cellSelector: '.gallery_cell',
					cellAlign: 'left',
					contain: true,
					resize: true,
					setGallerySize: false,	
					adaptiveHeight: true,	
					bgLazyLoad: 1,
				});

		var $wwidth = $(window).width();
		var item = $('.item').width();
		var gallery = $('.media_gallery').width();
		var sum = ((item - gallery)-20);
				
		function set_dots(){
			if ( $wwidth < 1200 ) {
				$('ol.flickity-page-dots').css({
					'left':'auto',
					'width': '100%'		
					});			
			} else if ( $wwidth > 1200 ) {
				$('ol.flickity-page-dots').css({
					'left': -sum+"px",
					'width': 'auto'		
					});			
			}		
		}
	
		set_dots();
		    
		  };
		});
		

/* DOCUMENT READY */
$(document).ready(function(){
	

});


/* LOAD */
$(window).load(function() {
	
	function set_dots(){
		if ( $wwidth < 1200 ) {
			$('ol.flickity-page-dots').css({
				'left':'auto',
				'width': '100%'		
				});			
		} else if ( $wwidth > 1200 ) {
			$('ol.flickity-page-dots').css({
				'left': -sum+"px",
				'width': 'auto'		
				});			
		}		
	}

	var $wwidth = $(window).width();
	var item = $('.item').width();
	var gallery = $('.media_gallery').width();
	var sum = ((item - gallery)-20);
	
	set_dots();	

});


/* RESIZE */	
$(window).on('resize', function() {		
	
		function set_dots(){
		if ( $wwidth < 1200 ) {
			$('ol.flickity-page-dots').css({
				'left':'auto',
				'width': '100%'		
				});			
		} else if ( $wwidth > 1200 ) {
			$('ol.flickity-page-dots').css({
				'left': -sum+"px",
				'width': 'auto'		
				});			
		}		
	}	

	var $wwidth = $(window).width();
	var item = $('.item').width();
	var gallery = $('.media_gallery').width();
	var sum = ((item - gallery)-20);
	
	set_dots();

}).trigger('resize');



});