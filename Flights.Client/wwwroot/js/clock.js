let intervalId = -1;

function startClock() {
    intervalId = setInterval(setTime, 1000);
}

function setTime() {    
    const time = new Date().toTimeString().slice(0, 5);
    const element = document.getElementById("clock-div");
    element.textContent = time;
}

function stopClock() {
    if(intervalId !== -1) {
        clearInterval(intervalId);
    }
}