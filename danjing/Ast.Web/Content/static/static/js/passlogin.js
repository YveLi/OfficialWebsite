
// 判断是windows系统
var isWindows = function () {
    return /windows|win32/i.test(navigator.userAgent);
}

var zPassport = {
    ConuntryCodePhoneShowEffectEle: null,
    getConuntryCodePhoneShowEffectEle: function (thisPhoneInput) {
        zPassport.ConuntryCodePhoneShowEffectEle = thisPhoneInput;
        if ($('#country-area-code').length) {
            zPassport.ConuntryCodePhoneShowEffectEle = thisPhoneInput.parents('.country-phone-covers');
            return zPassport.ConuntryCodePhoneShowEffectEle;
        }
    },
    mainlandAreaCodeEle: $('#country-code-ipt'),
    getMainlandAreaCodeEle: function () {
        if (!$('#country-area-code').is(':visible')) {
            zPassport.mainlandAreaCodeEle = $('');
        } else {

        }
    },
    mainlandConuntryCode: "+86",
    messagesWeb: {
        common_send: "发送",
        form_validation_nameorpass: "请输入账户名和密码",
        form_validation_name: "请填写账户名",
        form_validation_pass: "请输入密码",
        pop_click_send_voice_vericode: "点击发送获取语音验证码",
        "logIn": {
            login_copy: "登录",
            login_logining: "登录中...",
            login_error_registered_tip1: "帐号未注册,你可以",
            login_error_registered_tip2: "去注册",
            login_error_pass_easy: "密码过于简单，前往修改密码？",
            login_toast_black: "该帐号已被管理员拉黑",
            login_accect_disabled: "您的账号已被管理员禁用",
            login_pass_wrong_time: "帐号或密码错误，你还可以尝试",
            login_pass_wrong_time2: "次",
            login_user_locked: "您的账号由于密码累计输错五次已被锁定，重置密码或两小时后自动解锁",
            login_name_offline_tips: "为了更好的维护帐号安全体系，用<br>户名登录功能已下线，请使用：",
        },
        "commentVerification": {
            phone_num_format: "请输入正确的手机号",
            phone_empty: "请填写手机号",
            phone_not_reg_tips1: "该手机号尚未注册，你可以",
            phone_not_reg_tips2: "去注册",
            vericode_wrong: "验证码不正确",
            vericode_empty: "请输入验证码"
        },

    }

}

var loginWindowEle = {
    passLoginShowTipsDom: $('#pass-error-tips'),
    smsLoginErrorTipsDom: $('#sms-error-tips'),
    passLoginErrorTipsDom:$('.error-tips')
}


var zCheckAll = {
    str: {
        phoneA: /^((13[0-9])|(14[0-9])|(16[0-9])|(17[0-9])|(15[0-9])|(18[0-9]|(19[0-9])))\d{8}$/,
        vericodes: /^\d+$/,
        NonMainlandPhonNumber: /^[0-9]+$/
    },
    formMsg: {
        msg0: zPassport.messagesWeb.commentVerification.phone_empty,
        msg1: zPassport.messagesWeb.commentVerification.phone_num_format,
        msg2: zPassport.messagesWeb.commentVerification.phone_not_reg_tips1,
        msg3: zPassport.messagesWeb.commentVerification.phone_not_reg_tips2,
        msg4: zPassport.messagesWeb.commentVerification.vericode_wrong,
        msg5: zPassport.messagesWeb.commentVerification.vericode_empty
    },
    checkEmpty: function (value) {
        if (value == " " || value == "") {
            return 0;
        } else {
            return 1;
        }
    },
    checkCountryPhone: function (value) {
        if (value == " " || value == "") {
            return 0;
        } else {
            if (this.str.NonMainlandPhonNumber.test(value)) {
                return 1;
            } else {
                return 0;
            }
        }
    },
    checkPhone: function (value) {
        if (value == " " || value == "") {
            return 0;
        } else {
            if (this.str.phoneA.test(value)) {
                return 1;
            } else {
                return 0;
            }
        }
    },
    countryCodeDiffCheck: function (value) {
        zPassport.getMainlandAreaCodeEle();
        if ($.trim(zPassport.mainlandAreaCodeEle.val()) != zPassport.mainlandConuntryCode) {
            $('#sms-phone').attr('maxlength', '')
            return zCheckAll.checkCountryPhone(value)

        } else {
            $('#sms-phone').attr('maxlength', '11')
            return zCheckAll.checkPhone(value)

        }
    },
    checkvericode: function (value) {
        if (value == " " || value == "") {
            return 0;
        } else {
            if (this.str.vericodes.test(value)) {
                return 1;
            } else {
                return 0;
            }
        }

    },
    checkNc: function () {
        if ($('.nc-lang-cnt').attr('data-nc-lang') != '_yesTEXT') {
            return false;
        }
    }
}

function reinitIframe() {
    try {
        var bHeight = window.frames['loginChild'].contents().find("body").height();
        var dHeight = $('#login-box').outerHeight();
        var height = Math.max(bHeight, dHeight);
        $("#loginChild").css({ "height": height });
        console.log(height);
    } catch (ex) { }
}
(function autoHeight(){
var b_width = Math.max(document.body.scrollWidth,document.body.clientWidth);
var b_height = Math.max(document.body.scrollHeight,document.body.clientHeight);

})();
window.setInterval("reinitIframe()", 200);

// toast提示
function passPageToast(msg) {
    if (!$('.pass-toast-div').length) {
        $('body').append('<div class="pass-toast-div">' + msg + '</div>')
    }
    $('.pass-toast-div').fadeIn().css('display','flex').html(msg)
    setTimeout(function () {
        $('.pass-toast-div').fadeOut().html('');
    }, 1000)
}

// 确定layer
function createPassConfirmPop(popMsg, popMsg2, confirmCb, cancelCb, popCloseCb) {
    var popHtml = '<div class="pop-up-confirm">' +
        '<p class="tips-text">' + popMsg + '</p>' +
        '<div class="text-center pop-btn-con">' +
        '<input type="button" name="" class="btn-default-main pop-confirm" value="' + "确定" + '">' +
        '<input type="button" name="" class="btn-default-secondary pop-cancel" value="' + "取消" + '">' +
        '</div>' +
        '<i class="pop-close"></i>' +
        '</div>'
    if ($('.pop-up-confirm').length == 0) {
        $('body').append(popHtml);
    } else {
        $('.pop-up-confirm').show();
    }
    $('.pop-confirm').on('click', function () {
        if (confirmCb) {
            confirmCb()
        }
    })
    $('.pop-cancel').on('click', function () {
        $('.pop-up-confirm').remove();
        if (cancelCb) {
            cancelCb()
        }
    })
    $('.pop-close').on('click', function () {
        $('.pop-up-confirm').remove();
        if (popCloseCb) {
            popCloseCb()
        }
    })
}

// 显示错误红色横幅提示
function loginErrorTipsShow(sTarget, errMsg) {
    if(sTarget.parents('.sms-login-covers').is(':visible')){
        // 判断是手机登录时
        $('#sms-error-tips').show().find('.js-err-l').html(errMsg);
        sTarget.parents('.sms-login-covers').addClass('js-sms-covers-style')
    }else{
        sTarget.parents('.pass-login-covers').addClass('js-pass-covers-style')
        $('#pass-error-tips').show().find('.js-err-l').html(errMsg);
    }
}
// 隐藏错误红色横幅提示
function loginErrorTipsHide(sTarget) {
    
    if(sTarget.parents('.sms-login-covers').is(':visible')){
        sTarget.parents('.sms-login-covers').removeClass('js-sms-covers-style')
        $('#sms-error-tips').hide().find('.js-err-l').html("");
    }else{
        sTarget.parents('.pass-login-covers').removeClass('js-pass-covers-style')
        $('#pass-error-tips').hide().find('.js-err-l').html("");
    }
}

function alertUnRegisteredMessage(url) {
    // 账号未注册,请注册
    loginWindowEle.passLoginShowTipsDom.find('.js-err-l').html(zPassport.messagesWeb.logIn.login_error_registered_tip1 + "<a href='" + url + "' target='_blank'>" + zPassport.messagesWeb.logIn.login_error_registered_tip2 + "</a>");
    loginWindowEle.passLoginShowTipsDom.show();
    loginWindowEle.passLoginShowTipsDom.parents('.pass-login-covers').addClass('js-pass-covers-style')
}

// 用户名下线通知弹窗
function zcoolLoginSRemindAlert(message, recommendLogin, fn1AlertCallback) {
    var alertHtml = '<div class="alert-confirm login-success-tip-wrap hide">' +
        '<p class="tips-text tips-alert-text">提示文案</p>' +
       ' <div class="text-center pop-btn-con">' +
            '<input type="button" name="" class="btn-default-main pop-confirm" value="我知道了">' +
        '</div>' +
       ' <i class="pop-close"></i>' +
    '</div>';
    if ($('.alert-confirm').length == 0) {
        $('body').append(alertHtml);
    } else {
        $('.alert-confirm').show();
    }
    $('.alert-confirm').removeClass('hide');
    $('.alert-confirm .tips-alert-text').html(message);
    $('.tips-alert-text').next('.seconds-alert-text').remove();
    $('.alert-confirm').addClass('login-success-tip-wrap');
    $('.tips-alert-text').after("<p class='seconds-alert-text' style='text-align: center; margin-top: 15px; line-height: 26px; margin-bottom: -15px'><span class='hightlight'>" + recommendLogin + "</span>登录</p>");
    $('.alert-confirm .pop-confirm').on('click', function () {
        $(this).parents('.alert-confirm').addClass('hide')
        if (fn1AlertCallback) {
            fn1AlertCallback();
        }

    })
    $('.alert-confirm .pop-close').on('click', function () {
        $(this).parents('.alert-confirm').addClass('hide')
    })
}


// 图文验证码
function loadRandomCodeImg() {
    var t = new Date().valueOf();
    var imageCode = $('#l-image-code');
    $('#l-image-code').show();
    $('.js-image-code-tips').addClass('code-wrong').removeClass('code-right')
    $('#randomCodeImg').attr('src',
        '/loginRandomImg.jpg?t=' + t);

    if (imageCode.is(':visible')) {
        if ($('#ipt-code').val() == "") {
            $('#loginbtn').removeClass('btn-default-main').addClass('btn-disabled').attr('disabled',true);
        }

        if ($('#ipt-code').val().length < 5) {
            $('.js-image-code-tips').addClass('code-wrong').removeClass('code-right')
        }
    }
}

// 登录的微信二维码
// redirect_uri暂不支持https
function wxCodeLogin() {
    if ($('#login_container').length > 0) {
        var obj = new WxLogin({
            id: "login_container",
            appid: "wxef5765dd395d8504",
            scope: "snsapi_login",
            redirect_uri: "http://passport.zcool.com.cn/thirdlogin/wechat_callback.do?appId=" + appId,
            state: "STATE",
            style: "",
            href: "https://static.zcool.cn/passport4.0/css/wxCode.css?v=0.1"
        })
    }
}

// 获取location登录应用Id
function getQueryString(name) {
    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
    var r = location.href.split('?')[1].match(reg);
    if (r != null) {
        return unescape(r[2]);
    }
    return null;
}

/**
 * cback 登录成功后回调的地址，默认为产品首页
 * loginType 1为弹窗登录，不填为页面登录
 * appId 登录应用Id
 * appLogin 可指定解析用户TOKEN地址，默认为各产品配置的login_cb
 * 
 */
var appId = getQueryString("appId");
var appLogin = $('#appLogin').attr('value');
var cback = $('#cback').attr('value');
var loginType = $('#loginType').attr('value');
var aa_username;
var aa_callback;
var aa_needconfirmpassnum;
/**
 * 添加登录的各个注册事件
 */
window.passLoginWindowEvent = function (appId, appLogin) {
    // 清空手机号相关信息
    $('#sms-phone').val('');
    $('#sms-cd').val('');
    loginWindowEle.smsLoginErrorTipsDom.hide();
    $('.js-ipt-h').hide();
    $('#sms-send-cd-btn').attr('disabled', true).removeClass('btn-default-main').addClass('btn-disabled');
    noCaptchaNc()
    // wxCodeLogin()
    /**
     * 点击登录按钮执行内容；
     * @param {*} clickThisDom 当前点击的登录按钮元素
     */
    function registeredFn(clickThisDom) {
        if (!clickThisDom.hasClass('btn-disabled')) {
            // 清空一下错误提示
            loginErrorTipsHide(loginWindowEle.passLoginErrorTipsDom)
            clickThisDom.val('登录中...').attr('disabled', true)

            if (clickThisDom.prop('id') == "loginbtn") {
                // 账号密码登录
                doLogin(appId, appLogin);
            } else {
                // 短信登录
                smsDoLogin(appId, appLogin)
            }
        }
    }
    // 账号密码登录按钮注册事件；
    $('#loginbtn').on('click', function () {
        registeredFn($(this))
    })
    // 短信登录按钮注册事件
    $('#sms-login-sub').on('click', function () {
        registeredFn($(this))
    })
    // 回车事件登录
    $(document).on('keyup', function (event) {
        var e = event || window.event || arguments.callee.caller.arguments[0];
        if (e.keyCode == 13) {
            if ($('#loginbtn').is(':visible')) {
                registeredFn($('#loginbtn'))
            } else if ($('#sms-login-sub').is(':visible')) {
                registeredFn($('#sms-login-sub'))
            }

        }
    })
    // 账号密码的图文验证交互
    $('#ipt-code').on('keyup', function () {
        var code = $('#ipt-code').val();
        var codeLength = code.length;
        if (codeLength >= 5) {
            $('#loginbtn').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
            var codeTips = $('.js-image-code-tips')
            $.getJSON('/check_imgcode.do?code=' + code + '&jsonpCallback=?', {}, function (data) {
                if (data.result) {
                    codeTips.addClass('code-right').removeClass('code-wrong')
                } else {
                    codeTips.addClass('code-wrong').removeClass('code-right')
                }
            });
        } else {
            $('#loginbtn').removeClass('btn-default-main').addClass('btn-disabled').attr('disabled',true);
        }
    })
    // 下次自动登录
    $('.auto-login label').on('click', function () {
        $("input[name=autolog]").parent().removeClass().addClass('auto-login-c')
        $("input[name=autolog]:checked").parent().removeClass().addClass('auto-login-cd')
    })
}
passLoginWindowEvent(appId, appLogin);

// function closeLoginWindow() {
//     $('.pop-login').hide();
//     hideGlobalMaskLayer();
// }
// $('.login-box .login-close').on('click', function () {
//     closeLoginWindow();
//     $('#loginbtn').off('click');
//     $('#sms-login-sub').off('click');
// })


/**
 * 账号密码登录
 * @param {*} appId 登录应用Id
 * @param {*} appLogin 可指定解析用户TOKEN地址，默认为各产品配置的login_cb
 * @param {*} needconfirmpassnum 
 */
function doLogin(appId, appLogin, needconfirmpassnum) {
    var aa_username = $.trim($('#username').val());
    var password = $.trim($('#password').val());
    var code = $('#ipt-code').val();
    if ($(".pass-login-covers input[name=autolog]").parent().hasClass('auto-login-c')) {
        var remember = 0;
    } else {
        var remember = 1;
    }
    var callback = createCallback();
    aa_needconfirmpassnum = needconfirmpassnum;
    if (aa_username.length == 0 || password.length == 0) {
        alert("username and password can not be null！");//此处代码不应该走到
        return;
    }
    if (!cback) {
        cback = document.referrer;
    }
    // appId 登录应用Id
    // aa_username 登录账号
    // password 登录密码
    // remember 自动登录
    // code 图文验证码
    // callback 通行证登录后，回调函数
    // cback 为了传回到callback中的参数：被追加到了appLogin参数后面，回调地址
    // passportDomain passport服务器主机地址
    // appLogin 为了传回到callback中的参数：redirectUrl
    $z.loginActive(appId, aa_username, password, remember, code, callback, cback,
        "//" + passportDomain, appLogin);
}

// 登录成功后的回调函数
function createCallback() {
    
    return function (data) {
        // active 邮箱是否激活 passNum 密码复杂度
        // repeatLogin 是否重复登录 repeat_account 重复登录账号是否同一个 userId 重复登录时返回登录名 userpassword 重复登录时返回用户密码 repeatRedirectUrl 重复登录时返回跳转地址
        // redirectUrl 跳转网址
        
        if (data.result) {//登录成功
            //判断是否是主站，（主站有拉黑功能）
            data.redirectUrl = data.redirectUrl.replace('http:','')
            var service = data.authentication.attributes.service;
            if(service.indexOf('www.zcool.com.cn') > -1 || service.indexOf("www.test.zcool.cn") > -1){
                 $.ajax({
                    type: "GET",
                    url: data.redirectUrl,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    headers: {
                        "X-Requested-With": "XMLHttpRequest"
                    },
                    dataType: "json",
                    success: function(data){
                        if (data && (data.code === 0 && data.success === "true")) {
                            if(parseInt(loginType)){
                                // 弹窗登录成功
                                window.parent.postMessage('loginSuccess','*'); 
                                $('#loginbtn,#sms-login-sub').val('登录').removeAttr('disabled');
                            }else{
                                top.location.href = data.data;
                            }
                        } else if(data.code == 1100) {
                            //printUnloginHTML();
                            console.log(data.code + ":" + data.msg)
                            var zcoolMainDomain = app_host;
                            top.location.href = zcoolMainDomain + "/register?originUrl=" + data.data;
                        } else if(data.code == 1101){
                            loginErrorTipsShow(loginWindowEle.passLoginErrorTipsDom, zPassport.messagesWeb.logIn.login_accect_disabled)
                            
                        }else{
                            // 正常应该不会走到该代码
                            alert(data.msg)
                            
                        }
                        $('#loginbtn,#sms-login-sub').val('登录').removeAttr('disabled');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log("=====" + XMLHttpRequest);
                    }
                })
            }else{
                if(parseInt(loginType)){
                    // 弹窗登录成功
                    window.parent.postMessage('loginSuccess','*'); 
                    $('#loginbtn,#sms-login-sub').val('登录').removeAttr('disabled');
                }else{
                    // 页面登录成功
                    //top.location.href = cback;
                    top.location.href = data.redirectUrl;
                }
                
            }
            
            
            // if (data.repeatLogin) {
            //     if (data.repeat_account == "ERR_ANOTHER_ACCOUNT") { //重复登录（密码正确）,且前后登录账号不一样
                   
            //         setTimeout(function () {
            //             beforelogincb(data);//这里逻辑有待验证正确性
            //             //location.href = data.repeatRedirectUrl;
            //         }, 1000);
            //     } else if (data.repeat_account == "ERR_SAME_ACCOUNT") {//重复登录（密码正确）,且前后登录账号一样
            //         beforelogincb(data);
            //     } else {
            //         alert("unknown error!");//未知异常
            //     }
            // } else {
            //     beforelogincb(data);
            // }

        } else {//登录失败
            if (data.code == 'ACCOUNT.LACK.CAPTCHA') {
                if (!data.residualTryNum && data.residualTryNum != 0) {
                    loginErrorTipsShow(loginWindowEle.passLoginErrorTipsDom, data.msg)
                } else if (data.residualTryNum == 0) {
                    // 您的账号由于密码累计输错五次已被锁定，重置密码或两小时后自动解锁
                    loginErrorTipsShow(loginWindowEle.passLoginErrorTipsDom, zPassport.messagesWeb.logIn.login_user_locked)
                } else {
                    // 用户名或密码错误,还有  次尝试机会
                    loginErrorTipsShow(loginWindowEle.passLoginErrorTipsDom, zPassport.messagesWeb.logIn.login_pass_wrong_time + data.residualTryNum + zPassport.messagesWeb.logIn.login_pass_wrong_time2)

                }
            } else if ("ACCOUNT.IS.NOT.EXIST" == data.code) {
                alertUnRegisteredMessage("//" + passportDomain + "/regPhone.do?appId=" + appId);

            } else if ("CAPTCHA.IS.ERROR" == data.code) {
                loginErrorTipsShow(loginWindowEle.passLoginErrorTipsDom, data.msg)
            } else if (data.isUname) {
                // 为了更好的维护帐号安全体系，用<br>户名登录功能已下线，请使用：
                zcoolLoginSRemindAlert(zPassport.messagesWeb.logIn.login_name_offline_tips, data.recommendLogin);

            } else {
                loginErrorTipsShow(loginWindowEle.passLoginErrorTipsDom, data.msg)
            }
            if ("ACCOUNT.IS.NOT.VERFIED.CODE" == data.code) {
                top.location.href = data.redirectUrl;
                    // "/verifyEmail.do?active=0&name=" + aa_username + "&appId=" + appId;
            } else if ("ACCOUNT.LACK.CAPTCHA" == data.code || "CAPTCHA.IS.ERROR" == data.code) {
                loadRandomCodeImg();

            }
            $('#loginbtn,#sms-login-sub').val('登录').removeAttr('disabled');
        }
    } 
}

function smsDoLogin(appId, appLogin) {
    var loginphoneSuccess = createCallback()
    
    if (!cback) {
        cback = document.referrer;
    }
    if ($('#sms-send-cd-btn').attr('pass') == "y" && $('#sms-phone').attr('pass') == "n") {

        $('#sms-login-sub').val(zPassport.messagesWeb.logIn.login_logining).attr('disabled', true);
        $z.loginPhone(appId, $('#sms-phone').val(), $('#sms-cd').val(), loginphoneSuccess, cback, "//" + passportDomain, appLogin, $('#country-code-ipt').val())
    } else {
        $('#sms-login-sub').val('登录').removeAttr('disabled');
        phoneSubmitVerForm()
    }
}

// 第三方登录的跳转（iframe）
$('.account-login>a').on('click',function(){
    top.location.href = $(this).attr('data-href')
})

// 光标移走用户名交互效果
$('#username').on('blur', function () {
    var name = $.trim($('#username').val());
    var password = $.trim($(".password input[type='password']").val());
    if (name.length > 5) {
        $('#loginbtn').removeClass('btn-disabled').addClass('btn-default-main').removeAttr('disabled')
        $('#username').off('blur');
    }
})
// 验证 用户名、密码、验证码是否为空，高亮登录按钮交互；
function toEmpty() {
    var name = $.trim($('#username').val());
    var password = $.trim($('#password').val());

    if (name != '' && password != '') {
        $('#loginbtn').removeClass('btn-disabled').addClass('btn-default-main').removeAttr('disabled')
    } else {
        $('#loginbtn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true)
    }
    if ($('#ipt-code').closest('.code').is(':visible') && $('#ipt-code').val() == "") {
        $('#loginbtn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true)
    }
}
// 背景图太大时，因谷歌浏览器执行机制记录账号、密码了，但再加载js同时却获取不到input所填充的值，导致登录按钮不会被激活高亮（高亮是个交互效果），所以用以下代码判断激活按钮；
$('#username').on('keyup input', function () {
    var name = $.trim($('#username').val());
    if (name.length < 5) {
        toEmpty()
        $('#username').off('blur');
    }
    if ($('#password').val() != "") {
        toEmpty()
        $('#username').off('blur');
    }
})
$('#password').on('keyup input', function () {
    toEmpty()
})

// window.onload = function () {
//     if ($('.main-login').length > 0) {
//         var name = $.trim($('#username').val());
//         var password = $.trim($(".password input[type='password']").val());
//         if (name != '' || password != '') {
//             $('#loginbtn').removeClass('btn-disabled').addClass('btn-default-main')
//         }
//     }
// }

// 手机号登录验证
function phoneSubmitVerForm() {
    var Phone = $('#sms-phone');
    if (zCheckAll.checkEmpty($('#sms-phone').val()) == 0) {
        // 判断为空
        loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg0)
        return false;
    } else if (zCheckAll.countryCodeDiffCheck($('#sms-phone').val()) == 0) {
        // 手机号格式
        loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg1)
        $('#sms-send-cd-btn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true);
        return false;
    } else if (Phone.val() != "" && zCheckAll.checkEmpty($('#sms-cd').val()) == 0) {
        // 验证码为空
        loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg5)
        return false;
    } else {
        // 提交时判断手机号是否注册
        isExistsAsync($('#sms-phone'), isExistsuccessCb)
        function isExistsuccessCb(data) {
            switch (data.status) {
                case "n":
                    if ($('#sms-send-cd-btn').attr('pass') != "n") {
                        loginErrorTipsHide(loginWindowEle.smsLoginErrorTipsDom);
                        $('#sms-phone').attr('pass', 'n');
                    }
                    break;
                case "y":
                    loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg2 + "<a href='//" + passportDomain + "/regPhone.do?appId=" + appId + "' target='_blank'>" + zCheckAll.formMsg.msg3 + "</a>")
                    $('#sms-phone').attr('pass', 'y')
                    break;
            }
        }

        if ($('#sms-phone').attr('pass') == "n" && zCheckAll.checkEmpty($('#sms-cd').val()) != 0) {
            // // 提交时判断手机验证码是否正确
            checkPhoneCodeAsync($('#sms-phone'), $('#sms-cd'), submitCheckCodeSuccessCb)
            function submitCheckCodeSuccessCb(data) {
                if (data.status == 'y') {
                    loginErrorTipsHide(loginWindowEle.smsLoginErrorTipsDom)
                    $('#sms-send-cd-btn').attr('pass', 'y')
                } else {
                    loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg4)
                    $('#sms-send-cd-btn').attr('pass', 'n')
                }
            }
        }
    }

}

// 手机验证码是否正确
function checkPhoneCodeAsync(phoneDom, codeDom, checkPhoneCodeSuccessCb) {
    $.ajax({
        url: "/checkPhoneCode.do?jsonpCallback=?",
        type: "get",
        data: { phoneNum: phoneDom.val(), param: codeDom.val() },
        dataType: 'jsonp',
        jsonp: 'callback',
        success: function (data) {
            checkPhoneCodeSuccessCb(data);
        }
    });
}

// 激活手机号登录按钮状态
function loginSMSBtnLighthight() {
    if ($('#sms-phone').val() != "" && $('#sms-cd').val() != "") {
        $('#sms-login-sub').addClass('btn-default-main').removeClass('btn-disabled').removeAttr('disabled');
    } else {
        $('#sms-login-sub').removeClass('btn-default-main').addClass('btn-disabled').attr('disabled',true);
    }
}

// 激活发送验证码按钮状态
function activatePhoneLoginVerCode() {
    if (zCheckAll.countryCodeDiffCheck($('#sms-phone').val()) == 1 && $('#sms-send-cd-btn').attr('time') != '0') {
        $('#sms-send-cd-btn').removeClass('btn-disabled').addClass('btn-default-main').removeAttr('disabled');
    } else {
        $('#sms-send-cd-btn').addClass('btn-disabled').removeClass('btn-default-main').attr('disabled',true);
    }
}

// 手机号是否存在
function isExistsAsync(thisDom, isExistsuccessCb) {
    $.ajax({
        url: "/exists.do?jsonpCallback=?",
        type: "get",
        data: { param: thisDom.val() },
        dataType: 'jsonp',
        jsonp: 'callback',
        success: function (data) {
            isExistsuccessCb(data)
        }
    })
}

$('#sms-phone').bind('keyup', function () {
    if ($.trim($('#sms-phone').val()) != "" && zCheckAll.checkNc() != false) {
        $('.js-ipt-h').show();
    }
    loginSMSBtnLighthight()
    activatePhoneLoginVerCode()
})
// 手机验证码
$('#sms-cd').bind('keyup', function () {
    var Phone = $('#sms-phone');
    var verificationCodeInput = $('#sms-cd');
    if (zCheckAll.checkEmpty($(this).val()) == 0) {
        loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg5)
    }else{
        if(Phone.val() != ""){
            checkPhoneCodeAsync(Phone, verificationCodeInput, blurPhoneCodeSuccessCb);
            function blurPhoneCodeSuccessCb(data) {
                if (data.status == 'y') {
                    $('#sms-send-cd-btn').attr('pass', 'y')
                } else {
                    $('#sms-send-cd-btn').attr('pass', 'n')
                }
            }
        }
    }
    loginSMSBtnLighthight()
})

$('#sms-phone').bind('blur', function () {
    var _this = $(this);
    activatePhoneLoginVerCode()
    // if (zCheckAll.countryCodeDiffCheck($('#sms-phone').val()) != 0) {
        isExistsAsync(_this, isExistsuccessCb)
        function isExistsuccessCb(data) {
            switch (data.status) {
                case "n":
                    $('#sms-phone').attr('pass', 'n')
                    break;
                case "y":
                    $('#sms-phone').attr('pass', 'y')
                    break;
            }
        }
    // }
})

$('#sms-cd').bind('blur', function () {
    var Phone = $('#sms-phone');
    var verificationCodeInput = $('#sms-cd');
    // if (Phone.val() != "" && zCheckAll.checkEmpty(verificationCodeInput.val()) != 0) {
    //     checkPhoneCodeAsync(Phone, verificationCodeInput, blurPhoneCodeSuccessCb);
    //     function blurPhoneCodeSuccessCb(data) {
    //         if (data.status == 'y') {
    //             $('#sms-send-cd-btn').attr('pass', 'y')
    //         } else {
    //             $('#sms-send-cd-btn').attr('pass', 'n')
    //         }
    //     }
    // }

})

// 手机验证码倒计时；
wait = 60;
function smsWaitTime(o, waitP) {
    if (wait == 0) {
        $(o).removeAttr('disabled');
        $(o).removeClass("btn-disabled").addClass('btn-default-main').removeAttr('disabled');
        $(o).val("发送验证码");
        wait = waitP;
        $(o).attr('time', '1')
        $('.js-no-ver-code').show();
    } else {
        $(o).prop('disabled', true);
        $(o).addClass("btn-disabled").removeClass('btn-default-main').attr('disabled',true);
        $(o).attr('disabled', 'disabled');
        $(o).val(wait + 's' + "后重新发送");
        $(o).attr('time', '0')
        wait--;
        setTimeout(function () { smsWaitTime(o, waitP) }, 1000);
    }
}

// 发送手机验证码
function verifyCodeSend(regkey) {
    if ($('#sms-phone').val() != '') {
        if ($('#sms-phone').attr('pass') == "n") {
            loginErrorTipsHide(loginWindowEle.smsLoginErrorTipsDom)
            
            $.ajax({
                url: '/phoneSendcodeNew.do?TOKEN=' + regkey,
                data: { phoneNum: $('#sms-phone').val(), a_session: $('#a_session').val(), a_sig: $('#a_sig').val(), a_token: $('#a_token').val(), a_scene: $('#a_scene').val(), a_from: 3, countryCode: $('#country-code-ipt').val() },
                dataType: 'json',
                success: function (data) {
                    if (!data.result) {
                        passPageToast(data.msg);
                        // window.location.reload();
                    }else{
                        smsWaitTime($('#sms-send-cd-btn'), 60);
                    }
                },
                error: function (data) {
                    console.log("手机验证码发送异常");
                },
            });
        } else {
            if(zCheckAll.countryCodeDiffCheck($('#sms-phone').val()) != 0){
                loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zCheckAll.formMsg.msg2 + "<a href='//" + passportDomain + "/regPhone.do?appId=" + appId + "' target='_blank'>" + zCheckAll.formMsg.msg3 + "</a>")
            }else{
                loginErrorTipsShow(loginWindowEle.smsLoginErrorTipsDom, zPassport.messagesWeb.commentVerification.phone_num_format)
            }

        }
    } else {
        alert("请填写手机号");
    }
}

// 发送语音验证码
function Voicecode(regkey) {
    $.ajax({
        url: '/phoneSendVoicecode.do?TOKEN=' + regkey,
        data: { phoneNum: $('#sms-phone').val(), a_session: $('#a_session').val(), a_sig: $('#a_sig').val(), a_token: $('#a_token').val(), a_scene: $('#a_scene').val(), a_from: 3, countryCode: $('#country-code-ipt').val() },
        dataType: 'json',
        success: function (data) {
            $('.pop-up-confirm .pop-confirm').val("发送").removeAttr('disabled');
            if (!data.result) {
                passPageToast(data.msg)
            } else {
                passPageToast("已向你发送语音验证码")
            }
            $('.pop-up-confirm').hide();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("发送语音验证码" + XMLHttpRequest)
            $('.pop-up-confirm .pop-confirm').val("发送").removeAttr('disabled');
        },
    });
}

// 点击发送验证码事件注册
$('#sms-send-cd-btn').on('click', function () {
    var regkey = $('#regkey').val();
    verifyCodeSend(regkey)
})

// 未收到验证码发送语音验证码事件
$('.js-no-ver-code').on('click', function () {
    function smsCodeConfirm() {
        $('.pop-up-confirm .pop-confirm').val("提交中...").attr('disabled', true);
        var regkey = $('#regkey').val();
        Voicecode(regkey)
    }
    if($('#sms-send-cd-btn').attr('time') == 0){
        passPageToast("正在发送验证码，请稍后再试");
        return false;
    }
    createPassConfirmPop(zPassport.messagesWeb.pop_click_send_voice_vericode, "", smsCodeConfirm)
    $('.pop-up-confirm .pop-confirm').val(zPassport.messagesWeb.common_send)
})

// 给区号列表注册click事件
function bindEventArea() {
    $('body .code-list-ul').on('click', 'li', function () {
        $('.js-area-box').hide();
        var areaId = $(this).attr('data-id');
        var areaCode = $(this).attr('data-code');
        $('#country-area-code').text(areaCode);
        $('#country-code-ipt').val(areaCode)
        activatePhoneLoginVerCode()
    })
}

// 获取手机区号成功后的回调
function countryPhoneCodeSuccessCb(data) {
    var areaList = '<div class="js-scroll-covers"><ul class="code-list-ul">'
    $.each(data, function (areaIndex, areaVal) {
        areaList += '<li data-id="' + areaVal.id + '" data-code="' + areaVal.country_code + '">' + areaVal.country_name +
            '<span class="p-area-code">' + areaVal.country_code + '</span>' +
            '</li>';
    })

    areaList += '</ul></div>';
    if (!$('.code-list-ul').length) {
        $('.area-code-covers').html(areaList)
    }
    // 重新赋值获取元素，因为是动态生成的html
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
var passDomain = location.host.indexOf('test') !== -1 ? 'http://static.test.zcool.cn' : 'https://static.zcool.cn'
// 请求ajax获取手机区号
function getPassAreaAsync() {
    $('.area-code-covers').html("");
    var loaddingUrl = passDomain + '/passport4.0/images/common/page_loading.gif'
    $('.area-code-covers').show().append('<div class="text-center"><img src="' + loaddingUrl + '" /></div>');
    $.ajax({
        url: "/getCountryPhoneCodes.do?jsonpCallback=?",
        type: "get",
        dataType: 'jsonp',
        jsonp: 'callback',
        success: function (data) {
            if (data.result && data.data.length) {
                countryPhoneCodeSuccessCb(data.data)
            } else {
                passPageToast(data.msg);
            }
        },
        error: function () {
            passPageToast('操作失败，请稍后重试');
        }
    });
}

// 区号弹窗show，弹窗append到了父级页面，因为有遮罩
function areaPopUpShow() {
    var html = '<div class="conutry-area-code-wrap js-area-box">' +
        '<div class="area-code-covers">' +

        '</div>' +
        '</div>'
    if ($('.js-area-box').is(':visible')) {
        $('.js-area-box').hide()
    } else {
        if (!$('.js-area-box').length) {
            $('.js-phone-div').append(html)
        } else {
            $('.js-area-box').show();
        }
    }
    // 如果国家区号请求成功后不用再请求一遍了
    if (!$('.code-list-ul').length) {
        getPassAreaAsync()
    }
}

// 点击区号区域显示地区区号列表
$('#country-area-code').on('click', function () {
    areaPopUpShow()
})

//   密码，手机 登录TAB切换
$('.l-tab-covers .l-tab-list').on('click', function () {
    var thisIndex = $(this).index();
    $(this).addClass('current').siblings('.l-tab-list').removeClass('current');
    $('.static-module-covers > div[class*="login-covers"]').hide();
    $('.static-module-covers > div[class*="login-covers"]').eq(thisIndex).show();
    // parent.reinitIframe() 
})

// 微信登录，账号登录，图标切换事件
$('.login-switch .icon-l').on('click', function () {
    if ($(this).hasClass('static-l')) {
        $('.login-box').addClass('module-l-quick').removeClass('module-l-static')
        wxCodeLogin()
    } else {
        $('.login-box').addClass('module-l-static').removeClass('module-l-quick')
    }
})

// 密码，手机登录切换事件
$('.pass-login-tab').on('click', function () {
    $('.login-box').addClass('module-l-static').removeClass('module-l-quick');
    $('.l-tab-covers .l-tab-list:first').addClass('current').siblings('.l-tab-list').removeClass('current');
    $('.static-module-covers > div[class*="login-covers"]').hide();
    $('.static-module-covers > div[class*="login-covers"]:first').show();
})

// aliyun滑块
function noCaptchaNc() {
    var nc = new noCaptcha();
    var nc_appkey = 'FFFF00000000016E5A77';  // 应用标识,不可更改
    var nc_scene = 'message';  //场景,不可更改
    var nc_token = [nc_appkey, (new Date()).getTime(), Math.random()].join(':');
    var nc_option = {
        renderTo: '#dom_id',//渲染到该DOM ID指定的Div位置
        appkey: nc_appkey,
        scene: nc_scene,
        token: nc_token,
        callback: function (data) {// 校验成功回调

            $('#a_session').val(data.csessionid);
            $('#a_sig').val(data.sig);
            $('#a_token').val(nc_token);
            $('#a_scene').val(nc_scene);
            $('#a_from').val(3);
            if ($('#sms-phone').val() != 0) {
                $('.js-ipt-h').show();
            }
            activatePhoneLoginVerCode()
        }
    };
    nc.init(nc_option);
}
noCaptchaNc()
