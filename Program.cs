using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ls_feed {
  class Program {
    static void Main(string[] args) {
      Server.Start();
      //-> open browser
      string exeDir = Directory.GetParent(
        Assembly.GetExecutingAssembly().Location
      ).FullName.Replace('\\', '/');
      System.Diagnostics.Process.Start(
        "file:///" + exeDir + "/public/index.html"
      );
    }
  }
}
