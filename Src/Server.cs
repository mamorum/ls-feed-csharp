using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace lsFeed {
  class Server {
    static string contentDir = Directory.GetParent(
      Assembly.GetExecutingAssembly().Location
    ) + "\\Content";
    static HttpListener lisn = new HttpListener();
    internal static void Start(string url) {
      lisn.Prefixes.Add(url);
      try { lisn.Start(); }
      catch (HttpListenerException) { return; }
      Conf.Init();
      (new Thread(new ThreadStart(Listen))).Start();
    }
    static void Listen() {
      while (true) {
        HttpListenerContext ctxt = lisn.GetContext();
        HttpListenerRequest req = ctxt.Request;
        string path = req.Url.AbsolutePath;
        using (HttpListenerResponse res = ctxt.Response) {
          try {
            if ("/fetch".Equals(path)) Fetch(req, res);
            else if ("/read".Equals(path)) Read(req, res);
            else if ("/write".Equals(path)) Write(req, res);
            else if ("/stop".Equals(path)) break;
            else Contenet(path, req, res);
          } catch (Exception e) {
            Console.WriteLine(e.StackTrace);
            res.StatusCode = 500;
          }
        }
      }
    }
    static void Contenet(string path, HttpListenerRequest req, HttpListenerResponse res) {
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
    static void Fetch(HttpListenerRequest req, HttpListenerResponse res) {
      string url = req.QueryString.Get("url");
      using (WebResponse rss = WebRequest.Create(url).GetResponse()) {
        res.StatusCode = (int)((HttpWebResponse) rss).StatusCode;
        res.ContentType = rss.ContentType;
        using (Stream from = rss.GetResponseStream()) {
          from.CopyTo(res.OutputStream);
        }
      }
    }
    static void Read(HttpListenerRequest req, HttpListenerResponse res) {
      res.StatusCode = 200;
      res.ContentType = "application/json;charset=utf-8";
      using (FileStream conf = File.Open(Conf.file, FileMode.Open)) {
        conf.CopyTo(res.OutputStream);
      }
    }
    static void Write(HttpListenerRequest req, HttpListenerResponse res) {
      res.StatusCode = 200;
      using (Stream from = req.InputStream) {
        using (FileStream to = File.Open(Conf.file, FileMode.Create)) {
          from.CopyTo(to);
        }
      }
    }
  }
}