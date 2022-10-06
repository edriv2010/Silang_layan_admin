//Keystroke Validation
function NumericValidation(e) {
    if ([e.keyCode || e.which] == 8) //this is to allow backspace
        return true;
    if ([e.keyCode || e.which] == 9) //this is to allow tab
        return true;
    if ([e.keyCode || e.which] == 13) //this is to allow enter
        return true;
    if ([e.keyCode || e.which] == 37) //this is to allow left arrow
        return true;
    if ([e.keyCode || e.which] == 39) //this is to allow right arrow
        return true;
    evt = e || window.event // IE support
    var ctrlDown = evt.ctrlKey || evt.metaKey // Mac support
    if (ctrlDown && ([e.keyCode || e.which] == 67 || [e.keyCode || e.which] == 99)) return true // allow CTRL + c
    else if (ctrlDown && ([e.keyCode || e.which] == 86 || [e.keyCode || e.which] == 118)) return true // allow CTRL + v 
    else if (ctrlDown && ([e.keyCode || e.which] == 88 || [e.keyCode || e.which] == 120)) return true // allow CTRL + x
    else if (ctrlDown && ([e.keyCode || e.which] == 65 || [e.keyCode || e.which] == 97)) return true // allow CTRL + 
    if ([e.keyCode || e.which] < 48 || [e.keyCode || e.which] > 57)
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
};
function NumericValidationMinusAllow(e) {
    if ([e.keyCode || e.which] == 8) //this is to allow backspace
        return true;
    if ([e.keyCode || e.which] == 9) //this is to allow tab
        return true;
    if ([e.keyCode || e.which] == 13) //this is to allow enter
        return true;
    if ([e.keyCode || e.which] == 37) //this is to allow left arrow
        return true;
    if ([e.keyCode || e.which] == 39) //this is to allow right arrow
        return true;
    if ([e.keyCode || e.which] == 45) //this is to allow minus
        return true;
    evt = e || window.event // IE support
    var ctrlDown = evt.ctrlKey || evt.metaKey // Mac support
    if (ctrlDown && ([e.keyCode || e.which] == 67 || [e.keyCode || e.which] == 99)) return true // allow CTRL + c
    else if (ctrlDown && ([e.keyCode || e.which] == 86 || [e.keyCode || e.which] == 118)) return true // allow CTRL + v 
    else if (ctrlDown && ([e.keyCode || e.which] == 88 || [e.keyCode || e.which] == 120)) return true // allow CTRL + x
    else if (ctrlDown && ([e.keyCode || e.which] == 65 || [e.keyCode || e.which] == 97)) return true // allow CTRL + a
    if ([e.keyCode || e.which] < 48 || [e.keyCode || e.which] > 57) // block unnumeric
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
};
function DecimalValidation(ctl, e) {
    if ([e.keyCode || e.which] == 8) //this is to allow backspace
        return true;
    if ([e.keyCode || e.which] == 9) //this is to allow tab
        return true;
    if ([e.keyCode || e.which] == 13) //this is to allow enter
        return true;
    if ([e.keyCode || e.which] == 37) //this is to allow left arrow
        return true;
    if ([e.keyCode || e.which] == 39) //this is to allow right arrow
        return true;
    evt = e || window.event // IE support
    var ctrlDown = evt.ctrlKey || evt.metaKey // Mac support
    if (ctrlDown && ([e.keyCode || e.which] == 67 || [e.keyCode || e.which] == 99)) return true // allow CTRL + c
    else if (ctrlDown && ([e.keyCode || e.which] == 86 || [e.keyCode || e.which] == 118)) return true // allow CTRL + v 
    else if (ctrlDown && ([e.keyCode || e.which] == 88 || [e.keyCode || e.which] == 120)) return true // allow CTRL + x
    else if (ctrlDown && ([e.keyCode || e.which] == 65 || [e.keyCode || e.which] == 97)) return true // allow CTRL + a
    if ([e.keyCode || e.which] == 46) { //this is to allow Dot
        if (ctl.value.indexOf(String.fromCharCode(46)) != -1) { //Check If Already Existing Dot
            e.preventDefault ? e.preventDefault() : e.returnValue = false;
        }
        else {
            return true;
        }
    }
    if ([e.keyCode || e.which] < 48 || [e.keyCode || e.which] > 57) // block unnumeric
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
};
function MARCCodeValidation(e) {
    if ([e.keyCode || e.which] == 8) //this is to allow backspace
        return true;
    if ([e.keyCode || e.which] == 9) //this is to allow tab
        return true;
    if ([e.keyCode || e.which] == 13) //this is to allow enter
        return true;
    if ([e.keyCode || e.which] == 37) //this is to allow left arrow
        return true;
    if ([e.keyCode || e.which] == 39) //this is to allow right arrow
        return true;
    evt = e || window.event // IE support
    var ctrlDown = evt.ctrlKey || evt.metaKey // Mac support
    if (ctrlDown && ([e.keyCode || e.which] == 67 || [e.keyCode || e.which] == 99)) return true // allow CTRL + c
    else if (ctrlDown && ([e.keyCode || e.which] == 86 || [e.keyCode || e.which] == 118)) return true // allow CTRL + v 
    else if (ctrlDown && ([e.keyCode || e.which] == 88 || [e.keyCode || e.which] == 120)) return true // allow CTRL + x
    else if (ctrlDown && ([e.keyCode || e.which] == 65 || [e.keyCode || e.which] == 97)) return true // allow CTRL + a
    if ([e.keyCode || e.which] == 36) // block $
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
    if ([e.keyCode || e.which] == 94) // block ^
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
    return true;
};
function PhoneValidation(e) {
    if ([e.keyCode || e.which] == 8) //this is to allow backspace
        return true;
    if ([e.keyCode || e.which] == 9) //this is to allow tab
        return true;
    if ([e.keyCode || e.which] == 13) //this is to allow enter
        return true;
    if ([e.keyCode || e.which] == 43) //this is to allow +
        return true;
    if ([e.keyCode || e.which] == 37) //this is to allow left arrow
        return true;
    if ([e.keyCode || e.which] == 39) //this is to allow right arrow
        return true;
    if ([e.keyCode || e.which] == 45) //this is to allow -
        return true;
    if ([e.keyCode || e.which] == 40) //this is to allow (
        return true;
    if ([e.keyCode || e.which] == 41) //this is to allow )
        return true;
    if ([e.keyCode || e.which] == 32) //this is to allow space
        return true;
    if ([e.keyCode || e.which] == 88 || [e.keyCode || e.which] == 120) //this is to allow x
        return true;
    evt = e || window.event // IE support
    var ctrlDown = evt.ctrlKey || evt.metaKey // Mac support
    if (ctrlDown && ([e.keyCode || e.which] == 67 || [e.keyCode || e.which] == 99)) return true // allow CTRL + c
    else if (ctrlDown && ([e.keyCode || e.which] == 86 || [e.keyCode || e.which] == 118)) return true // allow CTRL + v 
    else if (ctrlDown && ([e.keyCode || e.which] == 88 || [e.keyCode || e.which] == 120)) return true // allow CTRL + x
    else if (ctrlDown && ([e.keyCode || e.which] == 65 || [e.keyCode || e.which] == 97)) return true // allow CTRL + a
    if ([e.keyCode || e.which] < 48 || [e.keyCode || e.which] > 57) // block unnumeric
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
};
function DateValidation(e) {
    if ([e.keyCode || e.which] == 8) //this is to allow backspace
        return true;
    if ([e.keyCode || e.which] == 9) //this is to allow tab
        return true;
    if ([e.keyCode || e.which] == 13) //this is to allow enter
        return true;
    if ([e.keyCode || e.which] == 37) //this is to allow left arrow
        return true;
    if ([e.keyCode || e.which] == 39) //this is to allow right arrow
        return true;
    if ([e.keyCode || e.which] < 47 || [e.keyCode || e.which] > 57) // block unnumeric
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
};

var numb = '0123456789';
function isValid(parm, val) {
    if (parm == "") return true;
    for (i = 0; i < parm.length; i++) {
        if (val.indexOf(parm.charAt(i), 0) == -1)
            return false;
    } return true;
};

//Date Validation
function isDate(ctl) {
    try {
        var dtStr = ctl.value;
        var msg1 = "Enter date in dd/MM/yyyy format";
        var YL = 1900; // Lower limit for year
        var YU = 2100; // Upper limit for year
        if (dtStr.length <= 10 && dtStr.length >= 1) {
            var date_array = dtStr.split("\/");

            if (date_array.length != 3) {
                alert(msg1);
                ctl.focus();
                return "";
            }
            else {
                var strYear = date_array[2];
                var strMonth = date_array[1];
                var strDay = date_array[0];

                var year = parseInt(strYear);
                var month = parseInt(strMonth);
                var day = parseInt(strDay);

                for (var i = 0; i < date_array.length; i++) {
                    // starting year validation
                    if (i == 2) {
                        if (strYear.length != 4) {
                            alert('Enter year in yyyy format');
                            ctl.focus();
                            return "";
                        }
                        else {
                            if (!isValid(strYear, numb)) {
                                alert('Enter numeric value for year');
                                ctl.focus();
                                return "";
                            } else {
                                if (year > YU || year < YL) {
                                    alert('Enter year between ' + YL + ' & ' + YU);
                                    ctl.focus();
                                    return "";
                                }
                            }
                        }
                    } //End of year validation
                    // starting month validation
                    if (i == 1) {
                        if (strMonth.length != 2 && strMonth.length != 1) {
                            alert('Enter month in mm format');
                            ctl.focus();
                            return "";
                        }
                        else {
                            if (!isValid(strMonth, numb)) {
                                alert('Enter numeric value for month');
                                ctl.focus();
                                return "";
                            }
                            else {
                                if (month > 12 || month < 1) {
                                    alert('Enter month between 1 & 12');
                                    ctl.focus();
                                    return "";
                                }
                            }
                        }
                    } // End of month validation
                    // starting day validation
                    if (i == 0) {
                        if (strDay.length != 2 && strDay.length != 1) {
                            alert('Enter day in dd format');
                            ctl.focus();
                            return "";
                        }
                        else {
                            if (!isValid(strDay, numb)) {
                                alert('Enter numeric value for date');
                                ctl.focus();
                                return "";
                            }
                            else {
                                if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
                                    if (day > 31) {
                                        alert('For month ' + month + ' enter day between 1 & 31');
                                        ctl.focus();
                                        return "";
                                    }
                                }
                                else if (month == 4 || month == 6 || month == 9 || month == 11) {
                                    if (day > 30) {
                                        alert('For month ' + month + ' enter day between 1 & 30');
                                        ctl.focus();
                                        return "";
                                    }
                                }
                                else if (month == 2) {
                                    // Leap year check
                                    if (year % 4 == 0) {
                                        var flg = false;
                                        if (year % 100 != 0) flg = true;
                                        else if (datea % 400 == 0) flg = true;
                                        if (flg) {
                                            if (day > 29) {
                                                alert('For Leap year ' + year + ' & month ' + month + ' enter day between 1 & 29');
                                                ctl.focus();
                                                return "";
                                            }
                                        }
                                    }
                                    else {
                                        if (day > 28) {
                                            alert('For month ' + month + ' enter day between 1 & 28');
                                            ctl.focus();
                                            return "";
                                        }
                                    }
                                }
                            }
                        }
                    } //End of day Validation
                }
            }
        }
        else {
            if (dtStr.length != 0) {
                alert(msg1);
                ctl.focus();
                return "";
            }
            else {
                return dtStr;
            }
        }
    }
    catch (e) { };
    return dtStr;
};

LoadForm = function (name, target, data, oncomplete) {
    $("#div-loading").html('Loading Form ' + name + '.. Please Wait..');
    $("#div-loading").show();
    $.ajax({
        url: name,
        data: data, dataType: 'html',
        type: "POST",
        success: function (data) {
            $("#div-loading").hide();
            $("#ul-usermenu").css('display', 'none');
            if (data == 'You have no authorize to view this page.') {
                document.location.href = 'Logout.aspx';
            }
            else {
                $("#" + target).html(data);
                if (oncomplete != null)
                    oncomplete(data);
            }
        },
        error: function (msg, ajaxOptions, thrownError) {
            $("#div-loading").hide();
            alert(msg.responseText);
        }
    });
};

LoadPage = function (name, target, oncomplete) {
    $("#div-loading").html('Loading Page ' + name + '.. Please Wait..');
    $("#div-loading").show();
    $("#" + target).html('');
    $.ajax({ url: name, type: "POST", dataType: 'html',
        success: function (data) {
            $("#div-loading").hide();
            if (data == 'You have no authorize to view this page.') {
                document.location.href = 'logout.aspx';
            }
            else {
                $("#" + target).html(data);
            }
        },
        error: function (msg, ajaxOptions, thrownError) {
            $("#div-loading").hide();
            alert(msg.responseText);
        }
    });
};

Execute = function (name, data, oncomplete) {
    $("#div-loading").html('Execute ' + name + '.. Please Wait..');
    $("#div-loading").show();
    $.ajax({
        url: name,
        data: data, dataType: 'html',
        type: "POST",
        success: function (data) {
            $("#div-loading").hide();
            if (oncomplete != null)
                oncomplete(data);
        },
        error: function (msg, ajaxOptions, thrownError) {
            $("#div-loading").hide();
            alert(msg.responseText);
        }
    });
};


ExecuteNoLoading = function (name, data, oncomplete) {
    //$("#div-loading").html('Execute ' + name + '.. Please Wait..');
    //$("#div-loading").show();
    $.ajax({
        url: name,
        data: data, dataType: 'html',
        type: "POST",
        success: function (data) {
            $("#div-loading").hide();
            if (oncomplete != null)
                oncomplete(data);
        },
        error: function (msg, ajaxOptions, thrownError) {
            $("#div-loading").hide();
            alert(msg.responseText);
        }
    });
};

InitLoading = function (name, data, oncomplete) {
    var myWidth;
    var myHeight;
    //        $('#downuser').click(function() {
    //            $('ul.usermenu').slideToggle('medium');
    //            $('ul.usermenu').show();
    //        });

    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        myWidth = window.innerWidth;
        myHeight = window.innerHeight;
    }
    else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        myWidth = document.documentElement.clientWidth;
        myHeight = document.documentElement.clientHeight;
    }
    else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        myWidth = document.body.clientWidth;
        myHeight = document.body.clientHeight;
    }

    var el = $('#div-loading');
    el.offset({ top: parseInt(myHeight / 2), left: parseInt(myWidth / 2) - 100 });
    var elpos_original = el.offset().top;
    $(window).scroll(function () {
        var elpos = el.offset().top;
        var windowpos = $(window).scrollTop();
        var finaldestination = windowpos;
        if (windowpos < elpos_original) {
            finaldestination = elpos_original;
            el.stop().animate({ 'top': 400 }, 500);
        }
        else {
            el.stop().animate({ 'top': windowpos + parseInt(myHeight / 2) }, 500);
        }
    });
};

TableCheckID = function (TableID, CheckString, ColumnIndex) {
    var i1 = 0;
    var IsFound = false;
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            if (i2 == ColumnIndex && !IsFound) {
                $(this).find('input').each(function () {
                    if ($(this).val() == CheckString && !IsFound) {
                        IsFound = true;
                        return false;
                    }
                });
            }
            i2 = i2 + 1;
        });
        i1 = i1 + 1;
    });
    return !IsFound;
};

TableCheckIDFromSpan = function (TableID, CheckString, ColumnIndex) {
    var i1 = 0;
    var IsFound = false;
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            if (i2 == ColumnIndex && !IsFound) {
                $(this).find('span').each(function () {
                    if ($(this).text() == CheckString && !IsFound) {
                        IsFound = true;
                        return false;
                    }
                });
            }
            i2 = i2 + 1;
        });
        i1 = i1 + 1;
    });
    return !IsFound;
};

TableRemoveRow = function (tr) {
    $(tr).parent().parent().remove();
    return false;
};

TableClearRows = function (TableID) {
    var i1 = 0;
    $("#" + TableID + " tr").each(function () {
        if (i1 != 0) {
            $(this).remove();
        }
        i1 += 1;
    });
    return false;
};

TableGetItems = function (TableID, JoinDelimeter) {
    var ListItemID = new Array();
    var i1 = 0;
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            $(this).find('input').each(function () {
                if ($(this).attr('type') != 'checkbox') {
                    var t = $(this).val();
                    ListItemID.push(t);
                } else {
                    var t = $(this).is(':checked');
                    ListItemID.push(t);
                }
            });
            i2 = i2 + 1;
        });
        i1 = i1 + 1;
    });
    return ListItemID.join(JoinDelimeter);
};

TableGetItemsFromSpan = function (TableID, JoinDelimeter) {
    var ListItemID = new Array();
    var i1 = 0;
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            $(this).find('input').each(function () {
                var t = $(this).val();
                ListItemID.push(t);
            });
            $(this).find('span').each(function () {
                var t = $(this).text();
                ListItemID.push(t);
            });
            i2 = i2 + 1;
        });
        i1 = i1 + 1;
    });
    return ListItemID.join(JoinDelimeter);
};

TableGetItemsSpecificColumn = function (TableID, JoinDelimeter, ColumnIndex, ObjectID) {
    var ListItemID = new Array();
    var i1 = 0;
    $("#" + TableID + " tr").each(function () {
        if ($(this).closest('table').attr("id") == TableID) {
            if (i1 > 0) {
                var i2 = 0;
                var IsFoundColumn = false;
                var IsFoundObject = false;
                $(this).find('td').each(function () {
                    if (i2 == ColumnIndex && !IsFoundColumn) {
                        IsFoundColumn = true;
                        if (!IsFoundObject) {
                            $(this).find('input').each(function () {
                                IsFoundObject = true;
                                if ($(this).attr("id").indexOf(ObjectID) != -1) {
                                    if ($(this).attr('type') != 'checkbox') {
                                        var t = $(this).val();
                                        ListItemID.push(t);
                                    }
                                    else {
                                        var t = $(this).is(':checked');
                                        ListItemID.push(t);
                                    };
                                    return false;
                                };
                            });
                            $(this).find('span').each(function () {
                                IsFoundObject = true;
                                if ($(this).attr("id").indexOf(ObjectID) != -1) {
                                    var t = $(this).text();
                                    ListItemID.push(t);
                                    return false;
                                }
                            });
                        }
                        if (!IsFoundObject) {
                            ListItemID.push("");
                        }
                    }
                    i2 = i2 + 1;
                });
            };
        };
        i1 = i1 + 1;
    });
    return ListItemID.join(JoinDelimeter);
};

TableSetItems = function (TableID, PrimaryIDValue, SetValue, PrimaryColIndex, TargetColIndex) {
    var i1 = 0;
    var IsFound = false;
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            if (i2 == PrimaryColIndex) {
                $(this).find('input').each(function () {
                    var t = $(this).val();
                    if (t == PrimaryIDValue) {
                        IsFound = true;
                    }
                });
            };
            i2 = i2 + 1;
        });

        if (IsFound) {
            i2 = 0;
            $(this).find('td').each(function () {
                if (i2 == TargetColIndex) {
                    $(this).find('input').each(function () {
                        if ($(this).attr("type") == "text") {
                            $(this).val(SetValue);
                            $(this).focus();
                            IsFound = false;
                            return $(this);
                        }
                    });
                };
                i2 = i2 + 1;
            });
        };
        i1 = i1 + 1;
    });
};

TableGetItem = function (TableID, TargetID, ColIndex) {
    var i1 = 0;
    var IsFound = false;
    var Result = "";
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            if (i2 == ColIndex) {
                $(this).find('input').each(function () {
                    if ($(this).attr("id").indexOf(TargetID) != -1) {
                        var t = $(this).val();
                        IsFound = true;
                        Result = t;
                        return false;
                    }
                });
            };
            if (Result != "") {
                return false;
            }
            i2 = i2 + 1;
        });
        if (Result != "") {
            return false;
        }
        i1 = i1 + 1;
    });
    return Result;
};

TableGetItemWithKey = function (TableID, PrimaryIDValue, TargetID, PrimaryColIndex, TargetColIndex) {
    var i1 = 0;
    var IsFound = false;
    var Result = "";
    $("#" + TableID + " tr").each(function () {
        var i2 = 0;
        $(this).find('td').each(function () {
            if (i2 == PrimaryColIndex) {
                $(this).find('input').each(function () {
                    if ($(this).val() == PrimaryIDValue) {
                        IsFound = true;
                        return false;
                    }
                });
            };
            i2 = i2 + 1;
        });

        if (IsFound) {
            IsFound = false;
            i2 = 0;
            $(this).find('td').each(function () {
                if (i2 == TargetColIndex) {
                    $(this).find('input').each(function () {
                        if ($(this).attr("id").indexOf(TargetID) != -1) {
                            IsFound = true;
                            Result = $(this).val();
                            return false;
                        }
                    });
                };
                if (IsFound) {
                    return false;
                }
                i2 = i2 + 1;
            });
        };
        i1 = i1 + 1;
    });
    return Result;
};

TableGetItemFromSameTD = function (td, TargetID) {
    var i1 = 0;
    var IsFound = false;
    var Result = "";
    $(td).find('input').each(function () {
        if ($(this).attr("id").indexOf(TargetID) != -1) {
            var t = $(this).val();
            IsFound = true;
            Result = t;
            return false;
        }
    });
    return Result;
};

function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
};
