function preloadFile(audioId, sourceUrl) {
    let audio = document.getElementById(audioId);
    audio.src = sourceUrl;
    audio.load();
}

function play(audioId){
    let audio = document.getElementById(audioId);
    audio.play();
}

function pause(audioId){
    let audio = document.getElementById(audioId);
    audio.pause();
}

function stop(audioId){
    let audio = document.getElementById(audioId);
    audio.pause();
    audio.currentTime = 0;
}

function restart(audioId){
    let audio = document.getElementById(audioId);
    audio.pause();
    audio.currentTime = 0;
    audio.play()
}

function clearSource(audioId){
    document.getElementById(audioId).src = null;
}