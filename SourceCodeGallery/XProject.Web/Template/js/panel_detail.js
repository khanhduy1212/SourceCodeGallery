var timeoutPay1;
var timeoutPay2;
var exitPanelTimeout;

function panels_detail_ready_desktop() {

	
	windowWidth = $(window).width();
	windowHeight = $(window).height();

	
	$('#main_veil').removeClass('grey').hide();
	detail_loaded = true;
	
	side_menu = false;
	
	controller = "panel_detail";
	
	$('body').attr('id',current);
	
	//$('#content').attr('data-current',current);
	//$('#content').attr('data-controller',controller);
	
	if(typeof myScroll !== 'undefined'){
		myScroll.destroy();
		$('.iScrollVerticalScrollbar ').remove();
		initialize_vertical_scroll();

	} 
	
	reset_menu();
	
	popEnabled = false;
	
	$('#menu_center_side,#player_side').removeClass('hidden');
	
	$('#section_scroller').addClass('disabled');
	
	pageY = 0; /*non rimuovere */
	
	gallery_focus = true;
	
	
	if(first_run){
		first_run = false;
		 var stateObj = { controller: controller, current:current, panelIndex:$('.section_panel.active').index(),url:window.location.href };
		 history.replaceState(stateObj, "", window.location);
	}
	
	if(!panelDetail_ready){
		
		pageX = 0; /* non rimuovere */
		panels_initialized = false; 
		panelDetail_ready = false; /* è true solo se si sta nel controller "panel" */
		panel_scroll_num = Math.ceil($('#section_scroller .section_panel').length / 3);
 
		current_scroll = Math.floor($('#section_scroller .section_panel.active').index() / 3);
		
		
	
		$('#section_animation .section_panel .section_pic img').imagesLoaded(function () {
			

			check_section_pic ();
			$('body').removeClass('preload');
		 $('#section_scroller .section_panel .section_pic').removeClass('has_transition_1000_inout_quint').addClass('has_transition_800');
		 $('#section_scroller .section_panel.active .section_pic .section_veil').addClass('no_opacity');
		 
		 
		 $('#section_animation .section_panel .section_pic').css('transform','none').animate({
			 width:'100%',
			 left:0
		 },1500,'easeInOutQuint')
		 $('#section_animation .section_panel .section_pic img').animate({
			 left:0
		 },1500,'easeInOutQuint',function(){
			 $('.section_panel').each(function(n){
					setTimeout(function(){
						
						$('.section_panel:eq('+n+') .loading_cover').addClass('removed');
						$('.section_panel:eq('+n+') .section_pic').removeClass('scaled_full');


					},100*n)
					
				})
				panel_scroll_num = Math.ceil($('#section_scroller .section_panel').length / 3);
				$('.section_pic').removeClass('first_run');
				

		 });
	
	  
	 setTimeout(function(){
			$('.back_2').removeClass('no_width')

		},600);
		setTimeout(function(){
			$('.back_1').removeClass('no_width');

		},800);
		
		
		setTimeout(function(){
			$('.head_text._1').removeClass('top_hidden');	
		

		},1200);
		
		timeoutPay1 = setTimeout(function(){
			$('.part_1 img').removeClass('hidden');

		},1300);
		
		timeoutPay2 = setTimeout(function(){
			$('.part_2 img').removeClass('hidden');

		},1350);
		
		setTimeout(function(){
			$('.head_text._2').removeClass('top_hidden');
			$('#section_scroller').hide();
			 popEnabled = true;
			 loadEnabled = true;
		},1700);
		
		setTimeout(function(){
			$('.panel_name .separator:eq(0)').removeClass('hidden');
		},1800);
		
		setTimeout(function(){
			$('.panel_name .dark').removeClass('top_double');
		},1850)
		
		setTimeout(function(){
			$('.panel_name .white').removeClass('top_double');
		},1950)
		
			setTimeout(function(){
				$scrollerWidth = $('#section_scroller .section_panel').width()*$('#section_scroller .section_panel').length;
				$boundaryX = -($scrollerWidth - windowWidth);
				if(current_scroll < panel_scroll_num-1){
					pageX = (-window.innerWidth * current_scroll) ;
					$('#section_scroller_container').addClass('no_transition').css('transform','translateX('+ pageX + 'px)' );
				}
				
				
				if(current_scroll == panel_scroll_num-1){
					pageX = -$boundaryX;
					$('#section_scroller_container').addClass('no_transition').css('transform','translateX('+ (-pageX) + 'px)' );

				}
				$('.panel_name .separator:eq(1)').removeClass('hidden');

			},2050);
		
		setTimeout(function(){
			$('.section_name').removeClass('top_hidden');
			$('#section_scroller_container').removeClass('no_transition');	
		},2200);
		setTimeout(function(){
			$('.section_date').removeClass('top_hidden');
			$('.section_top').removeClass('top_hidden');
		},2300);
		})
	 
	}  else {
	
		check_section_pic ();
	
	
	$('.back_2').removeClass('no_width')
	setTimeout(function(){
		$('.back_1').removeClass('no_width')

	},200);
	
	
	setTimeout(function(){
		$('.head_text._1').removeClass('top_hidden');	

	},600);
	
	timeoutPay1 = setTimeout(function(){
		$('.part_1 img').removeClass('hidden');
	},700);
	
	timeoutPay2 = setTimeout(function(){
		$('.part_2 img').removeClass('hidden');
		

	},750);
	
	setTimeout(function(){
		$('.head_text._2').removeClass('top_hidden');
		$('#section_scroller').hide();
		 popEnabled = true;
		 loadEnabled = true;

	},1100);
	
	setTimeout(function(){
		$('.panel_name .separator:eq(0)').removeClass('hidden');
		
	},1200);
	
	setTimeout(function(){
		$('.panel_name .dark').removeClass('top_double');
	},1250)
	
	setTimeout(function(){
		$('.panel_name .white').removeClass('top_double');
	},1350)
	
		setTimeout(function(){
			$('.panel_name .separator:eq(1)').removeClass('hidden');
		},1450);
	
		setTimeout(function(){
			$('.section_name').removeClass('top_hidden');
		},1600);
		setTimeout(function(){
			$('.section_date').removeClass('top_hidden');
			$('.section_top').removeClass('top_hidden');

		},1700);
	}
	
	if($('#section_scroller .section_panel.active').index() > 1 && $('#section_scroller .section_panel.active').index()  < $('#section_scroller .section_panel').length -1){
		$prevEnabled = true;
		$nextEnabled = true;
		
	} else if($('#section_scroller .section_panel.active').index() == $('#section_scroller .section_panel').length -1){
		$prevEnabled = true;
		$nextEnabled = false;
	} else if($('#section_scroller .section_panel.active').index() == 1){
		$prevEnabled = false;
		$nextEnabled = true;
	}
	
	$('#panelPrev .panel_text').text($('#section_scroller .section_panel.active').prev().find('.section_panel_title').text());
	$('#panelNext .panel_text').text($('#section_scroller .section_panel.active').next().find('.section_panel_title').text());
	$('#panelPrev').attr('href',$('#section_scroller .section_panel.active').prev().attr('href'));
	$('#panelNext').attr('href',$('#section_scroller .section_panel.active').next().attr('href'));
	
	setTimeout(function(){
		if(scrollType == "iScroll"){
		myScroll.refresh();
		window[current+'_offsets']();
		}
	},100);

}
	

var waitForScroller;

function panels_detail_resize () {
	
	clearTimeout(waitForScroller);
	$('#section_scroller').show();
	if(!isHandheld){check_section_pic();}
	if(mobileCheck){
		check_section_mobile();
	}
	
	Math.ceil($scrollerWidth = $('.section_panel').width()*$('#section_scroller .section_panel').length);
	$('#section_scroller_container').width($scrollerWidth);
	$boundaryX = -($scrollerWidth - windowWidth);
	waitForScroller = setTimeout(function(){
		if(!isHandheld){$('#section_scroller').hide();}
	},100);
	
		
	if(has_gallery && !isHandheld){
		gallery_slider_setup();
	}

}

function showPageController () {
    $('#panelDetailController').css('pointer-events', 'all');

    if ($prevEnabled) {
        $('#panelPrev').removeClass('top_hidden');
    }

    setTimeout(function () {
        $('#backToPanels').removeClass('top_hidden');
    }, 50)

    if ($nextEnabled) {
        setTimeout(function () {
            $('#panelNext').removeClass('top_hidden');

        }, 100);
    }
	
	if(mobileCheck ){
		$('#panelMobileController').css('pointer-events','all').removeClass('top_hidden');
	
		setTimeout(function(){$('#mobileBack').removeClass('top_hidden');},100);

		
		if($prevEnabled){
			setTimeout(function(){
				$('#mobilePrev').removeClass('top_hidden');
			},150)
		}
			
		
		setTimeout(function(){
			$('.mobileCtrlText').removeClass('top_hidden');

		},200)
		
		
		if($nextEnabled){
			setTimeout(function(){
				$('#mobileNext').removeClass('top_hidden');
				},250);
		}
	
	}
}

function hidePageController () {
    $('#panelDetailController').css('pointer-events', 'none');

    setTimeout(function() {
        $('#panelPrev').addClass('top_hidden');
    }, 100);


    setTimeout(function() {
        $('#backToPanels').addClass('top_hidden');
    }, 150);


    setTimeout(function () {
        $('#panelNext').addClass('top_hidden');

    }, 200);

	
	if(mobileCheck ){
		$('#panelMobileController').css('pointer-events','none');
		
		setTimeout(function(){
		$('#mobilePrev').addClass('top_hidden');
		},50);
		
		
				$('#mobileBack').addClass('top_hidden');
		
		
		setTimeout(function(){
			$('.mobileCtrlText').addClass('top_hidden');

		},100)
			setTimeout(function(){
					$('#mobileNext').addClass('top_hidden');
				},150);
		
		setTimeout(function(){
			$('#panelMobileController').addClass('top_hidden')
		},200)
	}
	
}



function panels_detail_ready_mobile() {
	

	windowWidth = $(window).width();
	windowHeight = $(window).height();

	detail_loaded = true;
	controller = "panel_detail";
	$('#section_scroller').addClass('disabled');
	
	pageY = 0; /*non rimuovere */
	
	if($('#section_scroller .section_panel.active').index() > 1 && $('#section_scroller .section_panel.active').index()  < $('#section_scroller .section_panel').length -1){
		$prevEnabled = true;
		$nextEnabled = true;
		
	} else if($('#section_scroller .section_panel.active').index() == $('#section_scroller .section_panel').length -1){
		$prevEnabled = true;
		$nextEnabled = false;
	} else if($('#section_scroller .section_panel.active').index() == 1){
		$prevEnabled = false;
		$nextEnabled = true;
	}
	
	
	$('#mobilePrev').attr('href', $('#section_scroller .section_panel.active').prev().attr('href'));
	var lbNext = $('#section_scroller .section_panel.active').next().attr('href');
	if (lbNext=="/home") {
	    lbNext = $('#hdNameMenu').val();
	}
	$('#mobileNext').attr('href', lbNext);
	
	
	if($('.paragraph.first .body_text').html().indexOf('<br') != -1){
		$('.paragraph.first .body_text').each(function(){
			$(this).shorten({
				showChars:$(this).html().indexOf('<br'),
				ellipsesText: "",
				moreText: '<span class="langMore">more</span>',
				lessText: '<span class="langLess">less</span>'
			});
		})
	
	}
	
	$('body').attr('id',current);
	
		$('#section_animation .section_panel .section_pic img').imagesLoaded(function(){
			
			viewport_height = window.innerHeight/1.5;
			$('#section_animation,#head_content,#section_scroller').height(viewport_height);

			check_section_mobile ();

			$('body').removeClass('preload');
			$('.section_pic').removeClass('first_run');
			$('#main_veil').addClass('hidden');
			setTimeout(function(){
				$('#section_animation .info').removeClass('top_translated');
			},300);
			
			setTimeout(function(){
			$('.paragraph.first .big_title').removeClass('top_hidden');
			$('.paragraph.first .body_text').removeClass('top_translated');

			},1000);
			
			setTimeout(function(){
				$('.paragraph.first .big_subtitle').removeClass('top_hidden');
			},1200);
			
		});

}
	

function check_section_mobile () {
	viewport_width = $(window).width();
	viewport_height = $('#section_animation').height();	
    screen_ratio = viewport_width / viewport_height;
    pic_ratio = 2400 / 1350;

    
    
  
    
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

