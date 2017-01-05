/// <reference path="../jquery-2.1.0-vsdoc.js" />
var AJAXEvents = {
    MelodyDiv: '#generatedMelody',
    Failure: function () {
        $(AJAXEvents.MelodyDiv).html('<p>Во время генерации произошла ошибка.</p>');
        $(AJAXEvents.MelodyDiv).show();
    },
    Complete: function () {
        $('#submitButton').attr('disabled', false);
    },
    Success: function () {
        var close = document.createElement("span");
        close.innerHTML = "Закрыть";
        
        close.style.position = "absolute";
        close.style.right = "0";
        close.style.top = "0";
        close.style.cursor = "pointer";
        close.id = "close"
        close.addEventListener("click", AJAXEvents.CloseGeneratedMelody);
        generatedMelody.appendChild(close);
    },
    Begin: function () {
        var p = document.getElementById("player");
        if (p)
            p.pause();
        AJAXEvents.ShowLoadingIndicator();
        $('html').scrollTop($('html')[0].scrollHeight);
        $('#submitButton').attr('disabled', true);
    },
    CloseGeneratedMelody: function () {
        $(AJAXEvents.MelodyDiv).hide();
        var p = document.getElementById("player");
        if (p)
            p.pause();
    },   
    ShowLoadingIndicator: function () {
        var melodyDiv = $(AJAXEvents.MelodyDiv);
        melodyDiv.children().remove();
        melodyDiv.append('<p class="indicator"><img src="./Content/images/loading.gif" /></p>');
        melodyDiv.append('<p class="indicator">Разучиваю</p>');
        melodyDiv.show();
    }
}

var RuleLinkIndicator = {
    originalText: $('#ruleLink').html(),
    opened: false,
    handleElement: null,
    showOpenHandle: function () {
        if (this.handleElement)
        {
            this.handleElement.remove();
            this.handleElement = null;
        }
        $('#ruleLink').parent().append('<span id="ruleHandle"> +</span>');
        this.handleElement = $('#ruleHandle');
        this.opened = false;
    },
    showCloseHandle: function () {
        if (this.handleElement) {
            this.handleElement.remove();
            this.handleElement = null;
        }
        $('#ruleLink').parent().append('<span id="ruleHandle"> −</span>');
        this.handleElement = $('#ruleHandle');
        this.opened = true;
    },
    toggle: function () {
        if (this.opened)
            this.showOpenHandle();
        else
            this.showCloseHandle();
    }
}

$(document).ready(function () {
    $('#examples').remove();
    $('#examplesJS').attr('id', 'examples');   
    $(".exampleLink").on("click", function () {
        $("#melodyTextarea").val($(this).data("melody"));
        return false;
    });
    RuleLinkIndicator.showOpenHandle();
    $("#ruleLink").on("click", function () {
        $("#rules").toggle();
        RuleLinkIndicator.toggle();
        return false;
    });
    $("#rules").hide();   
    $('#jsEnabled').attr('value', 'true');
});