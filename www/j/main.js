(function( window, document ){

  var $list = document.getElementById('mylist'),
      $a = document.createElement('a'),
      $li = document.createElement('li'),
      items = [
        { url: 'https://bing.com', title: 'This is a link from the main.js file' },
        { url: 'https://bing.com', title: 'This is another link from the main.js file' }
      ];

  window.populateItems = function( items )
  {
    if (typeof items === "string" && isJsonString(items))
    {
      items = JSON.parse(items);
    }
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

  function isJsonString(str) {
    try {
      JSON.parse(str);
    } catch (e) {
      return false;
    }
    return true;
  }

})(this, this.document);
