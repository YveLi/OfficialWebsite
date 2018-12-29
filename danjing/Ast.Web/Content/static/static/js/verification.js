// 验证

// 　　兼容其他不支持placeholder的浏览器：
　　var PlaceHolder = { 
　　_support: (function() { 
　　return 'placeholder' in document.createElement('input'); 
　　})(), 
　　//提示文字的样式，需要在页面中其他位置定义 
　　className: 'vericode', 
　　init: function() { 
　　if (!PlaceHolder._support) { 
　　//未对textarea处理，需要的自己加上 
// 　　var inputs = document.getElementsByTagName('input'); 
	var inutP = document.getElementById("inputparent");
　　var inputs = inutP.getElementsByTagName('input');
　　PlaceHolder.create(inputs); 

	inputs[2].className = "verification-code vericode";
        } 
    }, 
　　create: function(inputs) { 
　　var input; 
　　if (!inputs.length) { 
　　inputs = [inputs]; 
        } 
　　for (var i = 0, length = inputs.length; i <length; i++) { 
　　input = inputs[i]; 

　　if (!PlaceHolder._support && input.attributes && input.attributes.placeholder) { 
　　PlaceHolder._setValue(input); 
            } 
        } 
    }, 
　　_setValue: function(input) { 
　　input.value = input.attributes.placeholder.nodeValue; 
　　input.className =PlaceHolder.className; 
	// input[2]["className"] = "";
    } 
}; 
PlaceHolder.init();
　　//页面初始化时对所有input做初始化 
　　//PlaceHolder.init(); 
　　//或者单独设置某个元素 
　　//PlaceHolder.create(document.getElementById('t1'));
	
	var zPassport = {
		ConuntryCodePhoneShowEffectEle:null,
		getConuntryCodePhoneShowEffectEle:function(thisPhoneInput){
			// 获取有区号 huo 没区号的显示效果的dom
			zPassport.ConuntryCodePhoneShowEffectEle = thisPhoneInput;
			if($('#country-area-code').length){
				// 有区号的手机框显示效果的dom
				zPassport.ConuntryCodePhoneShowEffectEle = thisPhoneInput.parents('.country-phone-covers');
				return zPassport.ConuntryCodePhoneShowEffectEle;
			}
		},
		mainlandConuntryCode:"+86",
		mainlandAreaCodeEle:$('#country-code-ipt'),
		countryCodeIsn:function(){
			// 判断区号是否shi显示
			// 忘记密码页用到，邮箱，手机号是一个input
			if(!$('#country-area-code').is(':visible')){
				zPassport.mainlandAreaCodeEle = $('');
			}else{
				zPassport.mainlandAreaCodeEle = $('#country-code-ipt');
			}
			// 判断地区号是不是等于“+86” 
			if($.trim(zPassport.mainlandAreaCodeEle.val()) != zPassport.mainlandConuntryCode){
				// !="+86" 非大陆手机号
				$('#mobilephone,#oldmobilephone').attr('maxlength','')
				return false;
			}else{
				$('#mobilephone,#oldmobilephone').attr('maxlength','11')
				return true;
			}
		},
		
	}

function createPopConfirm(popMsg,confirmCb,cancelCb,popCloseCb){
	var popHtml = '<div class="pop-up-confirm">' + 
    '<p class="tips-text">' + popMsg + '</p>' +
		'<div class="text-center pop-btn-con">' +
			'<input type="button" name="" class="btn-default-main pop-confirm" value="' + jsMsg.btnConfirm + '">' +
			'<input type="button" name="" class="btn-default-secondary pop-cancel" value="' + jsMsg.btnCancel + '">' +
		'</div>' +
		'<i class="pop-close"></i>' +
	'</div>'
	if($('.pop-up-confirm').length == 0){
		$('body').append(popHtml);
	}else{
		$('.pop-up-confirm').show();
	}
	$('.pop-confirm').on('click',function(){
		if(confirmCb){
			confirmCb()
		}
	})
	$('.pop-cancel').on('click',function(){
		$('.pop-up-confirm').remove();
		if(cancelCb){
			cancelCb()
		}
	})
	$('.pop-close').on('click',function(){
		$('.pop-up-confirm').remove();
		if(popCloseCb){
			popCloseCb()
		}
	})
}

function createAlertEle(alertMsg,alertConfirmCb){
	var alertHtml = '<div class="alert-confirm">' + 
						'<p class="tips-text tips-alert-text">' + alertMsg + '</p>' +
						'<div class="text-center pop-btn-con">' + 
							'<input type="button" name="" class="btn-default-main pop-confirm" value="' + jsMsg.btnConfirm + '">' + 
						'</div>' +
						'<i class="pop-close"></i>' +
					'</div>'
	if($('.alert-confirm').length == 0){
		$(body).append(alertHtml);
			$('.alert-confirm .pop-close').on('click',function(){
			$('.alert-confirm').remove();
		})
		$('.alert-confirm .pop-confirm').on('click',function(){
			if(alertConfirmCb){
				alertConfirmCb()
			}
		})
	}
	
}

function createSimpleToastEle(simpToastMsg){
	var toastHtml = '<div class="pop-up-normal hide">' + 
		'<p class="tips-text tips-text-normal">' + simpToastMsg + '</p>' +
	'</div>'
	if($('.pop-up-normal').length == 0){
		$("body").append(toastHtml);
		$('.pop-up-normal').fadeIn()
		setTimeout(function(){
			$('.pop-up-normal').fadeOut(function(){
				$('.pop-up-normal').remove()
			})
		},1500)
	}
}

function controlRegistButtonIshighlighted(){
	if(checkAll.checkItem1($('#mobilephone').val()) != 0 && checkAll.checkItem1($('#vericode').val()) != 0 && $('.intensity').length > 0){
		$('#z-p-registered').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
	}else{
		$('#z-p-registered').removeClass('btn-default-main').addClass('btn-disabled').attr('disabled');
	}
}

function controlHasAccButtonIsHighlight(){
	if(checkAll.checkItem1($('#Existinguser').val()) != 0 && checkAll.checkItem1($('#passwords').val()) != 0){
		$('#loginbind').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
	}else{
		$('#loginbind').removeClass('btn-default-main').addClass('btn-disabled').attr('disabled');
	}
}


$('.checkbox-wrap label').on('click', function() {
	$("input[name=autolog]").parent().removeClass().addClass('check-c')
	$("input[name=autolog]:checked").parent().removeClass().addClass('check-cd')
})

function phoneVerAjax(thisInput,ajaxUrl,ajaxTips,transitionCurDom){
	// transitionCurDom   带区号的手机号dom
	if(!transitionCurDom){
		transitionCurDom = thisInput;
	}
	if(checkAll.checkItem3(thisInput.val()) == 1){
		$.ajax({
			url:ajaxUrl,
			type:"get",
			data:{param:thisInput.val()},
			dataType :'json',
			success:function(data){
				switch (data.status)
				{
					case "n":
					transitionCurDom.parent().find('.form-tip').html(ajaxTips).addClass("p-666 verify-tips-text");
					thisInput.attr('pass','n')
					$('#veribtn').removeClass('btn-default-main').addClass('btn-disabled').attr('disabled',true);
					break;
					case "y":
					transitionCurDom.parent().find('.form-tip').html('').removeClass("p-666 verify-tips-text");
					transitionCurDom.removeClass('transition-time text-wrong-style')
					checkAll.onoff = 1;
					thisInput.attr('pass','y')		
					$('#veribtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');			
					break;
				}
				
			}
		})
	}else{
		transitionCurDom.parent().find('.form-tip').html(checkAll.msg.msg1).addClass("red-color verify-tips-text");
		transitionCurDom.addClass('transition-time text-wrong-style');
		// $('#oldmobilephone').parent().find('.form-tip').html(checkAll.msg.msg1).addClass("red-color verify-tips-text");
		// $('#oldmobilephone').addClass('transition-time text-wrong-style');
		$('#veribtn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true);
	}
	if(checkAll.checkItem1(thisInput.val()) == 0){
		transitionCurDom.parent().find(".form-tip").html(checkAll.msg.msg11).removeClass("gray-color").addClass("red-color verify-tips-text");
		transitionCurDom.addClass('transition-time text-wrong-style');
	}else{
		if(checkAll.checkVerNc() != false){
			$('.js-h-from-ele').show()
		}
		if(checkAll.checkItem3(thisInput.val()) == 1 && thisInput.attr('pass') == "y"){
			transitionCurDom.parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
			transitionCurDom.removeClass('text-wrong-style');
			// $('#oldmobilephone').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
			// $('#oldmobilephone').removeClass('text-wrong-style');
			$('#veribtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
		}
	}
}
	var checkAll = {
	    /**
	     * str 字符串常量
	     */

	    str: {
	        UserAuthentication:/^[0-9A-Za-z\u4e00-\u9fa5_]+$/,
	        phoneA:/^((13[0-9])|(14[0-9])|(16[0-9])|(17[0-9])|(15[0-9])|(18[0-9]|(19[0-9])))\d{8}$/,
	        passwordA:/^[a-zA-Z0-9 &!#$%()*+,-=.\/:;?@\[\]^_`<>{|}~]+$/,
	        emailA:/^\w+([-.]\w+)*@\w+([-]\w+)*\.(\w+([-]\w+)*\.)*[a-z]{2,3}$/,
			vericodes:/^\d+$/,
			NonMainlandPhonNumber:/^[0-9]+$/
	    },
	    passwordStr: {
	        UPPER : "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
	        LOWER : "abcdefghijklmnopqrstuvwxyz",
	        NUMBER : "0123456789",
	        CHARACTER : "~`!@#$%^&*()_-+={}[]|;:,<>.?/"
	    },
	    msg: {
	    	msg0 : jsMsg.msg0,
	    	msg1 : jsMsg.msg1,
	    	msg2 : jsMsg.msg2,
	    	msg3 : $("<span><i></i><i></i><i></i></span>"),
	    	msg4 : jsMsg.msg4,
	    	msg5 : jsMsg.msg5,
	    	msg6 : jsMsg.msg6,
	    	msg7 : jsMsg.msg7,
	    	msg8 : jsMsg.msg8,
			msg9 : jsMsg.msg9,
			msg10 : jsMsg.msg10,
			msg11 : jsMsg.msg11,
			oldPhoneEmpty : jsMsg.oldPhoneEmpty,
			enterpass : jsMsg.enterpass
	    },
	    onoff:null,
	    /**
	     * 对应校验条目
	     * @type {Object}
	     */
	    checkItem1: function(value){
	        if(value == " " || value == ""){
	        	return 0;
	        }else{
	        	return 1;
	        }
	    },
	    checkItem2: function(value){
	        if(value == " " || value == ""){
	        }else{
	        	var reg = /^[\u4e00-\u9fa5a-z\d_]{2,}$/gi;
	        	if (reg.test(value))
	        	{
	        	    var len = value.replace(/[^\x00-\xff]/g,"aa").length;
	        	    if (len < 4 || len > 16)
	        	    {
	        	        return 0;
	        	    }
	        	
	        	    return 1;
	        	}
	        	 
	        	return 0;
	        }
		},
		checkCountryPhone:function(value){
			if(value == " " || value == ""){
	        	return 0;
	        }else{
				if (this.str.NonMainlandPhonNumber.test(value)) {
					return 1;
				}else{
					return 0;
				}
			}
			
		},
	    checkItem3: function(value){
	        if(value == " " || value == ""){
	        	return 0;
	        }else{
				if(zPassport.countryCodeIsn()){
					if (this.str.phoneA.test(value)) {
						return 1;
					}else{
						return 0;
					}
				}else{
					// !="+86" 非大陆手机号的验证
					if (this.str.NonMainlandPhonNumber.test(value)) {
						return 1;
					}else{
						return 0;
					}
				}  
	        }
	    },
	    checkItem4:function(value){

	    	 if(value == " " || value == ""){
	        	return 0;
	        }else if(value.length < 6 || value.length >20){
	        	return 0;
	        }else{
	                    if (this.str.passwordA.test(value)) {
	                       return 1;
	                    }else{
	                    	return 0;
	                    }
	        }
	    },
	     checkItem5: function(value){
	    	        if(value == ""){
	    	        	return 0;
	    	        }else{
	    	            var item3Result = {UPPER:0,LOWER:0,NUMBER:0,CHARACTER:0};
	    	            for (j in this.passwordStr) {
	    	                var strKey = j;
	    	                var strValue = this.passwordStr[strKey];
	    	                for (k = 0; k < value.length; k++) {
	    	                    if (strValue.indexOf(value.charAt(k)) > -1) {
	    	                        item3Result[strKey] = 1;
	    	                    }
	    	                }
	    	            }
	    	            if(item3Result.UPPER + item3Result.LOWER + item3Result.NUMBER +item3Result.CHARACTER > 1){
	    	            	return 1;
	    	            }else{
	    	            	return 0;
	    	            }
	    	        }
	    	    },
	    checkemail:function(value){
	    	if(value == " " || value == ""){
	    		return 0;
	    	}else{
	    		if(this.str.emailA.test(value)){
	    			return 1;
	    		}else{
	    			return 0;
	    		}
	    	}
	    },
	    checkveri:function(value){
	    	if(value == " " || value == ""){
	    		
	    		return 0;
	    	}else{
	    		return 1;
	    	}
	    },
	    checkvericode:function(value){
	    		if(value == " " || value == ""){
	    		
	    		return 0;
	    	}else{
	    		if(this.str.vericodes.test(value)){
	    			return 1;
	    		}else{
	    			return 0;
	    		}
	    	}
	    		
	    	
	    },
	    checkboxed:function(value){
	    	if(value){
	    		return 1;

	    	}else{
	    		return 0;
	    	}
	    },
//	    Vericountdown:function(){
//	    	jQuery('#veribtn').on("click",function(){
//	    		jQuery(this).hide().siblings(".vericode-btn").show();
//	    	})
//	    },
	    radios:function(){
			
			$('#Useragreement,#loginstatus').click(function(){
				if($(this).attr("checked")){
					$(this).removeAttr("checked")
				}else{
					$(this).attr("checked",true)
				}
				
			})
		},
	    checkTxzForm:function(){
	    	var formEmail = $('#email,#emails').attr('pass');
	    	if(formEmail == "n" || formEmail == undefined){
				// alert(jsMsg.alertCheckEmail)
				createSimpleToastEle(jsMsg.alertCheckEmail)
	    		return false;
	    	}
	    	if($('.nc-lang-cnt').attr('data-nc-lang') != '_yesTEXT'){
				// alert(jsMsg.alertCheckPass)
				createSimpleToastEle(jsMsg.alertCheckPass)
	    		return false;
	    	}
		},
		checkVerNc:function(){
			if($('.nc-lang-cnt').attr('data-nc-lang') != '_yesTEXT'){
	    		// alert(jsMsg.alertCheckPass)
	    		return false;
	    	}
		},
	    init : function(){
	        var that = this;
	        // 用户名
	        jQuery('#username,#usernames').bind('keyup',function(event){

	                // var keycode=event.which;
	                // if(keycode==13){
	                //     return;
	                // }
	                var value = jQuery(this).val();
	                if(that.checkItem1(value) == 0){
	                	$('#username,#usernames').parent().find(".user-name-tip").html(that.msg.msg0).removeClass("gray-color").addClass("red-color verify-tips-text");
	                	$('#username,#usernames').addClass('transition-time text-wrong-style');
	                }else if(that.checkItem2(value) == 0){
	                	
	                	$('#username,#usernames').parent().find(".user-name-tip").html(that.msg.msg0).removeClass("gray-color").addClass("red-color verify-tips-text");
	                	$('#username,#usernames').addClass('transition-time text-wrong-style');
	                }else{
	                	$('#username').parent().find(".user-name-tip").html('').removeClass("red-color verify-tips-text").addClass("gray-color");
	                	$('#usernames').parent().find(".user-name-tip").html('').removeClass("red-color verify-tips-text").addClass("gray-color");

						$('#username,#usernames').removeClass('text-wrong-style');
											
						if($('#s-name-sub-btn').length > 0){
							$('#s-name-sub-btn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
						}
	                }
	            });  
			    jQuery('#username').bind('blur',function(){
					var value = jQuery(this).val();
					if(that.checkItem1(value) == 0){
						$('#username,#usernames').parent().find(".user-name-tip").html(that.msg.msg10).removeClass("gray-color").addClass("red-color verify-tips-text");
						$('#username,#usernames').addClass('transition-time text-wrong-style');
						return false;
					}
				  	$.ajax({
	                	url:"exists_username.do",
	                	type:"get",
	                	data:{param:$('#username').val()},
	                	dataType :'json',
	                	success:function(data){
	                		switch (data.status)
	                		{
	                			case "n":
	                			$('#username').parent().find('.user-name-tip').html(data.info).removeClass("gray-color").addClass("red-color verify-tips-text");
	                			$('#username').addClass('transition-time text-wrong-style');
	                			$('username').attr('pass','n');
	                			break;
								case "y":
															// jsMsg.htmlUpdateUname
	                			$('#username').parent().find('.user-name-tip').html("").removeClass("red-color verify-tips-text").addClass("gray-color");
	                			$('#username').removeClass('text-wrong-style');
	                			$('username').attr('pass','y');
	                			break;
	                		}
	                		
	                	}
	                })
				  })

				  $('#jump-name-btn').on('click',function(){
					
					createPopConfirm(jsMsg.randomUserName + $('#randomName').val(),jumpSuccess,jumpCancel,popClose)
					$('.pop-up-confirm .tips-text').before('<p style="font-size:16px;text-align:center;position:absolute;top:30px;left:50%;margin-left:-40px;" class="js-add-title">' + jsMsg.popCopyTitle + '</p>')
					$('.pop-up-confirm .pop-confirm').val(jsMsg.btnJump);
					$('.pop-up-confirm .pop-cancel').val(jsMsg.btnNoJump);
					function jumpSuccess(){
						$('.pop-up-confirm .pop-confirm').val(jsMsg.btnConfirm);
						$('.pop-up-confirm .pop-cancel').val(jsMsg.btnCancel);
						window.location.href=$('#toUrl').val();
						$('.pop-up-confirm .js-add-title').remove()
					}
					function jumpCancel(){
						$('.pop-up-confirm .pop-confirm').val(jsMsg.btnConfirm);
						$('.pop-up-confirm .pop-cancel').val(jsMsg.btnCancel);
						$('.pop-up-confirm .js-add-title').remove()
					}
					function popClose(){
						$('.pop-up-confirm .js-add-title').remove()
					}
				  })
				  
				  $('#s-name-sub-btn').on('click',function(){
						$.ajax({
		                	url:'/updateUnameFirst.do',
		                	type:"post",
		                	data:{appId:$('#appId').val(),username:$('#username').val(),cback:$('#cback').val()},
		                	dataType :'json',
		                	success:function(data){
		                		if(data.result){
		                			window.location.href = data.toUrl;
		                		}else{
		                			alert(data.msg);
		                		}
		                	}
		                })
				  })
				
			// 手机
			$('#veribtn').attr('disabled',true);
			// 所有手机号格式判断
			jQuery('#mobilephone,#oldmobilephone').bind('keyup',function(){
				var value = jQuery(this).val();
				var _thisDom = $(this);
				if($('#country-area-code').length){
					_thisDom = $(this).parents('.country-phone-covers');
				}
				if(zPassport.countryCodeIsn()){
					// 大陆手机号
					if(value.length >= 11){
						checkPhoneFormat()
					}
				}else{
					checkPhoneFormat()
				}
				// if(value.length >= 11){
					function checkPhoneFormat(){
						if(that.checkItem3(value) == 0){
							_thisDom.parent().find('.form-tip').html(that.msg.msg1).addClass("red-color verify-tips-text");
							_thisDom.addClass('transition-time text-wrong-style');
							$('#veribtn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true);
						}else{
							_thisDom.parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
							_thisDom.removeClass('text-wrong-style');
							$('#veribtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
						}
					}
					
				// }else{
				// 	_thisDom.parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
				// 	_thisDom.removeClass('text-wrong-style');
				// }
				if($('#z-p-registered').length > 0){
					controlRegistButtonIshighlighted()
				}
			});

			// jQuery('#mobilephone').bind('blur',function(){
			// 	if(that.checkItem3($('#mobilephone').val()) == 1){
			// 		$.ajax({
	        //         	url:"exists.do",
	        //         	type:"get",
	        //         	data:{param:$('#mobilephone').val()},
	        //         	dataType :'json',
	        //         	success:function(data){
	        //         		switch (data.status)
	        //         		{
	        //         			case "n":
	        //         			$('#mobilephone').parent().find('.form-tip').html(jsMsg.htmlPhoneReg).addClass("red-color verify-tips-text");
	        //         			break;
	        //         			case "y":
	        //         			$('#mobilephone').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
	        //         			that.onoff = 1;
	        //         			break;
	        //         		}
	                		
	        //         	}
	        //         })
			// 	}
			// })

			// 注册页手机号 , 修改邮箱
			jQuery('#mobilephone[name=email],input[name=p-phone]').bind('blur',function(){
				var _this = $(this);
				var transitionEle = $(this);
				// 手机号Dom结构和其它的input不一样，判断是否是手机号的
				if($('#country-area-code').length){
					transitionEle = $(this).parents('.country-phone-covers');
				}
				// 这里的_this不能用transitionEle声明的，
				phoneVerAjax(_this,"exists.do",jsMsg.htmlPhoneReg + "<span class='err-tips-ml'>" + jsMsg.htmlPhoneReg2 + "<a href='" + $('.login-nav>a').attr('href') + "' class='err-link-tips'>" + jsMsg.phoneLogin + "</a></span>",transitionEle)
			})
			jQuery('input[name=p-bind-phone]').bind('blur',function(){
				var _this = $(this);
				var transitionEle = $(this);
				// 手机号Dom结构和其它的input不一样，判断是否是手机号的
				if($('#country-area-code').length){
					transitionEle = $(this).parents('.country-phone-covers');
				}
				if($('#isn').val() != ''){
					phoneVerAjax(_this,"exists.do",jsMsg.htmlPhoneReg + "<a href='" + $('.p-bottom-tips-covers>a').attr('href') + "&jumpPhone=" + $('input[name=p-bind-phone]').val() + "' class='err-link-tips err-tips-ml'>" + jsMsg.htmlBindAccount + "</a>",transitionEle)
				}else{
					phoneVerAjax(_this,"exists.do",jsMsg.htmlPhoneReg,transitionEle)
				}
				
			})
			// 绑定新手机号页 
			jQuery('#mobilephone[name=bind-new-phone]').bind('blur',function(){
				var transitionEle = $(this);
				// 手机号Dom结构和其它的input不一样，判断是否是手机号的
				if($('#country-area-code').length){
					transitionEle = $(this).parents('.country-phone-covers');
				}
				// 光标移走新手机号时判断原手机号格式
				if(checkAll.checkItem1($('#oldmobilephone').val()) == 0){
					$('#oldmobilephone').parent().find('.form-tip').html(that.msg.oldPhoneEmpty).addClass("red-color verify-tips-text");
					$('#oldmobilephone').addClass('transition-time text-wrong-style');
				}else{
					if(checkAll.checkCountryPhone($('#oldmobilephone').val()) == 1){
						$('#oldmobilephone').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
						$('#oldmobilephone').removeClass('text-wrong-style');
						if($('#oldmobilephone').attr('pass') != 'y'){
							$('#oldmobilephone').parent().find('.form-tip').html(checkAll.msg.msg1).addClass("red-color verify-tips-text");
							$('#oldmobilephone').addClass('transition-time text-wrong-style');
						}
					}else{
						$('#oldmobilephone').parent().find('.form-tip').html(checkAll.msg.msg1).addClass("red-color verify-tips-text");
						$('#oldmobilephone').addClass('transition-time text-wrong-style');
					}
				}
				
				phoneVerAjax($(this),"exists.do",jsMsg.htmlPhoneReg + "</span>",transitionEle);
			})
			
			
			jQuery('.checkbox-wrap label').on('click',function(){
				if(that.checkboxed($('#Useragreement').attr('checked')) == 0){
					$(this).parent().find('.form-tip').html(that.msg.msg9).addClass("red-color verify-tips-text");
				}else{
					$(this).parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
				}
			})

			function oldexistsAsync(thisDom,oldExistsSuccessCb){
				$.ajax({
					url:"oldexists.do",
					type:"get",
					data:{param:thisDom.val()},
					dataType :'json',
					success:function(data){
						oldExistsSuccessCb(data)
					}
				})
			}
			// 解绑手机号页的原手机号有地区号
			// 绑定新手机号页的原手机号没有地区号，但新手机号有
			jQuery('#oldmobilephone').bind('blur',function(){
				var _this = $('#oldmobilephone');
				var transitionEle = $('#oldmobilephone');
				if($('#oldmobilephone').siblings('#country-code-ipt').length){
					// 解绑手机号页有区号的
					transitionEle = transitionEle.parents('.country-phone-covers');
					if(that.checkItem3(_this.val()) == 1){
						oldexistsAsync(_this,oldExistsSuccessCb)
					}
				}else{
					// 解绑手机号页没有区号的
					// 绑定新手机号页的原手机号没有地区号是全国范围不必验证格式
					if(that.checkCountryPhone(_this.val()) == 1){
						oldexistsAsync(_this,oldExistsSuccessCb)
					}
				}
				function oldExistsSuccessCb(data){
					switch (data.status)
					{
						case "n":
						transitionEle.parent().find('.form-tip').html(data.info	).addClass("red-color verify-tips-text");
						transitionEle.addClass('transition-time text-wrong-style');
						$('#veribtn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true);
						_this.attr('pass','n')					
						break;
						case "y":
						transitionEle.parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
						transitionEle.removeClass('transition-time text-wrong-style');
						$('#veribtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
						_this.attr('pass','y')		
						break;
					}
				}
				
			})
			// 密码 
			jQuery('#password').bind('keyup blur',function(){
				var value = jQuery(this).val();
				if(that.checkItem4(value) == 0){
					$('#password').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
					$('#password').addClass('transition-time text-wrong-style');
				}
				else if(that.checkItem5(value) == 0){
					$('#password').parent().find('.form-tip').html(that.msg.msg8).addClass("red-color verify-tips-text");
					$('#password').addClass('transition-time text-wrong-style');
				}else{
					$('#password').parent().find('.form-tip').removeClass("red-color verify-tips-text");
					$('#password').removeClass('text-wrong-style');
					that.msg.msg3.attr('class','intensity');
				
						$('#password').parent().find('.form-tip').html(that.msg.msg3);
				
					
						 window.PasswordUtil.analyzePwd(value,$('#password').parent().find('.form-tip'))
				}
				if($('#z-p-registered').length > 0){
					controlRegistButtonIshighlighted()
				}
			});
			jQuery('.form-eye-icon').bind('click',function(){
				if($('#password').attr("type") == "password"){
					$('#password').attr("type","text");
					$(this).addClass('disabled-glance')
				}else{
					$('#password').attr("type","password");
					$(this).removeClass('disabled-glance')
				}
			});
			// 修改密码  确认密码
			$('#setpassword').bind('keyup blur',function(){
				// alert()
				var value = jQuery(this).val();

				if(that.checkItem4(value) == 0){
					$('#setpassword').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
					$('#setpassword').addClass('transition-time text-wrong-style');
				}
				else if(value == $('#confirmpassword').val()){
					$('#confirmpassword').parent().find('.form-tip').html("").removeClass("red-color verify-tips-text");
					$('#confirmpassword').removeClass('transition-time text-wrong-style');
				}
				else if(that.checkItem5(value) == 0){
					$('#setpassword').parent().find('.form-tip').html(that.msg.msg8).addClass("red-color verify-tips-text");
					$('#setpassword').addClass('transition-time text-wrong-style');
				}else{
					$('#setpassword').parent().find('.form-tip').removeClass("red-color verify-tips-text");
					$('#setpassword').removeClass('text-wrong-style');
					that.msg.msg3.attr('class','intensity');
				
						$('#setpassword').parent().find('.form-tip').html(that.msg.msg3);
						window.PasswordUtil.analyzePwd(value,$('#setpassword').parent().find('.form-tip'))
					}
			})
			$('#confirmpassword').bind('keyup blur',function(){
				var value = jQuery(this).val();
				if(value != $('#setpassword').val()){
					$('#confirmpassword').parent().find('.form-tip').html(that.msg.msg7).addClass("red-color verify-tips-text");
					$('#confirmpassword').addClass('transition-time text-wrong-style');
					
					
				}else{
					$('#confirmpassword').parent().find('.form-tip').html("").removeClass("red-color verify-tips-text");
					$('#confirmpassword').removeClass('text-wrong-style');
					
				}
			})
			// 
			$('#modifyPass').bind('click',function(){
				if($('#setpassword').val() != $('#confirmpassword').val()){
					$('#confirmpassword').parent().find('.form-tip').html(that.msg.msg7).addClass("red-color verify-tips-text");
					$('#confirmpassword').addClass('transition-time text-wrong-style');

				}else if(that.checkItem5($('#setpassword').val()) == 0){
					$('#setpassword').parent().find('.form-tip').html(that.msg.msg8).addClass("red-color verify-tips-text");
					$('#setpassword').addClass('transition-time text-wrong-style');
				}else if(that.checkItem4($('#setpassword').val()) == 1 && that.checkItem5($('#setpassword').val()) == 1 && $('#setpassword').val() == $('#confirmpassword').val()){
					$('#loginform').submit();
				}else{
					$('#setpassword').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
					$('#setpassword').addClass('transition-time text-wrong-style');
				}
			})
			// 邮箱
			jQuery('#email,#emails').bind('keyup',function(){
				var value = jQuery(this).val();
				if(that.checkemail(value) == 0 ){
					$('#email,#emails').parent().find('.form-tip').html(that.msg.msg5).addClass("red-color verify-tips-text");
					$('#email,#emails').addClass('transition-time text-wrong-style');
				}else{
					$('#email,#emails').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
					$('#email,#emails').removeClass('text-wrong-style');
				}
			});
			jQuery('#email,#emails').bind('blur',function(){
				var email = $('#email').length > 0 ? $('#email').val() : $('#emails').val();
				if(that.checkemail(email) == 1){
					$.ajax({
	                	url:"exists.do",
	                	type:"get",
	                	data:{param:email},
	                	dataType :'json',
	                	success:function(data){
	                		switch (data.status)
	                		{
	                			case "n":
								$('#email,#emails').parent().find('.form-tip').html(jsMsg.htmlEmailReg).addClass("red-color verify-tips-text");
								$('#email,#emails').addClass('transition-time text-wrong-style');
	                			$('#email,#emails').attr('pass','n');
	                			break;
	                			case "y":
								$('#email,#emails').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
								$('#email,#emails').removeClass('transition-time text-wrong-style');
	                			$('#email,#emails').attr('pass','y');
	                			that.onoff = 1;
	                			break;
	                		}
	                	}
	                })
				}
			})
			// 手机注册
			jQuery('#register').bind('click',function(){
				if(that.checkItem1($('#username').val()) == 1 && that.checkItem2($('#username').val()) == 1 && that.checkItem3($('#mobilephone').val()) == 1 && that.checkItem4($('#password').val()) == 1 && that.checkvericode($('#vericode').val()) == 1 && that.checkboxed($('#Useragreement').attr('checked')) == 1 && that.onoff == 1){
					$('#form').submit();
					$(this).attr('disabled',true);
				}else{
					if(that.checkItem1($('#username').val()) == 0 || that.checkItem2($('#username').val()) == 0){
						$('#username').parent().find(".user-name-tip").html(that.msg.msg0).removeClass("gray-color").addClass("red-color verify-tips-text");
	                	$('#username').addClass('transition-time text-wrong-style');
					} 
					if(that.checkItem3($('#mobilephone').val()) == 0){
						$('#mobilephone').parent().find('.form-tip').html(that.msg.msg1).addClass("red-color verify-tips-text");
						$('#mobilephone').addClass('transition-time text-wrong-style');
					}
					if(that.checkItem4($('#password').val()) == 0){
	                	$('#password').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
						$('#password').addClass('transition-time text-wrong-style');
					}
					if(that.checkvericode($('#vericode').val()) == 0){
				
						$('#vericode').parent().find('.form-tip').html(that.msg.msg6).addClass("red-color verify-tips-text");
						$('#vericode').addClass('transition-time text-wrong-style');
					}	
					
				}
				return false;
				
			})

		// // 主站手机注册
			jQuery('#z-p-registered').bind('click',function(){
				if(that.checkItem3($('#mobilephone').val()) == 1 && that.checkItem4($('#password').val()) == 1 && $('#vericode').attr('pass') == "n" && that.checkboxed($('#Useragreement').attr('checked')) == 1 && that.onoff == 1){
					$('#form').submit();
					$(this).attr('disabled',true).val(jsMsg.btnRegister);
				}else{
					if(that.checkItem3($('#mobilephone').val()) == 0){
						$('#mobilephone').parent().find('.form-tip').html(that.msg.msg1).addClass("red-color verify-tips-text");
						$('#mobilephone').addClass('transition-time text-wrong-style');
					}
					if(that.checkItem4($('#password').val()) == 0){
	                	$('#password').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
						$('#password').addClass('transition-time text-wrong-style');
					}
					if(that.checkvericode($('#vericode').val()) == 0){
				
						$('#vericode').parent().find('.form-tip').html(that.msg.msg6).addClass("red-color verify-tips-text");
						$('#vericode').addClass('transition-time text-wrong-style');
					}	
					
				}
				return false;
				
			})

		// 邮箱注册
		jQuery('#emailregister').bind('click',function(){
				// alert()
				if(that.checkItem1($('#username').val()) == 1 && that.checkItem2($('#username').val()) == 1 && that.checkItem4($('#password').val()) == 1 && that.checkemail($('#email').val()) == 1 && that.checkboxed($('#Useragreement').attr('checked')) == 1 && that.checkTxzForm() != false && that.onoff == 1){
					$(this).attr('disabled',true);
					$('#form').submit();
				}else{
					if(that.checkItem1($('#username').val()) == 0 || that.checkItem2($('#username').val()) == 0){
						$('#username').parent().find(".user-name-tip").html(that.msg.msg0).removeClass("gray-color").addClass("red-color verify-tips-text");
	                	$('#username').addClass('transition-time text-wrong-style');
					} 
					if(that.checkItem4($('#password').val()) == 0){
	                	$('#password').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
						$('#password').addClass('transition-time text-wrong-style');
					}	
					if(that.checkemail($('#email').val()) == 0){
						$('#email').parent().find('.form-tip').html(that.msg.msg5).addClass("red-color verify-tips-text");
						$('#email').addClass('transition-time text-wrong-style');
					}
				}
				return false;
				
			});
			// 手机验证码
			jQuery('#vericode').bind('keyup',function(){
				var value = $.trim(jQuery(this).val());
				if(that.checkvericode(value) == 0){
				
					$('#vericode').parent().find('.form-tip').html(that.msg.msg4).addClass("red-color verify-tips-text");
					$('#vericode').addClass('transition-time text-wrong-style');
				}else{
				
					$('#vericode').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
					$('#vericode').removeClass('text-wrong-style');
				}
				if($('#z-p-registered').length > 0){
					controlRegistButtonIshighlighted()
				}
			});
			$('#vericode').bind('blur',function(){
				var phone = $('#pwdusername').length > 0 ? $('#pwdusername').val() : $('#mobilephone').val();
				if(!phone){
					phone = $('#oldmobilephone').val();
				}
				if(phone != '' && $('#vericode').val() != ''){
					$.ajax({
		            	url:"checkPhoneCode.do",
		            	type:"get",
		            	data:{phoneNum:phone,param:$('#vericode').val()},
		            	dataType :'json',
		            	success:function(data){
		            		if(data.status == 'y'){
		            			$('#vericode').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
		    					$('#vericode').removeClass('text-wrong-style');
		    					$('#vericode').attr('pass','n')
		            		}else{
		            			$('#vericode').parent().find('.form-tip').html(checkAll.msg.msg4).addClass("red-color verify-tips-text");
								$('#vericode').addClass('transition-time text-wrong-style');
								$('#vericode').attr('pass','y')
		            		}
		            	}
		            });
				}
				if(phone != '' && that.checkItem1($('#vericode').val()) == 0){
					$('#vericode').parent().find('.form-tip').html(checkAll.msg.msg6).addClass("red-color verify-tips-text");
					$('#vericode').addClass('transition-time text-wrong-style');
				}
			});
			// 图文验证码
			jQuery('#vercodes').bind('keyup',function(){
				var value = $.trim(jQuery(this).val());
				if(that.checkveri(value) == 0){
					$('#vercodes').parent().find('.form-tip').html(that.msg.msg6).addClass("red-color verify-tips-text");
					$('#vercodes').addClass('transition-time text-wrong-style');
				}else{
					// alert("aa")
					$('#vercodes').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
					$('#vercodes').removeClass('text-wrong-style');
				}
			});
			$('#vercodes').bind('blur',function(){
				if($('#vercodes').val() != ''){
					$.ajax({
		            	url:"checkRandomCode.do",
		            	type:"get",
		            	data:{param:$('#vercodes').val()},
		            	dataType :'json',
		            	success:function(data){
		            		if(data.status == 'y'){
		            			$('#vercodes').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
		    					$('#vercodes').removeClass('text-wrong-style');
		            		}else{
		            			$('#vercodes').parent().find('.form-tip').html(checkAll.msg.msg4).addClass("red-color verify-tips-text");
								$('#vercodes').addClass('transition-time text-wrong-style');
		            		}
		            	}
		            });
				}
			});
			//找回密码
			jQuery('#pwdusername').bind('blur',function(event){
				//if enter return
				var _this = $(this);
				var transitionEle = _this.parents('.country-phone-covers');
			    var keycode=event.which;
			    if(keycode==13){
			        return;
				}
			    if(_this.val() != ''){
			        $.ajax({ 
			            url:'check_account_get_pass.do',  
			            data:{param:_this.val()},
			            dataType:'json',
			            success:function(data) {
			            	if(data.result){//通过存在
			            	    //如果是手机则显示手机验证码
			                    if(checkAll.checkCountryPhone(_this.val()) == 1){//是手机
			                    	$('#emailVerify').hide();
									$('#phoneVerify').show();		
									$('#veribtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
									$('.js-hide-country-code').show();	
									_this.removeClass('ipt-w-334');			
			                    }else{
			                    	$('#emailVerify').show();
									$('#phoneVerify').hide();
									$('.js-hide-country-code').hide();
									_this.addClass('ipt-w-334');
								}
								// 忘记密码，邮箱，手机一个input，dom结构一样
			                    transitionEle.parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
								transitionEle.removeClass('text-wrong-style');
								_this.attr('pass','n')
			            	}else{
								if(checkAll.checkCountryPhone(_this.val()) == 1){//是手机
			                    	$('#emailVerify').hide();
									$('#phoneVerify').show();		
									$('#veribtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
									$('.js-hide-country-code').show();	
									_this.removeClass('ipt-w-334');			
			                    }else{
			                    	$('#emailVerify').show();
									$('#phoneVerify').hide();
									$('.js-hide-country-code').hide();
									_this.addClass('ipt-w-334');
								}
			            		transitionEle.parent().find('.form-tip').html(data.msg).addClass("red-color verify-tips-text");
								transitionEle.addClass('transition-time text-wrong-style');
								$('#veribtn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true);
								_this.attr('pass','y')
			            	}
			            },  
			            error:function(data) {
			            	console.log("验证异常");
			            },
			        });
			    }
			});
			// 修改邮箱  
			jQuery('#Determine').bind('click',function(){
				if(that.checkveri($('#vercodes').val()) == 1 && that.checkemail($('#emails').val()) == 1){
					$.ajax({
						url:"modifyemail.do",
						type:"post",
						data:{appId:$("#appId").val(),email:$("#emails").val(),randomCode:$("#vercodes").val()},
						dataType:"json",
						success:function(data){
							if(data.result){
								msg(data.msg, 'verifyEmail.do?name='+data.name+'&appId=' + $("#appId").val());
							}else{
								msg(data.msg, null);
							}
						},
						error:function(){
							msg('操作失败，请稍后重试', null);
						}
					});
				}else{
					if(that.checkveri($('#vercodes').val()) == 0){
						$('#vercodes').parent().find('.form-tip').html(that.msg.msg4).addClass("red-color verify-tips-text");
						$('#vercodes').addClass('transition-time text-wrong-style');
					}
				if(that.checkemail($('#emails').val()) == 0){
						$('#emails').parent().find('.form-tip').html(that.msg.msg5).addClass("red-color verify-tips-text");
						$('#emails').addClass('transition-time text-wrong-style');
					}
				}
				return false;
					
			});
			
			function modifyPhoneFormat(){
				if(that.checkItem1($("#oldmobilephone").val()) == 0){
					$('#oldmobilephone').parent().find('.form-tip').html(that.msg.oldPhoneEmpty).addClass("red-color verify-tips-text");
					$('#oldmobilephone').addClass('transition-time text-wrong-style');
				}
				if(that.checkvericode($('#vericode').val()) == 0){
					$('#vericode').parent().find('.form-tip').html(that.msg.msg4).addClass("red-color verify-tips-text");
					$('#vericode').addClass('transition-time text-wrong-style');
				}
				// 获取有区号的展示效果的结构
				zPassport.getConuntryCodePhoneShowEffectEle($('#mobilephone'));
				if(that.checkItem3($("#mobilephone").val()) == 0){
					zPassport.ConuntryCodePhoneShowEffectEle.parent().find('.form-tip').html(that.msg.msg1).addClass("red-color verify-tips-text");
					zPassport.ConuntryCodePhoneShowEffectEle.addClass('transition-time text-wrong-style');
				}else{
					zPassport.ConuntryCodePhoneShowEffectEle.parent().find('.form-tip').html("").removeClass("red-color verify-tips-text");
					zPassport.ConuntryCodePhoneShowEffectEle.removeClass('transition-time text-wrong-style');
				}
			}
			// 修改手机
			jQuery('#Deter').bind('click',function(){
				if(that.checkItem1($("#oldmobilephone").val()) == 1 && that.checkItem3($("#mobilephone").val()) == 1 && that.checkvericode($('#vericode').val()) == 1 && $('#vericode').attr('pass') == "n"){
					$.ajax({
						url:"modifyphone.do",
						type:"post",
						data:{appId:$('#appId').val(),phoneCode:$('#vericode').val(),phoneNum:$("#mobilephone").val(),type:$('#type').val(),oldphoneNum:$('#oldmobilephone').val(),countryCode:$('#country-code-ipt').val()},
						dataType:"json",
						success:function(data){
							if(data.result){
								msg(data.msg, 'accountSafe.do?appId=' + $("#appId").val());
							}else{
								msg(data.msg, null);
							}
						},
						error:function(){
							msg('操作失败，请稍后重试', null);
						}
					});
				}else{
					modifyPhoneFormat()
				}
					return false;	
			});
			
			function oldMobilePhoneFormat(){
				// getConuntryCodePhoneShowEffectEle 获取有区号的展示效果的结构
				zPassport.getConuntryCodePhoneShowEffectEle($('#oldmobilephone'));
				if(that.checkItem3($("#oldmobilephone").val()) == 1){
					zPassport.ConuntryCodePhoneShowEffectEle.parent().find('.form-tip').html("").removeClass("red-color verify-tips-text");
					zPassport.ConuntryCodePhoneShowEffectEle.removeClass('transition-time text-wrong-style');
					return true;
				}else{
					if(that.checkItem3($("#oldmobilephone").val()) == 0){
						zPassport.ConuntryCodePhoneShowEffectEle.parent().find('.form-tip').html(that.msg.msg1).addClass("red-color verify-tips-text");
						zPassport.ConuntryCodePhoneShowEffectEle.addClass('transition-time text-wrong-style');
					}
					return false;
				}
			}
			// 解绑手机
			jQuery('#unbindBtn').bind('click',function(){
				if(oldMobilePhoneFormat() && $("#oldmobilephone").attr('pass') == "y"){
					$.ajax({
						url:"unbindphone.do",
						type:"post",
						data:{appId:$('#appId').val(),oldphoneNum:$("#oldmobilephone").val(),code:$('#vericode').val()},
						dataType:"json",
						success:function(data){
							if(data.result){
								msg(data.msg, 'accountSafe.do?appId=' + $("#appId").val());
							}else{
								msg(data.msg, null);
							}
						},
						error:function(){
							msg('操作失败，请稍后重试', null);
						}
					});
				}
				return false;	
			});
			// 修改用户名
			jQuery('#Modifyname').bind('click',function(){
				if(that.checkItem1($("#username").val()) == 1 && that.checkItem2($("#username").val()) == 1 && that.checkveri($('#vercodes').val()) == 1){
					$('#form').submit();
					$.ajax({
						url:"modifyUname.do",
						type:"post",
						data:{appId:$("#appId").val(),username:$("#username").val(),randomCode:$('#vercodes').val()},
						dataType:"json",
						success:function(data){
							if(data.result){
								msg(data.msg, 'accountSafe.do?appId=' + $("#appId").val());
								/*if(data.result){
									$z.addScriptTag('http://www.zcool.com.cn/sso/logout_jsonp.do?callback=$z.logoutCallBack&scriptId=ssoscript1006','ssoscript1006');
									$z.addScriptTag("http://www.zcooler.com/sso/logout_jsonp?callback=$z.logoutCallBack&scriptId=ssoscript1001",'ssoscript1001');
									$z.addScriptTag("http://www.hellorf.com/user/ssologout/?callback=$z.logoutCallBack&scriptId=ssoscript1007",'ssoscript1007');
									$z.addScriptTag("http://www.gogoup.com/sso/logout_jsonp?callback=$z.logoutCallBack&scriptId=ssoscript1010",'ssoscript1010');
								}*/
							}else{
								msg(data.msg, null);
							}
						},
						error:function(){
							msg('操作失败，请稍后重试', null);
						}
					});
				}else{
					if(that.checkveri($('#vercodes').val()) == 0){
						$('#vercodes').parent().find('.form-tip').html(that.msg.msg4).addClass("red-color verify-tips-text");
						$('#vercodes').addClass('transition-time text-wrong-style');
					}
					
				if(that.checkItem1($("#username").val()) == 0 || that.checkItem2($("#username").val()) == 0){
						$('#username').parent().find(".user-name-tip").html(that.msg.msg0).removeClass("gray-color").addClass("red-color verify-tips-text");
	                	$('#username').addClass('transition-time text-wrong-style');
					}
				}
					return false;	
			});

			// 修改密码
			$('#Modifypass').bind('click',function(){
				if(that.checkItem4($('#password').val()) == 1 && $('#oldpwd').val() != ''){
					$.ajax({
						url:"modifysafepwd.do",
						type:"post",
						data:{appId:$('#appId').val(),oldpwd:$('#oldpwd').val(),password:$('#password').val(),nowUrl:$('#nowUrl').val()},
						dataType:"json",
						success:function(data){
							if(data.result){
								var url = data.nowUrl != null ? data.nowUrl : 'accountSafe.do?appId=' + $("#appId").val();
								msg(data.msg, url);
							}else{
								msg(data.msg, null);
							}
						},
						error:function(){
							msg('操作失败，请稍后重试', null);
						}
					});
				}else{
					if(that.checkItem4($('#password').val()) == 0){
						$('#password').parent().find('.form-tip').html(that.msg.msg2).addClass("red-color verify-tips-text");
						$('#password').addClass('transition-time text-wrong-style');
					}
				}
			})
		}
		
	};
	checkAll.init();
//	checkAll.Vericountdown();
	checkAll.radios();
	// 绑定已有账号 
	$("#Existinguser").bind('blur',function(){
		if($('#Existinguser').val() != ''){
			$.ajax({
				url:"checkAccount.do",
				type:"get",
				data:{param:$('#Existinguser').val()},
				dataType:"json",
				success:function(data){
						if(data.status == "y"){
							$(".unregistered").hide();
							if($('.binding-tab').length == 0){
								$('#Existinguser').parent().find('.form-tip').html("").removeClass("red-color verify-tips-text");
								$('#Existinguser').removeClass('transition-time text-wrong-style');
							}
						}else{
							$(".unregistered").show();
							if($('.binding-tab').length == 0){
								$('#Existinguser').parent().find('.form-tip').html(jsMsg.msg13).addClass("red-color verify-tips-text");
								$('#Existinguser').addClass('transition-time text-wrong-style');
							}
							
						}
					}
				})
		}else{
			$('#Existinguser').parent().find('.form-tip').html(jsMsg.msg12).addClass("red-color verify-tips-text");
			$('#Existinguser').addClass('transition-time text-wrong-style');
		}
	})

	$('#passwords').bind('blur',function(){
		if(checkAll.checkItem1($('#passwords').val()) == 0){
			$('#passwords').parent().find('.form-tip').html(checkAll.msg.enterpass).addClass("red-color verify-tips-text");
			$('#passwords').addClass('transition-time text-wrong-style');
		}else{
			$('#passwords').parent().find('.form-tip').html('').removeClass("red-color verify-tips-text");
			$('#passwords').removeClass('transition-time text-wrong-style');
		}
	})

	$('#Existinguser,#passwords').bind('keyup',function(){
		controlHasAccButtonIsHighlight();
	})
	
	$('.register').on('click',function(){
		$(".bindingtab").find("span:eq(0)").removeClass("curent").siblings().addClass("curent");
		$(".bind-con:eq(0)").hide().siblings(".bind-con").show()
		$("#Existinguser").val("");
		$("#passwords").val("");
		$(".unregistered").hide();
	})
	// $('.passcard-btn').attr('disabled',true);						
	// 点击下一步验证
	$('.passcard-btn').on('click',function(){
		if($('#pwdusername').val() != ""){
			if($('#emailVerify').is(':visible') == true){
				if($('#pwdusername').attr('pass') == "y" || !$('#emailVerify .gt_ajax_tip').hasClass('gt_success')){
					return false;
				}
			}else{
				if($('#pwdusername').attr('pass') == "y" || $('#vericode').attr('pass') == "y"){
					return false;
				}
			}
			
		}
		
	})

function verCodeConfirm(){
	$('.pop-up-confirm .pop-confirm').val(jsMsg.btnSubmiting).attr('disabled',true);
	// 发送语音验证码 ajax

	$.ajax({  
			url:'/phoneSendVoicecode.do?TOKEN='+$('#regkey').val(),  
			data:{phoneNum:$('#mobilephone').val(),a_session:$('#a_session').val(),a_sig:$('#a_sig').val(),a_token:$('#a_token').val(),a_scene:$('#a_scene').val(),a_from:3,countryCode:$('#country-code-ipt').val()},
			dataType:'json',
			success:function(data) {
				$('.pop-up-confirm .pop-confirm').val(jsMsg.btnSend).removeAttr('disabled');
				if(!data.result){
					createSimpleToastEle(data.msg)
				}else{
					$('.pop-up-confirm').hide();
				}
			},  
			error:function(data) {
				console.log("验证码发送异常");
				$('.pop-up-confirm .pop-confirm').val(jsMsg.btnSend).removeAttr('disabled');
			},
		});
	
}
$('.js-no-ver-code').on('click',function(){
	createPopConfirm(jsMsg.confirmTipVioce,verCodeConfirm)
	$('.pop-up-confirm').val(jsMsg.btnSend)
})

//弹窗,自行绑定确定方法
function binds(obj){
	this.PopupBlock = $(obj.PopupBlock);
	this.closebtn = $(obj.closebtn);
	this.Deter = $(obj.Deter);
	this.Unbundling = $(obj.Unbundling);
	this.cancel = obj.cancel;
	this.cancelF($(this.cancel)).cancelF($(this.closebtn));
}
binds.prototype = {
	cancelF:function(close){
		var that = this;
		close.on('click',function(){
			that.PopupBlock.hide();
		})
		return this;
	}
}
function popupBlock(){
	return new binds({
		PopupBlock:'.pop-up-confirm',
		closebtn:'.pop-close',
		Deter:'.pop-confirm',
		Unbundling:'.unbunding',
		cancel:'.pop-cancel'
	})
}
popupBlock();

// msg
 function Msg(obj){
 	this.Unbundling = $(obj.Unbundling);
 	this.url = obj.url;
 	this.tips = obj.tips;
 	this.shows();
 }
 Msg.prototype = {
 	shows:function(){
 		var that = this;
 		this.Unbundling.show();
 		this.Unbundling.find('p').html(this.tips)
 		setTimeout(function(){
			that.Unbundling.hide();
				if(that.url){
					location.href = that.url;
				}						
								
		},1500)
 	}
 }
function msg(tiptxt,url){
	new Msg({
		Unbundling:'.unbunding',
		url:url,
		tips:tiptxt
	});
}

function regType(type){
	if(type == 1){
		$('form').attr('action','/regEmail.do?' + window.location.href.split('?')[1]);
	}else{
		$('form').attr('action','/reg.do?' + window.location.href.split('?')[1]);
	}
	$('form').removeAttr('onsubmit');
	$('form').submit();
}



function ncSuccessCb(){
	if(checkAll.checkItem1($('#mobilephone').val()) != 0){
		$('.js-h-from-ele').show();
	}
	// 手机号Dom结构和其它的input不一样，this指向不一样，判断是否是手机号的
	var transitionEle = null;
	if($('#country-area-code').length){
		transitionEle = $('#mobilephone').parents('.country-phone-covers');
	}

	if($('input[name=p-bind-phone]').length > 0){
		if($('#isn').val() != ''){
			phoneVerAjax($('input[name=p-bind-phone]'),"exists.do",jsMsg.htmlPhoneReg + "<a href='" + $('.p-bottom-tips-covers>a').attr('href') + "&jumpPhone=" + $('input[name=p-bind-phone]').val() + "' class='err-link-tips err-tips-ml'>" + jsMsg.htmlBindAccount + "</a>",transitionEle)
		}else{
			phoneVerAjax($('input[name=p-bind-phone]'),"exists.do",jsMsg.htmlPhoneReg,transitionEle)
		}
		
	}else{
		phoneVerAjax($('input[name=p-phone]'),"exists.do",jsMsg.htmlPhoneReg + "<span class='err-tips-ml'>" + jsMsg.htmlPhoneReg2 + "<a href='" + $('.login-nav>a').attr('href') + "' class='err-link-tips'>" + jsMsg.phoneLogin + "</a></span>",transitionEle)
	}
}


function thirdAccountTab(){
	if($('#type').val() == 1){
		$('.account-number-tab span').removeClass('current-status');
		$('.account-number-tab span').eq(1).addClass('current-status');
		$('.account-num-box .account-binding-box').show();
		$('.account-num-box .account-num-tabcon').hide();
	}
}

// 捎带已有账号值，到
var locationSearchPhoneIsNum = location.search.lastIndexOf('jumpPhone');
var jumpPhoneParam = location.search.substr(location.search.lastIndexOf('jumpPhone'));
$(function() { 
	if (navigator.userAgent.toLowerCase().indexOf("chrome") >= 0) { 
		$(window).load(function(){ 
			$('input[type=text]').val('');
			if(locationSearchPhoneIsNum != -1){
				$('#Existinguser').val(jumpPhoneParam.split('=')[1])
			}
		}); 
	}else{
		if(locationSearchPhoneIsNum != -1){
			$('#Existinguser').val(jumpPhoneParam.split('=')[1])
		}
	}
}); 

function showGlobalMaskLayer() {
	if(!$('.shade').length){
		$('body').append('<div class="shade"></div>')
	}else{
		$('.shade').removeClass('hide');
	}
    $('html').addClass('body-fixed')
}

function hideGlobalMaskLayer() {
	$('.shade').addClass('hide');
    $('html').removeClass('body-fixed');
}
var passDomain = location.host.indexOf('test') !== -1 ? 'http://static.test.zcool.cn' : 'https://static.zcool.cn'
function showLoading(obj) {
    var loaddingUrl = passDomain + '/passport4.0/images/common/page_loading.gif'
    obj.show().append('<div class="text-center"><img src="' + loaddingUrl + '"/></div>');
}

function countryPhoneCodeSuccessCb(data){
	var areaList = '<div class="js-scroll-covers"><ul class="code-list-ul">'
        $.each(data,function(areaIndex,areaVal){
           areaList += '<li data-id="' + areaVal.id + '" data-code="' + areaVal.country_code + '">' + areaVal.country_name +
                    '<span class="p-area-code">' + areaVal.country_code + '</span>' +
            '</li>';
        })
                
        areaList += '</ul></div>';
        if(!$('.code-list-ul').length){
            $('.area-code-covers').html(areaList)
		}
		$(".js-scroll-covers").mCustomScrollbar({
			theme: "dark", //主题颜色
			scrollButtons: {
				scrollSpeed: 100,
				enable: true //是否使用上下滚动按钮
			},
			autoHideScrollbar: true, //是否自动隐藏滚动条
			scrollInertia: 100, //滚动延迟
			horizontalScroll: false, //水平滚动条
			autoDraggerLength: true,
			callbacks: {
				onScrollStart: function () {
					$('.mCS-autoHide .mCustomScrollBox .mCSB_scrollTools').css({ "opacity": '1', '-webkit-animation': 'none' })
				},
				whileScrolling: function () {
					$('.mCS-autoHide .mCustomScrollBox .mCSB_scrollTools').css({ "opacity": '1', '-webkit-animation': 'none' })
				},
				onScroll: function () {
					//滚动完成后触发事件
					$('.mCS-autoHide .mCustomScrollBox .mCSB_scrollTools').removeAttr('style')
				}
			}
		});
		bindEventArea()
}

function getPassAreaAsync(){
    $('.area-code-covers').html("");
	showLoading($('.area-code-covers'))
	$.ajax({
		url:"/getCountryPhoneCodes.do",
		type:"get",
		dataType :'json',
		success:function(data){
			if(data.result && data.data.length){
				countryPhoneCodeSuccessCb(data.data)
			}else{
				msg(data.msg, null);
			}
		},
		error:function(){
			msg('操作失败，请稍后重试', null);
		}
	})   
    
}

function bindEventArea(){
    $('body .area-wrap .code-list-ul').on('click','li',function(){
        hideGlobalMaskLayer()
        $('.area-wrap').hide();
        var areaId = $(this).attr('data-id');
        var areaCode = $(this).attr('data-code');
		$('#country-area-code').text(areaCode);
		$('#country-code-ipt').val(areaCode)
    })

   
}



function areaPopUpShow(){
    var html = '<div class="pop-up area-wrap">' +
        '<div class="popup-title">' +
            '请选择区号' +
        '</div>'+
        '<div class="area-code-covers">' +
            
        '</div>' +
        '<div class="pop-close"></div>' +
    '</div>'
	!$('.area-wrap').length?$('body').append(html):$('.area-wrap').show();
	// 如果国家区号请求成功后不用再请求一遍了
	if(!$('.code-list-ul').length){
		getPassAreaAsync()
	}
     $('body .pop-up').on('click','.pop-cancel,.pop-close',function(){
        hideGlobalMaskLayer()
		$('.area-wrap').hide();
		
    })
}

$('#country-area-code').on('click',function(){
    showGlobalMaskLayer()
    areaPopUpShow()
})



