using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System;

namespace App_Code
{
    /// <summary>
    /// Summary description for GetVersionHistoryHandler.
    /// </summary>
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

            string path = request.QueryString["path"];

            List<BrowsePath> item;
            try
            {
                if (string.IsNullOrEmpty(BaseDirectory + path))
                {
                    item = (from device in Directory.GetLogicalDrives()
                            select new BrowsePath { Name = device, IsFolder = "true" }).ToList();
                }
                else
                {
                    item = (from directory in Directory.GetDirectories(BaseDirectory + path)
                            select new BrowsePath { Name = Path.GetFileName(directory) + "\\", IsFolder = "true" }).ToList();

                    List<BrowsePath> files = (from file in Directory.GetFiles(BaseDirectory + path, "*.*")
                                              select new BrowsePath { Name = file }).ToList();

                    files.ForEach(bp =>
                                      {
                                          CreateIco(bp.Name, server.MapPath("~/img/browser/"));
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
}