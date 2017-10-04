window.onload = function () {
    //process hash in the URL, link from game QR code
    if (window.location.hash != '') {
        document.getElementById('base-gameCode').value = window.location.hash.split('#')[1];
    }
    history.pushState(null, "Brawl o' Chums", location.href.split("#")[0].toUpperCase());
    //if the user has entered a name in the past, fill it in
    if (getCookie('boc-playername') !== null) {
        document.getElementById('base-playerName').value = getCookie('boc-playername');
    }
};
window.onhashchange = function () {
    if (window.location.hash != '') {
        document.getElementById('base-gameCode').value = window.location.hash.split('#')[1];
    }
    history.pushState(null, "Brawl o' Chums", location.href.split("#")[0].toUpperCase());
};

//TODO player name control (<=20 chars, emoji+utf8)

let ws;
var gameCode;
var playerName;
var identifier;
let questions;
let currentAnswers;

function DoJoinGame () { //TODO spinny animation to show work
    document.getElementById('base-submit').disabled = true;
    gameCode = document.getElementById('base-gameCode').value.toUpperCase();
    if (gameCode === "") {
        document.getElementById('error-nogamecode').style.display = "initial";
        return;
    }
    playerName = document.getElementById('base-playerName').value;
    if (playerName === "") {
        //TODO handle invalid or empty player name
        return;
    }
    document.cookie = "boc-playername="+playerName+"; expires=33071673599000; path=/"; //set the player name to a cookie for later retrieval
    ws = new WebSocket("ws://dev.brawlochums.gq/ws/");
    ws.onopen = function (event) {
        console.log("Connection Established!");
        //send type, code, name, and identifier (if we have one) to the server
        ws.send("{\"type\":\"client\",\"code\":\""+gameCode+"\",\"name\":\""+playerName+"\""+(getCookie('boc-identifier') !== null ? ",\"identifier\":\""+getCookie('boc-indentifier')+"\"" : "")+"}");
    };
    ws.onmessage = function (msg) {
        console.log(msg.data); //TODO remove
        let message = JSON.parse(msg.data);
        if (message.joingame) { //if this is a "client added to session" message, switch to the logo screen until a update command received
            var now = new Date();
            now.setTime(now.getTime()+1800*1000);
            identifier = message.identifier;
            document.cookie = "boc-identifier="+message.identifier+"; expires="+now.toUTCString()+"; path=/"; //store the identifier the server gave us for 30 mins
            document.getElementById('base-codeEntry').style.display = "none";
            document.getElementById('other-logo').style.display = "initial";
            return;
        }
        //handle receiving questions
        if (message.trivia === "loadquestions") {
            questions = message.questions;
        }
        //handle displaying current question
        if (message.trivia === "displayquestion") {
            document.getElementById('other-logo').style.display = "none";
            document.getElementById('trivia').style.display = "initial";
            let answers;
            switch (message.display) {
                case "1":
                    document.getElementById('trivia-question').innerText = questions.question1.question;
                    answers = shuffleArray(questions.question1.answers);
                    break;
                case "2":
                    document.getElementById('trivia-question').innerText = questions.question2.question;
                    answers = shuffleArray(questions.question2.answers);
                    break;
                case "3":
                    document.getElementById('trivia-question').innerText = questions.question3.question;
                    answers = shuffleArray(questions.question3.answers);
                    break;
                case "4":
                    document.getElementById('trivia-question').innerText = questions.question4.question;
                    answers = shuffleArray(questions.question4.answers);
                    break;
                case "5":
                    document.getElementById('trivia-question').innerText = questions.question5.question;
                    answers = shuffleArray(questions.question5.answers);
                    break;
            }
            currentAnswers = answers;
            document.getElementById('trivia-answer1').innerText = answers[0];
            document.getElementById('trivia-answer2').innerText = answers[1];
            document.getElementById('trivia-answer3').innerText = answers[2];
            document.getElementById('trivia-answer4').innerText = answers[3];
        }
        //handle forced hiding of trivia question
        if (message.trivia === "hidequestion") {
            document.getElementById('other-logo').style.display = "initial";
            document.getElementById('trivia').style.display = "none";
        }
    };
    ws.onclose = function (evt) {
        console.log("Connection lost! ("+evt.code+")");
        switch (evt.code) {
            case 4001:
                document.getElementById('error-nogamecode').style.display = "initial";
                break;
            case 1006:
            case 4002:
            default:
                document.getElementById('error-connection').style.display = "initial";
                break;
        }
    };
}

function SelectTriviaAnswer (selectedAnswer) {
    ws.send("{\"trivia\":\"response\",\"selected\":\""+currentAnswers[selectedAnswer]+"\"}");
    document.getElementById('other-logo').style.display = "initial";
    document.getElementById('trivia').style.display = "none";
}