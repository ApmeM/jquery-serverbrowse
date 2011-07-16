namespace App_Code
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Web;

    #endregion

    public class GetPathHandler : IHttpHandler
    {
        #region Constants and Fields

        private readonly string baseDirectory = string.Empty;

        #endregion

        #region Properties

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IHttpHandler

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            HttpServerUtility server = context.Server;

            List<BrowsePath> item;
            try
            {
                string path = request.Params["path"];

                if (string.IsNullOrEmpty(this.baseDirectory + path))
                {
                    item = (from device in Directory.GetLogicalDrives()
                            select new BrowsePath { Name = device.Trim(new[] { '/', '\\' }), IsFolder = true }).ToList();
                }
                else
                {
                    item = (from directory in Directory.GetDirectories(this.baseDirectory + path + "/")
                            select new BrowsePath { Name = Path.GetFileName(directory), IsFolder = true }).ToList();

                    List<BrowsePath> files =
                        (from file in Directory.GetFiles(this.baseDirectory + path + "/", "*.*")
                         select new BrowsePath { Name = file }).ToList();

                    string imagePath = server.MapPath("~/img/icons/");
                    files.ForEach(
                        bp =>
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
                item.Add(new BrowsePath { Name = ex.Message, IsError = true });
            }

            response.Clear();
            response.ContentType = "text/plain";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(item.GetType());
            ser.WriteObject(HttpContext.Current.Response.OutputStream, item);
            response.Flush();
            response.Close();
            response.End();
        }

        #endregion

        #endregion

        #region Methods

        private static void CreateIco(string file, string iconDirect)
        {
            if (File.Exists(file))
            {
                string ext = Path.GetExtension(file);
                if (string.IsNullOrEmpty(ext))
                {
                    ext = "none";
                }
                else
                {
                    ext = ext.Substring(1);
                }

                string iconame = iconDirect + ext + ".png";
                if (!File.Exists(iconame))
                {
                    using (FileStream fs = File.OpenWrite(iconame))
                    {
                        Icon icon = Icon.ExtractAssociatedIcon(file);
                        if (icon != null)
                        {
                            icon.ToBitmap().Save(fs, ImageFormat.Png);
                        }
                    }
                }
            }
        }

        #endregion

        [DataContract]
        private class BrowsePath
        {
            #region Properties

            [DataMember(Name = "isError", Order = 2)]
            public bool IsError { get; set; }

            [DataMember(Name = "isFolder", Order = 1)]
            public bool IsFolder { get; set; }

            [DataMember(Name = "name", Order = 0)]
            public string Name { get; set; }

            #endregion
        }
    }
}