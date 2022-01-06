var timeoutPrev;
var timeoutMenu;
var timeoutNext;

function panels_ready_desktop() {		


	panels_initialized = true;
	panelDetail_ready = true;
	controller = "panel";
	
	$('#main_veil').addClass('grey').hide();
	
	prev_visible = false;
	next_visible = true;
	

	popEnabled = false;
	
	$('#panel_controls .menu .menu_panel_controller').bind('click',show_menu);

    $('#ca_home').bind('click',
        function() {
            closePanels();
            var linkHome = $('.linkHome').val();
            navigateTimeout = setTimeout(function() {
                    base_load(linkHome + '/home', true);
                },
                800);
        });
	
	panel_scroll_num = Math.ceil($('#section_scroller .section_panel').length / 3);


	$scrollerWidth = Math.ceil($('.section_panel').width()*$('#section_scroller .section_panel').length);
	$('#section_scroller_container').addClass('no_transition').width($scrollerWidth);
	$boundaryX = -($scrollerWidth - windowWidth); /*** confine destro scroll **/
		
	
	if(typeof myScroll !== 'undefined'){
		myScroll.destroy();
		$('.iScrollVerticalScrollbar ').remove();
		
	}
	
	
	
	panelScroll = new IScroll('#section_scroller', {
			    mouseWheel: true,
			    scrollbars: false,
			    probeType: 2,
			    disablePointer: enableDrag(),
			    scrollX:true,
				scrollY:false,
				click:!enableDrag(),
				useTransition:true,
				momentum:false
			});
			
	panelScroll.on('scroll', function () {
	   
	    panels_scroll_desktop();

	});

	
	if(current_scroll < panel_scroll_num-1 && current_scroll > 0){
		panelScroll.scrollTo(-window.innerWidth * current_scroll,0);
		
	}
	
	
	if(current_scroll == panel_scroll_num-1){
		panelScroll.scrollTo($boundaryX,0);
	}
	
	introTextScroll = new IScroll('.panel_section_text_container ', {
		mouseWheel: true,
	    scrollbars: true,
	    probeType: 2,
	    disablePointer: enableDrag(),
	    scrollX:false,
		scrollY:true,
		click:!enableDrag(),
		useTransition:true,
		momentum:false
	});
	
	introTextScroll.on('scrollStart', function(){
		panelScroll.disable();
	});
	
	introTextScroll.on('scrollEnd', function(){
		panelScroll.enable();
	});
	
	
			
	reset_menu();
			
	$('#section_scroller_container').removeClass('no_transition')
	

	$('.section_pic img').imagesLoaded(function(){
		
			check_section_pic ();
		
	});
	
	
	if(first_run){
		first_run = false;
		
		 var stateObj = { controller: "panel", current:current, url:window.location.href };
		 history.replaceState(stateObj, "", window.location);
	} 
	
	
	
	$('#panel_controls .next').bind('click',panel_scroll_next);
	$('#panel_controls .prev').bind('click',panel_scroll_prev);
	
	
	
	$('#section_scroller_container .section_panel:eq(0) img, #section_scroller_container .section_panel:eq(1) img,#section_scroller_container .section_panel:eq(2) img ').imagesLoaded(function(){
		$('body').removeClass('preload');

		check_section_pic ();
		
		panels_resize ();
		
		panelScroll.refresh();
		
		$('img[data-src]').each(function(){
			$path = $(this).attr('data-src');
			$(this).attr('src',$path);
		});
		
		$('img').imagesLoaded(function(){
			check_section_pic ();
			panels_resize ();
			panelScroll.refresh();

		});
		
		
		$('.section_panel').each(function(n){
			setTimeout(function(){			
				$('.section_panel:eq('+n+') .loading_cover').addClass('removed');
				$('.section_panel:eq('+n+') .section_pic').removeClass('scaled_full');
			},100*n)
			setTimeout(function(){			
				$('.section_panel:eq('+n+') .info').removeClass('top_double');
			},1000+(100*n))
			
		})
		
		setTimeout(function(){
		$('.panel_section_title').removeClass('top_hidden');
	},1300)
	setTimeout(function(){
		$('.panel_section_subtitle').removeClass('top_hidden');
		$('#section_scroller').removeClass('disabled');
		popEnabled = true;
		loadEnabled = true;
	},1400)
	setTimeout(function(){
		$('.panel_section_text_container').removeClass('top_translated');

	},800)
	
	
	if(pageX < -$boundaryX && pageX > 0 ){ 
		timeoutPrev = setTimeout(show_panel_prev,2000);
		timeoutMenu = setTimeout(show_panel_menu,2100);
		timeoutNext = setTimeout(show_panel_next,2200);
		
	} else if(pageX < -$boundaryX){
		timeoutMenu = setTimeout(show_panel_menu,2100);
		timeoutNext = setTimeout(show_panel_next,2200);
		
	} else if(pageX > 0){
		timeoutNext = setTimeout(show_panel_prev,2000);
		timeoutMenu = setTimeout(show_panel_menu,2100);
	}
		
	
	})
	
	
	
	

		
	
	
	
	


	
	
	setTimeout(function(){
		$('.section_panel .section_pic').removeClass('has_transition_1000_inout_quint').addClass('has_transition_800');
		$('#section_scroller .section_panel:not(:first-child)').bind('mouseenter',panel_hover).bind('mouseleave',panel_out).bind('click',openPageDetail);
	},1200);
	
	
}

function panels_load() {
	
}



function panels_scroll_desktop() {
   
	pageX = -(panelScroll.x);
	if(pageX > 0 && !prev_visible){
		show_panel_prev();	
	}
	
	if(pageX <= 0 && prev_visible){
		hide_panel_prev();	
	}
	
	if(pageX >= $scrollerWidth - windowWidth){
		hide_panel_next();
	}
	
	if(pageX < $scrollerWidth - windowWidth){
		show_panel_next();
	}
	
	for(s=0;s<panel_scroll_num+1;s++){
		if(pageX >= windowWidth * s) {
			current_scroll = s+1;
		} else if(pageX <= 0){
			current_scroll = 0;
		}
	}
	
	
}

function panels_scroll_mobile() {
	
}

function panels_resize () {

	if(!isHandheld || (isHandheld && mobileCheck)){check_section_pic();}
	$scrollerWidth = Math.ceil($('.section_panel').width()*$('#section_scroller .section_panel').length);
	if(mobileCheck ){
		$scrollerWidth = Math.ceil((windowWidth-40)*$('#section_scroller .section_panel').length);

	}
	$('#section_scroller_container').width($scrollerWidth);
	$boundaryX = -($scrollerWidth - windowWidth); /*** confine destro scroll **/

	//panelScroll.refresh();

	}

function check_section_pic () {
	viewport_width = $(window).width();
	viewport_height = window.innerHeight;
	
    screen_ratio = viewport_width / viewport_height;
    pic_ratio = 2400 / 1350;

    if(isHandheld){
		$('#section_animation,#head_content,#section_scroller').height(window.innerHeight);
    }
    
  
    
    if (pic_ratio > screen_ratio) {
        $('#section_scroller .section_pic img,#section_animation .section_pic img').css({
            height: viewport_height,
            width: Math.round(viewport_height * pic_ratio),
            marginLeft: Math.round(-(viewport_height * pic_ratio - viewport_width) / 2),
            marginTop: 0
        })
    } else {
        $("#section_scroller .section_pic img, #section_animation .section_pic img").css({
            width: viewport_width,
            height: Math.round(viewport_width / pic_ratio),
            marginTop: Math.round(-(viewport_width / pic_ratio - viewport_height) / 2),
            marginLeft: 0
        })
    }
}


function panel_hover () {
	$panel = $(this);
	

	$('.section_pic', $panel).addClass('scaled');
	$('.details', $panel).removeClass('no_opacity');
	
	setTimeout(function(){$('.info_details .panel_detail:eq(0) .det_num',$panel).removeClass('top_hidden');},200);
	setTimeout(function(){$('.info_details .panel_detail:eq(0) .det_black',$panel).removeClass('top_hidden');},220);
	setTimeout(function(){$('.info_details .panel_detail:eq(0) .det_white',$panel).removeClass('top_hidden');},240);
	
	setTimeout(function(){$('.info_details .panel_detail:eq(1) .det_num',$panel).removeClass('top_hidden');},300);
	setTimeout(function(){$('.info_details .panel_detail:eq(1) .det_black',$panel).removeClass('top_hidden');},320);
	setTimeout(function(){$('.info_details .panel_detail:eq(1) .det_white',$panel).removeClass('top_hidden');},340);
	
	setTimeout(function(){$('.info_details .panel_detail:eq(2) .det_num',$panel).removeClass('top_hidden');},400);
	setTimeout(function(){$('.info_details .panel_detail:eq(2) .det_black',$panel).removeClass('top_hidden');},420);
	setTimeout(function(){$('.info_details .panel_detail:eq(2) .det_white',$panel).removeClass('top_hidden');},440);

}

function panel_out () {
	$panel = $(this);
	
	$('.section_pic', $panel).removeClass('scaled');
	$('.details', $panel).addClass('no_opacity');
	
	$('.info_details .panel_detail .det_num, .info_details .panel_detail .det_black, .info_details .panel_detail .det_white',$panel).addClass('top_hidden');
	
}

var panel_scroll_num;
var current_scroll = 0;

function panel_scroll_next() {
	
	if(current_scroll < panel_scroll_num-1){
		current_scroll++;
		show_panel_prev();
		panelScroll.scrollTo(-window.innerWidth * current_scroll,0);
	}
	
	
	if(current_scroll == panel_scroll_num-1){
		panelScroll.scrollTo($boundaryX,0);
		hide_panel_next();
	}
	
}

function panel_scroll_prev() {

	if(current_scroll > 0){
		
		current_scroll--;
		show_panel_next();
		panelScroll.scrollTo(-window.innerWidth * current_scroll,0);

	}
	
	if(current_scroll == 0){
		hide_panel_prev();
	}

}
	
function openPageDetail (e) {
	check_section_pic();
	$detail = $(this);
	
	popEnabled = false;
	
	e.preventDefault();
	
	
	
	$(this).addClass('active');
	
	$('#section_scroller').addClass('disabled');
	
	clearTimeout(timeoutPrev);
	clearTimeout(timeoutMenu);
	clearTimeout(timeoutNext);
	
	
	
	$detail.trigger('mouseleave');
	$('.section_veil',$detail).addClass('no_opacity');
	
	$('.panel_separator > *',$detail).addClass('no_height');
	$('.panel_separator > *',$detail.prev()).addClass('no_height');
	
	$('.info', $detail).addClass('has_transition_1000').addClass('top_double');
	
	hide_panel_prev();
	setTimeout(hide_panel_menu,100);
	setTimeout(hide_panel_next,200);

	 $('#section_animation').append($detail.clone());
	 $('#section_animation .section_panel .info, #section_animation .section_panel .details, #section_animation .section_panel .panel_separator, #section_animation .section_panel .loading_cover ').remove();
	
	
	 $('#section_animation .section_panel .section_pic').addClass('no_transition').addClass('no_visibility');
	 $('#section_animation .section_panel .section_pic').css('left',$detail.offset().left+'px');
     
	 
	 setTimeout(function(){
		 $('#section_animation .section_panel .section_pic').removeClass('no_visibility');
		 $('#section_animation .section_panel .section_pic').css('transform','none').animate({
			 width:'100%',
			 left:0
		 },1500,'easeInOutQuint')
		 $('#section_animation .section_panel .section_pic img').animate({
			 left:0
		 },1500,'easeInOutQuint')
	 },300);
	 
	
	 
	 
	 /*$('#section_animation .section_panel .section_pic,#section_animation .section_panel .section_pic_scroller').addClass('no_transition').addClass('no_visibility');
	 $('#section_animation .section_panel .section_pic img').addClass('no_transition');
	 $('#section_animation .section_panel .section_pic').css($$transform[0],'translateX('+$detail.offset().left+'px)');
	 $('#section_animation .section_panel .section_pic_scroller').css($$transform[0],'translateX('+(-windowWidth+(windowWidth/3))+'px)');
	 
	 
	 $('#section_animation .section_panel .section_pic img').css({
		
		 'transform':'translateX('+$('#section_animation .section_panel .section_pic img').css('left')+')',
		 'left':'0',
	 })

	

	 setTimeout(function(){
		 $('#section_animation .section_panel .section_pic,#section_animation .section_panel .section_pic_scroller,#section_animation .section_panel .section_pic_scroller img').removeClass('no_visibility').removeClass('no_transition').removeClass('has_transition_800').addClass('has_transition_1500_inout')
		// $('#section_animation .section_panel .section_pic, #section_animation .section_panel .section_pic_scroller,#section_animation .section_pic_scroller img ').css($$transform[0],'translateX(0)');
		

	 },300);*/

	  
	 load_panel_detail(e,$detail);
	 
}

function closePageDetail() {
	
	$openedPanel = $('#section_scroller_container .section_panel.active');
	$openedPanel.removeClass('active');
	
	popEnabled = false;
	
	clearTimeout(wait_for_clearing);
	clearTimeout(timeoutFocus);
	clearTimeout(timeoutPay1);
	clearTimeout(timeoutPay2);
	clearTimeout(exitPanelTimeout);
	
	$('.iScrollVerticalScrollbar').remove();


	
	if(current == "photogallery_detail"){
		if($('.pic_big.active').index() != 0) {
		$('.pic_big').addClass('no_transition');
	    $('.pic_big.active').css('z-index',1).siblings().css('z-index',2);
	    $(".pic_big.active").nextAll().css("transform","translate3d("+windowWidth+"px, 0, 0)");
	    $(".pic_big.active").prevAll().css("transform","translate3d("+(-windowWidth)+"px, 0, 0)");
	    setTimeout(function(){
		    $('.pic_big').removeClass('no_transition');
		    $(".gallery_slider .pic_big.active").css("transform","translate3d("+windowWidth/3+"px, 0, 0)").removeClass("active");
		    $(".gallery_slider .pic_big:eq(0)").addClass("active");
	        $("#fullscreen_gallery .pic_big.active").css("transform","translate3d(0, 0, 0)");
	           
	    },20);
	    
	    $('#gallery_panel').addClass('hidden');
		$('.button_right .circle').addClass('hidden_by_scaling_full');	
		$('.button_left .circle').addClass('hidden_by_scaling_full');	
		$('.button_down .back_circle').addClass('hidden_by_scaling_full');	

	    
	    
	    setTimeout(function(){
	    	$('#fullscreen_gallery').addClass('no_opacity');
	
	    },1300)
	    
	    setTimeout(function(){
	    	start_closing();
	    },600);
		} else {
		    $('#gallery_panel').addClass('hidden');
		   $('.button_right .circle').addClass('hidden_by_scaling_full');	
			$('.button_left .circle').addClass('hidden_by_scaling_full');	
			$('.button_down .back_circle').addClass('hidden_by_scaling_full');	
		    setTimeout(function(){	
			$('#fullscreen_gallery').addClass('no_opacity');
		    	start_closing();
		    },150)

		}
		hide_side_menu();
		
		

	} else {
	
		$headerPic.style[$$transform[0]] = 'translateY(0px)';

		
		gallery_focus = false;
		
		if(pageY == 0 || isHandheld){
			$(window).scrollTop(0);
				$('.part_1 img').addClass('has_transition_1500').addClass('hidden');
				$('.part_2 img').addClass('has_transition_1500').addClass('hidden');
				

				
				
				
				setTimeout(function(){
					start_closing();			

				},300)
				
				setTimeout(function(){
					$('#head_content').addClass('exit');
					if(scrollType == "iScroll"){
					myScroll.destroy();
					}
					$('.iScrollVerticalScrollbar').remove();
				},500);
		} else if(pageY <= windowHeight) {
			if(scrollType == "iScroll"){
				 myScroll.scrollTo(0,0);
			 } else {
				 $('body,html').animate({
					 scrollTop:0
				 },1500,'easeOutQuint');
			 }
			hide_side_menu();
	
			setTimeout(function(){
				$('.part_1 img').addClass('has_transition_1500').addClass('hidden');
				$('.part_2 img').addClass('has_transition_1500').addClass('hidden');
				

				},200);
				
				
				
				setTimeout(function(){
					start_closing();			

				},500)
				
				setTimeout(function(){
					$('#head_content').addClass('exit');
					myScroll.destroy();
					$('.iScrollVerticalScrollbar').remove();
				},700);
		} else if (pageY > windowHeight) {
			hide_side_menu();
			if(scrollType == "iScroll"){
			myScroll.scrollBy(0, 300);
			$('#section_animation').addClass('bottomOut').css('top',pageY-300);
			setTimeout(function(){
				$('#section_animation').removeClass('no_transition');
	
				$('#section_animation').addClass('bottomIn');
				
			},200);
			} else {
				$('#section_animation').addClass('bottomOut').css('top',pageY);
				setTimeout(function(){
					$('#section_animation').removeClass('no_transition');
					$('#section_animation').addClass('bottomIn');
				},20);
			}
			
			setTimeout(function(){
			$('.part_1 img').addClass('has_transition_1500').addClass('hidden');
			$('.part_2 img').addClass('has_transition_1500').addClass('hidden');
			

			},500);
			
			
			
			setTimeout(function(){
				start_closing();			

			},800)
			
			setTimeout(function(){
				$('#head_content').addClass('exit');

			},1000);
			
			
			
		
			
			
			setTimeout(function(){
				
				$('#section_animation').addClass('no_transition').removeClass('bottomOut').removeClass('bottomIn').css('top','0');
				$('#main_scroller').addClass('no_transition');
				if(scrollType == "iScroll"){
					myScroll.scrollTo(0, 0);
					myScroll.destroy();
					$('.iScrollVerticalScrollbar').remove();
				} else {
					$('body,html').scrollTop(0);
				}
				
				
				
	
			},1500)
			
			exitPanelTimeout = setTimeout(function(){
				
				$('#main_scroller').removeClass('no_transition');
				
				
	
			},1700);
		} 
	
	}
	
	function start_closing(){
		
		
		$('#section_scroller').show();
			
			detail_loaded = false;
			setTimeout(function(){
				$('#section_animation .section_panel .section_pic').animate({
					 width:'33.333vw',
					 left:$openedPanel.offset().left
				 },1500,'easeInOutQuint')
				 $('#section_animation .section_panel .section_pic img').animate({
					 left:$('.section_pic img',$openedPanel).css('left')
				 },1500,'easeInOutQuint',function(){
					 $('#section_animation .section_panel').remove();
					 $('.section_veil',$openedPanel).removeClass('no_opacity');
					 $('#head_content').remove();
					 $('#section_content').empty();
						$('#section_scroller').removeClass('disabled');
						popEnabled = true;

				 })
			},300);
			
			current = current.split('_detail')[0];
			$('body').attr('id',current);
			
			
			
			if(!panels_initialized){
				setTimeout(panels_ready_desktop,600);
			} else {

				
			setTimeout(function(){
				$('#main_veil').addClass('grey');
				panelDetail_ready = true;
				setTimeout(function(){	
				$('.info',$openedPanel).removeClass('top_double');
				},300);
					
				$('.panel_separator > *',$openedPanel).removeClass('no_height');
				$('.panel_separator > *',$openedPanel.prev()).removeClass('no_height');


				
				if(pageX < -$boundaryX && pageX > 0 ){ 
					show_panel_prev();
					setTimeout(show_panel_menu,100);
					setTimeout(show_panel_next,200);
					
				} else if(pageX < -$boundaryX){
					show_panel_menu();
					setTimeout(show_panel_next,100);
					
				} else if(pageX > 0){
					show_panel_prev();
					setTimeout(show_panel_menu,100);
				}
				
				
			},1500)
		}

	}
	
}

function closePanels(){
	hide_panel_prev();
	setTimeout(hide_panel_menu,100);
	setTimeout(hide_panel_next,200);	
	
	clearTimeout(timeoutPrev);
	clearTimeout(timeoutMenu);
	clearTimeout(timeoutNext);
	
	$('.section_panel').each(function(n) {
	    setTimeout(function() {
	        $('.section_panel:eq(' + n + ') .loading_cover').removeClass('removed');
	    }, 100 * n);
	});	
}


function show_panel_menu() {
	$('#panel_controls .menu').removeClass('hidden');
    $('.menu_panel_controller .lines > hr').each(function(i) {
        setTimeout(function() {
            $('.menu_panel_controller .lines > hr:eq(' + i + ')').removeClass('no_width')
        }, 100 * i);
    });
}

function hide_panel_menu() {
	$('#panel_controls .menu').addClass('hidden');

}


function show_panel_next() {
	next_visible = true;

	$('#panel_controls .next').removeClass('hidden');
	setTimeout(function(){$('#panel_controls .next .panel_circle').removeClass('hidden_by_scaling_full')},200);
	$('#panel_controls .next .panel_circle img').addClass('animating');

	
}

function hide_panel_next() {
	next_visible = false;

	//$('#panel_controls .next .panel_circle').addClass('hidden_by_scaling_full');
	$('#panel_controls .next').addClass('hidden');
	$('#panel_controls .next .panel_circle img').removeClass('animating');


}


function show_panel_prev() {
	prev_visible = true;

	$('#panel_controls .prev').removeClass('hidden');
	setTimeout(function(){$('#panel_controls .prev .panel_circle').removeClass('hidden_by_scaling_full')},200);
	$('#panel_controls .prev .panel_circle img').addClass('animating');


}

function hide_panel_prev() {
	prev_visible = false;

	$('#panel_controls .prev').addClass('hidden');
	//$('#panel_controls .prev .panel_circle').addClass('hidden_by_scaling_full');
	$('#panel_controls .prev .panel_circle img').removeClass('animating');

}

















function panels_ready_mobile () {
	
	
	controller = "panel";
	
	$('#main_veil').addClass('grey').hide();


	$('#panel_controls .menu .menu_panel_controller').bind('click',show_menu);

    $('#ca_home').bind('click', function() {
        closePanels();
        var linkHome = $('.linkHome').val();
        console.log(linkHome);
        navigateTimeout = setTimeout(function() {
            base_load(linkHome + '/home', true);
            current_scroll = 0;
        }, 800);
    });
	
	panel_scroll_num = Math.ceil($('#section_scroller .section_panel').length / 3)+40;
	$scrollerWidth = Math.ceil((windowWidth-40)*$('#section_scroller .section_panel').length);
	$('#section_scroller_container').addClass('no_transition').width($scrollerWidth);
	$boundaryX = -($scrollerWidth - windowWidth); /*** confine destro scroll **/
		
	
	
	introTextScroll = new IScroll('.panel_section_text_container ', {
		 mouseWheel:false,
		    scrollbars: false,
		    probeType: 2,
		    disablePointer: false,
		    scrollX:false,
			scrollY:true,
			click:true,
			useTransition:true,
			 momentum:false
	});
	
	panelScroll = new IScroll('#section_scroller', {
			    mouseWheel:false,
			    scrollbars: false,
			    probeType: 2,
			    disablePointer: false,
			    scrollX:true,
				scrollY:false,
				click:true,
				useTransition:true,
				 momentum:false,
				 snap:true
			});
			
	panelScroll.on('scrollStart', function(){
			$panel = $('.section_panel');
			$('.section_panel').trigger('mouseleave');
	});
	panelScroll.on('scrollEnd', function(){
		
		$('.section_panel').trigger('mouseenter');
	});
	
	
	$('.swipe_ad').bind('click',function(){
		panelScroll.goToPage(1,0);
	})
	
	

	$('.section_pic img').imagesLoaded(function(){
			check_section_pic ();
	});
	
	
	
	$('.section_panel:eq(0) img, .section_panel:eq(1) img ').imagesLoaded(function(){
		$('body').removeClass('preload');

		check_section_pic ();
		panels_resize ();
		panelScroll.refresh();
		introTextScroll.refresh();
		
		
		
		$('.section_panel').each(function(n){
			setTimeout(function(){			
				$('.section_panel:eq('+n+') .loading_cover').addClass('removed');
				$('.section_panel:eq('+n+') .section_pic').removeClass('scaled_full');
			},100*n)
			setTimeout(function(){			
				$('.section_panel:eq('+n+') .info').removeClass('top_double');
			},1000+(100*n));
			
			
			setTimeout(function(){
				$('.section_panel.intro .circle_button.right .back').removeClass('hidden_by_scaling_full');	
			},1300)
			
			setTimeout(function(){
				$('.section_panel.intro .circle_button.right .arrow').removeClass('hidden');	
				$('.section_panel.intro .swipe_ad p').removeClass('top_hidden');	
			},1400)
	
			
		})
	})
	
	setTimeout(function(){
		$('.panel_section_title').removeClass('top_hidden');
	},1300)
	setTimeout(function(){
		$('.panel_section_subtitle').removeClass('top_hidden');
		$('#section_scroller').removeClass('disabled');
		popEnabled = true;
		console.log('popEnabled')
	},1400)
	setTimeout(function(){
		$('.panel_section_text_container').removeClass('top_translated');
	},1000)
	
	setTimeout(function(){
		$('.section_panel .section_pic').removeClass('has_transition_1000_inout_quint').addClass('has_transition_800');
		$('#section_scroller .section_panel:not(:first-child)').bind('mouseenter',panel_hover_mobile).bind('mouseleave',panel_out_mobile).bind('click',openContext);
		
	},1200);
	
	$('img[data-src]').each(function(){
			$path = $(this).attr('data-src');
			$(this).attr('src',$path);
		});
		
		
	
}



function panel_hover_mobile () {
	

	$('.section_pic').addClass('scaled');
	$('.details').removeClass('no_opacity');
	
	setTimeout(function(){$('.info_details .panel_detail:eq(0) .det_num',$('.section_panel')).removeClass('top_hidden');},200);
	setTimeout(function(){$('.info_details .panel_detail:eq(0) .det_black',$('.section_panel')).removeClass('top_hidden');},220);
	setTimeout(function(){$('.info_details .panel_detail:eq(0) .det_white',$('.section_panel')).removeClass('top_hidden');},240);
	
	setTimeout(function(){$('.info_details .panel_detail:eq(1) .det_num',$('.section_panel')).removeClass('top_hidden');},300);
	setTimeout(function(){$('.info_details .panel_detail:eq(1) .det_black',$('.section_panel')).removeClass('top_hidden');},320);
	setTimeout(function(){$('.info_details .panel_detail:eq(1) .det_white',$('.section_panel')).removeClass('top_hidden');},340);
	
	setTimeout(function(){$('.info_details .panel_detail:eq(2) .det_num',$('.section_panel')).removeClass('top_hidden');},400);
	setTimeout(function(){$('.info_details .panel_detail:eq(2) .det_black',$('.section_panel')).removeClass('top_hidden');},420);
	setTimeout(function(){$('.info_details .panel_detail:eq(2) .det_white',$('.section_panel')).removeClass('top_hidden');},440);

}

function panel_out_mobile () {
	
	$('.section_pic').removeClass('scaled');
	$('.details').addClass('no_opacity');
	
	$('.info_details .panel_detail .det_num, .info_details .panel_detail .det_black, .info_details .panel_detail .det_white').addClass('top_hidden');
	
}


function openContext(e){
	e.preventDefault();

	panelScroll.disable();
	
	$detail = $(this);
	$detail.trigger('mouseleave');
	  $('#main_veil').addClass('hidden').show().removeClass('grey');
	  
	  setTimeout(function(){
		  $('#main_veil').removeClass('hidden');
	  },100);
	
	$('.info', $detail).addClass('has_transition_1000').addClass('top_double');

	
	$('.section_veil',$detail).addClass('no_opacity');

	setTimeout(function(){
		window.location.href = $detail.attr('href');
	},800);
	
	
	/*$(this).addClass('active');
	$detail = $(this)
	e.preventDefault();

	$('#main_veil').show().removeClass('hidden');
	
	$detail.trigger('mouseleave');
	
	$('.section_veil',$detail).addClass('no_opacity');

	
	$('.info', $detail).addClass('has_transition_1000').addClass('top_double');
	
	$('.section_pic',$detail).animate({
		 width:'0'
	 },1500,'easeInOutQuint')*/
}