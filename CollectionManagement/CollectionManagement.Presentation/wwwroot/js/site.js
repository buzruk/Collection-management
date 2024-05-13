// for dark mode
function myFunction() {
    var element = document.body;
    element.classList.toggle("dark-mode");
}


/*for like*/
function toggleLike(button) {
    if (button.style.backgroundColor === 'red') {
        button.style.backgroundColor = 'white';
        button.style.color = 'black';
    } else {
        button.style.backgroundColor = 'red';
        button.style.color = 'white';
    }
}



