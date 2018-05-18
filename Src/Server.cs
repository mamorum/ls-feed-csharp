using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace lsFeed {
  class Server {
    static HttpListener lisn = new HttpListener();
    internal static void Start() {
      lisn.Prefixes.Add("http://localhost:8080/");
      try { lisn.Start(); }
      catch (HttpListenerException) { return; }
      (new Thread(new ThreadStart(Listen))).Start();
    }
    static void Listen() {
      Conf.Init();
      while (true) {
        HttpListenerContext ctxt = lisn.GetContext();
        HttpListenerRequest req = ctxt.Request;
        string path = req.Url.AbsolutePath;
        using (HttpListenerResponse res = ctxt.Response) {
          try {
            res.AddHeader("Access-Control-Allow-Origin", "*");
            if ("/fetch".Equals(path)) Fetch(req, res);
            else if ("/read".Equals(path)) Read(req, res);
            else if ("/write".Equals(path)) Write(req, res);
            else if ("/stop".Equals(path)) break;
            else res.StatusCode = 404;
          } catch (Exception) {
            res.StatusCode=500;
          }
        }
      }
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