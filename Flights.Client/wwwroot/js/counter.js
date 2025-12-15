function runCounter(objId, from, to){
    const obj = document.getElementById(objId);

    const abs = Math.abs(from - to);
    const duration = scaleDuration(abs,[0, 60], [500, 1000])

    animateValue(obj, from, to, duration);
}

function animateValue(obj, start, end, duration) {
  let startTimestamp = null;
  const step = (timestamp) => {
    if (!startTimestamp) 
        startTimestamp = timestamp;

    const progress = Math.min((timestamp - startTimestamp) / duration, 1);
    obj.textContent = Math.floor(progress * (end - start) + start);
    
    if (progress < 1) {
      window.requestAnimationFrame(step);
    }
  };
  window.requestAnimationFrame(step);
}

const scaleDuration = (number, fromInterval, toInterval) => {
         if(number >= fromInterval[0] && number <= fromInterval[1]) {
                let oldIntervalUnits = fromInterval[1] - fromInterval[0] + 1;
                let newIntervalUnits = toInterval[1] - toInterval[0] + 1;
    
                let oldNumberPosition = -fromInterval[0] + number + 1;
    
                let percentage = oldNumberPosition / oldIntervalUnits;
                
                let newNumberPosition = Math.round(percentage * newIntervalUnits);
       
                return toInterval[0] + newNumberPosition - 1;
        }
        
        return NaN;
};

