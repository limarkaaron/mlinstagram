'use strict';

const imageInput = document.querySelector('#upload-post');
const createSubmitBtn = document.querySelector('.create-submit-btn');
const createNextBtn = document.querySelector('.create-next-btn');
const createBackBtn = document.querySelector('.create-back-btn');
const createBodyRow = document.querySelector('.create-body-row');
const createCardLeft = document.querySelector('.create-card-left');
const createCardRight = document.querySelector('.create-card-right');

let uploadedImage = "";

function editPageLayout() {
    createNextBtn.classList.toggle('d-none');
    createBackBtn.classList.toggle('d-none');
    document.querySelector('.create-elements').classList.add('invisible');
    document.querySelector('#upload-text').textContent = 'Upload Preview';
}

imageInput.addEventListener("change", function () {
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        let uploadedImage = new Image();
        uploadedImage = reader.result;
        document.querySelector('.create-card-left').style.backgroundImage = `url(${uploadedImage})`;
        editPageLayout();
    })
    reader.readAsDataURL(this.files[0]);
});

createNextBtn.addEventListener("click", function () {
    createCardLeft.classList.replace('col-10', 'col-8');
    createCardRight.classList.remove('d-none');
    createNextBtn.classList.add('d-none');
    createSubmitBtn.classList.remove('d-none');
});

