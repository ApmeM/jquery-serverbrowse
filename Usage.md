

# Introduction #

Usage of this plugin is simple. Please check you have all [requirements](Requirements.md) before you start.


# Details #

This plugin have client and server sides, that should be configured to use this plugin.

## Client side ##

To use this plugin you will need to include jquery.serverBrowser.js file with other jQuery js files on the page.

```
<script src="/js/jquery.js" type="text/javascript"></script>

<script src="/js/jquery-ui.js" type="text/javascript"></script>
    
<script src="/js/jquery.serverBrowser.js" type="text/javascript"></script>

<link href="/css/jquery-ui.css" rel="stylesheet" type="text/css" />
```

After this you will need to put a button on the page. This button will trigger main plugin action, and will show server browser dialog. Also you will need to create textbox to store result from dialog box.

```
<input type="text" id="TextBox1" />
<input id="Button1" type="button" value="Select" />
```

Then you will need to run plugin with this script:

```
<script type="text/javascript" language="javascript">
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
</script>
```

For more configuration parameters see [plugin configuration](PluginConfiguration.md) wiki page


## Server side ##

This plugin will create 'get' request to the server with the following parameters:

action - specify the action of the plugin (always 'browse')

path - path, which childs should be returned to a client

time - tics on the client

As a result of this request should be the list of the childs in the json format.

Asp.Net code for this action can be the following:
```
    public class GetPathHandler : IHttpHandler
    {
        [DataContract]
        private class BrowsePath
        {
            private string _Name;
            private string _IsFolder;
            private string _IsError;

            [DataMember(Name = "name", Order = 0)]
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            [DataMember(Name = "isFolder", Order = 1)]
            public string IsFolder
            {
                get { return _IsFolder; }
                set { _IsFolder = value; }
            }

            [DataMember(Name = "isError", Order = 2)]
            public string IsError
            {
                get { return _IsError; }
                set { _IsError = value; }
            }
        }

        private string BaseDirectory = "";

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            HttpServerUtility server = context.Server;


            List<BrowsePath> item;
            try
            {
                string path = request.Params["path"];

                if (string.IsNullOrEmpty(BaseDirectory + path))
                {
                    item = (from device in Directory.GetLogicalDrives()
                            select new BrowsePath { Name = device.Trim(new char[]{'/', '\\'}), IsFolder = "true" }).ToList();
                }
                else
                {
                    item = (from directory in Directory.GetDirectories(BaseDirectory + path + "/")
                            select new BrowsePath { Name = Path.GetFileName(directory), IsFolder = "true" }).ToList();

                    List<BrowsePath> files = (from file in Directory.GetFiles(BaseDirectory + path + "/", "*.*")
                                              select new BrowsePath { Name = file }).ToList();

                    string imagePath = server.MapPath("~/img/icons/");
                    files.ForEach(bp =>
                        {
                            CreateIco(bp.Name, imagePath);
                            bp.Name = Path.GetFileName(bp.Name);
                        });

                    item.AddRange(files);

                }
            }
            catch (Exception ex)
            {
                item = new List<BrowsePath>();
                item.Add(new BrowsePath { Name = ex.Message, IsError = "true" });
            }

            response.Clear();
            response.ContentType = "text/plain";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(item.GetType());
            ser.WriteObject(HttpContext.Current.Response.OutputStream, item);
            response.Flush();
            response.Close();
            response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private static void CreateIco(string file, string iconDirect)
        {
            if (File.Exists(file))
            {
                string ext = Path.GetExtension(file);
                if (ext.Length > 0)
                    ext = ext.Substring(1);
                else
                    ext = "none";
                string iconame = iconDirect + ext + ".png";
                if (!File.Exists(iconame))
                {
                    using (FileStream fs = File.OpenWrite(iconame))
                    {
                        Icon.ExtractAssociatedIcon(file).Save(fs);
                    }
                }
            }
        }
    }
```

In this example BrowsePath is the definition of the objects that will be serialized into json format that plugin can use. For detailed information see [server response](ServerResponse.md) wiki page