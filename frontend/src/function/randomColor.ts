/* Генератор случайного цвета */
function randomColor() {
    let hex = Math.floor(Math.random() * 0xFFFFFF);
    return "#" + hex.toString(16);
}

const color : string = randomColor();

export default randomColor();