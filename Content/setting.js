let conf = null;
let mode = null; // 'edit' or 'add'
let feed = null; // edit target
function render() {
  $('#feeds').empty();
  let buf = "";
  for (let i=0; i<conf.feeds.length; i++) {
    buf +=
    '<li class="feed" data-index="' + i +'"><ul>' +
      '<li>' +
        '<a class="edit" href="#">' +
          '<i class="fas fa-pencil-alt"></i>' +
        '</a>' +
        conf.feeds[i].title +
      '</li>' +
      '<li>' + 
        '<a class="delete" href="#">' +
          '<i class="fas fa-trash-alt"></i>' + 
        '</a>' +
      '</li>' +
    '</ul></li>';
  } //-> rebuild data-index.
  $('#feeds').html(buf)
}
function del(index) {
  conf.feeds.splice(index, 1);
  ConfApi.write(conf, render);
}
function add(title, url) {
  conf.feeds.push(
    {"title": title, "url": url}
  );
  ConfApi.write(conf, writeDone);
}
function edit(title, url) {
  feed.title = title;
  feed.url = url;
  ConfApi.write(conf, writeDone);
}
function writeDone() {
  render();
  $('#modal-close').click();
  $('#m-title').val('');
  $('#m-url').val('');
}
function empty(title, url) {
  if (title === '' || url === '') return true;
}
function modal(md, msg, title, url, btn) {
  mode = md;
  $('#m-msg').html(msg);
  $('#m-title').val(title);
  $('#m-url').val(url);
  $('#m-btn').html(btn);
  $('#modal-trigger').prop('checked', true);
  $('#m-title').focus();
}

$(function() {
  $('#feeds').on('click', '.delete', (e) => {
    e.preventDefault();
    let $li = $(
      e.currentTarget
    ).parents('.feed');
    let index = $li.attr('data-index');
    del(index);
  });
  $('#feeds').on('click', '.edit', (e) => {
    e.preventDefault();
    let $li = $(
      e.currentTarget
    ).parents('.feed');
    let index = $li.attr('data-index');
    feed = conf.feeds[index];
    modal(
      'edit', 'Edit the feed',
      feed.title, feed.url, 'Save'
    );
  });
  $('#add').on('click', (e) => {
    e.preventDefault();
    modal(
      'add', 'Add a new feed', '', '', 'Add'
    );
  });
  $('#m-btn').on('click', (e) => {
    e.preventDefault();
    let $title = $('#m-title');
    let $url = $('#m-url');
    let title = $title.val();
    let url = $url.val();
    if (empty(title, url)) return;
    if (mode === 'add') add(title, url);
    else if (mode === 'edit') edit(title, url);
  });
  //-> onload
  ConfApi.read((data) => {
    conf = data;
    if (conf.feeds.length != 0) {
      render();
    } 
  });
});