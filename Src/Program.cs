using System;
using System.IO;
using System.Reflection;

namespace lsFeed {
  class Program {
    static void Main(string[] args) {
      Server.Start();
      //-> open browser
      string exeDir = Directory.GetParent(
        Assembly.GetExecutingAssembly().Location
      ).FullName.Replace('\\', '/');
      System.Diagnostics.Process.Start(
        "file:///" + exeDir + "/content/index.html"
      );
    }
  }
}
