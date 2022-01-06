function contacts_ready_desktop() {
	v_first_paragraph = false;
	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
		
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	v_emails = false;
	
	if(typeof myScroll !== 'undefined'){
		myScroll.destroy();
		$('.iScrollVerticalScrollbar ').remove();
		initialize_vertical_scroll();
	} 
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			contacts_offsets();
		})
	} else {
		has_gallery = false;
	}
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
	    if (scrollType == "iScroll") {
	        
			myScroll.scrollTo(0,-(o_gallery + windowHeight));
			pageY = o_gallery + windowHeight;
			common_scroll();
		}
	});
	
	contacts_offsets ();

	
	$headerPic = $('#section_animation  img')[0];
	
	
	check_section_pic ();
	
	if(!mobileCheck){$('#main_veil').removeClass('grey').hide();}
	
	controller = "page";
	current = "contacts"
	
	$('body').attr('id',current);
	
	reset_menu();
		
	pageY = 0; /*non rimuovere */
	
	gallery_focus = true;
	
	
	if(first_run){
		first_run = false;
		 var stateObj = { controller: controller, current:current, url:window.location.href };
		 history.replaceState(stateObj, "", window.location);
	}
	
	
		pageX = 0; /* non rimuovere */
		
		
	
		$('#section_animation .section_panel .section_pic img').imagesLoaded(function(){
			$('body').removeClass('preload');
		 $('#section_scroller .section_panel .section_pic').removeClass('has_transition_1000_inout_quint').addClass('has_transition_800');
		 $('#section_scroller .section_panel.active .section_pic .section_veil').addClass('no_opacity');
		 
		 $('#section_animation .section_panel .section_pic').css('transform','none').animate({
			 width:'100%',
			 left:0
		 },1500,'easeInOutQuint')
		 $('#section_animation .section_panel .section_pic img').animate({
		     left: (-($('#section_animation .section_panel .section_pic img').width() - viewport_width) / 2)
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
			popEnabled = true;
			loadEnabled = true;
			
		},1200);
		
		setTimeout(function(){
			$('.part_1 img').removeClass('hidden');

		},1300);
		
		setTimeout(function(){
			$('.part_2 img').removeClass('hidden');

		},1350);
		
		setTimeout(function(){
			$('.head_text._2').removeClass('top_hidden');

		},1700);
		
		setTimeout(function(){
			$('.panel_name .separator:eq(0)').removeClass('hidden');
		},1800);

		    setTimeout(function() {
		        $('.panel_name .dark').removeClass('top_double');
		    }, 1850);

		    setTimeout(function() {
		        $('.panel_name .white').removeClass('top_double');
		    }, 1950);
		
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
		
		/** SECURITY ***/
		setTimeout(function(){
			if(scrollType == "iScroll"){
				myScroll.refresh();
			}
			contacts_offsets ();

		},3000)
		
		/***********/
	 
}

function contacts_load() {
	contacts_offsets ();

}

function contacts_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	if(!v_emails && pageY > o_emails - windowHeight + scroll_threshold) {
		v_emails = true;
		
		
			$('.emails .diagonal_line').removeClass('hidden');
		
		
		setTimeout(function(){
			$('.block_title .desc_title').removeClass('top_hidden');
		},500);
		
		$('.emails .closing span').each(function(e){
			setTimeout(function(){
				$('.emails .closing span:eq('+e+')').removeClass('top_double');
			},100*e)
		})
		
	}
	
}

function contacts_scroll_mobile() {
	contacts_scroll_desktop();
}

function contacts_resize () {
	check_section_pic();
	contacts_offsets ();
	
}

function contacts_offsets () {
	animatedRow_offset = [];
	
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	
	o_emails = $('.row.emails').position().top;
	
	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').position().top);
	 });
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
    }
    if ($('#meteo_box').length>0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}


function contacts_ready_mobile () {
	contacts_ready_desktop();
	
	windowWidth = $(window).width();
	windowHeight = $(window).height();

	detail_loaded = true;
	controller = "page";
	current = "contacts"
	$('#section_scroller').addClass('disabled');
	
	pageY = 0; /*non rimuovere */
	
	$('body').attr('id',current);
	
	$('#section_animation .section_panel .section_pic img').imagesLoaded(function () {
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

function dining_ready_desktop() {
	panels_ready_desktop();
}

function  dining_load() {
	
}

function  dining_scroll_desktop() {
}

function  dining_scroll_mobile() {
	
}

function  dining_resize () {
	panels_resize();
}

function  dining_detail_ready_desktop() {
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	v_first_paragraph = false;	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		$('.page_form .form_submit').bind('click',mailer);
		hideReqDatepicker = null;
		
		if($('.req_date').length != 0) {
			reqDatepicker = $('.req_date').glDatePicker(
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
					    	reqDatepicker.render();
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
					    	hideReqDatepicker = setTimeout(function(){
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
					    	
					    	if(lang == "en"){
						    	arrivalDate = date;
						    	selected = mm+'/'+dd+'/'+yyyy;
						    	el.children('input').val(selected);
					    	} else {
					    		arrivalDate = date;
						    	selected = dd+'/'+mm+'/'+yyyy;
						    	el.children('input').val(selected);
					    	}
					    },
					}
				).glDatePicker(true);
		}

	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			dining_detail_offsets();
		})
	} else {
		has_gallery = false;
		dining_detail_offsets ()
	}

	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];

	
	panels_detail_ready_desktop();
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		dining_detail_offsets ()

	},3000)
	
	/***********/
}

function  dining_detail_load() {
	dining_detail_offsets ()

}

function dining_detail_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	for(i=0;i<animatedRow_length;i++) {
    	if(!animatedRow_visible[i] && pageY >  animatedRow_offset[i] - windowHeight + scroll_threshold){
    		animatedRow_visible[i] = true;
    		dining_animate_row(i);
    	}	
	};
}

function  dining_detail_scroll_mobile() {
	dining_detail_scroll_desktop();
}

function  dining_detail_resize () {
	panels_detail_resize();
	dining_detail_offsets ()
}

function dining_detail_offsets () {
	animatedRow_offset = [];
	
	$('.row').each(function(){
		$(this).find('.grey_box,.block_25.double').height($(this).find('.pic_62 img').height())
	})
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;

	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').offset().top);
	 });
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
    }
    if ($('#meteo_box').length>0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
	
}

function dining_animate_row (i){
	$targetRow = $('.row.animated:eq('+i+')');
	$oldTarget = $('.row.animated:eq('+(i-1)+')');
	
	
	$('.cover,.content',$targetRow).removeClass('hidden');
	
	setTimeout(function(){
		$('.top_translated._1, .top_translated_full._1',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},100);
	
	setTimeout(function(){
		$('.top_translated._2, .top_translated_full._2',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},200);
	
	setTimeout(function(){
		$('.top_translated._3,.top_translated_full._3',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},300);
	
	setTimeout(function(){
		$('.top_translated._4,.top_translated_full._4',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},400);
	
	
	setTimeout(function(){
		$('.block_title .desc_text',$targetRow).removeClass('top_hidden');
	},700);
	setTimeout(function(){
		$('.block_title .desc_title',$targetRow).removeClass('top_hidden');
	},800);
	
	setTimeout(function(){
		$('.circle_button .back',$targetRow).removeClass('hidden_by_scaling_full');
	},900);
	setTimeout(function(){
		$('.circle_button .arrow',$targetRow).removeClass('hidden');
	},1100);
	
	setTimeout(function(){
		$('.diagonal_line',$targetRow).removeClass('hidden');
	},900);
	
	if(i > 0){
	$('.top_translated._1, .top_translated_full._1',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.top_translated._2, .top_translated_full._2',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.top_translated._3, .top_translated_full._3',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.top_translated._4,.top_translated_full._4',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.block_title .desc_text',$oldTarget).removeClass('top_hidden');
	$('.block_title .desc_title',$oldTarget).removeClass('top_hidden');
	$('.cover,.content, .diagonal_line',$oldTarget).removeClass('hidden');
	$('.circle_button .back',$oldTarget).removeClass('hidden_by_scaling_full');
	$('.circle_button .arrow',$oldTarget).removeClass('hidden');


	}
	




	
	$('img',$targetRow).removeClass('top_double');
	$('img',$oldTarget).removeClass('top_double');
	

}

function dining_ready_mobile () {
	panels_ready_mobile();
	
}

function dining_detail_ready_mobile () {
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	v_first_paragraph = false;	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		$('.page_form .form_submit').bind('click',mailer);
		
		hideReqDatepicker = null;
		
		if($('.req_date').length != 0) {
			reqDatepicker = $('.req_date').glDatePicker(
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
					    	reqDatepicker.render();
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
					    	hideReqDatepicker = setTimeout(function(){
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
					    	
					    	if(lang == "en"){
						    	arrivalDate = date;
						    	selected = mm+'/'+dd+'/'+yyyy;
						    	el.children('input').val(selected);
					    	} else {
					    		arrivalDate = date;
						    	selected = dd+'/'+mm+'/'+yyyy;
						    	el.children('input').val(selected);
					    	}
					    },
					}
				).glDatePicker(true);
		}

	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			dining_detail_offsets();
		})
	} else {
		has_gallery = false;
		dining_detail_offsets ()
	}

	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];

	
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		dining_detail_offsets ()

	},3000)
	
	/***********/
	
	
	panels_detail_ready_mobile();
	
}

function facilities_ready_desktop() {
	panels_ready_desktop();
}

function facilities_load() {
	

}

function facilities_scroll_desktop() {
	
}

function facilities_scroll_mobile() {
	
}

function facilities_resize () {
	panels_resize();
}

function facilities_detail_ready_desktop() {
	v_first_paragraph = false;
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	has_parallax = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		$('.page_form .form_submit').bind('click',mailer);

	} else {
		has_form = false;

	}
	
	if($('.row.parallax').length != 0){
		$oPic = $('.row.parallax img')[0];
		has_parallax = true;
		
		$('.row.parallax img').imagesLoaded(function(){
			$oPicHeight = $('.row.parallax img').height();
			facilities_detail_offsets ();
			$oPic.style[$$transform[0]] = 'translate3d(0,'+(-$oPicHeight-((o_pic - windowHeight)-(o_pic - windowHeight)))/2+'px, 0)';

		})
		
		
	}

	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			facilities_detail_offsets();
		});
	} else {
		has_gallery = false;
		facilities_detail_offsets ();
	}
	
	
	
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
		
	panels_detail_ready_desktop();
	
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		facilities_detail_offsets ();

	},3000)
	
	/***********/
	
	

 
}

function facilities_detail_load() {
	facilities_detail_offsets ();

}

function facilities_detail_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	for(i=0;i<animatedRow_length;i++) {
    	if(!animatedRow_visible[i] && pageY >  animatedRow_offset[i] - windowHeight + scroll_threshold){
    		animatedRow_visible[i] = true;
    		facilities_animate_row(i);
    	}	
	};
	
	if(has_parallax && !isHandheld){
		if(pageY >= o_pic - windowHeight && pageY <= o_pic + windowHeight){
			$oPic.style[$$transform[0]] = 'translate3d(0,'+(-$oPicHeight-((o_pic - windowHeight)-pageY))/2+'px, 0)';
		}
	}
	
	

	
}

function facilities_detail_scroll_mobile() {
	facilities_detail_scroll_desktop()
}

function facilities_detail_resize () {
	facilities_detail_offsets ();
	panels_detail_resize();
}

function facilities_detail_offsets () {
	animatedRow_offset = [];
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;

	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').offset().top);
	 });
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	if(has_parallax && !isHandheld) {
		$oPicHeight = $('.row.parallax img').height();
		o_pic = $('.row.parallax').position().top;

	}
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
    }
    if ($('#meteo_box').length>0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}

function facilities_animate_row (i){
	$targetRow = $('.row.animated:eq('+i+')');
	$oldTarget = $('.row.animated:eq('+(i-1)+')');
	
	
	$('.cover,.content',$targetRow).removeClass('hidden');
	
	setTimeout(function(){
		$('.top_translated._1, .top_translated_full._1',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},100);
	
	setTimeout(function(){
		$('.top_translated._2, .top_translated_full._2',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},200);
	
	setTimeout(function(){
		$('.top_translated._3,.top_translated_full._3',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},300);
	
	setTimeout(function(){
		$('.top_translated._4,.top_translated_full._4',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},400);
	
	
	setTimeout(function(){
		$('.block_title .desc_text',$targetRow).removeClass('top_hidden');
	},700);
	setTimeout(function(){
		$('.block_title .desc_title',$targetRow).removeClass('top_hidden');
	},800);
	
	setTimeout(function(){
		$('.circle_button .back',$targetRow).removeClass('hidden_by_scaling_full');
	},900);
	setTimeout(function(){
		$('.circle_button .arrow',$targetRow).removeClass('hidden');
	},1100);
	
	setTimeout(function(){
		$('.diagonal_line',$targetRow).removeClass('hidden');
	},900);
	
	if(i > 0){
	$('.top_translated._1, .top_translated_full._1',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.top_translated._2, .top_translated_full._2',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.top_translated._3, .top_translated_full._3',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.top_translated._4,.top_translated_full._4',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
	$('.block_title .desc_text',$oldTarget).removeClass('top_hidden');
	$('.block_title .desc_title',$oldTarget).removeClass('top_hidden');
	$('.cover,.content, .diagonal_line',$oldTarget).removeClass('hidden');
	$('.circle_button .back',$oldTarget).removeClass('hidden_by_scaling_full');
	$('.circle_button .arrow',$oldTarget).removeClass('hidden');


	}
	
	

	




	
	$('img',$targetRow).removeClass('top_double');
	$('img',$oldTarget).removeClass('top_double');
	

}


function facilities_ready_mobile() {
	panels_ready_mobile();
}

function facilities_detail_ready_mobile() {

	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	has_parallax = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		$('.page_form .form_submit').bind('click',mailer);

	} else {
		has_form = false;

	}
	
	if($('.row.parallax').length != 0){
		$oPic = $('.row.parallax img')[0];
		has_parallax = true;
	}

	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			facilities_detail_offsets();
		})
	} else {
		has_gallery = false;
		facilities_detail_offsets ();
	}
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
		
	
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		facilities_detail_offsets ();

	},3000)
	panels_detail_ready_mobile();
}


function lifestyle_ready_desktop() {
	panels_ready_desktop();
}

function  lifestyle_load() {
	
}

function  lifestyle_scroll_desktop() {
}

function  lifestyle_scroll_mobile() {
	
}

function  lifestyle_resize () {
	panels_resize();
}

function  lifestyle_detail_ready_desktop() {
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	v_first_paragraph = false;	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		$('.page_form .form_submit').bind('click',mailer);

	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			lifestyle_detail_offsets();
		})
	} else {
		has_gallery = false;
		lifestyle_detail_offsets();
}
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){
		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	}); 
	
	$headerPic = $('#section_animation  img')[0];
		
	panels_detail_ready_desktop();
	lifestyle_detail_offsets ()
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		lifestyle_detail_offsets ();

	},3000)
	
	/***********/

}


function get_wellness_selection ($scope){
	var req_wellness = '';
	
	$('.wellness_selector li.selected', $scope).each(function(i){
		if($('.wellness_selector li.selected', $scope).length-1 > i){
			req_wellness += $(this).text();
			req_wellness += ', '
				
		} else {
			
			req_wellness += $(this).text();
		}
		
		
	});
	 return req_wellness;
}



function  lifestyle_detail_load() {
	lifestyle_detail_offsets ();
}

function lifestyle_detail_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	for(i=0;i<animatedRow_length;i++) {
    	if(!animatedRow_visible[i] && pageY >  animatedRow_offset[i] - windowHeight + scroll_threshold){
    		animatedRow_visible[i] = true;
    		lifestyle_animate_row(i);
    	}	
	};
	
}

function  lifestyle_detail_scroll_mobile() {
	lifestyle_detail_scroll_desktop()
}

function  lifestyle_detail_resize () {
	panels_detail_resize();
	lifestyle_detail_offsets ()
}

function lifestyle_detail_offsets () {
	
	animatedRow_offset = [];
	
	$('.row').each(function(){
		$(this).find('.grey_box').height($(this).height())
	})
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;

	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').offset().top);
	 });
	
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	if(scrollType == "iScroll"){
		myScroll.refresh();
    }
    if ($('#meteo_box').length>0) 
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
	
}

function lifestyle_animate_row (i){
	$targetRow = $('.row.animated:eq('+i+')');
	$oldTarget = $('.row.animated:eq('+(i-1)+')');
	
	
	$('.cover,.content',$targetRow).removeClass('hidden');
	
	setTimeout(function(){
		$('.top_translated._1, .top_translated_full._1',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},100);
	
	setTimeout(function(){
		$('.top_translated._2, .top_translated_full._2',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},200);
	
	setTimeout(function(){
		$('.top_translated._3,.top_translated_full._3',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},300);
	
	setTimeout(function(){
		$('.top_translated._4,.top_translated_full._4',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},400);
	
	
	setTimeout(function(){
		$('.block_title .desc_text',$targetRow).removeClass('top_hidden');
	},700);
	setTimeout(function(){
		$('.block_title .desc_title',$targetRow).removeClass('top_hidden');
	},800);
	
	setTimeout(function(){
		$('.download_item .circle_button .back',$targetRow).removeClass('hidden_by_scaling_full');
	},900);
	setTimeout(function(){
		$('.download_item .circle_button .arrow',$targetRow).removeClass('hidden');
		$('.form_cover img').removeClass('hidden_by_scaling_low');

	},1100);
	
	setTimeout(function(){
		$('.diagonal_line',$targetRow).removeClass('hidden');
	},600);
	
	$('.grey_box .letter',$targetRow).each(function(n){
		setTimeout(function(){
			$('.grey_box .letter:eq('+n+')',$targetRow).removeClass('top_hidden')
		},700+(50*n));
	})
	
	setTimeout(function(){
		$('.horizontal_line',$targetRow).removeClass('no_width');
	},1000);
	
	if(i > 0){
		$('.top_translated._1, .top_translated_full._1',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._2, .top_translated_full._2',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._3, .top_translated_full._3',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._4,.top_translated_full._4',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.block_title .desc_text',$oldTarget).removeClass('top_hidden');
		$('.block_title .desc_title',$oldTarget).removeClass('top_hidden');
		$('.cover,.content, .diagonal_line',$oldTarget).removeClass('hidden');
		$('.grey_box .letter',$oldTarget).each(function(n){
			setTimeout(function(){
				$('.grey_box .letter:eq('+n+')',$oldTarget).removeClass('top_hidden')
			},(50*n));
		})
		$('.download_item .circle_button .back',$oldTarget).removeClass('hidden_by_scaling_full');
		$('.download_item .circle_button .arrow',$oldTarget).removeClass('hidden');
		$('.form_cover img').removeClass('hidden_by_scaling_low');

		$('.horizontal_line',$oldTarget).removeClass('no_width');
		$('img',$oldTarget).removeClass('top_double').removeClass('top_translated');


	}
		

}

function lifestyle_ready_mobile () {
	panels_ready_mobile();
}

function lifestyle_detail_ready_mobile() {
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	v_first_paragraph = false;	
	
	scroll_threshold = 0;
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		$('.page_form .form_submit').bind('click',mailer);

	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			lifestyle_detail_offsets();
		})
	} else {
		has_gallery = false;
		lifestyle_detail_offsets();
}
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){
		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + $(window).height()
			 },1000,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
		
	panels_detail_ready_mobile();
	lifestyle_detail_offsets ()
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		lifestyle_detail_offsets ();

	},3000)
	
	/***********/
}

function location_ready_desktop() {
	v_first_paragraph = false;
	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
		
	has_gallery = false;
	has_form = false;
	has_call = false;
	

	if(typeof myScroll !== 'undefined'){
		myScroll.destroy();
		$('.iScrollVerticalScrollbar ').remove();
		initialize_vertical_scroll();
	} 
	
	$('.has_download').bind('click',manage_gmap);
	
	$('#gmap').click(function(){
			$(this).addClass('enabled')
		}).mouseleave(function(){
			$(this).removeClass('enabled')
	});

	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
		
		$('.page_form .form_submit').bind('click',mailer);
		
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			location_offsets();
		})
	} else {
		has_gallery = false;
		location_offsets();

	}
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
	$oPic = $('.row.parallax img')[0];
	

	
	location_offsets ();

	check_section_pic ();
	
	if(!mobileCheck ) {
		$('#main_veil').removeClass('grey').hide();
	}
	
	if(!isHandheld){$oPic.style[$$transform[0]] = 'translate3d(0,'+(-$oPicHeight-((o_pic - windowHeight)-(o_pic - windowHeight)))/2+'px, 0)';};

	
	controller = "page";
	current = "location"
	
	$('body').attr('id',current);
	
	reset_menu();
		
	pageY = 0; /*non rimuovere */
	
	gallery_focus = true;
	
	
	if(first_run){
		first_run = false;
		 var stateObj = { controller: controller, current:current, url:window.location.href };
		 history.replaceState(stateObj, "", window.location);
	}
	
	
		pageX = 0; /* non rimuovere */
		
		
	
		$('#section_animation .section_panel .section_pic img').imagesLoaded(function(){
			$('body').removeClass('preload');
		 $('#section_scroller .section_panel .section_pic').removeClass('has_transition_1000_inout_quint').addClass('has_transition_800');
		 $('#section_scroller .section_panel.active .section_pic .section_veil').addClass('no_opacity');
		 
		 $('#section_animation .section_panel .section_pic').css('transform','none').animate({
			 width:'100%',
			 left:0
		 },1500,'easeInOutQuint')
		 $('#section_animation .section_panel .section_pic img').animate({
		     left: (-($('#section_animation .section_panel .section_pic img').width() - viewport_width) / 2)
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
			popEnabled = true;
			loadEnabled = true;
			
		},1200);
		
		setTimeout(function(){
			$('.part_1 img').removeClass('hidden');

		},1300);
		
		setTimeout(function(){
			$('.part_2 img').removeClass('hidden');

		},1350);
		
		setTimeout(function(){
			$('.head_text._2').removeClass('top_hidden');

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
		
		
		/** SECURITY ***/
		setTimeout(function(){
			if(scrollType == "iScroll"){
				myScroll.refresh();
			}
			location_offsets ();

		},3000)
		
		/***********/
			
	
	 
}

function location_load() {
	location_offsets ();

}

function location_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	for(i=0;i<animatedRow_length;i++) {
    	if(!animatedRow_visible[i] && pageY >  animatedRow_offset[i] - windowHeight + scroll_threshold){
    		animatedRow_visible[i] = true;
    		location_animate_row(i);
    	}	
	};
	
	if(!isHandheld){
		if(pageY >= o_pic - windowHeight && pageY <= o_pic + windowHeight){
			$oPic.style[$$transform[0]] = 'translate3d(0,'+(-$oPicHeight-((o_pic - windowHeight)-pageY))/2+'px, 0)';
		}
	}
}

function location_ready_mobile () {
	location_ready_desktop();
	
	windowWidth = $(window).width();
	windowHeight = $(window).height();

	detail_loaded = true;
	controller = "page";
	current = "location"
	$('#section_scroller').addClass('disabled');
	
	pageY = 0; /*non rimuovere */
	
	$('body').attr('id',current);
	
	$('#section_animation .section_panel .section_pic img').imagesLoaded(function () {
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

function location_scroll_mobile() {
	location_scroll_desktop();
}

function location_resize () {
	if(!mobileCheck){
	check_section_pic();
	} 
	location_offsets ();
	
}

function location_offsets () {
	animatedRow_offset = [];
	
	
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	o_pic = $('.row.parallax').position().top;
	$oPicHeight = $('.row.parallax img').height();

	


	
	
	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').position().top);
	 });
	
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}
    if ($('#meteo_box').length > 0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}

function location_animate_row (i){
	$targetRow = $('.row.animated:eq('+i+')');
	$oldTarget = $('.row.animated:eq('+(i-1)+')');
	
	
	$('.cover,.content',$targetRow).removeClass('hidden');
	
	setTimeout(function(){
		$('.top_translated._1, .top_translated_full._1,.top_double._1',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},100);
	
	setTimeout(function(){
		$('.top_translated._2, .top_translated_full._2',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},200);
	
	setTimeout(function(){
		$('.top_translated._3,.top_translated_full._3',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},300);
	
	setTimeout(function(){
		$('.top_translated._4,.top_translated_full._4',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},400);
	
	
	setTimeout(function(){
		$('.block_title .desc_text',$targetRow).removeClass('top_hidden');
	},700);
	setTimeout(function(){
		$('.block_title .desc_title',$targetRow).removeClass('top_hidden');
	},800);
	
	setTimeout(function(){
		$('.download_item .circle_button .back',$targetRow).removeClass('hidden_by_scaling_full');
	},900);
	setTimeout(function(){
		$('.download_item .circle_button .arrow',$targetRow).removeClass('hidden');

	},1100);
	
	setTimeout(function(){
		$('.diagonal_line',$targetRow).removeClass('hidden');
	},600);
	
	setTimeout(function(){
		$('.horizontal_line',$targetRow).removeClass('no_width');
	},1000);
	
	if(i > 0){
		$('.top_translated._1, .top_translated_full._1',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._2, .top_translated_full._2',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._3, .top_translated_full._3',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._4,.top_translated_full._4',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.block_title .desc_text',$oldTarget).removeClass('top_hidden');
		$('.block_title .desc_title',$oldTarget).removeClass('top_hidden');
		$('.cover,.content, .diagonal_line',$oldTarget).removeClass('hidden');
		$('.download_item .circle_button .back',$oldTarget).removeClass('hidden_by_scaling_full');
		$('.download_item .circle_button .arrow',$oldTarget).removeClass('hidden');
		$('.horizontal_line',$oldTarget).removeClass('no_width');
		$('img',$oldTarget).removeClass('top_double').removeClass('top_translated');


	}
		

}

var map_opened = false;


function manage_gmap () {
	if(!map_opened) {
		map_opened = true;
		$('#gmap').addClass('loaded');
		if(scrollType == "iScroll"){
		//myScroll.scrollTo(0,-$('#gmap').position().top);
		}
		$('.download_item').css($$transform[0],'rotateZ(180deg)');
		setTimeout(function(){
			location_offsets();
			if(scrollType == "iScroll"){
			myScroll.refresh();
			}
		},1500)

	} else {
		map_opened = false;
		$('#gmap').removeClass('loaded');
		$('.download_item').css($$transform[0],'none');
		setTimeout(function(){
			if(scrollType == "iScroll"){
			myScroll.refresh();
			}
			location_offsets();

		},1500)

	}
}

function photogallery_ready_desktop() {
	panels_ready_desktop();
	
	
}

function photogallery_load() {
	

}

function photogallery_scroll_desktop() {
	
}

function photogallery_scroll_mobile() {
	
}

function photogallery_resize () {
	panels_resize();
}

function photogallery_detail_ready_desktop() {
	gallery_ready();
	
	
	
	has_gallery = false;
	
	$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
		gallery_load();
	});
		
	panels_detail_ready_desktop();
	
	if(scrollType == "iScroll"){
		myScroll.disable();
	}
	
	if(!panelDetail_ready){
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
	setTimeout(function(){
		$('#fullscreen_gallery').removeClass('no_visibility');
		forced_fullscreen = true;
		hide_menu_center();
		
	},1500);
	
	setTimeout(function(){
		
			side_menu = true;
			show_side_menu();
			$('#gallery_panel').removeClass('hidden');
		
	},2000)
	
	setTimeout(function(){
		$('.p_section.left').removeClass('top_hidden');
	},2100)
	
	setTimeout(function(){
		$('.p_name').removeClass('top_hidden');
	},2200)
	
	setTimeout(function(){
		$('.button_right .circle').removeClass('hidden_by_scaling_full');	
		$('.p_section.right').removeClass('top_hidden');

		
	},2300)
	
	setTimeout(function(){
		$('.button_left .circle').removeClass('hidden_by_scaling_full');	
	},2400)
	
	setTimeout(function(){
		$('.button_down .back_circle').removeClass('hidden_by_scaling_full');	
	},2500)
	
	$('.button_down .back_circle .line').each(function(l){
		setTimeout(function(){
			$('.button_down .back_circle .line:eq('+l+')').removeClass('no_height');
		},2700+(100*l))
	})	
		});
	
	} else {
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
		setTimeout(function(){
			$('#fullscreen_gallery').removeClass('no_visibility');
			$('#fullscreen_gallery .gallery_controller').removeClass('top_translated');
			forced_fullscreen = true;
			hide_menu_center();
		},950);
		
		setTimeout(function(){
			
			side_menu = true;
			show_side_menu();
			$('#gallery_panel').removeClass('hidden');
		
	},1450)
	
	setTimeout(function(){
		$('.p_section.left').removeClass('top_hidden');
	},1550)
	
	setTimeout(function(){
		$('.p_name').removeClass('top_hidden');
	},1650)
	
	setTimeout(function(){
		$('.button_right .circle').removeClass('hidden_by_scaling_full');	
		$('.p_section.right').removeClass('top_hidden');

		
	},1750)
	
	setTimeout(function(){
		$('.button_left .circle').removeClass('hidden_by_scaling_full');	
	},1850)
	
	setTimeout(function(){
		$('.button_down .back_circle').removeClass('hidden_by_scaling_full');	
	},1950)
	
	$('.button_down .back_circle .line').each(function(l){
		setTimeout(function(){
			$('.button_down .back_circle .line:eq('+l+')').removeClass('no_height');
		},2150+(100*l))
	})	
		});
	}
	
	$('.p_section.left p span').text($('#section_scroller .section_panel.active').prev().find('.section_panel_title').text());
	$('.p_section.right p span').text($('#section_scroller .section_panel.active').next().find('.section_panel_title').text());
	$('.p_left').attr('href',$('#section_scroller .section_panel.active').prev().attr('href'));
	$('.p_right').attr('href',$('#section_scroller .section_panel.active').next().attr('href'));
	
	$('.p_left,.p_right').bind('click',item_load);
	
	$('.button_down').bind('click',function(e){
		e.preventDefault();
		closePageDetail();
		 var stateObj = { controller: "panel", current:current.split('_detail')[0],panelIndex:"none",url:"/"+lang+"/"+current.split('_detail')[0]};
		 history.replaceState(stateObj, "","/"+lang+"/"+current.split('_detail')[0]);
		
		
	});
	
	if($('.p_section.left p span').text() == ''){
		$('.p_section.left p img').hide();
	}
	
	if($('.p_section.right p span').text() == ''){
		$('.p_section.right p img').hide();
	}
	
	if($('#fullscreen_gallery').attr('rel') == "rooms-photogallery"){
		$pos1 = $('.gallery_slider').attr('sectionStamp').split('.')[0];
		$pos2 = parseInt($pos1) +  parseInt($('.gallery_slider').attr('sectionStamp').split('.')[1]);
		$pos3 = parseInt($pos2) + parseInt($('.gallery_slider').attr('sectionStamp').split('.')[2]); 
		$pos4 = parseInt($pos3) + parseInt($('.gallery_slider').attr('sectionStamp').split('.')[3]); 
		$pos5 = parseInt($pos4) + parseInt($('.gallery_slider').attr('sectionStamp').split('.')[4]); 
		$pos6 = parseInt($pos5) + parseInt($('.gallery_slider').attr('sectionStamp').split('.')[5]); 
		$pos7 = parseInt($pos6) + parseInt($('.gallery_slider').attr('sectionStamp').split('.')[6]); 
		$pos8 = parseInt($pos7) + parseInt($('.gallery_slider').attr('sectionStamp').split('.')[7]); 

		
		
		
		
		
		$roomsData = [
		              {pos:0,name:'Junior Suite'}, 
		              {pos:$pos1,name:'Grand De Luxe'},
		              {pos:$pos2,name:'Terrace'},
		              {pos:$pos3,name:'Relaxing'},
		              {pos:$pos4,name:'Romantic'},
		              {pos:$pos5,name:'Canopy'},
		              {pos:$pos6,name:'Classic'},
		              {pos:$pos7,name:'Single'},
		              {pos:$pos8,name:'Eaudesea Experience'}];
	}

	
	
	
   
}

function photogallery_detail_load() {
	

}

function photogallery_detail_scroll_desktop() {


	
}

function photogallery_detail_scroll_mobile() {
	
}

function photogallery_detail_resize () {
	panels_detail_resize();
}

function photogallery_detail_offsets () {
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}
	
	
}

function photogallery_ready_mobile () {
	panels_ready_mobile();
}


function press_ready_desktop() {
	v_first_paragraph = false;
	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
		
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	if(typeof myScroll !== 'undefined'){
		myScroll.destroy();
		$('.iScrollVerticalScrollbar ').remove();
		initialize_vertical_scroll();
	} 
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			press_offsets();
		})
	} else {
		has_gallery = false;
	}
	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){
		myScroll.scrollTo(0,-(o_gallery + windowHeight));
		pageY = o_gallery + windowHeight;
		common_scroll();
		}
	});
	
	press_offsets ();

	
	$headerPic = $('#section_animation  img')[0];
	
	
	check_section_pic ();
	
	if(!mobileCheck){$('#main_veil').removeClass('grey').hide();};
	
	controller = "page";
	current = "press"
	
	$('body').attr('id',current);
	
	
	reset_menu();
		
	pageY = 0; /*non rimuovere */
	
	gallery_focus = true;
	
	
	if(first_run){
		first_run = false;
		 var stateObj = { controller: controller, current:current, url:window.location.href };
		 history.replaceState(stateObj, "", window.location);
	}
	
	
		pageX = 0; /* non rimuovere */
		
		
	
		$('#section_animation .section_panel .section_pic img').imagesLoaded(function () {
			$('body').removeClass('preload');
		 $('#section_scroller .section_panel .section_pic').removeClass('has_transition_1000_inout_quint').addClass('has_transition_800');
		 $('#section_scroller .section_panel.active .section_pic .section_veil').addClass('no_opacity');
		 
		 $('#section_animation .section_panel .section_pic').css('transform','none').animate({
			 width:'100%',
			 left:0
		 },1500,'easeInOutQuint')
		 $('#section_animation .section_panel .section_pic img').animate({
		     left: (-($('#section_animation .section_panel .section_pic img').width() - viewport_width) / 2)
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
			popEnabled = true;
			loadEnabled = true;
			
		},1200);
		
		setTimeout(function(){
			$('.part_1 img').removeClass('hidden');

		},1300);
		
		setTimeout(function(){
			$('.part_2 img').removeClass('hidden');

		},1350);
		
		setTimeout(function(){
			$('.head_text._2').removeClass('top_hidden');

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
		/** SECURITY ***/
		setTimeout(function(){
			if(scrollType == "iScroll"){
				myScroll.refresh();
			}
			press_offsets ();

		},3000)
		
		/***********/
}

function press_load() {
	press_offsets ();
}

function press_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	
	
}

function press_scroll_mobile() {
	press_scroll_desktop();
}

function press_resize () {
	check_section_pic();
	press_offsets ();
	
}

function press_offsets () {
	animatedRow_offset = [];
	
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	
	
	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').position().top);
	 });
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}
    if ($('#meteo_box').length > 0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}

function press_ready_mobile () {
	press_ready_desktop();
	
	windowWidth = $(window).width();
	windowHeight = $(window).height();

	detail_loaded = true;
	controller = "page";
	current = "press"
	$('#section_scroller').addClass('disabled');
	
	pageY = 0; /*non rimuovere */
	
	$('body').attr('id',current);
	
	$('#section_animation .section_panel .section_pic img').imagesLoaded(function () {
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

function rooms_ready_desktop() {
	panels_ready_desktop();
}

function rooms_load() {
	
}

function rooms_scroll_desktop() {
	
}

function rooms_scroll_mobile() {
	
}

function rooms_resize () {
	panels_resize();
}

function rooms_detail_ready_desktop() {

	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	
	o_services = $('#inroom_services').position().top
	
	v_first_paragraph = false;
	v_services = false;
	
	styleRow_length = 3;
	styleRow_offset = [];
	styleRow_visible = [];
	
	$('.style_row').each(function(){
		styleRow_visible.push(false);
	})
	
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			rooms_detail_offsets();
			if(isHandheld){
			 $(window).on('touchstart, touchmove',function(){
					$('body,html').stop(true);
					
			})
			}
		})
	} else {
		has_gallery = false;
		rooms_detail_offsets ()
	}

	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){
		myScroll.scrollTo(0,-(o_gallery + windowHeight + 80));
		pageY = o_gallery + windowHeight + 80;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
			 
			
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
	
	panels_detail_ready_desktop();
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		rooms_detail_offsets ();

	},3000)
	
	/***********/
	

}

function rooms_detail_load() {
	rooms_detail_offsets ();

}

function rooms_detail_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	if(!v_services && pageY > o_services - windowHeight) {
		v_services = true;
		
		$('.service_pic .cover,.service_pic .content').removeClass('hidden');
		
	}
	
	if(!styleRow_visible[0] && pageY > styleRow_offset[0] - windowHeight + scroll_threshold){
		styleRow_visible[0] = true;
		$('.style_title .desc_text').removeClass('top_hidden');
		setTimeout(function(){
			$('.style_title .desc_title').removeClass('top_hidden');
		},100)
		
		setTimeout(function(){
			$('.style_row:eq(0) img').removeClass('top_double');
		},300);
		
		setTimeout(function(){
			$('.style_row:eq(1) .w42 .style_label').removeClass('no_width');
		},200);

		setTimeout(function(){
			$('.style_row:eq(1) .w42 .style_label .desc_title').removeClass('top_hidden')
		},1100);
		
	}
	
	if(!styleRow_visible[1] && pageY > styleRow_offset[1] - windowHeight  + scroll_threshold){
		styleRow_visible[1] = true;
		
		
		$('.style_row:eq(1) .covered .cover,.style_row:eq(1) .covered .content').removeClass('hidden');
		
		setTimeout(function(){
			$('.style_row:eq(1) img').removeClass('top_double');
		},800);
		
		setTimeout(function(){
			$('.style_row:eq(0) .style_label .desc_title').removeClass('top_hidden')

		},1300);
		
		setTimeout(function(){
			$('.style_row:eq(1) .w58 .style_label:eq(0)').removeClass('no_width');

		},800)
		
		
		
		setTimeout(function(){
			$('.style_row:eq(1) .w58 .style_label:eq(0) .desc_title').removeClass('top_hidden')
		},1500);
	}
	
	if(!styleRow_visible[2] && pageY > styleRow_offset[2] - windowHeight  + scroll_threshold){
		styleRow_visible[2] = true;
		
		
		
		
		setTimeout(function(){
			$('.style_row:eq(2) .w58 img').removeClass('top_double');
			$('.style_row:eq(2) .w58 .style_label:eq(0)').removeClass('no_width');

		},300);
		
		
		
		setTimeout(function(){
			$('.style_row:eq(2) .w58 .style_label:eq(0) .desc_title').removeClass('top_hidden')
		},1200);
		
		setTimeout(function(){
			$('.style_row:eq(2) .w42 .covered .cover,.style_row:eq(2) .w42 .covered .content').removeClass('hidden');

		},800);
		
		setTimeout(function(){
			$('.style_row:eq(2) .w42 .style_label').removeClass('no_width');
		},1700);
		
		setTimeout(function(){
			$('.style_row:eq(2) .w42 .style_label .desc_title').removeClass('top_hidden')
		},2600);
		
		
	}
	
	
}

function rooms_detail_scroll_mobile() {
	rooms_detail_scroll_desktop();
}

function rooms_detail_resize () {
	rooms_detail_offsets ();
	panels_detail_resize();
	
	
}

function rooms_detail_offsets () {
	styleRow_offset = [];
	
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	o_services = $('#inroom_services').position().top
	
	$('.style_row').each(function(i){
		styleRow_offset.push($('.style_row:eq('+i+')').position().top);
	 });
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}
    if ($('#meteo_box').length > 0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}





function rooms_ready_mobile() {

panels_ready_mobile();
}



function rooms_detail_ready_mobile() {
    o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	
	o_services = $('#inroom_services').position().top
	
	v_first_paragraph = false;
	v_services = false;
	
	styleRow_length = 3;
	styleRow_offset = [];
	styleRow_visible = [];
	
	$('.style_row').each(function(){
		styleRow_visible.push(false);
	})
	
	
	has_gallery = false;
	has_form = false;
	has_call = false;
	
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			rooms_detail_offsets();
		})
	} else {
		has_gallery = false;
		rooms_detail_offsets ()
	}

	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){
		myScroll.scrollTo(0,-(o_gallery + windowHeight + 80));
		pageY = o_gallery + windowHeight + 80;
		common_scroll();
		} else {
			 $('body,html').animate({
				 scrollTop:o_gallery + windowHeight
			 },1500,'easeOutQuint');
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
	panels_detail_ready_mobile();

	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		rooms_detail_offsets ();

	},3000)
}

function specials_ready_desktop() {
	panels_ready_desktop();
}

function specials_load() {

}

function specials_scroll_desktop() {
	
}

function specials_scroll_mobile() {
	
}

function specials_resize () {
	panels_resize();
}

function specials_detail_ready_desktop() {   
    o_rates_detail = 0;
    if ($('.row.animated:eq(0)').length > 0) {
        o_rates_detail = $('.row.animated:eq(0)').position().top;
    }
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	o_special_title = $('.section_title').position().top;
	o_special_box = $('#more_specials_container').position().top
	v_first_paragraph = false;
	v_rates_detail = false;
	v_special_title = false;
	v_special_box = false;
	
	$headerPic = $('#section_animation  img')[0];

	has_gallery = false;
	has_form = false;
	has_pic = false;
	
	panels_detail_ready_desktop();
	
	set_book_urls();
	
	$('.pic_box:eq(0) > img').imagesLoaded(function(){
		$('.pic_box > img').height('100%');
		$('.pic_box > img').css('margin-left', -($('.pic_box:eq(0) > img').width() - 320) /2+'px');
	});

    if ($('#fullscreen_gallery').length != 0) {
        gallery_ready();
        has_gallery = true;
        $('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function () {
            gallery_load();
            specials_detail_offsets();
        })
    } else {
        has_gallery = false;
        specials_detail_offsets();

    }

	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		specials_detail_offsets();

	},3000)
	
	/***********/
   
}

function specials_detail_load() {
	specials_detail_offsets ();

}

function specials_detail_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	if(!v_rates_detail && pageY > o_rates_detail - windowHeight + scroll_threshold) {
		v_rates_detail = true;
		$('.closing').removeClass('top_translated_full');
		setTimeout(function(){$('.block_25 .desc_title').removeClass('top_hidden');},800);
	}
	
	if(!v_special_title && pageY > o_special_title - windowHeight + scroll_threshold) {
		v_special_title = true;
		$('.section_title').removeClass('top_hidden');
	}
	
	if(!v_special_box && pageY > o_special_box - windowHeight + scroll_threshold) {
		v_special_box = true;
		$('.special_box').each(function(i){
			setTimeout(function(){
				$('.special_box:eq('+i+') .cover').removeClass('hidden');
				$('.special_box:eq('+i+') .content').removeClass('hidden');
			},100*i)
			setTimeout(function(){
				$('.special_box:eq('+i+') .text_box:not(.date_box)').removeClass('top_double');
			},500+(250*(i+1)))
			setTimeout(function(){
				$('.special_box:eq('+i+') .date_box').removeClass('top_double');
			},500+(350*(i+1)))
		});
		
		setTimeout(function(){
			//$('.special_arrow.right').removeClass('hidden_by_scaling_full');
		},1000)
	}

	
}

function specials_detail_scroll_mobile() {
	specials_detail_scroll_desktop()
}

function specials_detail_resize () {
	specials_detail_offsets ();
	panels_detail_resize();
}

function specials_detail_offsets() {
    o_rates_detail = 0;
    if ($('.row.animated:eq(0)').length>0)
	    o_rates_detail = $('.row.animated:eq(0)').position().top;
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	o_special_title = $('.section_title').position().top;
    o_special_box = $('#more_specials_container').position().top
    if (has_gallery) {
        o_gallery = $('#fullscreen_gallery').position().top;
    }
	if(scrollType == "iScroll"){
		myScroll.refresh();
    }
    if ($('#meteo_box').length>0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}

function set_book_urls() {
   
        current_date = new Date();
        dd = current_date.getDate();
        mm = current_date.getMonth() + 1;
        yyyy = current_date.getFullYear();
        
        $(".special_book").each(function() {
        	
        	var $rate;
            var $nights;
            
            switch ($(this).attr("rel")) {
                case "spring-is-in-the-air":
                		$rate = 'HTLS01';
                		$nights = 2;
                    break;
                case "long-spring":
                		$rate = ''; 
                		$nights = 3;
                	break;
                case "moonlight-shadow":
                	$rate = '';  
                	$nights = 2;
                	break;
                case "romance-retreat":
                	$rate = 'HTLS02';
                	$nights = 2;
                	break;
                case "body-and-soul":
                	$rate = 'HTLS04';
                	$nights = 3;
                	break;
                case "easy-reach":
                	$rate = 'HTLS03';
                	$nights = 1;
                	break;
                case "dolce-vita":
                	$rate = 'HTLS05';
                	$nights = 2;
                	break;
                case "late-summer-sunshine":
                	$rate = 'SLMADV';
                	$nights = 2;
                	break;
            }
        	
        	
        	
            start_day = $(this).attr("sday");
            start_month = $(this).attr("smonth");
            start_year = $(this).attr("syear");
            end_day = $(this).attr("eday");
            end_month = $(this).attr("emonth");
            end_year = $(this).attr("eyear");
            start_date = new Date(start_year, start_month - 1, start_day);
            end_date = new Date(end_year, end_month - 1, end_day);
            if (current_date >= start_date && current_date <= end_date) {
              
                    today = current_date;
					 depart = new Date();
					 depart.setDate(today.getDate() + $nights);
                
            } else {
				
				start_date = new Date(parseInt(start_year), start_month - 1, start_day);
				
            	if(current_date >= start_date){ 
            	start_date = new Date(parseInt(start_year)+1, start_month - 1, start_day);
            	}
            	
                today = start_date;
                depart = new Date(today);
				depart.setDate(today.getDate() + $nights); 
				
				
               
			  
            }
            
            
            
          
            $(this).attr("href", "https://be.synxis.com/?adult=2&arrive="+today.getFullYear()+"-"+(today.getMonth() + 1)+"-"+today.getDate()+"&depart="+depart.getFullYear()+"-"+(depart.getMonth() + 1)+"-"+depart.getDate()+"&chain=22402&currency=EUR&hotel=79920&src=24C&locale="+get_locale()+"&Rate="+$rate);
			
			

        })
   
}


function specials_ready_mobile() {
	panels_ready_mobile();
}


function specials_detail_ready_mobile() {
    o_rates_detail = 0;
    if ($('.row.animated:eq(0)').length > 0) {
        o_rates_detail = $('.row.animated:eq(0)').position().top;
    }
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	o_special_title = $('.section_title').position().top;
	o_special_box = $('#more_specials_container').position().top
	v_first_paragraph = false;
	v_rates_detail = false;
	v_special_title = false;
	v_special_box = false;
	
	$headerPic = $('#section_animation  img')[0];

	has_gallery = false;
	has_form = false;
	has_pic = false;
	
	panels_detail_ready_mobile();
	
	set_book_urls();
	
	$('.pic_box:eq(0) > img').imagesLoaded(function(){
		$('.pic_box > img').height('100%');
		$('.pic_box > img').css('margin-left', -($('.pic_box:eq(0) > img').width() - 320) /2+'px');
	});

    if ($('#fullscreen_gallery').length != 0) {
        gallery_ready();
        has_gallery = true;
        $('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function () {
            gallery_load();
            specials_detail_offsets();
        })
    } else {
        has_gallery = false;
        specials_detail_offsets();
    }
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		specials_detail_offsets();

	},3000)
	
	/***********/
}

function travel_ready_desktop() {
	panels_ready_desktop();
}

function  travel_load() {
	
}

function  travel_scroll_desktop() {
}

function  travel_scroll_mobile() {
	
}

function  travel_resize () {
	panels_resize();
}

function  travel_detail_ready_desktop() {
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	v_first_paragraph = false;	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	$('.has_single_form').bind('click',function(){
		show_page_form($(this).attr('rel'));
	});
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			travel_detail_offsets();
		})
	} else {
		has_gallery = false;
		travel_detail_offsets ()
	}

	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		    myScroll.scrollTo(0,-(o_gallery + windowHeight + 80));
		    pageY = o_gallery + windowHeight + 80;
		    common_scroll();
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
		
	panels_detail_ready_desktop();
	
	/** SECURITY ***/
    setTimeout(function() {
        if (scrollType == "iScroll") {
            myScroll.refresh();
        }
        travel_detail_offsets();

    }, 3000);

    /***********/
}

function scroll_to_partners() {
    if ($('#partners').length > 0) {
        if (scrollType == "iScroll") {
            myScroll.refresh();
            myScroll.scrollTo(0, myScroll.maxScrollY, 0);
            pageY = myScroll.maxScrollY;
            common_scroll();
        } else {
            if (myScroll == null)
                hide_menu_mobile();
                $('body,html').stop(false).animate({
                    scrollTop: 5000
                }, 1200, 'easeOutQuint');
        }
    }
   
}
function scroll_to_element() {
    if ($('#experience_box').length > 0) {
        hide_menu_mobile();
        $('body,html').stop(false).animate({
            scrollTop: $("#experience_box").offset().top
        }, 600, 'easeOutQuint');
    }

}
function scroll_to_top() {
   
        if (scrollType == "iScroll") {
            myScroll.refresh();
            myScroll.scrollTo(0, 0, 0);
            pageY =0;
            common_scroll();
        }
    

}
function  travel_detail_load() {
	travel_detail_offsets ();
}

function travel_detail_scroll_desktop() {
	if(!isHandheld) {
		if(pageY < windowHeight+100){
			$headerPic.style[$$transform[0]] = 'translateY('+pageY/2+'px)';
		}
	}
	
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	for(i=0;i<animatedRow_length;i++) {
    	if(!animatedRow_visible[i] && pageY >  animatedRow_offset[i] - windowHeight + scroll_threshold){
    		animatedRow_visible[i] = true;
    		travel_animate_row(i);
    	}	
	};
	
}

function  travel_detail_scroll_mobile() {
	travel_detail_scroll_desktop();
}

function  travel_detail_resize () {
	panels_detail_resize();
	travel_detail_offsets ()
}

function travel_detail_offsets () {
	animatedRow_offset = [];
	
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;

	$('.row.animated').each(function(i){
		animatedRow_offset.push($('.row.animated:eq('+i+')').offset().top);
	 });
	
	if(has_gallery){
		o_gallery = $('#fullscreen_gallery').position().top;
	}
	
	if(has_call) {
		o_call = $('.call_to').position().top
	}
	
	if(has_form) {
		o_form = $('.row.has_form').position().top;
	}
	
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}

    if ($('#meteo_box').length>0)
	    o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
	
}

function travel_animate_row (i){
	$targetRow = $('.row.animated:eq('+i+')');
	$oldTarget = $('.row.animated:eq('+(i-1)+')');
	
	
	$('.cover,.content',$targetRow).removeClass('hidden');
	
	setTimeout(function(){
		$('.top_translated._1, .top_translated_full._1',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},100);
	
	setTimeout(function(){
		$('.top_translated._2, .top_translated_full._2',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},200);
	
	setTimeout(function(){
		$('.top_translated._3,.top_translated_full._3',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},300);
	
	setTimeout(function(){
		$('.top_translated._4,.top_translated_full._4',$targetRow).removeClass('top_translated').removeClass('top_translated_full');
	},400);
	
	
	setTimeout(function(){
		$('.block_title .desc_text',$targetRow).removeClass('top_hidden');
	},700);
	setTimeout(function(){
		$('.block_title .desc_title',$targetRow).removeClass('top_hidden');
	},800);
	
	setTimeout(function(){
		$('.download_item .circle_button .back',$targetRow).removeClass('hidden_by_scaling_full');
	},900);
	setTimeout(function(){
		$('.download_item .circle_button .arrow',$targetRow).removeClass('hidden');
		$('.form_cover img').removeClass('hidden_by_scaling_low');

	},1100);
	
	setTimeout(function(){
		$('.diagonal_line',$targetRow).removeClass('hidden');
	},600);
	
	$('.grey_box .letter',$targetRow).each(function(n){
		setTimeout(function(){
			$('.grey_box .letter:eq('+n+')',$targetRow).removeClass('top_hidden')
		},700+(50*n));
	})
	
	setTimeout(function(){
		$('.horizontal_line',$targetRow).removeClass('no_width');
	},1000);
	
	if(i > 0){
		$('.top_translated._1, .top_translated_full._1',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._2, .top_translated_full._2',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._3, .top_translated_full._3',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.top_translated._4,.top_translated_full._4',$oldTarget).removeClass('top_translated').removeClass('top_translated_full');
		$('.block_title .desc_text',$oldTarget).removeClass('top_hidden');
		$('.block_title .desc_title',$oldTarget).removeClass('top_hidden');
		$('.cover,.content, .diagonal_line',$oldTarget).removeClass('hidden');
		$('.grey_box .letter',$oldTarget).each(function(n){
			setTimeout(function(){
				$('.grey_box .letter:eq('+n+')',$oldTarget).removeClass('top_hidden')
			},(50*n));
		})
		$('.horizontal_line',$oldTarget).removeClass('no_width');
		$('img',$oldTarget).removeClass('top_double').removeClass('top_translated');


	}
		

}

function travel_ready_mobile(){
	panels_ready_mobile();
}

function travel_detail_ready_mobile(){
	o_first_paragraph = $('.paragraph.first .big_subtitle').position().top;
	v_first_paragraph = false;	
	
	animatedRow_length = $('.row.animated').length;
	animatedRow_offset = [];
	animatedRow_visible = [];
	
	$('.row.animated').each(function(){
		animatedRow_visible.push(false);
	});
	
	$('.has_single_form').bind('click',function(){
		show_page_form($(this).attr('rel'));
	});
	
	if($('.call_to').length != 0){
		has_call = true;
		v_call = false;
	} else {
		has_call = false;

	}
	if($('.page_form').length != 0){
		has_form = true;
		v_form = false;
	} else {
		has_form = false;

	}
	
	if($('#fullscreen_gallery').length != 0 ){
		gallery_ready();
		has_gallery = true;
		$('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function(){
			gallery_load();
			travel_detail_offsets();
		})
	} else {
		has_gallery = false;
		travel_detail_offsets ()
	}

	
	$('#fullscreen_gallery .button_down').bind('click', function () {console.log(2);
		if(scrollType == "iScroll"){

		myScroll.scrollTo(0,-(o_gallery + windowHeight + 80));
		pageY = o_gallery + windowHeight + 80;
		common_scroll();
		}
	});
	
	$headerPic = $('#section_animation  img')[0];
		
	panels_detail_ready_mobile();
	
	/** SECURITY ***/
	setTimeout(function(){
		if(scrollType == "iScroll"){
			myScroll.refresh();
		}
		travel_detail_offsets();

	},3000)
	
	/***********/
};

