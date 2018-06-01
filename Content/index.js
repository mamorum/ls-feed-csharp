// require conf.js
$(function() {

//-> click bar (show or hide side menu)
let showing = false;
function show($side) {
  $('#side').addClass('active');
  showing = true;
}
function hide() {
  $('#side').removeClass('active');
  showing = false;
}
$('#bar').on('click', function(e) {
  e.preventDefault();
  if (showing) hide();
  else show();
});
$('#side').on('click', function(e) {
  e.preventDefault();
  if (showing) hide();
});
//-> click side menu (get feed)
let loading = false;
let $li = null; // clicked
let $a = null;
$('#feeds').on('click', 'a', function(e) {
e.preventDefault();    
  if (loading) return;
  else loading = true;
  if ($(window).scrollTop() > 0) {
    $(window).scrollTop(0);
  }
  if ($li != null) {
    $li.removeClass('selected');
  }
  $a = $(this);
  $li = $a.parent();
  $li.addClass('selected');
  $('.bottom').addClass('hidden');
  $('#title').text('Loading...');
  $('#feed').html('');
  getFeed($a.attr('href'), renderFeed);
});  
function renderFeed(data) {    
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
}
//-> click control (get next or prev feed)
$('.next').click(function(e) {
  e.preventDefault();
  let $next = $('#feeds').find('.selected').next();
  if (!$next[0]) $next = $('#feeds').find('li').first();
  $next.find('a').click();
});
$('.prev').click(function(e) {
  e.preventDefault();
  let $prev = $('#feeds').find('.selected').prev();
  if (!$prev[0]) $prev = $('#feeds').find('li').last();
  $prev.find('a').click();
});
//-> click stop
$('#stop').on('click', function(e) {
  e.preventDefault();
  $.ajax({
    type: 'GET', url: '/stop'
  }).done((data) => {
    $('body').html('');
    alert("lsFeed has stopped.");
  });
});
//-> onload
if (isDemo) {
  $('#cog').addClass('hidden');
  $('#stop').addClass('hidden');
}
getConf(function() {
  if (conf.feeds.length == 0) return;
  $('.top').removeClass('hidden');
  let sb = "";
  for (let i=0; i<conf.feeds.length; i++) {
    sb += '<li><a href="' + conf.feeds[i].url + '">' + conf.feeds[i].title + '</a></li>';
  }
  $('#feeds').html(sb).find('a').first().click();
});

}); // $(function... ends
