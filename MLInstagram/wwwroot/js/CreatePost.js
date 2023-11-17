'use strict';

const imageInput = document.querySelector('#upload-post');
const createNextBtn = document.querySelector('.create-next-btn');
let uploadedImage = "";

function editPageLayout() {
    createNextBtn.classList.remove('d-none');
    document.querySelector('.create-elements').classList.add('invisible');
    document.querySelector('.upload-text').textContent = 'Upload Preview';
}

imageInput.addEventListener("change", function () {
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        let uploadedImage = new Image();
        uploadedImage = reader.result;
        document.querySelector('.create-card-body').style.backgroundImage = `url(${uploadedImage})`;
        editPageLayout();
    })
    reader.readAsDataURL(this.files[0]);
});

createNextBtn.addEventListener("click", function () {

});