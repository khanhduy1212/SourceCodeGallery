var myScroll;
var viewport_width;
var viewport_height;
var windowWidth;
var windowHeight;
var understatedHeight;


var $pointer = $('#menu_scroller .pointer');
var loadEnabled = true;

var side_menu = false;
var forced_fullscreen = false;
var windowHeight;
var windowWidth;
var mobilePlatform = isMobile();
var mobileCheck = mobilePlatform || viewport().width < 960;
var initSettings = mobileCheck;
var currentState = mobileCheck;
var isHandheld =  /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
var scroll_threshold = 200;
var pageY = 0;
var pageX = 0;
var current;
var controller;
var first_run = true;
var panels_initialized = false;
var panelDetail_ready = false;
var popstateDisabled = false;
var focusing = false;
var timeoutFocus;
var wait_for_clearing;
var arrivalDate = new Date();
var departureDate = addDays(arrivalDate, 2);
var hideDatepickerArrival;
var hideDatepickerDeparture;
var popTimeout;
var popEnabled = true;
var navigateTimeout;
var scrollType;
var deepResize;
var has_call = false;
var has_gallery = false;
var has_form = false;
var v_weather = false;
var o_weather;
var scrollerHeight;
var thanksOpened = false;
var langOpened = false;
var o_call = 9999;
var o_gallery = 9999;
var do_once;

var gallery_focus = true;
$(document).ready(function(){
	if(first_run){
		
	current = $('#content').attr('data-current');
	
	windowWidth = $(window).width();
	windowHeight = $(window).height();
	
	history.scrollRestoration = 'manual';
	
	$$transform = [Modernizr.prefixed('transform')];
	
	/************* POPSTATES **********/
	
	window.onpopstate = function(event) {
		clearTimeout(popTimeout);
		target = event.state.current;
		targetController = event.state.controller;
		targetUrl = event.state.url;
		managePopState(event); 
		
		
	}
	
	function managePopState (event) {

		if(popEnabled){
		popEnabled = false;
		clearTimeout(navigateTimeout);

		now = current;
		
		
		if(targetController == "panel"){
			if(now == target){
				popEnabled = true;
				return false;
				
			}
			
			if(detail_loaded && now.indexOf(target) != -1) {
				closePageDetail();
				 $('head title').html('CASA ANGELINA OFFICIAL WEBSITE | '+$('.panel_section_title').text().toUpperCase())
				
			} else if(controller == "panel"){
				closePanels();
				navigateTimeout = setTimeout(function(){
					base_load(targetUrl,false);
					current_scroll = 0;
				},800)
			} else if(controller != "panel") {
				$('#main_veil').addClass('grey');
				closePage();
				navigateTimeout = setTimeout(function(){
					base_load(targetUrl,false);
				},800)
			}
		}
		
		if (targetController == "panel_detail") {
		    
			if(!detail_loaded && targetUrl.indexOf(now) != -1) {
				$('#section_scroller_container .section_panel:eq('+event.state.panelIndex+')').trigger('click');
			} else if(targetUrl.indexOf(now) == -1) {
				if(controller == "panel"){
					closePanels();
				    navigateTimeout = setTimeout(function() {
				            base_load(targetUrl, false);
				            current_scroll = 0;
				        },
				        800);
				} else {
					$('#main_veil').addClass('grey');

					closePage();
				    navigateTimeout = setTimeout(function() {
				            base_load(targetUrl, false);
				        },
				        800);
				}
					
			}
		} 
		
		
		if(targetController == "page"){
			if(controller == "panel"){
				closePanels();
			    navigateTimeout = setTimeout(function() {
			            base_load(targetUrl, false);
			            current_scroll = 0;
			        },
			        800);
			} else {
				closePage();
				$('#main_veil').addClass('grey');

			    navigateTimeout = setTimeout(function() {
			            base_load(targetUrl, false);
			        },
			        800);
			}
		}
		
		
		
		current = target;
		controller = targetController;
		} else {
			clearTimeout(popTimeout);
		    popTimeout = setTimeout(function() {
		            managePopState(event);
		        },
		        500);
		}
		
	}
	
	
	/**********************************/
	
	if(isHandheld) {
	    $(window).on('touchstart, touchmove',
	        function() {
	            $('body,html').stop(true);

	        });


	}

	if(!mobileCheck){
		
		
		document.head.innerHTML = document.head.innerHTML + '<meta name="viewport" content="width=1280 user-scalable=no ">';
		
		windowWidth = $(window).width();
		windowHeight = $(window).height();
		
		/************ DESKTOP READY **************/
		
		$('a.menu_paragraph, .menu_panel.final a.link').bind('click',load_section);
		$('#menu_controller_container').bind('click',show_menu);
		$('.menu_title_open').bind('click',show_menu_content);
		$('.menu_hide_content').bind('click',hide_menu_content);
		$('#main_menu .menu_close').bind('click',close_menu);
		//$('#book_side, #book_menu_button').bind('click',show_book_interface);
		$('#book_close').bind('click',hide_book_interface);
		$('#sitemap_button').bind('click',show_sitemap);
		$('#sitemap_close').bind('click',hide_sitemap_interface);
		$('#credits_button').bind('click',show_credits);
		$('#credits_close').bind('click',hide_credits_interface);
		$('#thanks_button').bind('click',manage_thanks);
		$('#lang_button').bind('click',manage_lang);
		$('#lang_button a').bind('click',set_lang);
		
		$('.menu_panel .menu_title_open').bind('mouseenter',function(){
			$(this).parent().parent().css('background','#FFFFFF');
			$(this).parent().parent().find('.menu_number').css('color','#D4D4D8');
			
		}).bind('mouseleave',function(){
			$(this).parent().parent().css('background','#EAEAEC');
			$(this).parent().parent().find('.menu_number').css('color','#FFFFFF');
		});	
		
		
		$('#menu_logo,#home_menu_button').bind('click',function(e){
			e.preventDefault();
			closePage();
		    var linkHome = $('.linkHome').val();
		    navigateTimeout = setTimeout(function() {
		        base_load(linkHome+'/home', true);
		    }, 800);
		});
		
		 $("#book_submit").click(function() {
		        if ($("#arrival").val() != "" && $("#departure").val() != "" && $("#guests").val() != "" && isNaN($("#guests").val()) == false) {
		            
		        	$momentArrival = moment((arrivalDate.getMonth() + 1) + "/" + arrivalDate.getDate() +"/" + arrivalDate.getFullYear(), 'MM-DD-YYYY');
		        	$momentDeparture = moment((departureDate.getMonth() + 1) + "/" + departureDate.getDate() +"/" + departureDate.getFullYear(), 'MM-DD-YYYY');
		        	$momentNights = $momentDeparture.diff($momentArrival, 'days');
		        	
		        	
		            window.open("https://be.synxis.com/?adult="+$("#guests").val()+"&arrive="+arrivalDate.getFullYear()+"-"+(arrivalDate.getMonth() + 1)+"-"+arrivalDate.getDate()+"&depart="+departureDate.getFullYear()+"-"+(departureDate.getMonth() + 1)+"-"+departureDate.getDate()+"&chain=22402&currency=EUR&hotel=79920&src=24C&locale="+get_locale())

		            
		        } else {
		            alert("Please fill all fields with correct data!");
		        }
		    });
		
		
		initializeMenuScroller();
		initialize_sitemap_scroller();
		
		$('#panelPrev,#panelNext').bind('click',item_load);
		$('#backToPanels').bind('click',function(e){
			e.preventDefault();
            closePageDetail();      
            var url = $('#content').attr('data-url-back');
            if (url != null && url != "") {
             
                $.get(url, function (data, success) {
                    setTimeout(function () {
                        $newData = data;
                        $newTitle = $($newData).filter('title').text();
                        $('head title').html($newTitle);
                        detail_loaded = true;
                        current = $($newData).find('#content').attr('data-current');
                        controller = $($newData).find('#content').attr('data-controller');
                        $('body').attr('id', current);
                        $mainContent = $($newData).find('#content');
                        $('#content').remove();
                        $('#main_scroller').prepend($mainContent).addClass('no_transition');
                        var stateObj = { controller: "panel", current: current, url: url };
                        history.pushState(stateObj, "", url);
                        if (!mobileCheck) {
                            window[current + "_ready_desktop"]();
                        }

                         }, 300);

                    });
             
               // location.href = url;
                /**/
            } else {
                var _csUrl = current.split('_detail')[0] == "specials" ? "products" : current.split('_detail')[0];
                var stateObj = { controller: "panel", current: current.split('_detail')[0], panelIndex: "none", url: "/" + _csUrl };
                history.replaceState(stateObj, "", "/" + _csUrl);
                $('head title').html('Lamos Artistic Advertisement | ' + $('.panel_section_title').text().toUpperCase());


            }
          
		});
	
		
		////run_clocks();
		
		
		//arrivalDatepicker = $('.arrival').glDatePicker(
		//		{
		//			cssName: 'flatwhite',
		//			zIndex: 1000,
		//		    borderSize: 0,
		//		    monthNames : getMonthNames(),
		//		    dowNames : getDowNames(),
		//		    dowOffset: getDowOffset(),
		//		    selectableDateRange: [{ from: new Date(), to:new Date(3000,1,1)}],
		//		    selectedDate: arrivalDate,
		//		    onShow: function(calendar){
		//		    	arrivalDatepicker.render();
		//		    	clearTimeout(hideDatepickerArrival);
		//		    	calendar.find('div').addClass('top_double').addClass('has_transition_600');
		//		    	calendar.show();
		//		    	calendar.find('div').each(function(c){
		//		    		setTimeout(function(){
		//		    			calendar.find('div:eq('+c+')').removeClass('top_double')
		//		    		},5*c);
		//		    	})
				    	
		//		    },
		//		    onHide:function(calendar){
		//		    	hideDatepickerArrival = setTimeout(function(){
		//		    	calendar.addClass('has_transition_300').addClass('no_opacity')
		//		    	setTimeout(function(){calendar.hide();},300)
		//		    	},1);
		//		    },
		//		    onClick: function(el, cell, date, data) {
		//		    	var dd = date.getDate();
		//		    	var mm = date.getMonth()+1;
		//		    	var yyyy = date.getFullYear();
				    	
		//		    	if(dd<10) {
		//		    	    dd='0'+dd
		//		    	}
				    	
		//		    	if(mm<10) {
		//		    	    mm='0'+mm
		//		    	} 
				    	
		//		    	if(lang == "en"){
		//			    	arrivalDate = date;
		//			    	selected = mm+'/'+dd+'/'+yyyy;
		//			    	el.children('input').val(selected);
		//		    	} else {
		//		    		arrivalDate = date;
		//			    	selected = dd+'/'+mm+'/'+yyyy;
		//			    	el.children('input').val(selected);
		//		    	}
		//		    },
		//		}
		//	).glDatePicker(true);
		//	departureDatepicker = $('.departure').glDatePicker(
		//		{
		//			cssName: 'flatwhite',
		//			zIndex: 1000,
		//		    borderSize: 0,
		//		    monthNames : getMonthNames(),
		//		    dowNames : getDowNames(),
		//		    dowOffset: getDowOffset(),

		//		    selectableDateRange: [{ from: new Date(), to:new Date(3000,1,1)}],
		//		    selectedDate:departureDate,

		//		    onShow: function(calendar){
		//		    	clearTimeout(hideDatepickerDeparture);
		//		    	departureDatepicker.render();
		//		    	calendar.find('div').addClass('top_double').addClass('has_transition_600')
		//		    	calendar.show();
		//		    	calendar.find('div').each(function(c){
		//		    		setTimeout(function(){
		//		    			calendar.find('div:eq('+c+')').removeClass('top_double')
		//		    		},5*c);
		//		    	})
				    	
		//		    },
		//		    onHide:function(calendar){
		//		    	hideDatepickerDeparture = setTimeout(function(){
		//			    	calendar.addClass('has_transition_300').addClass('no_opacity')
		//			    	setTimeout(function(){calendar.hide();},300)
		//			    },1);
				    	
		//		    },
		//		    onClick: function(el, cell, date, data) {
		//		    	var dd = date.getDate();
		//		    	var mm = date.getMonth()+1;
		//		    	var yyyy = date.getFullYear();
				    	
		//		    	if(dd<10) {
		//		    	    dd='0'+dd
		//		    	}
				    	
		//		    	if(mm<10) {
		//		    	    mm='0'+mm
		//		    	} 
				    	
		//		    	if(lang == "en"){
		//		    		departureDate = date;
		//			    	selected = mm+'/'+dd+'/'+yyyy;
		//			    	el.children('input').val(selected);
		//		    	} else {
		//			    	departureDate = date;
		//			    	selected = dd+'/'+mm+'/'+yyyy;
		//			    	el.children('input').val(selected);
		//		    	}
		//		    },
		//		}
		//	).glDatePicker(true);
		
		
		/******* SCROLL DESKTOP *********/
			
			initialize_vertical_scroll();
		
		window[current+"_ready_desktop"]();	

		} else {
		
		/************* MOBILE READY **************/
		
			init_mobile_setup();
			
		/******* SCROLL MOBILE *********/
		$(window).scroll(function(){
			pageY = window.pageYOffset;
			window[current+"_scroll_mobile"]();	
			
		});
		
    }
	$('.btn-scroll-to-detail').on('click', function (e) {
	    scroll_to_element();
      
    });

 $('.btn-contact').on('click', function (e) {
        e.stopPropagation();
        $('.menu_close').click();
        scroll_to_partners();
      
    });
    $('#btnBackToTop').on('click', function (e) {
        e.stopPropagation();
      
        scroll_to_top();
      
    });
	}
});

$(window).on("load", function (e) {
	current = $('#content').attr('data-current');
	window[current+"_load"]();
	
	
	if(!mobileCheck){
		 launch_audio();
		 //$('#player_side').bind('click',launch_audio);
	} 
	
	if($('.call_to').length != 0) {
	    o_call = $('.call_to').position().top;
	}
	
	
	//setWeatherData();
	
	

});



/************ CORE ********************/

var $newData;
function load_section(e){
	e.preventDefault();

	if(loadEnabled){
		loadEnabled = false;
		panelDetail_ready = false;
		href = $(this).attr('href');
		pageX = 0;
		current_scroll = 0;
		hide_side_menu();
	
   var $newData = "loading";
   

   $.get(href, function(data,success){
		 $newData = data;
		 $newTitle = $($newData).filter('title').text();
		 $('head title').html($newTitle);
		

   });

	if($('#main_menu').hasClass('active')){
		$('.menu_panel').each(function(i){
			setTimeout(function(){
			$('.menu_panel:eq('+ (($('.menu_panel').length - 1) -i)  +') .menu_panel_container').addClass('no_opacity');
			},50*i);
		});
		
		
	}
	
	setTimeout(function(){
		$('#menu_back').addClass('grey');
	},500);
	
	function data_ready(){
		if($newData != "loading"){

			 $mainContent = $($newData).find('#content');
			 $('#content').remove();
			 $('#main_scroller').prepend($mainContent).addClass('no_transition');
			 if(scrollType == "iScroll"){
				 myScroll.scrollTo(0,0);
			 } else {
				 $(window).scrollTop(0);
			 }
			 setTimeout(function(){$('#main_scroller').removeClass('no_transition');},1);
			 current =  $($newData).find('#content').attr('data-current');
			 controller =  $($newData).find('#content').attr('data-controller');
			 
			 var stateObj = { controller: controller, current:current,panelIndex:"none",url:href};
			 history.pushState(stateObj, "", href);
			
			 if(!mobileCheck){
				 $('#menu_back').addClass('no_transition').addClass('no_opacity');
				 $('body').attr('id',current);
				 window[current+"_ready_desktop"]();	
				 $('#main_menu').removeClass('active');	
			 }	
		} else {
			setTimeout(data_ready,10);
		}
	}
	
	setTimeout(data_ready,800);
			 
	}
}

var detail_loaded = false;

function load_panel_detail(e,$detail){
	e.preventDefault();
	
	href=$detail.attr('href');
	   $.get(href, function(data,success){
			 $newData = data;
			 $newTitle = $($newData).filter('title').text();
			 $('head title').html($newTitle);
			
			 
			 detail_loaded = true;
			 current =  $($newData).find('#content').attr('data-current');
			 controller =  $($newData).find('#content').attr('data-controller');

			 if(history.state.panelIndex != $('.section_panel.active').index()){
				 var stateObj = { controller: "panel_detail", current: current, panelIndex:$('.section_panel.active').index(), url:href };
				 history.pushState(stateObj, "", href);
			 }
			 
			 $('body').attr('id',current );
             $('#content').attr('data-url-back', $($newData).find('#content').attr('data-url-back'))
			 $headContent = $($newData).find('#head_content');
			 $mainContent = $($newData).find('#section_content > *');
             //$section_scroller = $($newData).find('#section_scroller');
			 $section_scroller_container=$($newData).find('#section_scroller_container');
			 $('#section_animation').append($headContent);
             $('#section_content').append($mainContent);
			 $('#section_scroller_container').html($section_scroller_container.html());
             /*$('#section_scroller .section_scroller_container home_f').html($section_scroller.html());
             if ($('#section_scroller .section_scroller_container .home_l').length === 0 && $section_scroller.find('.home_l').length > 0) {
                 $('#section_scroller .section_scroller_container').append($section_scroller.find('.home_l'));
             }
             if ($('#section_scroller .section_scroller_container .home_f').length === 0 && $section_scroller.find('.home_f').length > 0) {
                 $('#section_scroller .section_scroller_container').prepend($section_scroller.find('.home_f'));
             }*/
            
			 setTimeout(window[current+"_ready_desktop"],200);
			
	   });
	   		
}

function item_load(e){
	e.preventDefault();
	closePage();
	$this = $(this);
	
	hide_side_menu();
	
	setTimeout(function(){
		panelDetail_ready = false;
		href = $this.attr('href');
		
	
		hide_side_menu();
		
	   var $newData = "loading";

	   $.get(href, function(data,success){
			 $newData = data;
			 $newTitle = $($newData).filter('title').text();
			 $('head title').html($newTitle);
			 

	   });

	   $('#main_veil').addClass('no_transition').removeClass('hidden');
	   
	   $('#main_veil').addClass('grey');
		
	   

		function data_ready(){

			if($newData != "loading"){

				 $mainContent = $($newData).find('#content');
				 $('#content').remove();
				
				 $('#main_scroller').prepend($mainContent).addClass('no_transition');
				 if(scrollType == "iScroll"){
					 myScroll.scrollTo(0,0);
				 } else {
					 $(window).scrollTop(0);
				 }
				 setTimeout(function(){$('#main_scroller').removeClass('no_transition');},1);
				 current =  $($newData).find('#content').attr('data-current');
				 
				
				 var stateObj = { controller: "panel_detail", current:current,panelIndex:$this.index(),url:href};
				 history.pushState(stateObj, "", href);
				 
				 if(!mobileCheck){
					 $('#menu_back').addClass('no_transition').addClass('no_opacity');
					 $('body').attr('id',current);
					 $('#main_veil').removeClass('no_transition');
					 window[current+"_ready_desktop"]();	
				 }	
			} else {
				setTimeout(data_ready,10);
			}
		}
		
		setTimeout(data_ready,800);
		
	},800);
	
}

function base_load(url,pushState){
	if(loadEnabled){
		loadEnabled = false;
	href = url;
	panelDetail_ready = false;
	hide_side_menu();
	
   var $newData = "loading";
   
   $.get(href, function(data,success){
		 $newData = data;
		 $newTitle = $($newData).filter('title').text();
		 $('head title').html($newTitle);
		
   });

   $('#main_veil').addClass('no_transition').removeClass('hidden');
	function data_ready(){
		if($newData != "loading"){
			 $mainContent = $($newData).find('#content');
			 $('#content').remove();
			 $('#main_scroller').prepend($mainContent).addClass('no_transition');
			 if(scrollType == "iScroll"){
			     myScroll.scrollTo(0, 0);
			     console.log(1);
			 } else {
			     $(window).scrollTop(0);
			     console.log(2);
			 }
			 setTimeout(function(){$('#main_scroller').removeClass('no_transition');},1);
			 current =  $($newData).find('#content').attr('data-current');
			 controller =  $($newData).find('#content').attr('data-controller');

			 if(pushState){
			 var stateObj = { controller: controller, current:current,panelIndex:"none",url:href};
			 history.pushState(stateObj, "", href);
			 } else {
				 
			 }
			 if(!mobileCheck){
				 $('#menu_back').addClass('no_transition').addClass('no_opacity');
				 $('body').attr('id',current);
				 $('#main_veil').removeClass('no_transition');
				 window[current+"_ready_desktop"]();	
			 }	
		} else {
			setTimeout(data_ready,10);
		}
	}
	
	setTimeout(data_ready,800);
	}	 	
}

function closePage() {
	
	$('#main_veil').addClass('has_transition_1000_inout').addClass('hidden').show();
	if(current != "index"){
		$('#main_veil').addClass('grey');
	}
	setTimeout(function(){
		$('#main_veil').removeClass('hidden');
	},50);
	
}

function enableDrag(){
	if(Modernizr.touchevents){
		dp = false;
		return dp;
	} else {
		dp = true; 
		return dp
	}
}

function initialize_vertical_scroll() {
	
	
	
	
	if(!isHandheld &&  navigator.userAgent.toLowerCase().indexOf('firefox') == -1){
	scrollType = "iScroll";
		
	myScroll = new IScroll('#main', {
	    mouseWheel: true,
	    scrollbars: true,
	    probeType: 2,
	    disablePointer:true, 
	    scrollY:true,
	    scrollX:false,
	    useTransition:true,
	    interactiveScrollbars:true
	});
	

	myScroll.on('scroll', function(){
		pageY = -(myScroll.y);
		common_scroll();
	   
	});
	
	} else {
		scrollType = "native";
		
		$('#main').css({
			'overflow':'visible',
			'will-change': 'transform'
		});
		
		$('#main_scroller').css({
			'will-change': 'transform',
			'overflow-x':'hidden'
		});

	    $(window).scroll(function() {
	        pageY = window.pageYOffset;
	        common_scroll();
	       

	    });
	}
}

function common_scroll() {
   
    console.log(scrollerHeight);
	if(!side_menu && pageY > windowHeight/2 && pageY <= scrollerHeight - windowHeight - 500) {
		side_menu = true;
		show_side_menu();
		
	}
	
	if(side_menu && pageY < windowHeight/2) {
		side_menu = false;
		hide_side_menu();
	}
	
	if(side_menu && pageY > scrollerHeight - windowHeight - 500) {
		side_menu = false;
		hide_side_menu();
	}
	if (current == "specials_detail") {
	    hidePageController();
	}
	
	
	if(has_call) {
		if(!v_call && pageY > o_call - windowHeight + scroll_threshold) {
			v_call = true;
			$('.call_to .desc_text').removeClass('top_hidden');
			setTimeout(function(){
				$('.call_to .main_block_title .desc_title').removeClass('top_hidden');

			},100)
			setTimeout(function(){
				$('.call_to').next().find('.cta').removeClass('top_translated');
				$('.diagonal_line').removeClass('hidden');
			},200)
		}

	}
	
	if(has_form){
		if(!v_form && pageY > o_form - windowHeight + scroll_threshold + 200) {
			v_form = true;
				
			$('.has_form .block_title .desc_title').removeClass('top_hidden');
			
			setTimeout(function(){
				$('.has_form .block_title .desc_text').removeClass('top_hidden');
			},100);
			
			setTimeout(function(){
				$('.has_form .form_description p').removeClass('top_hidden');
			},200);
			
			setTimeout(function(){
				$('.has_form .diagonal_line').removeClass('hidden');
			},300);
			
			
			$('.form_input').each(function(b){
				setTimeout(function(){
					$('.has_form .form_input:eq('+b+') input').removeClass('top_double');
					$('.has_form  .form_input:eq('+b+') label').removeClass('top_double');
				},500+(100*b));
				
			});
			
			setTimeout(function(){
				$('.choice_label').removeClass('top_double');
			},700);
			
			$('.page_form .checkbox').each(function(b){
				setTimeout(function(){
					$('.page_form .checkbox:eq('+b+')').removeClass('top_double');
				},800+(100*b));
				
			});
			
			
			setTimeout(function(){
				$('textarea').removeClass('top_double');
			},1200);
			
			setTimeout(function(){
				$('.radio_input').removeClass('top_double');
			},1300);
			
			setTimeout(function(){
				$('.form_reset').removeClass('top_double');
			},1400);
			
			setTimeout(function(){
				$('.form_submit').removeClass('top_double');
			},1550);
			setTimeout(function(){
				$('.form_submit .circle_button .back').removeClass('hidden_by_scaling_full');
			},1650);
			setTimeout(function(){
				$('.form_submit .circle_button .arrow').removeClass('hidden');
			},1850);
			
			
			

		}
			
	}
	
	if(!v_weather && pageY > o_weather - windowHeight + scroll_threshold + 200) {
		v_weather = true;
		weatherIn();
		
		
	}
		if(has_gallery){
            if (!forced_fullscreen && pageY > (o_gallery - windowHeight / 2 + $('#menu_center_side').height() / 2) && pageY < (o_gallery + windowHeight / 2 - $('#menu_center_side').height() / 2)){
                
				forced_fullscreen = true;
				hide_menu_center();
				
			}
			
            if (forced_fullscreen && (pageY < (o_gallery - windowHeight / 2 + $('#menu_center_side').height() / 2) || pageY > (o_gallery + windowHeight / 2 - $('#menu_center_side').height() / 2))){
				forced_fullscreen = false;
				show_menu_center();
				
			}
			
			
			
			if(gallery_focus && pageY > o_gallery - windowHeight/2 && pageY < o_gallery + windowHeight/2) {
				clearTimeout(timeoutFocus);
				wait_for_clearing = setTimeout(function(){
					if(pageY < o_gallery - windowHeight/5 ||  pageY > o_gallery + windowHeight/5){ 
						clearTimeout(wait_for_clearing);
						clearTimeout(timeoutFocus);
						focusing = false;

					}
				},450);
				timeoutFocus = setTimeout(function(){
					focusing = true;
					if(scrollType=="iScroll"){
					myScroll.scrollTo(0,-o_gallery);
					pageY = o_gallery;
					} else {
						 $('body,html').animate({
							 scrollTop:o_gallery
						 },1500,'easeOutQuint');
						 
						
					}
				},500);
			}
			
		}
	
	
	window[current+"_scroll_desktop"]();
	$pointer.css('transform','scale(1,'+(1/($('#main_scroller').height() - windowHeight))*pageY+')');
}

function initializeMenuScroller(){
    menuPanelExperience5 = new IScroll('#menu_panel_experience5', {
        mouseWheel: true,
        scrollbars: false,
        probeType: 2,
        disablePointer: false,
        scrollY: true,
        scrollX: false,
        click: true,
        useTransition: false,
        //indicators: {
        //    el: '#experience_scrollbar5',
        //    resize: false
        //}

    });
    menuPanelExperience5.disable();
    menuPanelExperience6 = new IScroll('#menu_panel_experience6', {
        mouseWheel: true,
        scrollbars: false,
        probeType: 2,
        disablePointer: false,
        scrollY: true,
        scrollX: false,
        click: true,
        useTransition: false,
        //indicators: {
        //    el: '#experience_scrollbar6',
        //    resize: false
        //}

    });
    menuPanelExperience6.disable();
    menuPanelExperience7 = new IScroll('#menu_panel_experience7', {
        mouseWheel: true,
        scrollbars: false,
        probeType: 2,
        disablePointer: false,
        scrollY: true,
        scrollX: false,
        click: true,
        useTransition: false,
        //indicators: {
        //    el: '#experience_scrollbar7',
        //    resize: false
        //}

    });
    menuPanelExperience7.disable();
    menuPanelExperience8 = new IScroll('#menu_panel_experience8', {
        mouseWheel: true,
        scrollbars: false,
        probeType: 2,
        disablePointer: false,
        scrollY: true,
        scrollX: false,
        click: true,
        useTransition: false,
        //indicators: {
        //    el: '#experience_scrollbar8',
        //    resize: false
        //}
    });
    menuPanelExperience8.disable();
	menuPanelHotelScroller = new IScroll('#menu_panel_hotel', {
	    mouseWheel: true,
	    scrollbars: false,
	    probeType: 2,
	    disablePointer: false,
	    scrollY: true,
	    scrollX: false,
	    click: true,
	    useTransition: false
	});
	menuPanelHotelScroller.disable();
	
	
	
	menuPanelRoomScroller = new IScroll('#menu_panel_rooms', {
	    mouseWheel: true,
	    scrollbars: false,
	    probeType: 2,
	    disablePointer: false,
	    scrollY:true,
	    scrollX:false,
	    click:true,
	    useTransition:false,
	    //indicators: {
	    //    el: '#rooms_scrollbar',
	    //    resize:false
	    //}
	});
	menuPanelRoomScroller.disable();
	
	menuPanelDiningScroller = new IScroll('#menu_panel_dining', {
	    mouseWheel: true,
	    scrollbars: false,
	    probeType: 2,
	    disablePointer: false,
	    scrollY:true,
	    scrollX:false,
	    click:true,
	    useTransition:false,
	    //indicators: {
	    //    el: '#dining_scrollbar',
	    //    resize:false
	    //}

	});
	menuPanelDiningScroller.disable();
	
	menuPanelExperience = new IScroll('#menu_panel_experience', {
	    mouseWheel: true,
	    scrollbars: false,
	    probeType: 2,
	    disablePointer: false,
	    scrollY:true,
	    scrollX:false,
	    click:true,
	    useTransition:false,
	    //indicators: {
	    //    el: '#experience_scrollbar',
	    //    resize:false,
	        
	    //}

    });

    menuPanelExperience.disable();
   
	
	menuPanelRoomScroller.on('scroll', function(){
		roomY = -(menuPanelRoomScroller.y);
		if(roomY > 0){
			$('#menu_panel_rooms').prev().removeClass('no_opacity');
		} else if(roomY == 0){
			$('#menu_panel_rooms').prev().addClass('no_opacity');
		}	
	});
	
	menuPanelDiningScroller.on('scroll', function(){
		diningY = -(menuPanelDiningScroller.y);
		if(diningY > 0){
			$('#menu_panel_dining').prev().removeClass('no_opacity');
		} else if(diningY == 0){
			$('#menu_panel_dining').prev().addClass('no_opacity');
		}	
	});
	
}

function initialize_sitemap_scroller () {
	sitemapScroller = new IScroll('#sitemapitem_box .items_container', {
	    mouseWheel: true,
	    scrollbars: false,
	    probeType: 2,
	    disablePointer: false,
	    scrollY:true,
	    scrollX:false,
	    click:true,
	    useTransition:false
	});  
	
	sitemapScroller.on('scroll', function(){
		sitemapY = -(sitemapScroller.y);
		if(sitemapY > 0){
			$('#sitemapitem_box .text_shadow_upper').removeClass('no_opacity');
		} else if(sitemapY == 0){
			$('#sitemapitem_box .text_shadow_upper').addClass('no_opacity');
		}	
	});
	
	
}


/**************************************************/



$(window).resize(function(){
	windowWidth = $(window).width();
	windowHeight = $(window).height();
	
	mobileCheck =  mobilePlatform || viewport().width < 960;
	
	if(initSettings == false) {
		if(mobileCheck) {
		closePage();
			setTimeout(function(){
		      location.reload();
			},800);
	}
	}
		
		if(initSettings == true) {
			if(!mobileCheck) {
				$('body').css('display','none')
			      location.reload();
			
		}
			
			
	}
	
	var mobile_resize;
	
	$('#main_veil').addClass('no_transition');
	
	if(mobileCheck){
		clearTimeout(mobile_resize);
		mobile_resize = setTimeout(function(){
			global_mobile_resize();
			window[current+"_resize"]();
			$('#main_veil').removeClass('no_transition');

			
		},200);
		
	} else {
		clearTimeout(deepResize);
		window[current+"_resize"]();
		
		deepResize = setTimeout(function(){
			$('#main_veil').removeClass('no_transition');
		window[current+"_resize"];	

		},1500);

	}
});


function show_menu(){
	
	$('.menu_panel .menu_panel_container').removeClass('no_transition');

	
	$('.menu_panel').each(function(i){
		setTimeout(function(){
		$('.menu_panel:eq('+i+') .menu_panel_container').removeClass('hidden');
		},100*i);
	});
	
	$('#main_menu').addClass('active');

	menuPanelRoomScroller.refresh();
	menuPanelHotelScroller.refresh();
	menuPanelDiningScroller.refresh(); 
	menuPanelExperience.refresh();
	menuPanelExperience5.refresh();
	menuPanelExperience6.refresh();
	menuPanelExperience7.refresh();
	menuPanelExperience8.refresh();
	
	setTimeout(function(){
		$('#menu_back').removeClass('no_opacity');
	},800);
	
	setTimeout(function(){
		$('.menu_show_content').each(function(i){
			setTimeout(function(){
				$('.menu_show_content:eq('+i+')').removeClass('hidden_by_scaling_full');
			},100*i);
		});
	},600);
}

function close_menu(){
	$('.text_shadow_upper').addClass('no_opacity');

	
	$('.menu_panel').each(function(i){
		setTimeout(function(){
		$('.menu_panel:eq('+ (($('.menu_panel').length - 1) -i)  +') .menu_panel_container').addClass('no_opacity');
		},50*i);
	});
	
	
	
	setTimeout(function(){
		$('#menu_back').addClass('no_opacity');
	},400);
	
	setTimeout(function(){
		$('.menu_panel .menu_panel_container').addClass('no_transition').removeClass('no_opacity').addClass('hidden');
		$('#main_menu').removeClass('active');
	},1000)
	
	$('.menu_show_content').addClass('hidden_by_scaling_full');
}

function show_menu_content() {

    if ($(this).parents('.menu_panel').eq(0).hasClass('opened')) {
        hide_menu_content_btn($(this).parents('.menu_panel').eq(0).find('.btnCrossHide')[0]);
        return;
    }
    $('.menu_panel.opened').find('.btnCrossHide').each(function() {
        hide_menu_content_btn(this);
    });
   

	$context = $(this).parent().parent().parent();
	
	switch($context.index()){
	case 1:
		
		menuPanelHotelScroller.enable();
		break;
	case 2:
		menuPanelRoomScroller.enable();
		break;
	case 3:
		
		menuPanelDiningScroller.enable();
		break;
	case 4:
	
		menuPanelExperience.enable();
        break;
	case 5:

	    menuPanelExperience5.enable();
        break;
	case 6:

	    menuPanelExperience6.enable();
        break;
	case 7:

	    menuPanelExperience7.enable();
        break;
	case 8:

	    menuPanelExperience8.enable();
	    break;

	}

    $('.menu_show_content', $context).addClass('hidden_by_scaling_full');
	
	setTimeout(function(){
		$context.addClass('opened');
		$('.menu_panel_scrollbar',$context).removeClass('no_height');

	},100);


    $('.menu_paragraph', $context).each(function(i) {
        setTimeout(function() {
                $('.menu_paragraph:eq(' + i + ') .par_number', $context).removeClass('no_opacity');
            },
            650 + (50 * i));

        setTimeout(function() {
                $('.menu_paragraph:eq(' + i + ') .menu_title', $context).removeClass('top_hidden');
            },
            700 + (50 * i));

        setTimeout(function() {
                $('.menu_paragraph:eq(' + i + ') .menu_subtitle', $context).removeClass('top_hidden');
            },
            750 + (50 * i));
    });
	
	setTimeout(function() {
	    $('.menu_hide_content', $context).removeClass('hidden_by_scaling_full');
	},500);
	
	setTimeout(function(){
		$panelOffset = $('#menu_panel_rooms').offset().top;
	},800);
}

function hide_menu_content () {
	$context = $(this).parent().parent().parent();
	
	switch($context.index()){
	case 1:
		
		menuPanelHotelScroller.disable();
		break;
	case 2:
		menuPanelRoomScroller.disable();
		break;
	case 3:
		
		menuPanelDiningScroller.disable();
		break;
	case 4:
	
		menuPanelExperience.disable();
        break;
	case 5:

            menuPanelExperience5.disable();
	    break;
	case 6:

            menuPanelExperience6.disable();
	    break;
	case 7:

            menuPanelExperience7.disable();
	    break;
	case 8:

            menuPanelExperience8.disable();
	    break;
	}
	
	$('.text_shadow_upper').addClass('no_opacity');

	
	$('.menu_hide_content',$context).addClass('hidden_by_scaling_full')
	
	$('.menu_paragraph',$context).each(function(i){
		setTimeout(function(){
			$('.menu_paragraph:eq('+i+') .par_number',$context).addClass('no_opacity');
		},(50*i));
		setTimeout(function(){
			$('.menu_paragraph:eq('+i+') .menu_subtitle',$context).addClass('top_hidden');
		},50+(50*i));
		
		setTimeout(function(){
			$('.menu_paragraph:eq('+i+') .menu_title',$context).addClass('top_hidden');
		},100+(50*i));
		
		
	})
	
	setTimeout(function(){
		$context.removeClass('opened');
		$('.menu_panel_scrollbar',$context).addClass('no_height');

	},300);
	
	setTimeout(function(){
		$('.menu_show_content',$context).removeClass('hidden_by_scaling_full');
	},700);
	
}

function hide_menu_content_btn(el) {
    $elx = $(el).parents('.menu_panel').eq(0);

    switch ($elx.index()) {
    case 1:

        menuPanelHotelScroller.disable();
        break;
    case 2:
        menuPanelRoomScroller.disable();
        break;
    case 3:

        menuPanelDiningScroller.disable();
        break;
    case 4:

        menuPanelExperience.disable();
        break;
    case 5:

        menuPanelExperience5.disable();
        break;
    case 6:

        menuPanelExperience6.disable();
        break;
    case 7:

        menuPanelExperience7.disable();
        break;
    case 8:

        menuPanelExperience8.disable();
        break;
    }

    $('.text_shadow_upper', $elx).addClass('no_opacity');


    $('.menu_hide_content', $elx).addClass('hidden_by_scaling_full')

    $('.menu_paragraph', $elx).each(function(i) {
        setTimeout(function() {
                $('.menu_paragraph:eq(' + i + ') .par_number', $elx).addClass('no_opacity');
            },
            (50 * i));
        setTimeout(function() {
                $('.menu_paragraph:eq(' + i + ') .menu_subtitle', $elx).addClass('top_hidden');
            },
            50 + (50 * i));

        setTimeout(function() {
                $('.menu_paragraph:eq(' + i + ') .menu_title', $elx).addClass('top_hidden');
            },
            100 + (50 * i));
    });

    setTimeout(function () {
        
        $elx.removeClass('opened');
        $('.menu_panel_scrollbar', $elx).addClass('no_height');

    }, 300);

    setTimeout(function () {
        $('.menu_show_content', $elx).removeClass('hidden_by_scaling_full');
    }, 700);

}

function show_side_menu() {
   
	$('#menu_side').removeClass('hidden');
	if (controller == "panel_detail" && current != "photogallery_detail" && current != "specials_detail") {
		showPageController ();
	}
    
	
}

function hide_side_menu(){
	
	$('#menu_side').addClass('hidden');
		hidePageController ();
	
}

function hide_menu_center () {
	if(controller != "page"){
		hidePageController ();
	}

	$('#menu_center_side').addClass('hidden');
	setTimeout(function(){
		//$('#player_side').addClass('hidden');

	},100);
	
}

function show_menu_center () {
	if(controller != "page" && current != "photogallery_detail"){
		showPageController ();
	}
	$('#menu_center_side').removeClass('hidden');
	setTimeout(function(){
		$('#player_side').removeClass('hidden');

	},100);
}

function reset_menu(){
	$('.text_shadow_upper').addClass('no_opacity');

	$('#main_menu').removeClass('active');
	$('#menu_back').removeClass('grey').removeClass('no_transition');
	$('#main_menu .menu_panel').removeClass('opened');
	$('#main_menu .menu_panel_container').removeClass('no_opacity').addClass('hidden');
	$('#main_menu .menu_hide_content, .menu_show_content').addClass('hidden_by_scaling_full');
	$('#main_menu .par_number').addClass('no_opacity');
	$('#main_menu .menu_titles .menu_title, #main_menu  .menu_titles .menu_subtitle').addClass('top_hidden');
	$('#main_menu .menu_panel_scrollbar').addClass('no_height');
}

function show_book_interface(){
	$('#book_interface').removeClass('hidden');
	setTimeout(function(){
		$('#book_interface .white_back').removeClass('hidden_by_scaling_low');
	},1)
	setTimeout(function(){
		$('#book_interface .left_panel').removeClass('hidden');
	},100);
	
	$('#book_form_container .form_input').each(function(b){
		setTimeout(function(){
			$('#book_form_container .form_input:eq('+b+') input').removeClass('top_double');
		},200+(100*b));
		setTimeout(function(){
			$('#book_form_container .form_input:eq('+b+') label').removeClass('top_double');
		},500+(150*b));
	})
	
	setTimeout(function(){
	$('#book_interface .diagonal_line').removeClass('hidden');
	},700);
	
	setTimeout(function(){
		$('#book_interface .overlay_title').removeClass('top_hidden');
	},1000);
	
	setTimeout(function(){
		$('#book_submit .submit').removeClass('hidden');
	},600);
	
	setTimeout(function(){
		$('#book_submit .circle_button .back').removeClass('hidden_by_scaling_full');
	},500);
	
	setTimeout(function(){
		$('#book_submit .circle_button .arrow').removeClass('hidden');
	},700)
	
	setTimeout(function(){
		$('#book_close').removeClass('hidden_by_scaling_full');
	},500)
	
	
}

function hide_book_interface(){
	$('#book_close').addClass('hidden_by_scaling_full');
	$('#book_interface .left_panel').addClass('has_transition_1000').addClass('hidden');
	
	$('#book_form_container .form_input').each(function(b){
		setTimeout(function(){
			$('#book_form_container .form_input:eq('+b+')').addClass('no_opacity');
		},(10*b));
	});
	
	setTimeout(function(){
		$('#book_submit').addClass('no_opacity');
	},30);
	
	setTimeout(function(){
		$('#book_interface').addClass('no_opacity');
	},500);
	
	setTimeout(function(){
		$('#book_interface').addClass('hidden').removeClass('no_opacity');
		$('#book_form_container .form_input, #book_submit').removeClass('no_opacity');
		$('#book_interface .white_back').addClass('hidden_by_scaling_low');
		$('#book_interface .left_panel').removeClass('has_transition_1000');
		$('#book_interface .overlay_title').addClass('top_hidden');
		$('#book_interface .diagonal_line').addClass('hidden');
		$('#book_submit .circle_button .back').addClass('hidden_by_scaling_full');
		$('#book_submit .circle_button .arrow').addClass('hidden');
		$('#book_form_container .form_input input').addClass('top_double');
		$('#book_form_container .form_input label').addClass('top_double');
		$('#book_submit .submit').addClass('hidden');
	},1000)	
}


function show_sitemap(){
	$('#sitemap_interface').removeClass('hidden');
	sitemapScroller.refresh();
	
	setTimeout(function(){
		$('#sitemap_interface .white_back').removeClass('hidden_by_scaling_low');
	},1)
	setTimeout(function(){
		$('#sitemap_interface .left_panel').removeClass('hidden');
	},100);
	
	$('#sitemap_interface .animating').each(function(b){
		setTimeout(function(){
			$('#sitemap_interface .animating:eq('+b+')').removeClass('top_double');
		},200+(50*b));
		
	})
	
	$('#sitemap_interface .section_separator').each(function(b){
		setTimeout(function(){
			$('#sitemap_interface .section_separator:eq('+b+')').removeClass('no_width');
		},200+(50*b));
		
	})
	
	setTimeout(function(){
	$('#sitemap_interface .diagonal_line').removeClass('hidden');
	},700);
	
	setTimeout(function(){
		$('#sitemap_interface .overlay_title').removeClass('top_hidden');
	},1000);
	
	
	
	setTimeout(function(){
		$('#sitemap_close').removeClass('hidden_by_scaling_full');
	},500)
	
	
}

function hide_sitemap_interface(){
	$('#sitemap_close').addClass('hidden_by_scaling_full');
	$('#sitemap_interface .left_panel').addClass('has_transition_1000').addClass('hidden');
	
	$('#sitemap_interface .animating').each(function(b){
		setTimeout(function(){
			$('#sitemap_interface .animating:eq('+b+')').addClass('no_opacity');
		},(10*b));
	});
	
	$('#sitemap_interface .section_separator').addClass('has_transition_800').addClass('no_width');

	
	setTimeout(function(){
		$('#sitemap_interface').addClass('no_opacity');
	},500);
	
	setTimeout(function(){
		$('#sitemap_interface .animating').addClass('top_double').removeClass('no_opacity');
		$('#sitemap_interface').addClass('hidden').removeClass('no_opacity');
		$('#sitemap_interface .white_back').addClass('hidden_by_scaling_low');
		$('#sitemap_interface .left_panel').removeClass('has_transition_1000');
		$('#sitemap_interface .overlay_title').addClass('top_hidden');
		$('#sitemap_interface .diagonal_line').addClass('hidden');
		$('#sitemap_interface .section_separator').removeClass('has_transition_800');

	},1000);
}

function show_credits(){
	$('#credits_interface').removeClass('hidden');
	
	setTimeout(function(){
		$('#credits_interface .white_back').removeClass('hidden_by_scaling_low');
	},1)
	setTimeout(function(){
		$('#credits_interface .left_panel').removeClass('hidden');
	},100);
	
	$('#credits_interface .animating').each(function(b){
		setTimeout(function(){
			$('#credits_interface .animating:eq('+b+')').removeClass('top_double');
		},200+(50*b));
		
	})
	
	$('#credits_interface .section_separator').each(function(b){
		setTimeout(function(){
			$('#credits_interface .section_separator:eq('+b+')').removeClass('no_width');
		},200+(50*b));
		
	})
	
	setTimeout(function(){
	$('#credits_interface .diagonal_line').removeClass('hidden');
	},700);
	
	setTimeout(function(){
		$('#credits_interface .overlay_title').removeClass('top_hidden');
	},1000);
	
	
	
	setTimeout(function(){
		$('#credits_close').removeClass('hidden_by_scaling_full');
	},500)
	
	
}

function hide_credits_interface(){
	$('#credits_close').addClass('hidden_by_scaling_full');
	$('#credits_interface .left_panel').addClass('has_transition_1000').addClass('hidden');
	
	$('#credits_interface .animating').each(function(b){
		setTimeout(function(){
			$('#credits_interface .animating:eq('+b+')').addClass('no_opacity');
		},(10*b));
	});
	
	$('#credits_interface .section_separator').addClass('has_transition_800').addClass('no_width');

	
	setTimeout(function(){
		$('#credits_interface').addClass('no_opacity');
	},500);
	
	setTimeout(function(){
		$('#credits_interface .animating').addClass('top_double').removeClass('no_opacity');
		$('#credits_interface').addClass('hidden').removeClass('no_opacity');
		$('#credits_interface .white_back').addClass('hidden_by_scaling_low');
		$('#credits_interface .left_panel').removeClass('has_transition_1000');
		$('#credits_interface .overlay_title').addClass('top_hidden');
		$('#credits_interface .diagonal_line').addClass('hidden');
		$('#credits_interface .section_separator').removeClass('has_transition_800');

	},1000);
}


function set_lang(e) {
    e.preventDefault();
    $this = $(this);
    current_url = window.location.href;
    new_url = current_url.replace("/" + lang + "/", "/" + $(this).attr("rel") + "/");
    closePage();
    setTimeout(function () {
        window.location = new_url;
    }, 800);
}

/************************ MAILER ******************************/

function mailer(){
	$scope =  $(this).parent().parent();
	
		
	full_lang = get_extendend_lang();
	form_type = $(this).parent().parent().attr('rel');

	
	controller = form_type;
	
	switch(controller){
	case 'map':
		subject = 'Richiesta di download di Casa Angelina Surroundings Map';
	break;
	case 'book':
		subject = 'Richiesta di download di Casa Angelina Digital Book';
	break;
	case 'wellness': 
		subject = 'Richiesta di informazioni / prenotazione trattamenti Wellness';
	break;
	case 'shop': 
		subject = 'Richiesta di informazioni sui prodotti Casa Angelina & Partners';
	break;
	case 'concierge': 
		subject = 'Richiesta di informazioni verso Concierge';
	break;
	case 'chef': 
		subject = 'Richiesta di informazioni verso Chef';
	break;
	case 'upnc': 
		subject = 'Richiesta di prenotazione tavolo Un Piano Nel Cielo';
	break;
	}
	
	name = $('#name').val();
	surname = $('#surname').val();
	email = $('#email').val();
	city = $('#city').val();
	country = $('#country').val();
	message = $('#message').val();
	date = $('#form_date').val();
	
	
	if(controller == "upnc"){
		date = moment($('#form_date').val(), "mm/DD/YYYY").format("DD/mm/YYYY");
	}
	
	phone = $('#phone').val();
	rooms = $('#form_rooms').val();
	time = $('#form_time').val();
	comment = $('#shop_other_comment').val();
	guests = $('#form_guests').val();
	
	if(form_type == "map" || form_type == "wellness"){
		message == 'not required' /**** skip message check **/
	}
	
	items = get_item_selection($scope);
	
	
	target = ''; /*** LATER ***/
	
	prv = $('#prv');
	
	
	
	var email_reg_exp = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-]{2,})+\.)+([a-zA-Z0-9]{2,})+$/;

	validating = true;
	
	$('.required',$scope).each(function(){
		if($(this).val() == "" || $(this).val() == undefined || $(this).val() == null){
			validating = false;
		}
	})
	
	if(!validating){
		switch(lang){
    	case 'en':
    	case 'de':
    		alert('Please fill all required fields!');
    	break;
    	case 'it':
    		alert('Si prega di compilare tutti i campi richiesti, grazie!');
    	break;
    	case 'fr':
    		alert('S\'il vous plaît, remplir tous les champs obligatoires, merci!');
    	break;

    	}
		return false;
	} 
	
	
	
	if (!email_reg_exp.test(email)) {
		switch(lang){
    	case 'en':
    	case 'de':
    		alert('Please provide a valid email address!');
    	break;
    	case 'it':
    		alert('Si prega di inserire una mail valida grazie!');
    		break;
    	case 'it':
    		alert('S\'il vous plaît, entrer une adresse email valide, merci!');
    	break;

    	}
		$("#email").focus();
		return false;
	}
	
	if(message == ''){
		switch(lang){
    	case 'en':
    	case 'de':
    		alert('Please insert a message!');
    	break;
    	case 'it':
    		alert('Si prega di inserire il messaggio!');
    	break;
    	case 'fr':
    		alert('S\'il vous plaît entrer dans le message!');
    	break;

    	}
		$("#message").focus();
		return false;
	}
	
	if(!prv.prop('checked')){
		switch(lang){
    	case 'en':
    	case 'de':
    		alert('You should accept the privacy policy in order to contact us!');
    	break;
    	case 'it':
    		alert('Si prega di accettare i termini della privacy policy per poterci contattare!');
    	break;
    	case 'fr':
    		alert('S\'il vous plaît, accepter les termes de la privacy policy pour nous contacter!');
    	break;

    	}
		return false;
	}
	
	vars = $.param({"site_lang":full_lang, "controller":controller, "subject": subject,"name": name,"surname":surname,"email": email,"city":city,"country":country,"message":message,"phone":phone,"date":date,"rooms": rooms, "guests": guests,"time": time,"items":items, "comment":comment});

	$('.form_submit',$scope).css({
		'pointer-events':'none',
		'opacity':'0.5'
	});
	
	$.ajax({
	      type: "POST",
	      url: "/"+lang+"/mailer",
	      data: vars,
	      dataType: "json",
	      success: function(msg)
	      {
	    	   if(msg.message == '0'){
			        	switch(lang){
			        	case 'en':
			        	case 'de':
			        		alert('Thank you! Your request has been submitted and we will contact you as soon as we can!');
			        	break;
			        	case 'it':
			        		alert('Grazie! La Vostra richiesta è stata inviata, Vi risponderemo appena possibile!');
			        		break;
			        	case 'fr':
			        		alert('Merci! Votre demande a été envoyée et nous vous répondrons dès que possible!');
			        	break;
			        	}
		        	
		        	
			       
		        	$('#prv').prop('checked',false);
		        	$('.form_submit',$scope).css({
		        		'pointer-events':'all',
		        		'opacity':'1'
		        	});

		        	
		        } else if(msg.message == '1'){
		        	switch(lang){
		        	case 'en':
		        	case 'de':
		        		alert('We are sorry, but there was a problem while processing your request, please try again later, thank you!');
		        	break;
		        	case 'it':
		        		alert('Nous sommes désolés mais il ya eu une erreur dans le traitement de votre demande, s\'il vous plaît essayer de nouveau plus tard, merci');
		        		break;

		        	}
		        	
		        	$('.form_submit',$scope).css({
		        		'pointer-events':'all',
		        		'opacity':'1'
		        	});

		        	
		        }
	      }
	     
	    });  
}

function get_extendend_lang(){
	var ext_lang;
	
	
		switch(lang){
			case 'it':
				ext_lang =  'italiano';
			break;
			case 'en':
				ext_lang = 'inglese';
			break;
			case 'fr':
				ext_lang =  'francese';
			break;
			case 'de':
				ext_lang =  'tedesco';
			break;
		}
	
	
	return ext_lang;
}

function get_item_selection ($scope){
	var req_item = '';
	
	$('.checkbox input:checked', $scope).each(function(i){
		if($('.checkbox input:checked', $scope).length-1 > i){
			req_item += $(this).val();
			req_item += ', '
				
		} else {
			req_item += $(this).val();
		}
		
		
	});
	 return req_item;
}


function manage_thanks (){
	if(!thanksOpened){
		thanksOpened = true;
		$('#thanks_button').addClass('active');
	} else {
		thanksOpened = false;

		$('#thanks_button').removeClass('active');

	}
	
}

function manage_lang (){
	if(!langOpened){
		langOpened = true;
		$('#lang_button').addClass('active');
	} else {
		langOpened = false;

		$('#lang_button').removeClass('active');

	}
	
}











var mesi = new Array();
mesi[0] = "Gennaio";
mesi[1] = "Febbraio";
mesi[2] = "Marzo";
mesi[3] = "Aprile";
mesi[4] = "Maggio";
mesi[5] = "Giugno";
mesi[6] = "Luglio";
mesi[7] = "Agosto";
mesi[8] = "Settembre";
mesi[9] = "Ottobre";
mesi[10] = "Novembre";
mesi[11] = "Dicembre";



var month = new Array();
month[0] = "January";
month[1] = "February";
month[2] = "March";
month[3] = "April";
month[4] = "May";
month[5] = "June";
month[6] = "July";
month[7] = "August";
month[8] = "September";
month[9] = "October";
month[10] = "November";
month[11] = "December";

var mois = new Array();
mois[0] = "Janvier";
mois[1] = "Février";
mois[2] = "Mars";
mois[3] = "Avril";
mois[4] = "Mai";
mois[5] = "Juin";
mois[6] = "Juillet";
mois[7] = "Août";
mois[8] = "Septembre";
mois[9] = "Octobre";
mois[10] = "Novembre";
mois[11] = "Décembre";

var monate = new Array();
monate[0] = "Januar";
monate[1] = "Februar";
monate[2] = "März";
monate[3] = "April";
monate[4] = "Mai";
monate[5] = "Juni";
monate[6] = "Juli";
monate[7] = "August";
monate[8] = "September";
monate[9] = "Oktober";
monate[10] = "November";
monate[11] = "Dezember";

var giorni = new Array();
giorni[1] = "Lun";
giorni[2] = "Mar";
giorni[3] = "Mer";
giorni[4] = "Gio";
giorni[5] = "Ven";
giorni[6] = "Sab";
giorni[0] = "Dom";


var jours = new Array();
jours[1] = "Lun";
jours[2] = "Mar";
jours[3] = "Mer";
jours[4] = "Jeu";
jours[5] = "Ven";
jours[6] = "Sam";
jours[0] = "Dim";

var tage = new Array();
tage[1] = "Mo";
tage[2] = "Di";
tage[3] = "Mi";
tage[4] = "Do";
tage[5] = "Fr";
tage[6] = "Sa";
tage[0] = "So";


function getMonthNames(){
	if(lang=="it"){
		return mesi;
	} else 	if(lang=="fr"){
		return mois;
	}	if(lang=="de"){
		return monate;
	} else {
		return month
	}
}



function getDowNames(){
	if(lang=="it"){
		return giorni;
	} else 	if(lang=="fr"){
		return jours;
	} else 	if(lang=="de"){
		return tage;
	} else {
		return null
	}
}

function getDowOffset(){
	 	if(lang=="it"){
	   		return 1;
	   	} else 	if(lang=="fr"){
	   		return 1;
	   	} else 	if(lang=="de"){
	   		return 1;
	   	} else {
	   		return 0  	
	   	}
}

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

function get_locale() {
    if (lang == "it") {
        return "it-IT"
    } else if (lang == "en") {
        return "en-GB"
    } else if (lang == "fr") {
        return "fr-FR"
    } else if (lang == "de") {
    	return "de-DE"
    }
}
	


function get_lang_number() {
    if (lang == "it") {
        return 5
    } else if (lang == "en") {
        return 1
    } else if (lang == "fr") {
        return 22
    } else if (lang == "de") {
    	return 4
    }
}