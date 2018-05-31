// require conf.js

let loading = false;
let $li = null; // selected li (side menu)

function fetch($a) {
  if (loading) return;
  else loading = true;
  if ($(window).scrollTop() > 0) {
    $(window).scrollTop(0);
  }
  if ($li != null) {
    $li.removeClass('selected');
  }
  $li = $a.parent();
  $li.addClass('selected');
  $('.bottom').addClass('hidden');
  $('#title').text('Loading...');
  $('#feed').html('');
  getFeed($a.attr('href'), (data) => {
    let list = '';
    let $items = $(data).find('item');
    for (let i=0; i<$items.length; i++) {
      if (i === 8) break;
      let $item = $($items[i]);
      list += '<li>' +
        '<a href="' + $item.find('link').text() +
        '" target="_blank">' +
        $item.find('title').text() + '</a>' + 
      '</li>';
    };
    $('#feed').html(list);
    $('#title').text($a.text());
    $('.bottom').removeClass('hidden');
    loading = false;
  });
}

$(function() {
  $('#feeds').on('click', 'a', (e) => {
    e.preventDefault();
    fetch($(e.currentTarget));
  });
  $('.next').click((e) => {
    e.preventDefault();
    let $next = $('#feeds').find('.selected').next();
    if (!$next[0]) $next = $('#feeds').find('li').first();
    $next.find('a').click();
  });
  $('.prev').click((e) => {
    e.preventDefault();
    let $prev = $('#feeds').find('.selected').prev();
    if (!$prev[0]) $prev = $('#feeds').find('li').last();
    $prev.find('a').click();
  });
  ///-> side
  let showing = false;
  function show($side) {
    $side.addClass('active');
    showing = true;
  }
  function hide($side) {
    $side.removeClass('active');
    showing = false;
  }
  $('#bar').on('click', (e) => {
    e.preventDefault();
    if (showing) hide($('#side'));
    else show($('#side'));
  });
  $('#side').on('click', (e) => {
    e.preventDefault();
    if (showing) hide($('#side'));
  });
  ///-> stop
  $('#stop').on('click', (e) => {
    e.preventDefault();
    $.ajax({
      type: 'GET', url: '/stop'
    }).done((data) => {
      $('body').html('');
      alert("lsFeed has stopped.");
    });
  });
  ///-> onload
  if (isDemo) {
    $('#cog').addClass('hidden');
    $('#stop').addClass('hidden');
  }
  getConf(function() {
    if (conf.feeds.length != 0) {
      $('.top').removeClass('hidden');
      let sb = "";
      for (let i=0; i<conf.feeds.length; i++) {
        sb += '<li><a href="' + conf.feeds[i].url + '">' + conf.feeds[i].title + '</a></li>';
      }
      $('#feeds').html(sb).find('a').first().click();
    }
  });
});
