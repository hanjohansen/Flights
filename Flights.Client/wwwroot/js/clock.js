function startClock() {
    setInterval(setTime, 1000);
}

function setTime() {    
    const time = new Date().toTimeString().slice(0, 5);
    const element = document.getElementById("clock-div");
    element.textContent = time;
}