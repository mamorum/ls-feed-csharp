// require conf.js
$(function() {

//-> render conf
function render() {
  if (conf.feeds.length == 0) return;
  let buf = '';
  for (let i=0; i<conf.feeds.length; i++) {
    buf +=
    '<li class="feed" id="' + i +'">' +
      '<a class="bar" href="#"><i class="fas fa-bars"></i></a>' +
      '<a class="edit" href="#"><i class="fas fa-pencil-alt"></i></a>' +
      conf.feeds[i].title +
      '<a class="delete" href="#"><i class="fas fa-trash-alt"></i></a>' +
    '</li>';
  }
  $('#feeds').html(buf)
}
//-> sort
$('#feeds').on('click', '.bar', function(e) {
  e.preventDefault();
});
$('#feeds').sortable({
  update: function() {
      let ids = $(this).sortable("toArray");
      let feeds = [];
      ids.forEach((id) => {
        feeds.push(conf.feeds[id]);
      });
      conf.feeds = feeds;
      postConf(render);
  }
});
//-> delete
$('#feeds').on('click', '.delete', function(e) {
  e.preventDefault();
  let id = $(this).parents('.feed').attr('id');
  conf.feeds.splice(id, 1);
  postConf(render);
});
//-> edit
let feed = null; // target
$('#feeds').on('click', '.edit', function(e) {
  e.preventDefault();
  let id = $(this).parents('.feed').attr('id');
  feed = conf.feeds[id];
  modal(
    'edit', 'Edit the feed',
    feed.title, feed.url, 'Save'
  );
});
//-> add
$('#add').on('click', function(e) {
  e.preventDefault();
  modal(
    'add', 'Add a new feed', '', '', 'Add'
  );
});
let mode = null; // 'edit' or 'add'
function modal(md, msg, title, url, btn) {
  mode = md;
  $('#m-msg').html(msg);
  $('#m-title').val(title);
  $('#m-url').val(url);
  $('#m-btn').html(btn);
  $('#modal-trigger').prop('checked', true);
  $('#m-title').focus();
}
//-> modal button
$('#m-btn').on('click', function(e) {
  e.preventDefault();
  let title = $('#m-title').val();
  let url = $('#m-url').val();
  if (title === '' || url === '') {
    return;
  }
  if (mode === 'add') {      
    conf.feeds.push(
      {"title": title, "url": url}
    );
    postConf(modalDone);
    return;
  }
  if (mode === 'edit') {
    feed.title = title;
    feed.url = url;
    postConf(modalDone);
    return;
  }
});
function modalDone() {
  $('#modal-close').click();
  $('#m-title').val('');
  $('#m-url').val('');
  render();
}
//-> onload
getConf(render);

}); // $(function... ends.