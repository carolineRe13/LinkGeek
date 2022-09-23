let carousel = document.querySelector('.user-cards');
let cells = carousel.querySelectorAll('.user-card');

const cellWidth = carousel.offsetWidth;
const cellHeight = carousel.offsetHeight;
const cellSize = cellWidth;
const cellCount = 5;

const xMultiplier = cellWidth/20;
const radius = 20;
const theta = 2 * Math.PI / cellCount;

let angle = 0;
let selected = 0;

function rotateCarousel() {
    const cells = document.querySelectorAll('.user-card');

    cells.forEach((cell, index) => {
        const newCellAngle = angle + index * theta;

        let newX = radius * Math.sin(newCellAngle);
        let newZ = radius * Math.cos(newCellAngle);
        cell.style.transform = 'translateZ(' + newZ + 'px) ' + 'translateX(' + newX * xMultiplier + 'px)';
    });
}

function selectPrev() {
    selected--;
    angle -= theta;
    rotateCarousel();
}

function selectNext() {
    selected++;
    angle += theta;
    rotateCarousel();
}

// let prevButton = document.querySelector('.previous-button');
// prevButton.addEventListener('click', selectPrev);
//
// let nextButton = document.querySelector('.next-button');
// nextButton.addEventListener('click', selectNext);

function initCarousel() {
    carousel = document.querySelector('.user-cards');
    cells = carousel.querySelectorAll('.user-card');
    for (let i = 0; i < cells.length; i++) {
        const cell = cells[i];
        cell.style.transform = 'translateZ(' + i*10 + 'px) ' + 'translateX(' + 0 + 'px)';
    }
    setTimeout(() => {
        for (let i = 0; i < cells.length; i++) {
            const cell = cells[i];
            const cellAngle = theta * i;

            let x = radius * Math.sin(cellAngle);
            let z = radius * Math.cos(cellAngle);
            cell.style.transform = 'translateZ(' + z + 'px) ' + 'translateX(' + x * xMultiplier + 'px)';
        }
    }, 10)

}

$("#open-pack").click(function () {
    $.ajax({
        method: 'get',
        url: '?handler=UserCards',
        success: function (result) {
            $('#user-cards').empty();
            $('#user-cards').html(result);
            initCarousel();
            initGameCarousel();
        }
    })
})