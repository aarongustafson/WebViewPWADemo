(function( window, document ){

  var $list = document.getElementById('mylist'),
      $a = document.createElement('a'),
      $li = document.createElement('li'),
      items = [
        { url: 'https://bing.com', title: 'Foo' },
        { url: 'https://bing.com', title: 'Bar' }
      ];

  window.populateItems = function( items )
  {
    items.forEach(function(item){
      var $item = $li.cloneNode(),
          $link = $a.cloneNode();

      $link.href = item.url;
      $link.innerText = item.title;

      $item.appendChild($link);
      $list.appendChild($item);
    });
  };

  // Prepopulate
  window.populateItems( items );

})(this, this.document);
