let mode = null; // 'edit' or 'add'
let feed = null; // edit target
function render() {
  $('#feeds').empty();
  let buf = "";
  for (let i=0; i<conf.feeds.length; i++) {
    buf +=
    '<li class="feed" id="' + i +'">' +
      '<a class="bar" href="#"><i class="fas fa-bars"></i></a>' +
        '<a class="edit" href="#">' +
          '<i class="fas fa-pencil-alt"></i>' +
        '</a>' +
        '' +
        conf.feeds[i].title + 
        '' + 
        '<a class="delete" href="#">' +
          '<i class="fas fa-trash-alt"></i>' + 
        '</a>' +
      '' +
    '</li>';
  } //-> rebuild id.
  $('#feeds').html(buf)
}
function del(index) {
  conf.feeds.splice(index, 1);
  postConf(render);
}
function add(title, url) {
  conf.feeds.push(
    {"title": title, "url": url}
  );
  postConf(writeDone);
}
function edit(title, url) {
  feed.title = title;
  feed.url = url;
  postConf(writeDone);
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
    let index = $li.attr('id');
    del(index);
  });
  $('#feeds').on('click', '.edit', (e) => {
    e.preventDefault();
    let $li = $(
      e.currentTarget
    ).parents('.feed');
    let index = $li.attr('id');
    feed = conf.feeds[index];
    modal(
      'edit', 'Edit the feed',
      feed.title, feed.url, 'Save'
    );
  });
  $('#feeds').on('click', '.bar', (e) => {
    e.preventDefault();
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
  getConf(() => {
    if (conf.feeds.length != 0) {
      render();
    } 
  });
});