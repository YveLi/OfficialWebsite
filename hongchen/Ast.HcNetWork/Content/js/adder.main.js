/*!
 * adder v1.0 (http://www.byrrw.com)
 * Copyright 2018.
 * Author.kehen
 * 2018.4.8
 */
var adder = {};
var tableIns;
var step = $('#step');
//url, tableid, cols
var To;
var Lay;
$(function () {
    if (window.top !== window.self) {
        To = window.top.toastr;
        Lay = window.top.layer;
    }
    else {
        To = toastr;
        Lay = layui.layer;
    }
});
var fw = {
    msg: function (content, type, title) {
        To.options = {
            closeButton: true,
            debug: false,
            progressBar: true,
            positionClass: 'toast-container toast-top-center',
            showDuration: '400',
            showEasing: 'swing',
            hideEasing: 'linear',
            showMethod: 'fadeIn',
            hideMethod: 'fadeOut',
            preventDuplicates: true,
            preventOpenDuplicates: true,
            timeOut: 2000,
            extendedTimeOut: 1000,
        }
        switch (type) {
            case 0:
                //错误消息提示，默认背景为浅红色 
                To.error(content, title ? title : "错误");
                break;
            case 1:
                //成功消息提示，默认背景为浅绿色
                To.success(content, title ? title : "成功");
                break;
            case 2:
                //警告消息提示，默认背景为橘黄色 
                To.warning(content, title ? title : "警告");
                break;
            default:
                To.info(content, title ? title : "提示");
                break;
        }
        //另一种调用方法
        //toastr["info"]("你有新消息了!", "消息提示");
    },
    confirm: function (content, callBack) {
        Lay.confirm(content, {
            icon: "fa-exclamation-circle",
            title: "消息提醒",
            btn: ['确认', '取消'],
            btnclass: ['btn btn-primary', 'btn btn-danger'],
        }, function (index) {
            callBack(true, index);
        }, function (index) {
            callBack(false, index);
        });
    },
    loading: {
        ix: 0,
        Open: function () {
            this.ix = Lay.load(1, { shade: [0.5, "#000"], time: 50 * 1000 }); //Lay.msg('请稍候...', { icon: 16, shade: [0.5, "#000"], time: 0 });
        },
        Close: function () {
            Lay.close(this.ix);
        }
    },
    Mapping: function (json, obj, objName) {
        //json 映射到 ko 实体中去
        for (var prop in json) {
            if (obj.hasOwnProperty(prop)) {
                eval(objName + "." + prop + "('" + (json[prop] ? json[prop] : "") + "')");
            }

        }
    },
    vModelEmpty: function (obj, objName) {
        //重置ko 的 实体
        for (item in obj) {
            eval(objName + "." + item + "('')");
        }
    },
}
adder.showbox = function (op) {
    var id = op.id ? op.id : "toast-container";
    To.options = {
        closeButton: true,
        debug: false,
        progressBar: true,
        positionClass: 'toast-container toast-top-right',
        showDuration: '400',
        showEasing: 'swing',
        hideEasing: 'linear',
        showMethod: 'fadeIn',
        hideMethod: 'fadeOut',
        preventDuplicates: false,
        preventOpenDuplicates: false,
        timeOut: 0,
        //autoDismiss: true,
        //newestOnTop: true,
        extendedTimeOut: 1000,
        containerId: id,
        onclick: function () {
            op.func(id, op.type, op.title);
        }
    }
    switch (op.type) {
        case 0:
            //错误消息提示，默认背景为浅红色 
            To.error(op.content, op.title ? op.title : "错误");
            break;
        case 1:
            //成功消息提示，默认背景为浅绿色
            To.success(op.content, op.title ? op.title : "成功");
            break;
        case 2:
            //警告消息提示，默认背景为橘黄色 
            To.warning(op.content, op.title ? op.title : "警告");
            break;
        default:
            To.info(op.content, op.title ? op.title : "提示");
            break;
    }
}
adder.initPower = function () {
    $.getJSON("/Sys/GetPower", { menuid: adder.getQuery("MenuId") }, function (PowerModel) {
        if (PowerModel) {
            for (var item in PowerModel) {
                if (!PowerModel[item]) {
                    $("*[data-power=" + item + "]").remove();
                }
            }
        }
    });
}
adder.loadlist = function (op) {
    var tableid = op.tableid ? op.tableid : "datagrid";
    var height = op.height == 0 ? "" : op.height ? 'full-' + op.height : 'full-130'
    var limit = op.limit ? op.limit : 15;
    var skin = op.skin ? op.skin : '';
    var size = op.size ? op.size : '';
    var page = op.page == false ? op.page : true;
    layui.use(['layer', 'table'], function () {
        var table = layui.table;
        var layer = layui.layer;
        tableIns = table.render({
            elem: '#' + tableid
            , id: tableid
            , url: op.url
            , cols: op.cols
            , height: height
            , cellMinWidth: 90
            , page: page
            , limits: [15, 20, 40, 60, 80]
            , limit: limit //默认采用10
            , skin: skin
            , size: size
            , response: {
                statusName: 'code' //数据状态的字段名称，默认：code
                , statusCode: 0 //成功的状态码，默认：0
                , msgName: 'msg' //状态信息的字段名称，默认：msg
                , countName: 'count' //数据总数的字段名称，默认：count
                , dataName: 'datalist' //数据列表的字段名称，默认：data
            }
            , done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                //console.log(res);
                //if (res != null) {
                //    firstdb = res.datalist[0].Id;
                //}
                //得到当前页码
                //console.log(curr);
                //$("#curPageIndex").val(curr);
                //得到数据总量
                //console.log(count);
            }
        });
    });
}
adder.getastro = function (t) {
    var str = "";
    switch (t) {
        case 0:
            str = "白羊座";
            break
        case 1:
            str = "金牛座";
            break
        case 2:
            str = "双子座";
            break
        case 3:
            str = "巨蟹座";
            break
        case 4:
            str = "狮子座";
            break
        case 5:
            str = "处女座";
            break
        case 6:
            str = "天秤座";
            break
        case 7:
            str = "天蝎座";
            break
        case 8:
            str = "射手座";
            break
        case 9:
            str = "摩羯座";
            break
        case 10:
            str = "水瓶座";
            break
        case 11:
            str = "双鱼座";
            break
    };
    return str;
}
adder.msg = function (t) {
    switch (t) {
        case 0:
            fw.msg("操作失败", 0)
            break;
        case 1:
            fw.msg("操作成功", 1)
            break;
    }
}
//弹窗
adder.dialog = function (s) {
    //var index = parent.admin.popupRight({ //一切正常
    //var index = parent.admin.popup({ //ifream 计算高度计算不对
    var index = layer.open({ //一切正常
        id: s.id,
        type: s.type ? s.type : 2,
        title: s.title,
        shadeClose: true,
        shade: s.shade ? s.shade : 0.2,
        maxmin: s.maxmin ? s.maxmin : true,
        area: s.area ? s.area : ['90%', '90%'],
        //skin: 'layui-anim-upbit',
        anim: s.anim ? s.anim : Math.floor(Math.random() * 6 + 1),
        resize: s.resize ? s.resize : true,
        content: s.url ? s.url : s.content,
        offset: s.offset,
        success: function (layero, index) {
        }
    });
    layer.iframeAuto(index);
}
adder.topdialog = function (s) {
    var index = parent.layer.open({ //一切正常
        id: s.id,
        type: s.type ? s.type : 2,
        title: s.title,
        shadeClose: true,
        shade: s.shade ? s.shade : 0,
        area: s.area ? s.area : ['90%', '90%'],
        //skin: 'layui-layer-lan',
        anim: s.anim ? s.anim : Math.floor(Math.random() * 6 + 1),
        content: s.url ? s.url : s.content,
        offset: s.offset,
        success: function (layero, index) {
        }
    });
}
adder.admindialog = function (s) {
    var index = admin.popup({ //一切正常
        id: s.id,
        type: s.type ? s.type : 2,
        title: s.title,
        shadeClose: true,
        shade: s.shade ? s.shade : 0,
        area: s.area ? s.area : ['90%', '90%'],
        skin: 'layui-anim-upbit',
        anim: s.anim ? s.anim : Math.floor(Math.random() * 6 + 1),
        content: s.url ? s.url : s.content,
        offset: s.offset,
        success: function (layero, index) {
        }
    });
}
adder.save = function (i) {
    layui.use('form', function () {
        var form = layui.form;
        //监听提交
        form.on('submit(form)', function (data) {
            $.ajax({
                url: i.url, //method controller
                data: i.type === undefined ? data.field : JSON.stringify(data.field),
                type: i.type === undefined ? "get" : "post",
                contentType: 'application/json;charset=utf-8',
                success: function (res) {
                    //window.parent.refresh();
                    window.parent.init();
                    window.parent.adder.msg(1);
                    var index = parent.layer.getFrameIndex(window.name);
                    parent.layer.close(index);
                },
                error: function (res) {
                    window.parent.adder.msg(0);
                }
            });
            return false;
        });
    });
}
adder.infosave = function (i) {
    layui.use('form', function () {
        var form = layui.form;
        //监听提交
        form.on('submit(form)', function (data) {
            $.ajax({
                url: i.url, //method controller
                data: i.type === undefined ? data.field : JSON.stringify(data.field),
                type: i.type === undefined ? "get" : "post",
                contentType: 'application/json;charset=utf-8',
                success: function (res) {
                    //$(parent.document.getElementById(i.id))[0].children[0].contentWindow.init();
                    i.func ? i.func() : "";
                    window.parent.init();
                    window.parent.adder.msg(1);
                    var index = parent.layer.getFrameIndex(window.name);
                    parent.layer.close(index);
                },
                error: function (res) {
                    window.parent.adder.msg(0);
                }
            });
            return false;
        });
    });
}
//编辑
adder.update = function (s) {
    var query = s.query ? s.query : "";
    layui.use(['layer', 'table'], function () {
        var table = layui.table;
        var layer = layui.layer;
        var selects = table.checkStatus(s.id ? s.id : "datagrid");
        if (selects.data.length === 0) {
            parent.layer.msg('很遗憾，请勾选您要修改的数据！', { icon: 5, time: 1200 });
            return;
        }
        if (selects.data.length > 1) {
            parent.layer.msg('只能勾选一条数据进行修改！', { icon: 5, time: 1200 });
            return;
        } else {
            var index = layer.open({
                type: 2,
                title: s.title,
                shadeClose: true,
                shade: s.shade ? s.shade : 0.2,
                maxmin: true,
                area: s.area ? s.area : ['90%', '90%'],
                //skin: 'layui-layer-admin',
                anim: Math.floor(Math.random() * 6 + 1),
                resize: true,
                content: s.url + "?id=" + selects.data[0].Id + query,
                offset: s.offset,
                success: function (layero, index) {
                }
            });
            layer.iframeAuto(index);
        }
    });
}

//删除
adder.del = function (data) {
    layer.confirm("确认要删除？", { icon: 3, title: '提示' }, function (index) {
        $.ajax({
            type: data.type === undefined ? "get" : "post",
            url: data.url,
            async: data.async === undefined ? true : false,
            success: function (e) {
                tableIns.reload({
                    page: {
                        curr: 1
                    }
                });
                parent.layer.msg('删除成功', { icon: 1, shade: 0.4, time: 1200 })
            }
        });
        layer.close(index);
    });
};

//批量删除
adder.batchDel = function (data) {
    layui.use(['layer', 'table'], function () {
        var table = layui.table;
        var layer = layui.layer;
        var tableid = data.tableid ? data.tableid : 'datagrid'
        var selects = table.checkStatus(tableid);
        if (selects.data.length === 0) {
            parent.layer.msg('很遗憾，请勾选您要删除的选项！', { icon: 5, time: 1200 });
            return;
        }
        var ids = "";
        $.each(selects.data, function (n, m) {
            if (m.Id == undefined) {
                ids += m.id + ",";
            } else {
                ids += m.Id + ",";
            }
        });
        ids = ids.substring(0, ids.length - 1);
        layer.confirm("确认删除勾选项？", { icon: 3, title: '提示' }, function (index) {
            $.ajax({
                type: data.type === undefined ? "get" : "post",
                url: data.url + "?id=" + ids,
                async: data.async === undefined ? true : false,
                success: function (e) {
                    table.reload(tableid, {
                        page: {
                            curr: 1
                        }
                    });
                    parent.layer.msg('删除成功', { icon: 1, shade: 0.4, time: 1200 })
                }
            });
            layer.close(index);
        });
    });
}

adder.render = function () {
    //重新渲染下 form 
    layui.use('form', function () {
        var form = layui.form;
        form.render();
    });
}
//日期初始化
adder.LayDate = function (e) {
    layui.use(['laydate', 'form'], function () {
        var form = layui.form;
        var mydata = new Date();
        var laydate = layui.laydate;
        if (e == null) {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this,
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "#1E9FFF",
                    //value: mydata
                });
            });
        } else if (e == 1) {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this,
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "#1E9FFF",
                    type: 'datetime'
                });
            });
        } else if (e == 2) {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this,
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "#1E9FFF",
                    range: true
                });
            });
        } else {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this
                    , trigger: 'click',
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "#1E9FFF",
                    type: 'datetime'
                });
            });
        }
    });
}
//格式化带T的日期
adder.formatDate = function (v) {
    var dateee = new Date(v).toJSON();
    var date = new Date(+new Date(dateee) + 8 * 3600 * 1000).toISOString().replace(/T00:00:00/g, ' ').replace(/\.[\d]{3}Z/, '');
    return date;
}
//table日期格式
adder.dateformat = function (value, type) {
    var str = "-";
    if (value != null) {
        var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        if (type == 1) {
            str = date.getFullYear() + "-" + month + "-" + day + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
        } else {
            str = date.getFullYear() + "-" + month + "-" + day;
        }
        if (str.indexOf('1-01-01') > -1) {
            str = '-';
        }
    }
    return str;
}
//日期加减
adder.addDate = function (date, days) {
    var d = new Date(date);
    if (days > 0) {
        d.setDate(d.getDate() + days - 1);
        var month = d.getMonth() + 1 < 10 ? "0" + (d.getMonth() + 1) : d.getMonth() + 1;
        var day = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
        return d.getFullYear() + "-" + month + "-" + day;
    } else {
        layer.msg("请填写行程天数！", { icon: 2, time: 1000 });
        return false;
    }
}
adder.getweek = function (date, days) {
    var d = new Date(date);
    d.setDate(d.getDate() + days - 1);
    var month = d.getMonth() + 1 < 10 ? "0" + (d.getMonth() + 1) : d.getMonth() + 1;
    var day = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
    var l = ["周日", "周一", "周二", "周三", "周四", "周五", "周六"];
    var week = l[d.getDay()];
    return d.getFullYear() + "-" + month + "-" + day + "-" + week;
}
//加载多选
adder.loadSelect = function () {
    layui.config({
        base: '../Content/js/'
    }).extend({
        formSelects: 'formSelects-v4'
    });
}
adder.producttype = function (x, y, url1, url2) {
    productEvent.one(x, y, url1, url2);
}
var productEvent = {
    one: function (x, y, url1, url2) {
        $.getJSON(url1, {}, function (res) {
            var proHtml = '';
            $.each(res, function (a, p) {
                proHtml += '<option value="' + p.Id + '">' + p.name + '</option>';
            });
            $("#" + x).html(proHtml);
            layui.use('form', function () {
                var form = layui.form;
                form.on('select(' + x + ')', function (data) {
                    if (data.value === "0") {
                        //worldAreaEvent.country_city("-1", y);
                    }
                    else {
                        productEvent.two(data.value, y, url2);
                        adder.render();
                    }
                });
            });
        });
    },
    two: function (Id, y, url2) {
        var twoHtml = '';
        $.getJSON(url2, { Id: Id }, function (twodata) {
            $.each(twodata, function (a, c) {
                twoHtml += '<option value="' + c.Id + '">' + c.name + '</option>';
            });
            $("#" + y).html(twoHtml);
            layui.use('form', function () {
                var form = layui.form;
                form.render();
            });
        });
    },
};
adder.strFormat = function (str) {
    if (str == null || str == undefined) {
        str = "-";
    }
    return str;
}
adder.getQuery = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}
adder.unique = function (arr) {
    var newarr = [];
    for (var i = 0; i < arr.length; i++) {
        var items = arr[i];
        //判断元素是否存在于new_arr中，如果不存在则插入到newarr的最后
        if ($.inArray(items, newarr) == -1) {
            newarr.push(items);
        }
    }
    return newarr.join(',');
}
adder.initStep = function (index) {
    //var id = adder.getQuery('Id');
    //if (id==0) {
    //    layer.msg("请提交团信息", { icon: 2, time: 1200 });
    //    return false;
    //}
    step.toStep(index);
    //switch (index) {
    //    case 0:
    //        location.href = '/Group/Info?Id=' + id;
    //        break
    //    case 1:
    //        location.href = '/Group/TripDesign?Id=' + id;
    //        break
    //    case 2:
    //        location.href = '/Group/TripQuote?Id=' + id;
    //        break
    //    case 3:
    //        location.href = '/Group/Operate?Id=' + id;
    //        break
    //    case 4:
    //        location.href = '/Group/Clearing?Id=' + id;
    //        break
    //}
}
adder.loadStep = function (index) {
    step.step({
        index: index,
        time: 1000,
        title: ["<a class='layui-btn layui-btn-xs layui-btn-black'  href='avascript:;'>创建团信息</a>", "<a class='layui-btn layui-btn-xs layui-btn-red'  href='avascript:;'>行程设计</a>", "<a class='layui-btn layui-btn-xs layui-btn-lan'  href='avascript:;'>行程报价</a>", "<a class='layui-btn layui-btn-xs layui-btn-nom'  href='avascript:;'>客户确认</a>", "<a class='layui-btn layui-btn-xs layui-btn-purple'  href='avascript:;' >下单操作</a>", "<a class='layui-btn layui-btn-xs layui-btn-suc'  href='avascript:;'>财务结算</a>"]
    });
}

adder.LocalChangeItem = function (x, y) {
    if (!window.localStorage) {
        return false;
    } else {
        var xtemp = adder.LocalGetItem(x);
        var ytemp = adder.LocalGetItem(y);
        adder.LocalSetItem(x, ytemp);
        adder.LocalSetItem(y, xtemp);
    }
}
//保存前端缓存
adder.LocalSetItem = function (id, value) {
    if (!window.localStorage) {
        return false;
    } else {
        localStorage.setItem(id, JSON.stringify(value));
    }
}
//获取前端缓存
adder.LocalGetItem = function (id) {
    if (!window.localStorage) {
        return false;
    } else {
        return JSON.parse(localStorage.getItem(id));
    }
}
//删除
adder.LocalRemoveItem = function (id) {
    if (!window.localStorage) {
        return false;
    } else {
        localStorage.removeItem(id);
    }
}
//城市拖拽
function cityDrop(ev) {
    ev.preventDefault();
}
var citysrcdiv = null;
function citydrag(ev, divdom) {
    citysrcdiv = divdom;
    ev.dataTransfer.setData("text/html", divdom.innerHTML);
}

function citydrop(ev, divdom) {
    ev.preventDefault();
    if (citysrcdiv != divdom) {
        var temp1 = citysrcdiv.innerHTML;
        var temp2 = divdom.innerHTML;
        var id1 = citysrcdiv.id;
        var id2 = divdom.id;
        $.each(dayarr, function (x, y) {
            $.each(y.place, function (j, k) {
                if (k.Id == id1) {
                    k.Id == id2;
                    k.name = temp2;
                }
                if (k.Id == id2) {
                    k.Id == id1;
                    k.name = temp1;
                }
            });
        });
        bandtable();
    }
}
//景点拖拽
function pointDrop(ev) {
    ev.preventDefault();
}
var pointsrcdiv = null;
function pointdrag(ev, divdom) {
    pointsrcdiv = divdom;
    ev.dataTransfer.setData("text/html", divdom.innerHTML);
}
function pointdrop(ev, divdom) {
    ev.preventDefault();
    if (pointsrcdiv != divdom) {
        var temp1 = pointsrcdiv.innerHTML;
        var temp2 = divdom.innerHTML;
        var id1 = pointsrcdiv.id;
        var id2 = divdom.id;
        $.each(dayarr, function (x, y) {
            $.each(y.point, function (j, k) {
                if (k.Id == id1) {
                    k.Id == id2;
                    k.name = temp2;
                }
                if (k.Id == id2) {
                    k.Id == id1;
                    k.name = temp1;
                }
            });
        });
        bandtable();
    }
}
//日期拖拽
function dateDrop(ev) {
    ev.preventDefault();
}
var datesrcdiv = null;
function datedrag(ev, divdom) {
    datesrcdiv = divdom;
    ev.dataTransfer.setData("text/html", divdom.innerHTML);
}

function datedrop(ev, divdom) {
    ev.preventDefault();
    if (datesrcdiv != divdom) {
        var temp1 = datesrcdiv.innerHTML;
        var temp2 = divdom.innerHTML;
        var id1 = datesrcdiv.id;
        var id2 = divdom.id;
        datesrcdiv.innerHTML = temp2;
        divdom.innerHTML = temp1;
        datesrcdiv.id = id2;
        divdom.id = id1;
    }
}

adder.customNo = function (first, table, z) {
    var Id = $("#Id").val();
    if (Id == 0) {
        $.getJSON("../Common/GetCustomNo", { name: first, table: table }, function (data) {
            $("#" + z + "").val(data);
        });
    }
}

adder.ReturnFixStr = function (str, len) {
    if (len == undefined || len == 0)
        len = 10;
    if (str.length > len) {
        return str.substring(0, len) + "...";
    }
    return str;
}
adder.openshowbox = function (id, type, title) {
    NoticeCount = NoticeCount - 1;
    var dialog = {
        title: title, // 窗口名称
        area: ["50%", "50%"],
        shade: 0.2,
        url: "../Common/MsgInfo?id=" + id + "&type=" + type
    }
    adder.admindialog(dialog);
}
adder.lobibox = function (op) {
    NoticeCount = NoticeCount + 1;
    var str = "";
    switch (op.type) {
        case 0:
            str = "error";
            break;
        case 1:
            str = "success";
            break;
        case 2:
            str = "info";
            break;
        case 3:
            str = "warning";
            break;
        default:
            str = "error";
            break;
    }
    Lobibox.notify(str,
        {
            sound: true,
            title: op.title,
            width: 285,
            icon: false,
            size: 'mini',
            //iconSource: 'fontAwesome',
            msg: op.content,
            delay: 0,
            onClick: function () {
                op.func(op.id, op.type, op.title);
            }
        });
}
