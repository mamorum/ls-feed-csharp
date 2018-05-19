using System;
using System.IO;
using System.Net;
using System.Threading;

namespace lsFeed {
  class Program {
    static void Main(string[] args) {
      string url = "http://localhost:8622/";
      Server.Start(url);
      Browse(url);
    }
    static void Browse(string url) {
      System.Diagnostics.Process.Start(url);
    }
  }
}
