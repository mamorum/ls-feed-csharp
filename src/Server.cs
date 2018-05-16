using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ls_feed {
  class Server {
    static HttpListener lisn = new HttpListener();
    internal static void Start() {
      lisn.Prefixes.Add("http://localhost:8080/");
      try { lisn.Start(); }
      catch (HttpListenerException) { return; }
      (new Thread(new ThreadStart(Listen))).Start();
    }
    static void Listen() {
      Data.Init();
      while (true) {
        HttpListenerContext ctxt = lisn.GetContext();
        HttpListenerResponse res = ctxt.Response;
        res.AddHeader("Access-Control-Allow-Origin", "*");
        HttpListenerRequest req = ctxt.Request;
        string path = req.Url.AbsolutePath;
        if ("/conf".Equals(path)) Conf(req, res);
        else if ("/fetch".Equals(path)) Fetch(req, res);
        else if ("/stop".Equals(path)) Stop(req, res);
        else NotFound(res);
      }
    }
    static void Conf(HttpListenerRequest req, HttpListenerResponse res) {
      res.StatusCode = 200;
      res.ContentType = "application/json;charset=utf-8";
      Data.ConfFile().CopyTo(res.OutputStream);
      res.Close();
    }
    static void Fetch(HttpListenerRequest req, HttpListenerResponse res) {
      string url = req.QueryString.Get("url");
      WebResponse rss = WebRequest.Create(url).GetResponse();
      res.StatusCode = (int) ((HttpWebResponse) rss).StatusCode;
      res.ContentType = rss.ContentType;
      rss.GetResponseStream().CopyTo(res.OutputStream);
      rss.Close();
      res.Close();
    }
    static void Stop(HttpListenerRequest req, HttpListenerResponse res) {
      res.StatusCode = 200;
      res.Close();
      Environment.Exit(0);
    }
    static void NotFound(HttpListenerResponse res) {
      res.StatusCode = 404;
      res.ContentType = "text/html;charset=utf-8";
      byte[] body = Encoding.UTF8.GetBytes("<p>Not Found.</p>");
      res.OutputStream.Write(body, 0, body.Length);
      res.Close();
    }
  }
}
