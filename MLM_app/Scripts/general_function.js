function add_comma_ctrl(ctrl)
{
    var separator = ",";
    var int = ctrl.value.replace ( new RegExp ( separator, "g" ), "" );
    var regexp = new RegExp ( "\\B(\\d{3})(" + separator + "|$)" );
    do
    {
        int = int.replace ( regexp, separator + "$1" );
    }
    while ( int.search ( regexp ) >= 0 )
    ctrl.value = int;
}

<!-- AUTO ROMOVE FOR SPECIAL USE IN document.ready    -->
function remove_comma_ctrl(ctrl)
{
    var separator = ",";
  
    ctrl.value = ctrl.value.replace ( new RegExp ( separator, "g" ), "" );
}

function add_comma(val)
{
    val=val.toString();
    var separator = ",";
    var int = val.replace ( new RegExp ( separator, "g" ), "" );
    var regexp = new RegExp ( "\\B(\\d{3})(" + separator + "|$)" );
    do
    {
        int = int.replace ( regexp, separator + "$1" );
    }
    while ( int.search ( regexp ) >= 0 )
    val = int;
    return val;
}

function remove_comma(val)
{
    var separator = ",";  
    val = val.replace ( new RegExp ( separator, "g" ), "" );  
    return val;
}

function convert_num(str_val)
{
    str_val=str_val.toString();
    var en_num=["0","1","2","3","4","5","6","7","8","9"];
    var fa_num=["غ°","غ±","غ²","غ³","غ´","غµ","غ¶","غ·","غ¸","غ¹"];
    for(var i=0	;i<en_num.length ; i++)
        str_val=str_val.replace(new RegExp(i,"g"),fa_num[i]);
    return str_val;
}

function remove_first_leading_zeros(str_val)
{
    str_val=str_val.toString();
    str_val=str_val.replace(/\,/g,"");
    if(str_val=="") str_val="0";
    while (str_val.charAt(0) == '0') //for remove first leading zeros
    {
        if (str_val.length == 1) {
            break;
        };
        if (str_val.charAt(1) == '.') 
        { 
            break ;
        };
        str_val = str_val.substr(1, str_val.length - 1);
    }
    return str_val;
}
