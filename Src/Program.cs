using System;
using System.IO;

namespace lsFeed {
  class Program {
    static string url = "http://localhost:8622/";
    static void Main(string[] args) {
      Server.Start(url);
      //-> open browser
      System.Diagnostics.Process.Start(url);
    }
  }
}
