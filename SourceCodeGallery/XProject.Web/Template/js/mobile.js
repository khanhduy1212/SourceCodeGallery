var _mousemove;
var _click;
var _mouseenter;
var _mouseleve;
var _mousedown;
var _mouseup;


var mobileMenuOpen = false;

if (Modernizr.touchevents) {
	
    _mousemove = "touchmove";
    _click = "touchend";
    _mousedown = "touchstart";
    _mouseup = "touchend";
    _mouseenter = "mouseenter";
    _mouseleave = "mouseleave";
} else {
    _mousemove = "mousemove";
    _click = "click";
    _mousedown = "mousedown";
    _mouseup = "mouseup";
    _mouseenter = "mouseenter";
    _mouseleave = "mouseleave";
}


function mobile_base_load(e){
	e.preventDefault();
	  $('#main_veil').addClass('hidden').show();
    setTimeout(function() {
        $('#main_veil').removeClass('hidden');

    }, 50);
	  
	  $this = $(this);
    setTimeout(function() {
        window.location.href = $this.attr('href');
    }, 1000);
}


function init_mobile_setup() {
	mobile_viewport_setup();
    $('.menu_controller').bind('click', manage_mobile_menu);
    $('.section_panel._1 .lower .next_chapter ').bind('click', function() {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 2, 0);
        setTimeout(function() { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);

    });

    $('.section_panel._2 .section_track img,.section_panel._2 .lower .next_chapter ').bind('click', function() {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 3, 0);
        setTimeout(function() { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });

    $('.section_panel._3 .section_track img,.section_panel._3 .lower .next_chapter ').bind('click', function() {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 4, 0);
        setTimeout(function() { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });
    $('.section_panel._4 .section_track img,.section_panel._4 .lower .next_chapter ').bind('click', function () {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 5, 0);
        setTimeout(function () { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });
    $('.section_panel._5 .section_track img,.section_panel._5 .lower .next_chapter ').bind('click', function () {
       $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 6, 0);
        setTimeout(function () { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });

    $('.section_panel._6 .section_track img,.section_panel._6 .lower .next_chapter ').bind('click', function () {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 7, 0);
        setTimeout(function () { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });
    $('.section_panel._7 .section_track img,.section_panel._7 .lower .next_chapter ').bind('click', function () {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 8, 0);
        setTimeout(function () { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });
    $('.section_panel._8 .section_track img,.section_panel._8 .lower .next_chapter ').bind('click', function () {
        $('#menu_mobile_scroller').addClass('menu_scroll_transition');
        mobileMenuScroll.scrollTo(-(windowWidth - 40) * 9, 0);
        setTimeout(function () { $('#menu_mobile_scroller').removeClass('menu_scroll_transition'); }, 50);


    });
	initialize_mobile_scroll();
	initialize_mobile_menu_scroll();
	initializeMobilePanelScroller();
	window[current+"_ready_mobile"]();
	
	$('.paragraphs .menu_titles').bind('click',mobile_base_load);
	$('.first_panel .center a:not(.no_link)').bind('click',mobile_base_load);
	//$('#side_mobile a.book_now').bind('click',show_book_interface);
	$('#panelMobileController a').bind('click',mobile_base_load);
	
	if($('.paragraph.first .big_subtitle').length != 0 && $('.paragraph.first .big_subtitle').html().indexOf('>&nbsp;<') != -1) {
		
		$('.paragraph.first .big_subtitle').html('');
	}
	
	$('#mobile_lang_button a').bind('click',set_lang);

	
	do_once = false ; /* used to manage gallery resize */
	
	scroll_threshold = 0;
	
	
	arrivalDatepicker = $('.arrival').glDatePicker(
			{
				cssName: 'flatwhite',
				zIndex: 1000,
			    borderSize: 0,
			    monthNames : getMonthNames(),
			    dowNames : getDowNames(),
			    dowOffset: getDowOffset(),
			    selectableDateRange: [{ from: new Date(), to:new Date(3000,1,1)}],
			    selectedDate: arrivalDate,
			    onShow: function(calendar){
			    	arrivalDatepicker.render();
			    	clearTimeout(hideDatepickerArrival);
			    	calendar.find('div').addClass('top_double').addClass('has_transition_600');
			    	calendar.show();
			    	calendar.find('div').each(function(c){
			    		setTimeout(function(){
			    			calendar.find('div:eq('+c+')').removeClass('top_double')
			    		},5*c);
			    	})
			    	
			    },
			    onHide:function(calendar){
			    	hideDatepickerArrival = setTimeout(function(){
			    	calendar.addClass('has_transition_300').addClass('no_opacity')
			    	setTimeout(function(){calendar.hide();},300)
			    	},1);
			    },
			    onClick: function(el, cell, date, data) {
			    	var dd = date.getDate();
			    	var mm = date.getMonth()+1;
			    	var yyyy = date.getFullYear();
			    	
			    	if(dd<10) {
			    	    dd='0'+dd
			    	}
			    	
			    	if(mm<10) {
			    	    mm='0'+mm
			    	} 
			    	
			    	arrivalDate = date;
			    	selected = dd+'/'+mm+'/'+yyyy;
			    	el.children('input').val(selected);
			    },
			}
		).glDatePicker(true);
		departureDatepicker = $('.departure').glDatePicker(
			{
				cssName: 'flatwhite',
				zIndex: 1000,
			    borderSize: 0,
			    monthNames : getMonthNames(),
			    dowNames : getDowNames(),
			    dowOffset: getDowOffset(),

			    selectableDateRange: [{ from: new Date(), to:new Date(3000,1,1)}],
			    selectedDate:departureDate,

			    onShow: function(calendar){
			    	clearTimeout(hideDatepickerDeparture);
			    	departureDatepicker.render();
			    	calendar.find('div').addClass('top_double').addClass('has_transition_600')
			    	calendar.show();
			    	calendar.find('div').each(function(c){
			    		setTimeout(function(){
			    			calendar.find('div:eq('+c+')').removeClass('top_double')
			    		},5*c);
			    	})
			    	
			    },
			    onHide:function(calendar){
			    	hideDatepickerDeparture = setTimeout(function(){
				    	calendar.addClass('has_transition_300').addClass('no_opacity')
				    	setTimeout(function(){calendar.hide();},300)
				    },1);
			    	
			    },
			    onClick: function(el, cell, date, data) {
			    	var dd = date.getDate();
			    	var mm = date.getMonth()+1;
			    	var yyyy = date.getFullYear();
			    	
			    	if(dd<10) {
			    	    dd='0'+dd
			    	}
			    	
			    	if(mm<10) {
			    	    mm='0'+mm
			    	} 
			    	
			    	departureDate = date;
			    	selected = dd+'/'+mm+'/'+yyyy;
			    	el.children('input').val(selected);
			    },
			}
		).glDatePicker(true);
		
		$('#book_close').bind('click',hide_book_interface);
		
		 $("#book_submit").click(function() {
		        if ($("#arrival").val() != "" && $("#departure").val() != "" && $("#guests").val() != "" && isNaN($("#guests").val()) == false) {
		            
		        	$momentArrival = moment((arrivalDate.getMonth() + 1) + "/" + arrivalDate.getDate() +"/" + arrivalDate.getFullYear(), 'MM-DD-YYYY');
		        	$momentDeparture = moment((departureDate.getMonth() + 1) + "/" + departureDate.getDate() +"/" + departureDate.getFullYear(), 'MM-DD-YYYY');
		        	$momentNights = $momentDeparture.diff($momentArrival, 'days');
		        	
		        	
					window.open("https://be.synxis.com/?adult="+$("#guests").val()+"&arrive="+arrivalDate.getFullYear()+"-"+(arrivalDate.getMonth() + 1)+"-"+arrivalDate.getDate()+"&depart="+departureDate.getFullYear()+"-"+(departureDate.getMonth() + 1)+"-"+departureDate.getDate()+"&chain=22402&currency=EUR&hotel=79920&src=24C&locale="+get_locale())
					
					
		          
		            
		         
		            
		        } else {
		            alert("Please fill all fields with correct data!")
		        }
		    });


}

function initialize_mobile_scroll(){
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
        mobile_scroll();
    });

    $(window).on('touchstart, touchmove', function() {
        $('body,html').stop(true);

    });
}

function mobile_scroll(){
	window[current+"_scroll_mobile"]();
	
	if(!v_weather && pageY > o_weather - windowHeight + scroll_threshold) {
		v_weather = true;
		weatherIn();
		
		
	}
	
	if(!side_menu && controller == "panel_detail" &&  pageY > windowHeight/3 && pageY <= $(document).height() - windowHeight - 100) {
		side_menu = true;
		showPageController ();
	}
	
	if(side_menu && controller == "panel_detail" && pageY < windowHeight/3  && current != "photogallery_detail" ) {
		side_menu = false;
		hidePageController ();
	}
	
	if(side_menu && controller == "panel_detail" && pageY > $(document).height() - windowHeight - 100){
		side_menu = false;
		hidePageController ();
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
		if(!v_form && pageY > o_form - windowHeight + scroll_threshold ) {
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
	
	
	if(has_gallery){
		
		if(!forced_fullscreen && windowWidth > windowHeight && pageY > o_gallery - windowHeight/3 && pageY < o_gallery + windowHeight/3){
			
			forced_fullscreen = true;
			if(controller == "panel_detail"){
				hidePageController();			}
			
		}
		
		if(forced_fullscreen && (pageY < o_gallery - windowHeight/3 ||  pageY > o_gallery + windowHeight/3)){
			forced_fullscreen = false;
			if(controller == "panel_detail"){

				showPageController();
			}
			
		}
		
		
		
		
		/*if(pageY > o_gallery - windowHeight/3 && pageY < o_gallery + windowHeight/3) {
			clearTimeout(timeoutFocus);
			wait_for_clearing = setTimeout(function(){
				if(pageY < o_gallery - windowHeight/3 ||  pageY > o_gallery + windowHeight/3){ 
					clearTimeout(wait_for_clearing);
					clearTimeout(timeoutFocus);
					focusing = false;

				}
			},450);
			timeoutFocus = setTimeout(function(){
				focusing = true;
				console.log('ok')
				 $('body,html').stop(true).animate({
					 scrollTop:o_gallery
				 },600,'easeOutQuint');
					 
					
				
			},500);
		}*/
		
	}
	


}


function initialize_mobile_menu_scroll() {
	mobileMenuScroll = new IScroll('#menu_mobile', {
	    mouseWheel: false,
	    scrollbars: false,
	    probeType: 2,
	    scrollX:true,
		scrollY:false,
		click:true,
		disableMouse:true,
		useTransition:true,
		 snap: true,
		 momentum:false
	});
	
	mobileMenuScroll.on('scroll', function(){
		pageX = -(mobileMenuScroll.x);
		
		
		
		
	});
	
	
	$('#chapters .chapter').bind('click',function(){
		//$('#menu_mobile_scroller').addClass('menu_scroll_transition');

		//mobileMenuScroll.scrollTo(-(windowWidth-40)*($(this).index('.chapter')+1),0);
		
		//setTimeout(function(){$('#menu_mobile_scroller').removeClass('menu_scroll_transition');},100);

        mobileMenuScroll.goToPage($(this).index('.chapter')+1, 0, 1000);
	});
	
}

function initializeMobilePanelScroller(){
	
	menuPanelHotelScroller = new IScroll('.section_panel._1 .paragraphs', {
		mouseWheel: false,
	    scrollbars: false,
	    probeType: 1,
	    scrollX:false,
		scrollY:true,
		click:true,
		disablePointer:false,
		disableMouse:true,
		useTransition:true,
		momentum: false
	});
	
	
	
	menuPanelRoomScroller = new IScroll('.section_panel._2 .paragraphs', {
		mouseWheel: false,
	    scrollbars: false,
	    probeType: 1,
	    scrollX:false,
		scrollY:true,
		click:true,
		disableMouse:true,
		useTransition:true,
		disablePointer:false,

		 momentum:false
	});
	
	menuPanelDiningScroller = new IScroll('.section_panel._3 .paragraphs', {
		mouseWheel: false,
	    scrollbars: false,
	    probeType: 1,
	    scrollX:false,
		scrollY:true,
		click:true,
		disableMouse:true,
		useTransition:true,
		 momentum:false

	});
	
	menuPanelExperience = new IScroll('.section_panel._4 .paragraphs', {
		mouseWheel: false,
	    scrollbars: false,
	    probeType: 1,
	    scrollX:false,
		scrollY:true,
		click:true,
		disableMouse:true,
		useTransition:true,
		 momentum:false

	});
	
	
}

function global_mobile_resize() {
	$('#side_mobile,#menu_mobile').height(window.innerHeight);
	 mobile_viewport_setup ();
	setTimeout(function(){
	menuPanelRoomScroller.refresh();
	menuPanelDiningScroller.refresh();
	menuPanelExperience.refresh();
	//menuPanelExperience5.refresh();
	//menuPanelExperience6.refresh();
	//menuPanelExperience7.refresh();
	//menuPanelExperience8.refresh();
	if(controller == "panel"){
    	introTextScroll.refresh();

	}
	},1000);
	
	if(has_gallery && window.innerWidth > window.innerHeight){
		do_once = true;
		setTimeout(function(){
			if(Math.abs(pageY - o_gallery) < window.innerWidth/1.5){
				gallery_slider_setup();
				$('body,html').stop(true).animate({
					 scrollTop:o_gallery
				 },600,'easeOutQuint');
			}
			$(window).trigger('scroll');

		},300);
		
		$('.advice').addClass('no_opacity');
	} else if (has_gallery && window.innerWidth < window.innerHeight) {
		if(do_once){
			do_once = false;
			$('.advice').removeClass('no_opacity');
			setTimeout(function(){
				gallery_slider_setup();
			$(window).trigger('scroll');
			$(window).trigger('resize');
			
		},300);
	}


	}
	
	window[current+"_resize"]();

}


function manage_mobile_menu () {
	if(!mobileMenuOpen){
		mobileMenuOpen = true;
		show_menu_mobile ();
	} else {
	    mobileMenuOpen = false;
		hide_menu_mobile ();
	}
}

function mobile_viewport_setup (){
	$('#side_mobile,#menu_mobile').height(window.innerHeight);
	$('#menu_mobile_scroller').width((windowWidth-39)*9);
}

function show_menu_mobile () {
	$('#menu_mobile').removeClass('hidden');
}
function show_menu_mobile_cus () {
	$('#menu_mobile').removeClass('hidden');
	$('#menu_mobile_scroller').css('transform', 'translate(0px, 0px) translateZ(0px)');
}
function hide_menu_mobile () {
	$('#menu_mobile').addClass('hidden');
}

function check_portrait(){
	if($('*:focus').length == 0){
		if(window.innerWidth > window.innerHeight){
			 $portrait = false;
				$('#rotation_advice').show();
				setTimeout(function(){$('#rotation_advice').removeClass('no_opacity')},1);		 
		} else {
			 $portrait = true;
				setTimeout(function(){$('#rotation_advice').addClass('no_opacity')},1);		 
		}
	}
}
