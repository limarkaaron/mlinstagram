'use strict';

const darkModeToggle = document.querySelector('.dark-mode-toggle');
const navBar = document.querySelector('.navbar');
const navbarText = document.querySelectorAll('.navbar-text');
const createBgColor = document.querySelectorAll('.create-body');
const siteText = document.querySelectorAll('.site-text');
let bgColors = ['white', 'black', '#242424'];
let bodyColorSwitch = 1;
let createColorSwitch = 2;

darkModeToggle.addEventListener("change", function () {
     document.body.style.backgroundColor = bgColors[bodyColorSwitch]; //change main background to black
     navBar.style.backgroundColor = bgColors[bodyColorSwitch]; //change navbar background to black
    bodyColorSwitch === 0 ? bodyColorSwitch = 1 : bodyColorSwitch = 0;

    for (let i = 0; i < navbarText.length; i++) {
         navbarText[i].style.color = bgColors[bodyColorSwitch]; //change navbar text color
    }

    for (let i = 0; i < createBgColor.length; i++) {
         createBgColor[i].style.backgroundColor = bgColors[createColorSwitch]; //change all bg colors in create page
    }

    for (let i = 0; i < siteText.length; i++) {
        siteText[i].style.color = bgColors[bodyColorSwitch]; 
    }
     document.querySelector('#caption-text').style.color = bgColors[bodyColorSwitch]; //change all text

    createColorSwitch === 2 ? createColorSwitch = 0 : createColorSwitch = 2;
});