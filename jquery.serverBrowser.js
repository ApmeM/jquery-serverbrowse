/*
    author: ApmeM
    date: 9-June-2010
    version: 1.0
*/

(function($) {
    $.fn.serverBrowser = function(settings) {
        this.each(function() {

            var config = {
                onSelect: function(file) {
                    alert('You select: ' + file);
                },
                imageUrl: 'img',
                handlerUrl: 'browserDlg.txt',
                title: 'Browse',
                basePath: '',
                knownExt: [],
                resizable: true,
                width: 300,
                height: 300
            };

            if (settings) $.extend(config, settings);

            var button = $(this);

            var browserDlg = $('<div title="' + config.title + '"></div>').appendTo(document.body);
            var browser = $('<div></div>').appendTo(browserDlg);

            browserDlg.dialog(
                {
                    autoOpen: false,
                    modal: true,
                    position: ['center', 'top'],
                    resizable: config.resizable,
                    width: config.width,
                    height: config.height,
                    buttons: {
                        "Cancel": function() {
                            $(this).dialog("close");
                        },
                        "Ok": function() {
                            config.onSelect(config.itemCurrentPath);
                            $(this).dialog("close");
                        },
                    }
                });

            button.bind('click', function() {
                loadPath(browser, config.basePath);
                browserDlg.dialog('open');
            });

            function loadPath(div, path) {
                var XHRRequest;

                if (XHRRequest)
                    XHRRequest.abort();

                XHRRequest = $.ajax({
                    url: config.handlerUrl,
                    type: 'GET',
                    data: {
                        action: 'browse',
                        path: path,
                        time: new Date().getTime()
                    },
                    beforeSend: function() {
                        div.empty().css({ 'text-align': 'center' });
                        $('<img />').attr({ src: config.imageUrl + 'loading.gif' }).css({ width: '32px' }).appendTo(div);
                    },
                    success: function(files) {
                        div.empty().css({ 'text-align': 'left' });
                        $.each(files, function(index, file) {
                            config.currentDir = file.dir;
                            var itemDiv = $('<div></div>').css({ margin: '2px' }).appendTo(div);
                            if(file.isError)
                            {
                                itemDiv.addClass('ui-state-error ui-corner-all').css({padding: '0pt 0.7em'});
                                var p = $('<p></p>').appendTo(itemDiv);
                                $('<span></span>').addClass('ui-icon ui-icon-alert').css({'float': 'left', 'margin-right': '0.3em'}).appendTo(p);
                                $('<span></span>').text(file.name).appendTo(p);
                            }else
                            {
                                itemDiv.addClass('ui-state-default').hover(
                                    function() { $(this).addClass('ui-state-hover'); },
                                    function() { $(this).removeClass('ui-state-hover'); }
                                );
                                var itemImage = $('<img />').css({ width: '32px', margin: '0 5px 0 0' }).appendTo(itemDiv);
                                var itemText = $('<span></span>').text(file.name).appendTo(itemDiv);
                                if (file.isFolder == 'true')
                                    itemImage.attr({ src: config.imageUrl + 'folder.png' });
                                else {
                                    ext = file.name.split('.').pop();
                                    if (ext == '' || ext == file.name || (config.knownExt.length > 0 && $.inArray(ext, config.knownExt) < 0))
                                        ext = 'unknown';
                                    itemImage.attr({ src: config.imageUrl + ext + '.png' });
                                }

                                var firstClick = true;

                                itemDiv.click( function(e) {
                                    var clickedItemLink = $(this);
                                    if (config.itemCurrent) {
                                        config.itemCurrent.removeClass('ui-state-active');
                                        config.itemCurrent.addClass('ui-state-default');
                                    }
                                    clickedItemLink.addClass('ui-state-active');
                                    clickedItemLink.removeClass('ui-state-default');
                                    config.itemCurrent = clickedItemLink;
                                    config.itemCurrentPath = path + file.name;

                                    if (file.isFolder == 'true') {
                                        if(firstClick)
                                        {
                                            var innerDiv = $('<div></div>').insertAfter(clickedItemLink).css({ padding: '0 0 0 10px' });
                                            loadPath(innerDiv, path + file.name);
                                        }else {
                                            clickedItemLink.next().animate({ height: ['hide'] }, 300, function() { $(this).remove(); });
                                        }
                                    }
                                    firstClick = !firstClick;
                                });
                            }
                        });
                    },
                    dataType: 'json'
                });
            }
        });
        return this;
    };
})(jQuery);
