var validator = {};

function GetForm(id) {
    var form = $(id + " form");
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('form');
    validator = form.validate();
    return form;
}



function CheckRBG(name, errorMessageId) {
    var radioButtons = $("input[name=" + name + "]:radio");
    var oneIsChecked = false;
    radioButtons.each(
        function () {
            if ($(this).is(':checked'))
                oneIsChecked = true;
        }
    )
    if (!oneIsChecked)
        $("#" + errorMessageId).show();
    else {
        $("#" + errorMessageId).hide();
    }

    return oneIsChecked;
}

/*
 <label for="UploadedImage" class="control-label col-md-3">Photo</label><br />
        <div class="col-xs-9">
            <img id="UploadedImage"
                    src="@Url.Content(Model.GetPhotoURL())"
                    style="height:86px;max-width:300px;"
                    data-toggle="tooltip"
                    title="Cliquez pour changer de photo..." />
            <input id="ImageUploader"
                    name="ImageUploader"
                    type="file"
                    style="display:none"
                    accept="image/jpeg,image/gif,image/png,image/bmp" /> 
        </div>
*/
$(document).ready(function () {
    AppUtilities_BindCallBack();
})

function AppUtilities_BindCallBack() {
    //$(".Phone").mask("(999) 999-9999");
    $(".datepicker").datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+10",
        dayNamesMin: ["D", "L", "M", "M", "J", "V", "S"],
        monthNames: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Decembre"]
    });
    $("#ImageUploader").change(function (e) { PreLoadImage(e); })
    $("#UploadButton").click(function () { $("#ImageUploader").trigger("click"); })
    $("#UploadedImage").click(function () { $("#ImageUploader").trigger("click"); })
}
function PreLoadImage(e) {
    // Saisir la référence sur l'image cible
    var imageTarget = $("#UploadedImage")[0];
    // Saisir la référence sur le fileUpload
    var input = $("#ImageUploader")[0];

    if (imageTarget != null) {
        var len = input.value.length;

        if (len != 0) {
            var fname = input.value;
            var ext = fname.substr(len - 3, len).toLowerCase();

            if ((ext != "png") &&
                (ext != "jpg") &&
                (ext != "bmp") &&
                (ext != "gif")) {
                bootbox.alert("Ce n'est pas un fichier d'image de format reconnu. Sélectionnez un autre fichier.");
            }
            else {
                var fReader = new FileReader();
                fReader.readAsDataURL(input.files[0]);
                fReader.onloadend = function (event) {
                    // event.target.result contiens les données de l'image
                    imageTarget.src = event.target.result;
                }
            }
        }
        else {
            imageTarget.src = null;
        }
    }
    return true;
}
