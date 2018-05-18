using System;
using System.IO;
using System.Text;

namespace lsFeed {
  class Conf {
    static string dir = Environment.GetFolderPath(
      Environment.SpecialFolder.UserProfile
    ) + "\\.lsFeed";
    internal static string file = dir + "\\conf.json";
    internal static void Init() {
      if (Directory.Exists(dir)) {
        if (File.Exists(file)) return;
      } else {
        Directory.CreateDirectory(dir);
      }
      using (FileStream f = File.Create(file)) {
        byte[] json = Encoding.UTF8.GetBytes("{\"feeds\":[]}");
        f.Write(json, 0, json.Length);
      }
    }
  }
}
