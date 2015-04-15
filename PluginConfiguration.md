

# Introduction #

It is possible to configure plugin behaviour on the creating object stage:
```
        $(document).ready(function() {
            $('#Button1').serverBrowser({
                onSelect: function(path) {
                    $('#TextBox1').val(path);
                },
                onLoad: function() {
                    return $('#TextBox1').val();
                },
 		imageUrl: '<%=ResolveUrl("~/img/icons/") %>',
 		systemImageUrl: '<%=ResolveUrl("~/img/browser/") %>',
                handlerUrl: '<%=ResolveUrl("~/GetPath.aspx") %>',
                title: 'Browse1',
                basePath: 'C:',
            });
        });
```

# Details #

## Required configuration parameters ##

The following parameters are required. Even they have default value, it will be good to reconfigure them for your need:

| **Parameter** | **Description** | **Default value**|
|:--------------|:----------------|:-----------------|
| onSelect | Function, that will be callled when user click 'Open' button on the server browser dialog. The only parameter is the path list, that was selected by the user | function(file) { alert('You select: ' + file); } |
| imageUrl | Url, that will be added for all png files with extension name (for example 'exe.png', 'zip.png'. If systemImageUrl is not set, imageUrl will be used for system images | img |
| handlerUrl | Page on the server, that will handle 'GET' request from the plugin, and return data in json format | browserDlg.txt |

## Other supported parameters ##

The following parameters are optional and can be used with default values:

| **Parameter** | **Description** | **Default value**|
|:--------------|:----------------|:-----------------|
| title | Dialog title string | 'Browse' |
| basePath | Path that will be used as root path on the browser dialog. Also this can be configured on the server side (and it always should be checked on the server side for security reasons) | '' |
| knownExt | Array with file extension list that will be used to display icons. If this list is empty, all files will have image for its extension. Otherwise only extension specified in this list will have its images. Other files will show 'unknown.png' image | [[ ]] |
| onCancel | Function, that will be called when user click 'Cancel' button. No parameters provided. |function() { } |
| onLoad | Function that return path, that should be opened after dialog displayed. Usually it should be a path from a textBox associated with this dialog. |function() { return config.basePath; } |
| multiselect | If set, user will be able to select more then 1 file/directory at once. onSuccess function will receive array as parameter. | false |
| systemImageUrl | Url to system images like 'folder.png', 'loading.gif', 'unknown.png', and other images from knownPath list | '' |
| showUpInList | If set, '..' directory will be added on the top of the directories list |false |
| separatorPath | Separator string that will be added between directories | '/' |
| useKnownPaths | If set, left part of the dialog will contain elements from knownPaths list | true|
| knownPaths | List paths, images and descriptions, that will be shown on the left side of the dialog. When user click on image, specified path will be opened. For more information see [Known Paths](KnownPaths.md) wiki page. | Documents, Desktop array |
| resizable | If it is true, dialog will be resizable  | true |
| width | default dialog width | 300 |
| height | default dialog height | 300|