//-> globals
let conf;
let isDemo = 'localhost' != location.hostname;
if (isDemo) conf = {"feeds":
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
}]}; // cors feeds

//-> ajax apis
function getConf(fnc) {
  if (isDemo) { fnc(); return; }
  $.ajax({
    type: 'GET', url: '/read'
  }).done((data) => {
    conf = data;
    if (!('feeds' in conf)) {
      conf.feeds=[]; // default
    }
    fnc();
  })
}
function postConf(fnc) {
  $.ajax({
    type: 'POST', url: '/write',
    data: JSON.stringify(conf),
    contentType: 'application/json;charset=utf-8'
  }).done(fnc);
}
function getFeed(url, fnc) {
  let feedUrl;
  if (isDemo) feedUrl = url;
  else feedUrl = '/fetch?url='+url;
  $.ajax({
    type: 'GET', url: feedUrl
  }).done(fnc);
}