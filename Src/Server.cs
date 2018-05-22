using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace lsFeed {
  class Server {
    static HttpListener listener = new HttpListener();
    internal static void Start(string url) {
      listener.Prefixes.Add(url);
      try { listener.Start(); }
      catch (HttpListenerException) {
        return; // url is used.
      }
      Conf.Init();
      (new Thread(Listen)).Start();
    }
    internal static void Listen() {
      HttpListenerContext ctxt;
      while (true) {
        try {
          //-> thread stops for a request.
          ctxt = listener.GetContext();
        } catch (HttpListenerException) {
          break; // listener is closed.
        }
        ThreadPool.QueueUserWorkItem(
          new WaitCallback(Handle), ctxt
        );
      }
    }
    static void Handle(object o) {
      HttpListenerContext ctxt = (HttpListenerContext)o;
      HttpListenerRequest req = ctxt.Request;
      HttpListenerResponse res = ctxt.Response;
      res.Headers.Add("Server", null); // delete header.
      string path = req.Url.AbsolutePath;
      if ("/stop".Equals(path)) {
        res.StatusCode = 200;
        res.Close();
        listener.Close();
        return;
      }
      try {
        if ("/fetch".Equals(path)) Fetch(req, res);
        else if ("/read".Equals(path)) Conf.Read(req, res);
        else if ("/write".Equals(path)) Conf.Write(req, res);
        else Content.Read(path, req, res);
      } catch (Exception) {
        res.StatusCode = 500;
      } finally {
        res.Close();
      }
    }
    static void Fetch(HttpListenerRequest req, HttpListenerResponse res) {
      string url = req.QueryString.Get("url");
      using (WebResponse rss = WebRequest.Create(url).GetResponse()) {
        res.StatusCode = (int)((HttpWebResponse)rss).StatusCode;
        res.ContentType = rss.ContentType;
        using (Stream from = rss.GetResponseStream()) {
          from.CopyTo(res.OutputStream);
        }
      }
    }
  }
}