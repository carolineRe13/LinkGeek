let carousel = document.querySelector('.user-cards');
let cells;

let cellWidth = 0;
let cellHeight = 0;
let cellSize = 0;
const cellCount = 5;

let xMultiplier = 0;
const radius = 20;
const theta = 2 * Math.PI / cellCount;
const zOffset = -20;

let angle = 0;
let selected = 0;

function rotateCarousel() {
    const cells = document.querySelectorAll('.user-card');

    cells.forEach((cell, index) => {
        const newCellAngle = angle + index * theta;

        let newX = radius * Math.sin(newCellAngle);
        let newZ = radius * Math.cos(newCellAngle);
        cell.style.transform = 'translateZ(' + (newZ + zOffset) + 'px) ' + 'translateX(' + newX * xMultiplier + 'px)';
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

function initCarousel() {
    carousel = document.querySelector('.user-carousel');

    cellWidth = carousel.offsetWidth;
    cellHeight = carousel.offsetHeight;
    cellSize = cellWidth;
    xMultiplier = cellWidth / 40;

    if (carousel != null) {
        cells = carousel.querySelectorAll('.user-card');
        for (let i = 0; i < cells.length; i++) {
            const cell = cells[i];
            cell.style.transform = 'translateZ(' + i * 10 + 'px) ' + 'translateX(' + 0 + 'px)';
        }
        setTimeout(() => {
            for (let i = 0; i < cells.length; i++) {
                const cell = cells[i];
                const cellAngle = theta * i;

                let x = radius * Math.sin(cellAngle);
                let z = radius * Math.cos(cellAngle);
                cell.style.transform = 'translateZ(' + (z + zOffset) + 'px) ' + 'translateX(' + x * xMultiplier + 'px)';
            }
        }, 10)
    }
}
