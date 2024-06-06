$(document).ready(function () {
    var tedad_ValidNumberReq = 3;
    var tedad_ValidIntegerReq = 3;
    var tedad_ValidPositiveReq = 3;
    var tedad_ValidPositIntegerReq = 3;
    var tedad_ValidDigitSepReq = 3;
    var tedad_ValidDigitPositSepReq = 3;
    var tedad_item = 0;
    var tedad_form = document.forms.length;
    for (i = 0; i < tedad_form ; i++) {
        tedad_item = document.forms.item(i).length;
        for (j = 0; j < tedad_item ; j++) {
            if (document.forms.item(i).elements.item(j).getAttribute("id")) {
                id_item = document.forms.item(i).elements.item(j).getAttribute("id");
                if (id_item.indexOf("ValidNumber") > -1) tedad_ValidNumberReq++;
                if (id_item.indexOf("ValidInteger") > -1) tedad_ValidIntegerReq++;
                if (id_item.indexOf("ValidPositive") > -1) tedad_ValidPositiveReq++;
                if (id_item.indexOf("ValidPositInteger") > -1) tedad_ValidPositIntegerReq++;

                if (id_item.indexOf("ValidDigitSep") > -1) tedad_ValidDigitSepReq++;
                if (id_item.indexOf("ValidDigitPositSep") > -1) tedad_ValidDigitPositSepReq++;

            }
        }

    }
    //alert(tedad_ValidDigitSepReq); 
    //alert(tedad_ValidDigitPositSepReq); 
    for (i = 1; i <= tedad_ValidNumberReq; i++) {
        $("#ValidNumberReq" + i).numeric();
        $("#ValidNumber" + i).numeric();
    };
    for (i = 1; i <= tedad_ValidIntegerReq; i++) {
        $("#ValidIntegerReq" + i).numeric(false);
        $("#ValidInteger" + i).numeric(false);
    };
    for (i = 1; i <= tedad_ValidPositiveReq; i++) {
        $("#ValidPositiveReq" + i).numeric({ negative: false });
        $("#ValidPositive" + i).numeric({ negative: false });
    };
    for (i = 1; i <= tedad_ValidPositIntegerReq; i++) {
        $("#ValidPositIntegerReq" + i).numeric({ decimal: false, negative: false });
        $("#ValidPositInteger" + i).numeric({ decimal: false, negative: false });
    };

    for (i = 1; i <= tedad_ValidDigitSepReq; i++) {
        $("#ValidDigitSepReq" + i).numeric(false);
        $("#ValidDigitSep" + i).numeric(false);

        //for separating--added by Vahid Karimi
        $("#ValidDigitSepReq" + i).keyup(function () {
            add_comma_ctrl(this);
        });
        $("#ValidDigitSep" + i).keyup(function () {
            add_comma_ctrl(this);
        });
        //
    };
    for (i = 1; i <= tedad_ValidDigitPositSepReq; i++) {
        $("#ValidDigitPositSepReq" + i).numeric({ decimal: false, negative: false });
        $("#ValidDigitPositSep" + i).numeric({ decimal: false, negative: false });

        //for separating--added by Vahid Karimi
        $("#ValidDigitPositSepReq" + i).keyup(function () {
            add_comma_ctrl(this);
        });
        $("#ValidDigitPositSep" + i).keyup(function () {
            add_comma_ctrl(this);
        });
        //
    };

});