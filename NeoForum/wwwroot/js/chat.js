class Message {
    constructor(username, text, when) {
        this.userName = username;
        this.text = text;
        this.when = when;
    }
}

// userName is declared in razor page.
const username = userName;
const textInput = document.getElementById('messageText');
const whenInput = document.getElementById('when');
const chat = document.getElementById('chat');
const messagesQueue = [];

document.getElementById('submitButton').addEventListener('click', () => {
    var currentdate = new Date();
    when.innerHTML =
        currentdate.getDate() + "."
        + (currentdate.getMonth() + 1) + "."
        + currentdate.getFullYear() + " "
        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: false })
});

function clearInputField() {
    messagesQueue.push(textInput.value);
    textInput.value = "";
}

function sendMessage() {
    let text = messagesQueue.shift() || "";
    if (text.trim() === "") return;
    
    let when = new Date();
    let message = new Message(username, text);
    sendMessageToHub(message);
}

function addMessageToChat(message) {
    let isCurrentUserMessage = message.userName === username;

    let containerrow = document.createElement('div');
    containerrow.className = "row";

    let containeroffset = document.createElement('div');
    containeroffset.className = "chat__offset";

    let containerimage = document.createElement('div');
    containerimage.className = "chat__offset__image";

    let imageicon = document.createElement('i');
    imageicon.className = "fas fa-user-circle fa-3x";

    let container = document.createElement('div');
    container.className = isCurrentUserMessage ? "container bg__primary" : "container bg__default";

    let containernametime = document.createElement('div');
    containernametime.className = "container__name__time";

    let sender = document.createElement('p');
    sender.className = "sender";
    sender.innerHTML = message.userName;
    let text = document.createElement('p');
    text.className = "chatp";
    text.innerHTML = message.text;

    let when = document.createElement('span');
    when.className = isCurrentUserMessage ? "time-left" : "time-right";
    var currentdate = new Date();
    when.innerHTML =
        currentdate.getDate() + "."
        + (currentdate.getMonth() + 1) + "."
        + currentdate.getFullYear() + " "
        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: false })
  
    containernametime.appendChild(sender);
    containernametime.appendChild(when);
    container.appendChild(containernametime);
    container.appendChild(text);
    containerimage.appendChild(imageicon);
    containeroffset.appendChild(containerimage);
    containeroffset.appendChild(container);
    containerrow.appendChild(containeroffset);  
    chat.appendChild(containerrow);
}