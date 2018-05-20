using System;
using System.IO;
using System.Net;
using System.Text;

namespace lsFeed {
  class Conf {
    static string dir = Environment.GetFolderPath(
      Environment.SpecialFolder.UserProfile
    ) + "\\.lsFeed";
    static string file = dir + "\\conf.json";
    internal static void Init() {
      if (File.Exists(file)) return;
      if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
      using (FileStream f = File.Create(file)) {
        byte[] json = Encoding.UTF8.GetBytes("{\"feeds\":[]}");
        f.Write(json, 0, json.Length);
      }
    }
    internal static void Read(HttpListenerRequest req, HttpListenerResponse res) {
      res.StatusCode = 200;
      res.ContentType = "application/json;charset=utf-8";
      using (FileStream conf = File.Open(Conf.file, FileMode.Open)) {
        conf.CopyTo(res.OutputStream);
      }
    }
    internal static void Write(HttpListenerRequest req, HttpListenerResponse res) {
      res.StatusCode = 200;
      using (Stream from = req.InputStream) {
        using (FileStream to = File.Open(Conf.file, FileMode.Create)) {
          from.CopyTo(to);
        }
      }
    }
  }
}
