window.allSignaturePads = {}

window.loadSig = (canvasId) => {
    resizeSig(canvasId);
    window.allSignaturePads[canvasId] = new SignaturePad(document.getElementById(canvasId))
}

window.saveSig = (canvasId) => {
    return window.allSignaturePads[canvasId].toDataURL();
}

window.resizeSig = (canvasId) => {
    window.addEventListener('resize', window.resizeCanvas(canvasId), false);
};

window.unbindSig = (canvasId) => {
    window.removeEventListener('resize', window.resizeCanvas(canvasId), false);
    return window.allSignaturePads[canvasId].off();
}

window.resizeCanvas = (canvasId) => {
    let newSize = {
        width: window.innerWidth / 1.3,
        height: window.innerHeight / 1.3
    };

    if (newSize.width > 500) {
        newSize.width = 500;
    }
    if (newSize.height > 500) {
        newSize.height = 500;
    }

    let sigCanvas = document.getElementById(canvasId);
    sigCanvas.width = newSize.width;
    sigCanvas.height = newSize.height;
}
