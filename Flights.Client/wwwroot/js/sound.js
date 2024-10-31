function preloadFile(audioId, sourceUrl) {
    let audio = document.getElementById(audioId);
    audio.src = sourceUrl;
    audio.load();
}

function play(audioId){
    console.log(audioId);
    let audio = document.getElementById(audioId);
    console.log(audio);
    audio.play();
}

function restart(audioId){
    let audio = document.getElementById(audioId);
    console.log(audio);
    audio.pause();
    audio.currentTime = 0;
    audio.play()
}

function clearSource(audioId){
    document.getElementById(audioId).src = null;
}