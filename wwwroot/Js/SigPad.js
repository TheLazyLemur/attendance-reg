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
