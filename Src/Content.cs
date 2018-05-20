using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;

namespace lsFeed {
  class Content {
    static string dir = Directory.GetParent(
      Assembly.GetExecutingAssembly().Location
    ) + "\\Content";
    internal static void Serve(string path, HttpListenerRequest req, HttpListenerResponse res) {
      string file;
      if ("/".Equals(path)) {
        file = dir + ("\\index.html");
      } else {
        file = dir + path.Replace('/', '\\');
      }
      if (!File.Exists(file)) {
        res.StatusCode = 404;
        return;
      }
      string lastModified = LastModified(file);
      string ifModifiedSince = req.Headers.Get("If-Modified-Since");
      if (lastModified.Equals(ifModifiedSince)) {
        res.StatusCode = 304;
        return;
      }
      res.AddHeader("Last-Modified", lastModified);
      res.AddHeader("Cache-Control", "no-cache");
      res.StatusCode = 200;
      res.ContentType = ContentType(file);
      using (FileStream content = File.Open(file, FileMode.Open)) {
        content.CopyTo(res.OutputStream);
      }
    }
    static string LastModified(string file) {
      DateTime time = File.GetLastWriteTime(file);
      return time.ToUniversalTime().ToString(
        "ddd, dd MMM yyyy HH:mm:ss",
        CultureInfo.CreateSpecificCulture("en-US")
      ) + " GMT";
    }
    static string ContentType(string file) {
      if (file.EndsWith(".html")) return "text/html;charset=utf-8";
      else if (file.EndsWith(".js")) return "text/javascript;charset=utf-8";
      else if (file.EndsWith(".css")) return "text/css;charset=utf-8";
      else if (file.EndsWith(".ico")) return "img/x-icon";
      else return "text/plain;charset=utf-8";
    }
  }
}
