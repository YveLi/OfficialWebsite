/**
 * Copyright 2018 totravel
 *
 * Licensed under Apache License 2.0 (https://github.com/totravel/toc/blob/master/LICENSE)
 */

function Toc() {
    // 配置
    this.config = {

        // 标签元素的选择器，直接决定哪些级别的标题会出现在目录中
        headingSelector: 'h2, h3, h4',

        // 每个标题锚点名的前缀
        anchorPrefix: 'toc-',

        /**
         * 生成目录的每个条目，默认使用被 li 标签包含的 a 标签
         *
         * @param  {string} anchor 对应标题的锚点
         * @param  {string} text   对应标题的文本内容
         * @param  {int}    level  对应标题级别
         * @return {[type]}        [description]
         */
        makeItem: function (anchor, text, level) {

            return '<li><a href="#' + anchor + '">' + text + '</a></li>';
        },

        /**
         * 生成每个目录层级的容器，默认使用 ul 标签
         *
         * @param  {int}    level 对应标题级别
         * @return {[type]}       [description]
         */
        makeWrapper: function (level) {

            // this.makeWrapper = function (level) {
            //     return '<ul></ul>';
            // };

            // return '<ul class="list-unstyled"></ul>';
            return '<ul></ul>';
        },
    };

    // 上一个标题的级别
    this.lastLevel = 0;

    // 上一个标题对象
    this.lastHeading = null;

    // 设置
    this.setting = function (config) {

        /**
         * 合并 config 到 this.config
         *
         * 后者覆盖前者，但如果后者有前者不存在的 key，
         * 则该 key 将被忽略，以确保不会破坏前者的结构。
         */
        this.config = $.extend({}, this.config, config);

        return this;
    }

    /**
     * 获取标题的级别，如：对于 h1 返回 1，对于 h2 返回 2。
     *
     * @param  obj heading 标题对象
     * @return int         标题的级别
     */
    this.getLevel = function (heading) {

        return parseInt(heading[0].tagName.slice(1));
    };

    /**
     * 开始制作目录
     *
     * [make description]
     * @param  {string}   content  要为其中的标题元素建立目录索引的容器
     * @param  {string}   toc      目录容器
     * @param  {Function} callback 目录生成完成的回调函数
     * @return {[type]}            [description]
     */
    this.make = function (content, toc, callback = function () {console.log('totravel/toc: done!')}) {

        var that     = this;

        var config   = this.config;

        var headings = content.find(config.headingSelector);

        var tocWarp  = toc;

        // 当前标题所在目录层级的容器
        var wrapper;

        that.lastHeading = tocWarp;

        headings.each(function (index) {

            var heading = $(this);
            // 标题文本
            var text    = heading.text();
            // 标题锚点名
            var anchor  = config.anchorPrefix + index;

            // 为标题设置锚点
            heading.attr('id', anchor);

            var level = that.getLevel(heading);

            if (level > that.lastLevel) {
                /**
                 * 当前标题的级别数值上大于上一标题，
                 * 意味着当前标题为上一标题的子标题。
                 *
                 * 新建一个目录层级的容器追加到上一标题元素中。
                 */
                wrapper = $(config.makeWrapper(level));
                that.lastHeading.append(wrapper);

            } else if (level < that.lastLevel) {
                /**
                 * 当前标题的级别数值上小于上一标题，
                 * 意味着当前标题与上一标题的父标题同级。
                 *
                 * 将 wrapper 设为上一标题父标题所在容器。
                 *
                 * 第一次 parent() 得到上一标题，
                 * 第二次 parent() 得到上一标题父标题所在容器。
                 */
                wrapper = wrapper.parent().parent();
            }

            var item = $(config.makeItem(anchor, text, level)); // 将当前标题的目录索引条目
            wrapper.append(item);                               // 追加到当前目录层级的容器中

            that.lastLevel   = level;
            that.lastHeading = item;
        }); // headings.each() end

        callback();
    };
}