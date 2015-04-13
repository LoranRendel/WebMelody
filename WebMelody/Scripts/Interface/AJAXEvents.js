var AJAXEvents = {
    indicator: {
        animation: null,
        text: "Генерирую",
        frame: 0,
        frames: [".", "..", "..."],
        animate: function () {
            var f = AJAXEvents.indicator.text + AJAXEvents.indicator.frames[AJAXEvents.indicator.frame];
            if (AJAXEvents.indicator.frame < AJAXEvents.indicator.frames.length - 1)
                AJAXEvents.indicator.frame++;
            else
                AJAXEvents.indicator.frame = 0;
            submitButton.value = f;
        },
        startAnimation: function () {
            if (AJAXEvents.indicator.animation == null) {
                AJAXEvents.indicator.animation = setInterval(AJAXEvents.indicator.animate, 100);
            } else {
                clearInterval(AJAXEvents.indicator.animation);
                AJAXEvents.indicator.animation = null;
                AJAXEvents.indicator.animation = setInterval(AJAXEvents.indicator.animate, 100);
            }
        },
        endAnimation: function () {
            if (AJAXEvents.indicator.animation != null) {
                clearInterval(AJAXEvents.indicator.animation);
                AJAXEvents.indicator.animation = null;
                submitButton.value = "Сгенерировать мелодию";
            }
        }
    },
    Failure: function () {
        AJAXEvents.OpenGeneratedMelody();
        generatedMelody.innerHTML = "<p>Во время генерации произошла ошибка.</p>";
        AJAXEvents.indicator.endAnimation();
    },
    Success: function () {
        var close = document.createElement("span");
        close.innerHTML = "Закрыть";
        close.style.backgroundColor = "white";
        close.style.position = "absolute";
        close.style.right = "0";
        close.style.top = "0";
        close.style.cursor = "pointer";
        close.id = "close"
        close.addEventListener("click", AJAXEvents.CloseGeneratedMelody);
        generatedMelody.appendChild(close);
        AJAXEvents.OpenGeneratedMelody();
        AJAXEvents.indicator.endAnimation();
        AJAXEvents.NewMelodyAnimation.run();
    },
    Begin: function () {
        var p = document.getElementById("player");
        if(p != undefined && p != null)
            p.pause();
        AJAXEvents.indicator.startAnimation();
    },

    CloseGeneratedMelody: function() {
        generatedMelody.style.display = "none";
        var p = document.getElementById("player");
        if (p != undefined && p != null)
            p.pause();
    },
    OpenGeneratedMelody: function() {
        generatedMelody.style.display = "block";
    },
    NewMelodyAnimation: {
        frames: ["#DDD6BA", "#F8F7F1"],
        frame: 0,
        rounds: 3,
        round: 0,
        animationInterval: null,
        run: function () {
            if (AJAXEvents.NewMelodyAnimation.animationInterval != null) {
                clearInterval(AJAXEvents.NewMelodyAnimation.animationInterval);
                AJAXEvents.NewMelodyAnimation.animationInterval = null;
            }
            AJAXEvents.NewMelodyAnimation.animationInterval = setInterval(AJAXEvents.NewMelodyAnimation.animate, 150);
        },
        animate: function () {
            var element = document.getElementById("generatedMelody");
            if (AJAXEvents.NewMelodyAnimation.round < AJAXEvents.NewMelodyAnimation.rounds)
                element.style.backgroundColor = AJAXEvents.NewMelodyAnimation.frames[AJAXEvents.NewMelodyAnimation.frame];
            else {
                element.style.backgroundColor = AJAXEvents.NewMelodyAnimation.frames[0];
                AJAXEvents.NewMelodyAnimation.frame = 0;
                AJAXEvents.NewMelodyAnimation.round = 0;
                clearInterval(AJAXEvents.NewMelodyAnimation.animationInterval);
                return;
            }           
            if (AJAXEvents.NewMelodyAnimation.frame < AJAXEvents.NewMelodyAnimation.frames.length)
                AJAXEvents.NewMelodyAnimation.frame++;
            else {
                AJAXEvents.NewMelodyAnimation.frame = 0;
                AJAXEvents.NewMelodyAnimation.round++;
            }
        }
    }
}

$(document).ready(function () {
    $(".exampleLink").on("click", function () {
        $("#melodyTextarea").val($(this).data("melody"));
    });
})

var js = document.getElementById("jsEnabled");
js.setAttribute("value", "true");