class Conf {
  //-> set conf.json val or default
  constructor(json) {
    if ('feeds' in json) {
      this.feeds=json.feeds;
    } else {
      this.feeds=[];
    }
  }
}
class ConfApi {
  static read(done) {
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
