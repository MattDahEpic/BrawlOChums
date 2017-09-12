window.onload = function () {
    if (window.location.hash != '') {
        document.getElementById('base-gameCode').value = window.location.hash.split('#')[1];
        window.location.hash = "";
    }
};

function DoJoinGame () {
    //TODO
}