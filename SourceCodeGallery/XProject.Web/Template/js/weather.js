function set_weather(day,data){
                
				if($(data).find('day:eq('+day+') hour').length == 8){
			    	if($nowHour < 3 ){
			    		now_node = 0;
			    		actual_node = now_node;

			    	} else {
			    		now_node = Math.floor($nowHour / 3);
			    		actual_node = now_node;
			    	}
			    } else {
		    		now_node = Math.floor($nowHour / 6);
		    		actual_node = now_node + 4;
		    		
		    		 $('.hour_box').each(function(h){
		    			 if(h < 4){
		    				 $(this).addClass('no_opacity');
		    			 }
		    		 });

			    }
	
                // SET WHICH HOUR IS ACTIVE //
                
                $('.hour_box:eq('+actual_node+')').addClass('active');
                
                //DAY ICO //
                
                day_ico_0 = $(data).find('day:eq('+day+') hour[value="00:00"] description').text();
                day_ico_1 = $(data).find('day:eq('+day+') hour[value="03:00"] description').text();
                day_ico_2 = $(data).find('day:eq('+day+') hour[value="06:00"] description').text();
                day_ico_3 = $(data).find('day:eq('+day+') hour[value="09:00"] description').text();
                day_ico_4 = $(data).find('day:eq('+day+') hour[value="12:00"] description').text();
                day_ico_5 = $(data).find('day:eq('+day+') hour[value="15:00"] description').text();
                day_ico_6 = $(data).find('day:eq('+day+') hour[value="18:00"] description').text();
                day_ico_7 = $(data).find('day:eq('+day+') hour[value="21:00"] description').text();
                
                $('.hour_box').each(function(h){
                	if((h == 0 || h == 1 || h == 7) && (window['day_ico_'+h] == 1 || window['day_ico_'+h] == 2 || window['day_ico_'+h] == 3)){
                	$('.hour_box:eq('+h+') .hour_ico img').attr('src','/style/images/weather/n'+window['day_ico_'+h]+'.png');
                	} else {
                    	$('.hour_box:eq('+h+') .hour_ico img').attr('src','/style/images/weather/'+window['day_ico_'+h]+'.png');

                	}
                });
                
                $('.hour_box .hour_ico img').imagesLoaded(function(){
                	if(scrollType == "iScroll"){
                		myScroll.refresh();
                	}
                });
                
                // DAY DESCRIPTION //
                
                $('#meteo_box .weather_ico .meteo_stats').text(get_day_text(window['day_ico_'+actual_node]));
                $('#meteo_box .weather_ico .meteo_ico_big img').attr('src','/style/images/weather/'+window['day_ico_'+actual_node]+'.png');
                
                if((actual_node == 0 || actual_node == 1 || actual_node == 7 && window['day_ico_'+actual_node]) && (window['day_ico_'+actual_node] == 1 || window['day_ico_'+actual_node] == 2 || window['day_ico_'+actual_node] == 3)) {
                    $('#meteo_box .weather_ico .meteo_ico_big img').attr('src','/style/images/weather/n'+window['day_ico_'+actual_node]+'.png');
                }

                
                // WIND DIR //
                
                $windDirection = $(data).find('day:eq('+day+') hour:eq(' + now_node + ') windir').text();
                $('#meteo_box .wind_direction p').text(get_wind_dir($windDirection));
                
                // WIND SPEED //
                
                $windSpeed = $(data).find('day:eq('+day+') hour:eq(' + now_node + ') windvel').text();
                $('#meteo_box .wind_ico .meteo_stats').text(get_wind_speed($windSpeed));
                $('#meteo_box .wind_ico img').attr('src','/style/images/weather/wind/'+get_wind_ico($windSpeed)+'.png');
                $('#meteo_box .wind_speed p').text($windSpeed + ' m/s');
                
                // TEMP //
                
                $temp = Math.round(parseInt($(data).find('day:eq('+ day +') hour:eq(' + now_node + ') temp').text()));
                $('.temp_text .temp_num').text($temp);																		
              
 };
 
 function set_marine(day,data){
     
     $nowDate = new Date();
     $nowHour = $nowDate.getHours();               
     
     if($(data).find('day:eq('+day+') hour').length == 8){
     	if($nowHour < 3 ){
     		now_node = 0;
     	} else {
     		now_node = Math.floor($nowHour / 3);
     	}
     }
     
     $waveHeight = $(data).find("day:eq(" + day + ") hour:eq(" + now_node + ") waveheight").text();
     if ($waveHeight == "") {
    	 $waveHeight = 0
     }
     
     
     //WAVE HEIGHT //
     
     $('#meteo_box .wave_height p').text($waveHeight + ' M');
     $('#meteo_box .sea_ico .meteo_ico_big img').attr('src','/style/images/weather/sea/'+get_wave_pic($waveHeight)+'.png');
     $('#meteo_box .sea_ico .meteo_stats').text(get_wave_height($waveHeight));



   
};


function setWeatherData() { // MAIN LAUNCHER //
            
	
	
	$.get("/weather/get-weather", function(data) {
		
		weatherData = data;
		
	    $nowDate = new Date();
        $nowHour = $nowDate.getHours();
        
        switch (lang) {
        case "en":
            days_short = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
            days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            months = month; 
            break;
        case "it":
            days_short = ["Dom", "Lun", "Mar", "Mer", "Gio", "Ven", "Sab"];
            days = ["Domenica", "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"];
            months = mesi; 
            break;
        case "fr":
            days_short = ["Dim", "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam"];
            days = ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"];
        	months = mois;
            break
        case "de":
            days_short = ["Son", "Mon", "Die", "Mit", "Don", "Fre", "Sam"];
            days = ["Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag"];
        	months = monate;
            break;
        }
        
        switch (lang) {
            case "en":
              
                break;
            case "it":
               
                break;
            case "fr":
            	
                break;            
            case "de":
            	
                break
        }
        
        // SETUP CALENDAR //
        
        $('#date .year').text($nowDate.getFullYear());
        if(lang == 'en'){
        $('#date .day').text(days[$nowDate.getDay()] + ' ' +$nowDate.getDate()+''+nth($nowDate.getDate())+' ' + months[($nowDate.getMonth())])
        } else {
            $('#date .day').text($nowDate.getDate()+' '+ months[($nowDate.getMonth())]+', '+days[$nowDate.getDay()] ) 

        }
        
       
        
        // SETUP FORECAST DAYS //
        $day0 = new Date();
        $day1 = addDays($day0,1);
        $day2 = addDays($day0,2);
        $day3 = addDays($day0,3);
        $day4 = addDays($day0,4);
        $day5 = addDays($day0,5);
        $day6 = addDays($day0,6);
        
        $('#days_box .day_name').each(function(d){
        	if(d > 0){
        		$('#days_box .day_name:eq('+d+') p').text(days_short[window['$day'+d].getDay()]);
        	}
        });
        
        
        
        $('#days_box .day_name').bind('click',manage_forecast);
		
                    
            set_weather(0,weatherData);
            
            $.get("/weather/get-sea", function(data) {
            	seaData = data;
            	set_marine(0,seaData);

            })
        });
}

function manage_forecast () {
	$dayIndex = $(this).index();
	
	 $('#days_box .day_name').each(function(d){
     	if(d > 0){
     		$('#days_box .day_name:eq('+d+') p').text(days_short[window['$day'+d].getDay()]);
     	}
     });
	
	$('.day_name').removeClass('active');
	$(this).addClass('active');
	
	if($dayIndex>0){
		$(this).find('p').text(days[window['$day'+$dayIndex].getDay()]);
	}
	
	weatherOut();
	setTimeout(function(){
		$('#meteo_box .animating').addClass('no_transition').addClass('top_double').removeClass('no_opacity');
	},480);
	setTimeout(function(){
		$('#meteo_box .animating').removeClass('no_transition'); 
		set_weather($dayIndex,weatherData);
		 set_marine($dayIndex,seaData);
		 weatherIn ();
		 
	},500);
}

function weatherIn () {
	$('#meteo_box .animating').each(function(w) {
		setTimeout(function(){
			$('#meteo_box .animating:eq('+w+')').removeClass('top_double');
		},50*w);
	})
}

function weatherOut () {
	$('#meteo_box .animating').each(function(w) {
		setTimeout(function(){
			$('#meteo_box .animating:eq('+w+')').addClass('no_opacity');
		},20*w);
	})
}


function get_wind_dir(wind_direction) {
    if (wind_direction >= 0 && wind_direction <= 11.25) {
        return "N"
    }
    if (wind_direction >= 11.26 && wind_direction <= 33.75) {
        return "NNE"
    }
    if (wind_direction >= 33.76 && wind_direction <= 56.25) {
        return "NE"
    }
    if (wind_direction >= 56.26 && wind_direction <= 78.75) {
        return "ENE"
    }
    if (wind_direction >= 78.76 && wind_direction <= 101.25) {
        return "E"
    }
    if (wind_direction >= 101.26 && wind_direction <= 123.75) {
        return "ESE"
    }
    if (wind_direction >= 123.76 && wind_direction <= 146.25) {
        return "SE"
    }
    if (wind_direction >= 146.26 && wind_direction <= 169.75) {
        return "SSE"
    }
    if (wind_direction >= 169.76 && wind_direction <= 191.25) {
        return "S"
    }
    if (wind_direction >= 191.26 && wind_direction <= 213.75) {
        return "SSW"
    }
    if (wind_direction >= 213.76 && wind_direction <= 236.25) {
        return "SW"
    }
    if (wind_direction >= 236.26 && wind_direction <= 258.75) {
        return "WSW"
    }
    if (wind_direction >= 258.76 && wind_direction <= 281.25) {
        return "W"
    }
    if (wind_direction >= 281.26 && wind_direction <= 303.75) {
        return "WNW"
    }
    if (wind_direction >= 303.76 && wind_direction <= 326.25) {
        return "NW"
    }
    if (wind_direction >= 326.26 && wind_direction <= 348.75) {
        return "NNW"
    }
    if (wind_direction >= 348.76 && wind_direction <= 360) {
        return "N"
    }
}

function get_wind_speed(wind_speed) {
    if (wind_speed == 0) {
        switch (lang) {
            case "en":
                return "no wind";
                break;
            case "it":
                return "assente";
                break;
            case "fr":
                return "pas de vent";
                break;
            case "de":
                return "kein wind";
                break
        }
    }
    
    if (wind_speed >= .1 && wind_speed < 5) {
        switch (lang) {
            case "en":
                return "weak";
                break;
            case "it":
                return "debole";
                break;
            case "fr":
                return "faible";
                break;
            case "de":
                return "leichter";
                break;
                
        }
    }
    if (wind_speed >= 5 && wind_speed < 10) {
        switch (lang) {
            case "en":
                return "moderate";
                break;
            case "it":
                return "debole";
                break;
            case "fr":
                return "modéré";
                break;
            case "de":
                return "mäßiger";
                break;
        }
    }
    if (wind_speed >= 10 && wind_speed < 17) {
        switch (lang) {
            case "en":
                return "strong";
                break;
            case "it":
                return "forte";
                break;
            case "fr":
                return "fort";
                break;
            case "en":
                return "starker";
                break;
        }
    }
    if (wind_speed >= 17) {
        switch (lang) {
            case "en":
                return "very strong";
                break;
            case "it":
                return "molto forte";
                break;
            case "fr":
                return "trés fort";
                break;
            case "de":
                return "sehr starker";
                break;    
                
        }
    }
}

function get_day_text(day_weather) {
    if (day_weather == 1) {
        switch (lang) {
            case "en":
                return "sunny";
                break;
            case "it":
                return "sereno";
                break;
            case "fr":
                return "soleil";
                break;
            case "de":
                return "sonnig";
                break;
        }
    }
    if (day_weather == 2) {
        switch (lang) {
            case "en":
                return "sunny";
                break;
            case "it":
                return "sereno";
                break;
            case "fr":
                return "soleil";
                break;
            case "de":
                return "sonnig";
                break;
        }
    }
    if (day_weather == 3) {
        switch (lang) {
            case "en":
                return "partly cloudy";
                break;
            case "it":
                return "poco nuvoloso";
                break;
            case "fr":
                return "partiellement nuageux";
                break;
            case "de":
                return "teils bewölkt";
                break;
        }
    }
    if (day_weather == 4) {
        switch (lang) {
            case "en":
                return "partly cloudy";
                break;
            case "it":
                return "nubi sparse";
                break;
            case "fr":
                return "nuages ​​épars";
                break;
            case "de":
                return "bewölkt";
                break;
        }
    }
    if (day_weather == 5) {
        switch (lang) {
            case "en":
                return "rain and sunny intervals";
                break;
            case "it":
                return "pioggia e schiarite";
                break;
            case "fr":
                return "pluie";
                break;
            case "de":
                return "bewölkt mit regen";
                break;
        }
    }
    if (day_weather == 6) {
        switch (lang) {
            case "en":
                return "rain / snow and sunny intervals";
                break;
            case "it":
                return "pioggia mista a neve e schiarite";
                break;
            case "fr":
                return "pluie et neige";
                break;
            case "de":
                return "regen mit schnee";
                break;
        }
    }
    if (day_weather == 7) {
        switch (lang) {
            case "en":
                return "snow and sunny intervals";
                break;
            case "it":
                return "neve e schiarite";
                break;
            case "fr":
                return "neige";
                break;
            case "de":
                return "sonnig mit schnee";
                break;
        }
    }
    if (day_weather == 8) {
        switch (lang) {
            case "en":
                return "very cloudy";
                break;
            case "it":
                return "coperto";
                break;
            case "fr":
                return "couvert";
                break;
            case "de":
                return "bedeckt";
                break;
        }
    }
    if (day_weather == 9) {
        switch (lang) {
            case "en":
                return "weak rain";
                break;
            case "it":
                return "pioggia debole";
                break;
            case "fr":
                return "faible pluie";
                break;
            case "de":
                return "leichtem regen";
                break;
        }
    }
    if (day_weather == 10) {
        switch (lang) {
            case "en":
                return "rainy";
                break;
            case "it":
                return "pioggia";
                break;
            case "fr":
                return "pluie";
                break;
            case "de":
                return "regen";
                break;
        }
    }
    if (day_weather == 11) {
        switch (lang) {
            case "en":
                return "snow";
                break;
            case "it":
                return "neve";
                break;
            case "fr":
                return "neige";
                break;
            case "de":
                return "schnee";
                break;
        }
    }
    if (day_weather == 12) {
        switch (lang) {
            case "en":
                return "rain and snow";
                break;
            case "it":
                return "pioggia mista a neve";
                break;
            case "fr":
                return "pluie et neige";
                break;
            case "de":
                return "regen mit schnee";
                break;
        }
    }
    if (day_weather == 13) {
        switch (lang) {
            case "en":
                return "storm";
                break;
            case "it":
                return "temporale";
                break;
            case "fr":
                return "orage";
                break;
            case "de":
                return "sturm";
                break;
        }
    }
    if (day_weather == 14) {
        switch (lang) {
            case "en":
                return "fog";
                break;
            case "it":
                return "nebbia";
                break;
            case "fr":
                return "brouillarde";
                break;
            case "de":
                return "nebel";
                break;
        }
    }
    if (day_weather == 15) {
        switch (lang) {
            case "en":
                return "fog at dawn";
                break;
            case "it":
                return "nebbia al mattino";
                break;
            case "fr":
                return "brouillard le matin";
                break;
            case "de":
                return "nebel am morgen";
                break;
        }
    }
    if (day_weather == 16) {
        switch (lang) {
            case "en":
                return "storm and sunny intervals";
                break;
            case "it":
                return "temporale e schiarite";
                break;
            case "fr":
                return "orage";
                break;
            case "de":
                return "gewitter und blitz";
                break;
        }
    }
    if (day_weather == 17) {
        switch (lang) {
            case "en":
                return "hailstorm";
                break;
            case "it":
                return "grandine";
                break;
            case "fr":
                return "averse de grêle";
            break;
            case "de":
                return "hagel";
                break;
        }
    }
    if (day_weather == 18) {
        switch (lang) {
            case "en":
                return "weak snow";
                break;
            case "it":
                return "neve debole";
                break;
            case "fr":
                return "neige légère";
            break;
            case "de":
                return "schneeregene";
                break;
        }
    }
}

function get_wind_ico(wind_speed) {
    if (wind_speed == 0) {
       return 1;
    }
    
    if (wind_speed >= .1 && wind_speed < 5) {
        return 2;
    }
    if (wind_speed >= 5 && wind_speed < 10) {
       return 3;
    }
    if (wind_speed >= 10 && wind_speed < 17) {
        return 4;
    }
    if (wind_speed >= 17) {
       return 5;
    }
}

function get_wave_pic(waveHeight) {
    if (waveHeight == 0) {
        return 1;
    }
    if (waveHeight > 0 && waveHeight <= .099) {
        return 2;
    }
    if (waveHeight >= .1 && waveHeight <= .499) {
        return 3;
    }
    if (waveHeight >= .5 && waveHeight <= 1.249) {
        return 4;
    }
    if (waveHeight >= 1.25 && waveHeight <= 2.499) {
        return 5;
    }
    if (waveHeight >= 2.5 && waveHeight <= 3.999) {
        return 6;
    }
    if (waveHeight >= 4 && waveHeight <= 5.999) {
        return 7;
    }
    if (waveHeight >= 6 && waveHeight <= 8.999) {
        return 8;
    }
    if (waveHeight >= 9 && waveHeight <= 13.999) {
        return 9;
    }
    if (waveHeight >= 14) {
        return 10;
    }
}

function get_wave_height(waveHeight) {
    if (waveHeight == 0) {
        switch (lang) {
            case "en":
               return "calm (glassy)";
                break;
            case "fr":
                return "calme";
                break;
            case "it":
                return "calmo";
                break;
            case "de":
                return "ruhig (glasig)";
                 break;
        }
    }
    if (waveHeight > 0 && waveHeight <= .099) {
        switch (lang) {
            case "en":
                return "calm (rippled)";
                break;
            case "fr":
                return "calme (ridée)";
                break;
            case "it":
                return "quasi calmo";
                break;
            case "de":
                return "ruhig (wellig)";
                break;
        }
    }
    if (waveHeight >= .1 && waveHeight <= .499) {
        switch (lang) {
            case "en":
                return "smooth";
                break;
            case "fr":
                return "belle";
                break;
            case "it":
                return "poco mosso";
                break;
            case "de":
                return "smooth";
                break;
        }
    }
    if (waveHeight >= .5 && waveHeight <= 1.249) {
        switch (lang) {
            case "en":
                return "slight";
                break;
            case "fr":
                return "peu agitée";
                break;
            case "it":
                return "mosso";
                break;
            case "de":
                return "slight";
                break;
        }
    }
    if (waveHeight >= 1.25 && waveHeight <= 2.499) {
        switch (lang) {
            case "en":
                return "moderate";
                break;
            case "fr":
                return "agitée";
                break;
            case "it":
                return "molto mosso";
                break;
            case "de":
                return "moderate";
                break;
        }
    }
    if (waveHeight >= 2.5 && waveHeight <= 3.999) {
        switch (lang) {
            case "en":
                return "rough";
                break;
            case "fr":
                return "forte";
                break;
            case "it":
                return "agitato";
                break;
            case "de":
                return "rough";
                break;
        }
    }
    if (waveHeight >= 4 && waveHeight <= 5.999) {
        switch (lang) {
            case "en":
                return "very rough";
                break;
            case "fr":
                return "trés forte";
                break;
            case "it":
                return "molto agitato";
                break;
            case "de":
                return "very rough";
                break;
        }
    }
    if (waveHeight >= 6 && waveHeight <= 8.999) {
        switch (lang) {
            case "en":
                return "high";
                break;
            case "fr":
                return "grosse";
                break;
            case "it":
                return "grosso";
                break;
            case "de":
                return "very rough";
                break;
        }
    }
    if (waveHeight >= 9 && waveHeight <= 13.999) {
        switch (lang) {
            case "en":
                return "very high";
                break;
            case "fr":
                return "trés grosse";
                break;
            case "it":
                return "molto grosso";
                break;
            case "de":
                return "very rough";
                break;
        }
    }
    if (waveHeight >= 14) {
        switch (lang) {
            case "en":
                return "phenomenal";
                break;
            case "fr":
                return "énorme";
                break;
            case "it":
                return "tempestoso";
                break;
            case "de":
                return "phenomenal";
                break;
        }
    }
}

function nth(d) {
	  if(d>3 && d<21) return 'th'; 
	  switch (d % 10) {
	        case 1:  return "st";
	        case 2:  return "nd";
	        case 3:  return "rd";
	        default: return "th";
	    }
	} 