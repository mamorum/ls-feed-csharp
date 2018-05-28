let isDemo = 'localhost' != location.hostname;
if (isDemo) {
  $('#cog').addClass('hidden');
  $('#stop').addClass('hidden');
}
let demoData = {"feeds":
[{
  "title": "NHK 主要",
  "url": "https://www3.nhk.or.jp/rss/news/cat0.xml",
},{
  "title": "NHK 政治",
  "url": "https://www3.nhk.or.jp/rss/news/cat4.xml"
},{
  "title": "NHK 経済",
  "url": "https://www3.nhk.or.jp/rss/news/cat5.xml"
},{
  "title": "NHK 国際",
  "url": "https://www3.nhk.or.jp/rss/news/cat6.xml"
}]};  //-> CORS Feeds.

class Conf {
  constructor(json) {
    if ('feeds' in json) {
      this.feeds=json.feeds;
    } else { //-> default
      this.feeds=[];
    }
  }
}
class ConfApi {
  static read(done) {
    if (isDemo) {
      done(new Conf(demoData))
      return;
    }
    $.ajax({
      type: 'GET', url: '/read'
    }).done((data) => {
      done(new Conf(data));
    });
  }
  static write(conf, done) {    
    $.ajax({
      type: 'POST', url: '/write',
      data: JSON.stringify(conf),
      contentType: 'application/json;charset=utf-8'
    }).done(done);
  }
}
class FetchApi {
  static fetch(url, done) {
    let reqUrl;
    if (isDemo) reqUrl = url;
    else reqUrl = '/fetch?url='+url;
    $.ajax({
      type: 'GET', url: reqUrl
    }).done(done);
  }
}