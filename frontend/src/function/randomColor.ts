/* Генератор случайного цвета */
function randomColor() {
    let hex = Math.floor(Math.random() * 0xFFFFFF);
    return "#" + hex.toString(16);
}

export default randomColor();