let tree = document.getElementById('tree');

readFile();

function readFile() {
  fetch('./items.json')
    .then(response => response.json())
    .then(data => {
      tree.innerHTML = createList(data);
    })
    .catch(error => console.log(error));
}

function createList(items) {
  let html = '<ul>';
  
  items.forEach(item => {
    html += `<li>${item.Name}`;
    
    if (item.Children.length > 0) {
      html += createList(item.Children);
    }
    
    html += '</li>';
  });
  
  html += '</ul>';
  return html;
}