using System;
using System.IO;
using System.Net;
using System.Threading;

namespace lsFeed {
  class Program {
    static string url = "http://localhost:8622/";
    static void Main(string[] args) {
      Server.Start(url);
      Browse(url);
    }
    static void Browse(string url) {
      System.Diagnostics.Process.Start(url);
    }
  }
}
