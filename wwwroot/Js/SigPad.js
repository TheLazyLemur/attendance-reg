window.signaturePad = undefined

window.loadSig = (canvasId) => {
    const canvas = document.getElementById(canvasId);
    window.signaturePad = new SignaturePad(canvas);
}

window.saveSigDataPoints = () => {
    return JSON.stringify(window.signaturePad.toData());
}

window.saveSig = () => {
    return window.signaturePad.toDataURL();
}

window.resizeSig = (canvasId) => {
    let canvas = document.getElementById(canvasId) 

    window.addEventListener('resize', resizeCanvas, false);

    function resizeCanvas() {
        canvas.width = window.innerWidth / 1.3;
        canvas.height = window.innerHeight /1.3;
    }
    
    resizeCanvas();
};