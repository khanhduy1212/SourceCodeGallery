/************ OFFSETS **************/


var $flowers;
var $flowers_slide;

var o_flowers;
var o_first_paragraph;
var o_chic;
var e_chic;
var o_blockTitle;
var v_blockTitle;
var o_experience;
var v_experience;


var $chic_el_1;
var $chic_el_2;
var $chic_el_3;
var $chic_el_4;
var $chic_el_5;
var $chic_el_6;

var stripe_length;
var stripes_offset = [];

var video_loaded = false;

current = "index";

/*********** VISIBILITY *************/

var v_first_paragraph = false;
var stripes_visible = [false,false,false,false,false,false];

/*************************************/



function index_offsets (){
	o_first_paragraph = $('.paragraph.first').position().top;
	o_chic = $('#understated_pic').position().top;
	e_chic = o_chic +  $('#understated_pic').height();
	o_blockTitle = $('.main_block_title').position().top;
	o_experience = $('#experience_box').position().top + 130;
	understatedHeight = $('#understated_pic').height();

	//o_flowers = $flowers_slide.position().top;
	//$flowersHeight = $($flowers).height();
    if (has_gallery) {
        o_gallery = $('#fullscreen_gallery').position().top;
    }
	
	
	o_journal_title = $('#latest_journal .section_title').position().top;
	o_journal_box = $('.journal_container').position().top
	
	stripes_offset = [];
	
	$('.stripe').each(function(i){
		stripes_offset.push($('.stripe:eq('+i+')').position().top);
	 });
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}
	
	/////o_weather = $('#meteo_box').position().top;
	scrollerHeight = $('#main_scroller').height();
}

function index_ready_desktop() {
	stripes_visible = [false,false,false,false];
	stripes_offset = [];
	v_first_paragraph = false;
	v_blockTitle = false;
	v_experience = false;
	v_journal_title = false;
	v_journal_box = false;
	
	$headerPic = $('#header > img.desktop')[0];

	video_loaded = false;
	
	reset_menu();
	
	$('#menu_center_side,#player_side').removeClass('hidden');


	controller = "page";
	
	$('#main_veil').removeClass('has_transition_1000_inout').show();
	
	$('#right_content .menu_controller').bind('click',show_menu);
	
	//$('.book_now_top').bind('click',show_book_interface);
	
	$('#experience_controller .controller_right, #experience_controller .controller_left').bind('click',experience_move);
	$('.exp_data .more').bind('click',function(e){
		e.preventDefault();
		
		href = $(this).attr('href');
		closePage();
		$('#main_veil').addClass('grey');

		setTimeout(function(){
			base_load(href,true);
		},800)
	})

    //$('a.cta').bind('click', function(e) {
    //    e.preventDefault();

    //    href = $(this).attr('href');
    //    closePage();
    //    $('#main_veil').addClass('grey');

    //    setTimeout(function() {
    //        base_load(href, true);
    //    }, 800);
    //});
	
	stripe_length = $('.stripe').length;
	
	$('.stripe').each(function(i){
		stripes_visible[i] = false;
	});
		
		$flowers = document.getElementById('flowers');
		$flowers_slide = $('#flowers').parent();
	
	check_header();
	
	if($('.call_to').length != 0){
		hasCall = true;
		v_call = false;
		o_call = $('.call_to').position().top
	} else {
		hasCall = false;

	}
	
	has_gallery = false;
	
	if(first_run){
		first_run = false;
		 var stateObj = { controller: "page", current:"index", url:window.location.href };
		 history.replaceState(stateObj, "", window.location);
	}
	
	
	
	$chic_el_1 = $('#understated_pic .h1,#understated_pic .h2');
	$chic_el_2 = $('#understated_pic .c1, #understated_pic .understated_layer');
	$chic_el_3 = $('#understated_pic .i');
	$chic_el_4 = $('#understated_pic .c2');
	$chic_el_5 = $('#understated_pic .c3');
	$chic_el_6 = $('#understated_pic .c4');
	
	
	if(isHandheld){
		$('#header').height(windowHeight);
		$chic_el_1.addClass('has_transition_600');
		$chic_el_2.addClass('has_transition_600');
		$chic_el_3.addClass('has_transition_600');
		$chic_el_4.addClass('has_transition_600');
		$chic_el_5.addClass('has_transition_600');
		$chic_el_6.addClass('has_transition_600');
		
	}
	
    if ($('#fullscreen_gallery').length != 0) {
        gallery_ready();
        has_gallery = true;
        $('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function () {
            gallery_load();
            index_offsets();
        })
    } else {
        has_gallery = false;
        index_offsets();

    }
    $('#fullscreen_gallery .button_down').bind('click', function () {
        if (scrollType == "iScroll") {

            console.log(1);

            myScroll.refresh();
          myScroll.scrollTo(0, -(o_gallery + windowHeight), 0);
            //myScroll.scrollToElement("#latest_journal");
            pageY = o_gallery + windowHeight;
            common_scroll();
        }
    });
 
	index_offsets ();
	
	if(typeof myScroll !== 'undefined'){
		myScroll.destroy();
		$('.iScrollVerticalScrollbar ').remove();
		initialize_vertical_scroll();
	} 
	
	
	experience_box_setup();
	
	
	/************ DESKTOP ANIMATION **************/

		$('#header img.desktop, #right_panel > img').imagesLoaded(function(){
			$('body').removeClass('preload');
			
			$('img[data-src]').each(function(){
				$path = $(this).attr('data-src');
				$(this).attr('src',$path);
			});
			
			$('img').imagesLoaded(function(){
		
				index_offsets();
				
				if(scrollType == "iScroll"){
					myScroll.refresh();
				}
				//$.ajax({
				//	  url: 'http://journal.casangelina.com/feed/post',
				//	  success: function(data) {
				//		  var $xml = $(data);
				//		    var limit = 3;
				//		    $xml.find("item").each(function(i) {
				//		    	if(i<=limit-1) {
				//		        var $this = $(this),
				//		            item = {
				//		                title: $this.find("title").text(),
				//		                link: $this.find("link").text(),
				//		                image: $this.find("url").text(),
				//		                time: $this.find("time").text()			               
				//		        }
						      
				//		        $('#latest_journal .journal_record:eq('+i+')').attr('href',item.link);
						        
				//		        $('#latest_journal .journal_record:eq('+i+') .pic_box img').attr('src',item.image);
						       
						        
				//		        $('#latest_journal .journal_record:eq('+i+') .journal_title p span.j_title').text(item.title);
				//		        $('#latest_journal .journal_record:eq('+i+') .journal_title p span.j_time').text(item.time);

						       
				//		    	}
						    	
						       
				//		    });
				//	},
				//	dataType: 'xml'
				//	});
				
				
				
			    //
				if (mobileCheck) {
			        $('.journal_record').each(function (i) {
			            setTimeout(function () {
			                $('.journal_record:eq(' + i + ') .cover').removeClass('hidden');
			                $('.journal_record:eq(' + i + ') .content').removeClass('hidden');
			            }, 150 * i)
			            setTimeout(function () {
			                $('.journal_record:eq(' + i + ') .journal_title').removeClass('top_double');
			            }, 1000 + (100 * (i + 1)))

			        });
			    }
				if (!mobileCheck && !video_loaded) {
				  
					video_loaded = true;
				setTimeout(function(){
					$('.video_panel').append('<div class="size_container self_showing has_transition_1600_expo"><video muted onclick="launch_video(this)" playsinline autoplay width="100%" height="100%" loop><source src="/Template/images/ca2.mp4" type="video/mp4"></video></div>');
                    $('.video_panel .size_container').height("100%");
				},1000);
				} else {
				    $('.pic_big').eq(0).removeClass('active');
				    $('.pic_big').eq(1).addClass('active');
				    $('.pic_big').eq(1).css('transform', 'translate3d(0px, 0px, 0px)');
				}
			});

			setTimeout(function(){
				$('#header img.desktop').removeClass('scaled');
				$('#main_veil').addClass('hidden').removeClass('grey');
				$('.white_panel').removeClass('hidden');
				$('.c_letter').removeClass('hidden');
				
			}, 100);
			
			setTimeout(function(){
				$('.casangelina').removeClass('top_hidden');
				$('#header img.desktop').addClass('scroll_transition');
			},1200);
			
			setTimeout(function(){
				$('.lifestyle').removeClass('top_hidden');	
			},1500);
			
			setTimeout(function(){
				$('#true_perfection hr').removeClass('no_width');
				popEnabled = true;
				loadEnabled = true;

			},1500);
			
			setTimeout(function(){
				$('#true_perfection .true').removeClass('top_hidden');	
				$('#true_perfection .perfection').removeClass('bottom_hidden');	
			},1900);


		    //setTimeout(function() {
		    //    $('#right_content .menu_controller .lines > hr').each(function(i) {
		    //        setTimeout(function() {
		    //            $('#right_content .menu_controller .lines > hr:eq(' + i + ')').removeClass('no_width');
		    //        }, 100 * i);
		    //    });
		    //}, 2500);
			setTimeout(function () {
			    $('#menucontrol').removeClass('no_opacity');
			}, 2900)
			setTimeout(function(){
					$('#right_content .book_now_top').removeClass('no_opacity');
			},2800)

		    setTimeout(function() {
		        $('#scroll_down').removeClass('no_opacity');
		    }, 2900);
		    setTimeout(function() {
		        $('#scroll_down1').removeClass('no_opacity');
		    }, 2900);
			setTimeout(function(){
				$('#true_mobile hr').removeClass('no_width');	
			},1400);
			
			setTimeout(function(){
				$('#true_mobile .true_mobile').removeClass('top_hidden');	
				$('#true_mobile .perfection_mobile').removeClass('bottom_hidden');	

			},1800);
			

		})
		
		/*
		 * danno  problemi
		 * setTimeout(function(){
			if(scrollType == "iScroll"){
				myScroll.refresh();
			}
			index_offsets();
		},3000)
		
		setTimeout(function(){
			if(scrollType == "iScroll"){
				myScroll.refresh();
			}
			index_offsets();

		},6000)*/ 
	
		
}



function index_resize () {
	if(!isHandheld){
		understatedHeight = $('#understated_pic').height();
		check_header();
	}
	experience_box_setup();
	index_offsets ();
	
	if(scrollType == "iScroll"){
		myScroll.refresh();
	}
	
    $('.video_panel .size_container').height("100%");
	

}

function index_scroll_desktop() {
	
	
	
	if(!isHandheld){
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
	
	if(pageY > o_chic-windowHeight && pageY < e_chic){
		//$('#understated_pic').css($$transform[0],'translateY('+(windowHeight/2 + ((pageY/2) + (o_chic/2)))+'px)');

	    console.log("L" + ((-(windowWidth / 6) + ((pageY - (o_chic - understatedHeight / 1.3)) / 3.5))));
	    console.log("L"+windowWidth);
	    var toado = ((-(windowWidth / 6) + ((pageY - (o_chic - understatedHeight / 1.3)) / 3.5)));
	    console.log(toado);
	    if (toado >= -10 && toado <= 71) {
	       
	        $chic_el_1.css($$transform[0], 'translateY(152.9445px)');
	        $chic_el_2.css($$transform[0], 'translateY(34.0064px)');
	        $chic_el_3.css($$transform[0], 'translateY(184.73282px)');
	        $chic_el_4.css($$transform[0], 'translateY(138.8893px)');
	        $chic_el_5.css($$transform[0], 'translateY(158.4476px)');
	        $chic_el_6.css($$transform[0], 'translateY(153.96175px)');
	       
	    }
	    else if (toado >= 72 && toado <= 180)
	    {
	        $chic_el_1.css($$transform[0], 'translateY(152.9445px)');
	        $chic_el_2.css($$transform[0], 'translateY(295.435px)');
	        $chic_el_3.css($$transform[0], 'translateY(184.73282px)');
	        $chic_el_4.css($$transform[0], 'translateY(243.689px)');
	        $chic_el_5.css($$transform[0], 'translateY(274.5905px)');
	        $chic_el_6.css($$transform[0], 'translateY(153.96175px)');
	    }
		 else 
	    {
	     $chic_el_1.css($$transform[0],'translateY('+ (-(windowWidth/9)+((pageY-(o_chic-understatedHeight/1.3))/5.2)) +'px)');
		$chic_el_2.css($$transform[0], 'translateY(' + (-(windowWidth / 6) + ((pageY - (o_chic - understatedHeight / 1.3)) / 3.5)) + 'px)');
		$chic_el_3.css($$transform[0],'translateY('+ (-(windowWidth/15)+((pageY-(o_chic-understatedHeight/1.3))/8.2)) +'px)');
		$chic_el_4.css($$transform[0],'translateY('+ (-(windowWidth/11)+((pageY-(o_chic-understatedHeight/1.3))/5)) +'px)');
		$chic_el_5.css($$transform[0],'translateY('+ (-(windowWidth/18)+((pageY-(o_chic-understatedHeight/1.3))/7)) +'px)');
		$chic_el_6.css($$transform[0],'translateY('+ (-(windowWidth/20)+((pageY-(o_chic-understatedHeight/1.3))/9)) +'px)');
	    }
	    //$chic_el_1.css($$transform[0], 'translateY(' + (-(windowWidth / 9) + ((pageY - (o_chic - understatedHeight / 1.3)) / 5.2)) + 'px)');
	    //$chic_el_2.css($$transform[0], 'translateY(' + (-(windowWidth / 6) + ((pageY - (o_chic - understatedHeight / 1.3)) / 3.5)) + 'px)');
	    //$chic_el_3.css($$transform[0], 'translateY(' + (-(windowWidth / 15) + ((pageY - (o_chic - understatedHeight / 1.3)) / 8.2)) + 'px)');
	    //$chic_el_4.css($$transform[0], 'translateY(' + (-(windowWidth / 11) + ((pageY - (o_chic - understatedHeight / 1.3)) / 5)) + 'px)');
	    //$chic_el_5.css($$transform[0], 'translateY(' + (-(windowWidth / 18) + ((pageY - (o_chic - understatedHeight / 1.3)) / 7)) + 'px)');
	    //$chic_el_6.css($$transform[0], 'translateY(' + (-(windowWidth / 20) + ((pageY - (o_chic - understatedHeight / 1.3)) / 9)) + 'px)');
	}
	
	if(!v_blockTitle && pageY > o_blockTitle - windowHeight/2) {
		v_blockTitle = true;
		$('.main_block_title .desc_text').removeClass('top_hidden');
		setTimeout(function(){
			$('.main_block_title .desc_title').removeClass('top_hidden');
		},100);
		
			$('.main_block_title .diagonal_line').removeClass('hidden');
		
		
	}
	
	if(!v_experience && pageY > o_experience - windowHeight + (scroll_threshold/2)) {
		v_experience = true;
		$('#experience_box, #experience_box > .right.body').height(Math.round((windowWidth/2)/1.7777));

		$('#experience_box .cover,#experience_box .content').removeClass('hidden');
		$('#experience_box  .box_title').removeClass('top_hidden');
		setTimeout(function(){
			$('#experience_box .exp_data.active .desc_title').removeClass('top_hidden');
			$('#experience_box .exp_data.active .desc_text').removeClass('bottom_hidden');
			$('#experience_box .exp_data.active .section_separator .pointer').removeClass('no_width');
		},300)
		
		setTimeout(function(){
			$('#experience_box .exp_data.active .more .line').removeClass('no_height');
			
		},500)
		setTimeout(function(){
			$('#experience_box .exp_data.active .more p').removeClass('top_single');	
		},700)
		setTimeout(function(){
			$('#experience_controller .circle_button.right .back').removeClass('hidden_by_scaling_full');	
		},800)
		setTimeout(function(){
			$('#experience_controller .circle_button.right .arrow').removeClass('hidden');	
			$('#experience_controller .controller_right p').removeClass('top_hidden');	

		},1000)
		
		
	}
	
	if(!v_journal_title && pageY > o_journal_title - windowHeight + scroll_threshold) {
		v_journal_title = true;
		$('.section_title').removeClass('top_hidden');
	}
	
	if(!v_journal_box && pageY > o_journal_box - windowHeight + scroll_threshold) {
		v_journal_box = true;
		$('.journal_record').each(function(i){
			setTimeout(function(){
				$('.journal_record:eq('+i+') .cover').removeClass('hidden');
				$('.journal_record:eq('+i+') .content').removeClass('hidden');
			},150*i)
			setTimeout(function(){
				$('.journal_record:eq('+i+') .journal_title').removeClass('top_double');
			},1000+(100*(i+1)))
			
		});
		
	}
	
	for(i=0;i<stripe_length;i++) {
    	if(!stripes_visible[i] && pageY >  stripes_offset[i] - windowHeight + scroll_threshold){
    		stripes_visible[i] = true;
    		show_strip(i);
    	}	
	};
	
	/*if(pageY > o_flowers - windowHeight && pageY < o_flowers + windowHeight){
		$flowers.style[Modernizr.prefixed('transform')] = 'translate3d(0,'+(-$flowersHeight-((o_flowers - windowHeight)-pageY))/3+'px, 0)';
	}*/
}




function index_load () {
	
}


function show_strip(i){
	
	$('.stripe:eq('+i+') >*').each(function(n){
	setTimeout(function(){
		$('.stripe:eq('+i+') >*:eq('+n+')').removeClass('top_translated');
	},100*n)
})
}

function experience_box_setup() {
	$totalExp = $('.exp_box_pic').length;
	if(!isHandheld) {
		$('#experience_box_scroller').width((windowWidth/2) * $totalExp);

		$('#experience_box, #experience_box > .right.body').height(Math.round((windowWidth/2)/1.7777));
	} else {
		$('#experience_box_scroller').width((windowWidth) * $totalExp);

	}
}

function experience_move () {
	if($(this).hasClass('controller_left')){
		$out = $('.exp_data.active');
		$in = $('.exp_data.active').prev();
	} else {
		$out = $('.exp_data.active');
		$in = $('.exp_data.active').next();
	}
	
	$this = $(this);
	
	if($in.prev().index() == -1){
		$('.circle_button.left .back').addClass('hidden_by_scaling_full');
		$('.circle_button.left .arrow').addClass('hidden');	
		$('.controller_left').css('pointer-events','none');
	}
	
	if($in.next().index() == -1){
		$('.circle_button.right .back').addClass('hidden_by_scaling_full');
		$('.circle_button.right .arrow').addClass('hidden');	
		$('.controller_right').css('pointer-events','none');

	}
	
	
	
	$('.exp_data .desc_title').addClass('top_hidden');
	$('.exp_data .desc_text').addClass('bottom_hidden');
	$('.exp_data .pointer').addClass('no_width');
	$('.exp_data .line').addClass('no_height');
	$('.exp_data .more p').addClass('top_single');
	$('#experience_controller .controller_text').addClass('top_hidden');
	if(!mobileCheck ) {
	$('#experience_box_scroller').css($$transform[0], 'translateX(-'+(windowWidth/2)*$in.index()+'px)');
	} else {
		$('#experience_box_scroller').css($$transform[0], 'translateX(-'+windowWidth*$in.index()+'px)');

	}
	
	setTimeout(function(){
		$('.exp_data').removeClass('active');
		$in.addClass('active');
		if($this.hasClass('controller_left')){
			console.log($('.desc_title',$in.prev()));
			$('#experience_controller .controller_left .controller_text').text($('.desc_title',$in.prev()).text());
			$('#experience_controller .controller_right .controller_text').text($('.desc_title',$out).text());
		} else {
			$('#experience_controller .controller_left .controller_text').text($('.desc_title',$out).text());
			$('#experience_controller .controller_right .controller_text').text($('.desc_title',$in.next()).text());
		}
	},580)
	
	setTimeout(function(){
		$('.desc_title',$in).removeClass('top_hidden');
		$('.desc_text',$in).removeClass('bottom_hidden');
		$('.section_separator .pointer',$in).removeClass('no_width');
	},600)
	
	setTimeout(function(){
		$('.more .line',$in).removeClass('no_height');
		
	},800)
	setTimeout(function(){
		$('.more p',$in).removeClass('top_single');	
	},1000)
	setTimeout(function(){
		if($in.next().index() != -1){
			$('.circle_button.right .back').removeClass('hidden_by_scaling_full');
		}
		if($in.prev().index() != -1){
			$('.circle_button.left .back').removeClass('hidden_by_scaling_full');
		}
	},600)
	setTimeout(function(){
		if($in.next().index() != -1){
		$('.circle_button.right .arrow').removeClass('hidden');	
		$('.controller_right p').removeClass('top_hidden');
		$('.controller_right').css('pointer-events','all');

		}
		if($in.prev().index() != -1){
			$('.circle_button.left .arrow').removeClass('hidden');	
			$('.controller_left p').removeClass('top_hidden');
			$('.controller_left').css('pointer-events','all');

		}

	},800);
}














/********************** MOBILE *************************/

function index_ready_mobile () {
	stripes_visible = [false,false,false,false];
	stripe_length = $('.stripe').length;
	stripes_offset = [];
	v_first_paragraph = false;
	v_blockTitle = false;
	v_experience = false;
	v_journal_title = false;
	v_journal_box = false;
    has_gallery = false;
	$('#experience_controller .controller_right, #experience_controller .controller_left').bind('click',experience_move);
	
	
	index_offsets();

	$totalExp = $('.exp_box_pic').length;
	$('#experience_box_scroller').width(windowWidth * $totalExp);


    $('#header img.mobile').imagesLoaded(function() {
        $('body').removeClass('preload');

        $('img[data-src]').each(function() {
            $path = $(this).attr('data-src');
            $(this).attr('src', $path);
        });

        $('img').imagesLoaded(function() {

            index_offsets();

            if (scrollType == "iScroll") {
                myScroll.refresh();
            }
            //$.ajax({
            //	  url: 'http://journal.casangelina.com/feed/post',
            //	  success: function(data) {
            //		  var $xml = $(data);
            //		    var limit = 3;
            //		    $xml.find("item").each(function(i) {
            //		    	if(i<=limit-1) {
            //		        var $this = $(this),
            //		            item = {
            //		                title: $this.find("title").text(),
            //		                link: $this.find("link").text(),
            //		                image: $this.find("url").text(),
            //		                time: $this.find("time").text()			               
            //		        }

            //		        $('#latest_journal .journal_record:eq('+i+')').attr('href',item.link);

            //		        $('#latest_journal .journal_record:eq('+i+') .pic_box img').attr('src',item.image);


            //		        $('#latest_journal .journal_record:eq('+i+') .journal_title p span.j_title').text(item.title);
            //		        $('#latest_journal .journal_record:eq('+i+') .journal_title p span.j_time').text(item.time);


            //		    	}


            //		    });
            //	},
            //	dataType: 'xml'
            //	});


            if (mobileCheck) {
                $('.journal_record').each(function(i) {
                    setTimeout(function() {
                        $('.journal_record:eq(' + i + ') .cover').removeClass('hidden');
                        $('.journal_record:eq(' + i + ') .content').removeClass('hidden');
                    }, 150 * i)
                    setTimeout(function() {
                        $('.journal_record:eq(' + i + ') .journal_title').removeClass('top_double');
                    }, 1000 + (100 * (i + 1)))

                });
            }
            //!mobileCheck &&
            if (!mobileCheck && !video_loaded) {

                video_loaded = true;
                setTimeout(function() {
                    $('.video_panel').append('<div class="size_container self_showing has_transition_1600_expo"><video muted onclick="launch_video(this)" playsinline autoplay width="100%" height="100%" loop><source src="/Template/images/ca2.mp4" type="video/mp4"></video></div>');
                    $('.video_panel .size_container').height("100%");
                }, 1000);
            } else {
               // $('.pic_big').eq(0).removeClass('active');
               // $('.pic_big').eq(1).addClass('active');
               // $('.pic_big').eq(1).css('transform', 'translate3d(0px, 0px, 0px)');
            }
        });

        if ($('#fullscreen_gallery').length != 0) {
            $('#fullscreen_gallery').data('html', $('#fullscreen_gallery').html());
            $('#fullscreen_gallery .video_panel').each(function (i) {
                $(this).parents('.pic_big').remove();
            });
            $('#fullscreen_gallery .pic_big').eq(0).addClass('active');
            gallery_ready();
            has_gallery = true;
            $('#fullscreen_gallery .pic_big:eq(0) img').imagesLoaded(function() {
                gallery_load();
                index_offsets();
            });
        } else {
            has_gallery = false;
            index_offsets();

        }


        $('#main_veil').addClass('hidden');

        setTimeout(function() {
            $('#side_mobile').removeClass('hidden');
        }, 800);

        setTimeout(function() {
            $('#true_mobile hr').removeClass('no_width');
        }, 1000);

        setTimeout(function() {
            $('#true_mobile .true_mobile').removeClass('top_hidden');
            $('#true_mobile .perfection_mobile').removeClass('bottom_hidden');

        }, 1400);

        setTimeout(function() {
            $('#scroll_mobile').removeClass('top_double');

        }, 1400);


    });

    if ($('.paragraph.first .body_text').html().indexOf('<br') != -1) {


        var vi = getCookie("Language");
       var changeText = vi === "en-GB" ? "less" : "thu gọn";
       var changeText1 = vi === "vi-VN" ? "thu gọn" : "less";
			$('.paragraph.first .body_text').shorten({
				showChars: $('.paragraph.first .body_text').html().indexOf('<br'),
				ellipsesText: "",
				moreText: '<span class="langMore">' + changeText + '</span>',
				lessText: '<span class="langLess">' + changeText1 + '</span>'
			});
			
			}
}





function index_scroll_mobile () {
	if(!v_first_paragraph && pageY > o_first_paragraph - windowHeight + scroll_threshold) {
		v_first_paragraph = true;
		$('.paragraph.first .big_title').removeClass('top_hidden');
		setTimeout(function(){
			$('.paragraph.first .big_subtitle').removeClass('top_hidden');
		},200);
		$('.paragraph.first .body_text').removeClass('top_translated');
	}
	
	if(!v_blockTitle && pageY > o_blockTitle - windowHeight + scroll_threshold) {
		v_blockTitle = true;
		$('.main_block_title .desc_text').removeClass('top_hidden');
		setTimeout(function(){
			$('.main_block_title .desc_title').removeClass('top_hidden');
		},100);
		
			$('.main_block_title .diagonal_line').removeClass('hidden');
		
		
	}
	
	if(!v_experience && pageY > o_experience - windowHeight - 130) {
		v_experience = true;
		$('#experience_box .cover,#experience_box .content').removeClass('hidden');
		$('#experience_box  .box_title').removeClass('top_hidden');
		setTimeout(function(){
			$('#experience_box .exp_data.active .desc_title').removeClass('top_hidden');
			$('#experience_box .exp_data.active .desc_text').removeClass('bottom_hidden');
			$('#experience_box .exp_data.active .section_separator .pointer').removeClass('no_width');
		},300)
		
		setTimeout(function(){
			$('#experience_box .exp_data.active .more .line').removeClass('no_height');
			
		},500)
		setTimeout(function(){
			$('#experience_box .exp_data.active .more p').removeClass('top_single');	
		},700)
		setTimeout(function(){
			$('#experience_controller .circle_button.right .back').removeClass('hidden_by_scaling_full');	
		},800)
		setTimeout(function(){
			$('#experience_controller .circle_button.right .arrow').removeClass('hidden');	
			$('#experience_controller .controller_right p').removeClass('top_hidden');	

		},1000)
		
		
	}
	
	
	for(i=0;i<stripe_length;i++) {
		if(!stripes_visible[i] && pageY >  stripes_offset[i] - windowHeight + scroll_threshold){
    		stripes_visible[i] = true;
    		show_strip(i);
    	}	
	};
}


/***********************************************************/


function check_header () {
	windowWidth = $(window).width();
	windowHeight = $(window).height();

	viewport_width = windowWidth;
	viewport_height = windowHeight;
	
    screen_ratio = viewport_width / viewport_height;
    pic_ratio = 1920 / 1080;

    if (pic_ratio > screen_ratio) {
        $('#header > img.desktop').css({
            height: viewport_height,
            width: Math.round(viewport_height * pic_ratio),
        })
    } else {
        $("#header > img.desktop").css({
            width: viewport_width,
            height: Math.round(viewport_width / pic_ratio),
        })
    }
}