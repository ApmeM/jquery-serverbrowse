

# Introduction #

Response data that should be returned by the server.


# Details #

Server should return data in json format to be usable by the plugin.

Each row should contain the following parameters:

  * name - this string will be displayed on the dialog, and will be joined with upper level names for future requests.
  * isFolder - if not null, plugin will add 'folder.png' image to the folder name, and will make it expandable.
  * isError - if not null ui-state-error css class will be added to this element and it become unselectable.