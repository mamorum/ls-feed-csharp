using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ls_feed {
  class Data {
    static string dir = Environment.GetFolderPath(
      Environment.SpecialFolder.UserProfile
    ) + "\\.lsfeed";
    static string conf = dir + "\\conf.json";
    internal static void Init() {
      if (!Directory.Exists(dir)) {
        Directory.CreateDirectory(dir);
      }
      if (!File.Exists(conf)) {
        using (FileStream fs = File.Create(conf)) {
          byte[] json = Encoding.UTF8.GetBytes("{\"feeds\":[]}");
          fs.Write(json, 0, json.Length);
        }
      }
    }
    internal static FileStream ConfFile() {
      return File.OpenRead(conf);
    }
  }
}
