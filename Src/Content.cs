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
    internal static void Read(
      string path, HttpListenerRequest req, HttpListenerResponse res
    ) {
      string file = dir + path.Replace('/', '\\'); // full path
      if (File.Exists(file)) Serve(file, req, res);
      else res.StatusCode = 404;
    }
    static void Serve(string file, HttpListenerRequest req, HttpListenerResponse res) {
      string lastModified = LastModified(file); // file timestamp.
      string ifModifiedSince = req.Headers.Get("If-Modified-Since");
      if (lastModified.Equals(ifModifiedSince)) {
        res.StatusCode = 304;
        return;
      }
      res.AddHeader("Last-Modified", lastModified);
      res.AddHeader("Cache-Control", "no-cache");
      res.ContentType = ContentType(file);
      res.StatusCode = 200;
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
