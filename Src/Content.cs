using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;

namespace lsFeed {
  class Content {
    static string contentDir = Directory.GetParent(
      Assembly.GetExecutingAssembly().Location
    ) + "\\Content";
    internal static void Serve(string path, HttpListenerRequest req, HttpListenerResponse res) {
      string file;
      if ("/".Equals(path)) {
        file = contentDir + ("\\index.html");
      } else {
        file = contentDir + path.Replace('/', '\\');
      }
      if (File.Exists(file)) {
        DateTime time = File.GetLastWriteTime(file);
        string lastModified = time.ToUniversalTime().ToString(
          "ddd, dd MMM yyyy HH:mm:ss",
          CultureInfo.CreateSpecificCulture("en-US")
        ) + " GMT";
        string ifModifiedSince = req.Headers.Get("If-Modified-Since");
        if (lastModified.Equals(ifModifiedSince)) {
          res.StatusCode = 304;
          return;
        }
        res.AddHeader("Last-Modified", lastModified);
        res.AddHeader("Cache-Control", "no-cache");
        res.StatusCode = 200;
        res.ContentType = Ctype(file);
        using (FileStream content = File.Open(file, FileMode.Open)) {
          content.CopyTo(res.OutputStream);
        }
      } else {
        res.StatusCode = 404;
      }
    }
    private static string Ctype(string file) {
      if (file.EndsWith(".html")) return "text/html;charset=utf-8";
      else if (file.EndsWith(".js")) return "text/javascript;charset=utf-8";
      else if (file.EndsWith(".css")) return "text/css;charset=utf-8";
      else if (file.EndsWith(".ico")) return "img/x-icon";
      else return "text/plain;charset=utf-8";
    }
  }
}
