function $(selector) {
    return document.getElementById(selector);
}

function getTextElement(k) {
    if (!k.startsWith("#")) {
        k = "#" + k;
    }
    element = document.querySelector(`${k} > *`);
    if (element != null) {
        if (element.tagName === "use") {
            k = element.getAttribute("xlink:href");
            element = document.querySelector(`${k} > tspan`);
        }
        return element;
    }
}

function updateText(key, value) {
    $(key).textContent = value;
}

function updateColor(key, value) {
    $(key).setAttribute('fill', value);
}

function updateImage(key, value) {
    $(key).setAttribute('xlink:href', value);
}

function updateSpanText(textElementId, newTextArray) {
    const textElement = document.getElementById(textElementId);
    if (!textElement) return;

    let currentX = 0;
    const tspans = textElement.querySelectorAll("tspan");
    let prevWidth;
    let prevX;

    tspans.forEach((tspan, index) => {
        if (index >= newTextArray.length) return;
        tspan.textContent = newTextArray[index];
        if (index > 0) {
            console.log(prevWidth);
            currentX += prevX + prevWidth + 10;
            tspan.setAttribute("x", currentX);
        }
        prevWidth = tspan.getComputedTextLength();
        prevX = parseFloat(tspan.getAttribute("x"));
    });
}

function editSpanTextReverse(textElementId, newTextArray) {
    const textElement = document.getElementById(textElementId);
    if (!textElement) return;
    newTextReverse = newTextArray.reverse();
    const tspans = Array.from(textElement.querySelectorAll("tspan"));

    tspans.reverse().forEach((tspan, index) => {
        tspan.textContent = newTextReverse[index];
        console.log("index", index);
        if (index > 0) {
            let last = tspans[index - 1];
            let newX = last.getAttribute("x") - last.getComputedTextLength();
            tspan.setAttribute("x", newX);
        }
    });
}

function alignText(k, alignment) {
    const elem = document.getElementById(k);
    const tspan = getTextElement(k);
    const textWidth = tspan.getBBox().width;
    const parentWidth = elem.parentElement.getBBox().width;

    let x;
    switch (alignment) {
        case 'end':
            x = parentWidth - textWidth;
            elem.style.textAnchor = 'end';
            break;
        case 'middle':
            x = (parentWidth - textWidth) / 2;
            elem.style.textAnchor = 'middle';
            break;
        case 'start':
        default:
            x = 0;
            elem.style.textAnchor = 'start';
            break;
    }

    tspan.setAttribute('x', x);
}

function fadeText(k, v) {
    let element = getTextElement(k);
    if (element.innerHTML === v) {
        return;
    }
    ftl = gsap.timeline({ paused: true });
    ftl
        .to(element, { duration: 0.2, opacity: 0 })
        .call(updateText, [k, v], ">")
        .to(element, { duration: 0.2, opacity: 1 })
        .play();
}