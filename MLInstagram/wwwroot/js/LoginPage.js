'use strict';

const iphoneWallpaper = document.querySelector('.iphone-frame');
let x = 1;
function changeImage() {
    x===1 ? x = 2 : x = 1;
    let imageUrl = `/images/iphonewallpaper${x}.png`;
    iphoneWallpaper.style.backgroundImage = `url(${imageUrl})`
    //iphoneWallpaper.style.backgroundImage = `url(../images/iphonewallpaper1.png)`
}
setInterval(changeImage, 3000);