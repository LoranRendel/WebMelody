var indicator = {
    animation: null,
    text: "Генерирую",
    frame: 0,
    frames: [".", "..", "..."],
    animate: function () {
        var f = indicator.text + indicator.frames[indicator.frame];
        if (indicator.frame < indicator.frames.length - 1)
            indicator.frame++;
        else
            indicator.frame = 0;
        submitButton.value = f;
    },
    startAnimation: function () {
        if (indicator.animation == null) {
            indicator.animation = setInterval(indicator.animate, 100);
        } else {
            clearInterval(indicator.animation);
            indicator.animation = null;
            indicator.animation = setInterval(indicator.animate, 100);
        }
    },
    endAnimation: function () {
        if (indicator.animation != null) {
            clearInterval(indicator.animation);
            indicator.animation = null;
            submitButton.value = "Сгенерировать мелодию";
        }
    }
}

function generated() {
    var close = document.createElement("span");
    close.innerHTML = "Закрыть";
    close.style.backgroundColor = "white";
    close.style.position = "absolute";
    close.style.right = "0";
    close.style.top = "0";
    close.style.cursor = "pointer";
    close.addEventListener("click", closeGeneratedMelody);
    generatedMelody.appendChild(close);
    generatedMelody.style.display = "block";
    indicator.endAnimation();
}
function closeGeneratedMelody(){
    generatedMelody.style.display = "none";
    player.pause();
}
function generationFailed() {
    generatedMelody.style.display = "block";
    generatedMelody.innerHTML = "<p>Во время генерации произошла ошибка.</p>";
    indicator.endAnimation();
}
function loadingIndicator() {
    indicator.startAnimation();
}

$(document).ready(function () {
    $(".exampleLink").on("click", function () {
        $("#melodyTextarea").val($(this).data("melody"));
    });
})

var js = document.getElementById("jsEnabled");
js.setAttribute("value", "true");