let conf = null;

function render() {  
  $('#feeds').empty();
  let buf = "";
  for (let i=0; i<conf.feeds.length; i++) {
    buf += '<li data-index="' + i +'"><a class="delete" href="#"><i class="fas fa-trash-alt"></i></a> ' + conf.feeds[i].title + '</li>';
  } //-> rebuild data-index.
  $('#feeds').html(buf)
}
function del($li) {
  let index = $li.attr('data-index');
  conf.feeds.splice(index, 1);
  $.ajax({
    type: 'POST', url: '/write',
    data: JSON.stringify(conf)
  }).done(() => {
    render();
  });
}
function add($title, $url) {
  let title = $title.val();
  let url = $url.val();
  if (title === '' || url === '') {
    return;
  }
  conf.feeds.push(
    {"title": title, "url": url}
  );
  $.ajax({
    type: 'POST', url: '/write',
    data: JSON.stringify(conf)
  }).done(() => {
    render();
    $('#modal-close').click();
    $title.val('');
    $url.val('');
  });
}

$(function() {
  $('#feeds').on('click', '.delete', (e) => {
    e.preventDefault();
    del($(e.currentTarget).parent('li'));
  });
  $('#add').on('click', (e) => {
    e.preventDefault();
    $('#modal-trigger').prop('checked', true);
    $('#m-title').focus();
  });
  $('#m-add').on('click', (e) => {
    e.preventDefault();
    add($('#m-title'), $('#m-url'));
  });
  //-> onload
  $.ajax({
    type: 'GET', url: '/read'
  }).done((data) => {
    conf = data;
    if (conf.feeds.length != 0) {
      render();
    } 
  });
});