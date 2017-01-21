var ProductControls = ProductControls || {};
ProductControls.PeoplePicker = (function ($) {
    var picker;
    var appweburl = getQueryStringParameter('SPAppWebUrl');
    var hostweburl = getQueryStringParameter('SPHostUrl');
    var restSource = appweburl + "/_api/SP.UI.ApplicationPages.ClientPeoplePickerWebServiceInterface.clientPeoplePickerSearchUser";
    var personaTmpl = "<div class='ms-PeoplePicker-result' tabindex='1'><div class='ms-Persona ms-Persona--sm'><div class='ms-Persona-imageArea'><div class='ms-Persona-initials ms-Persona-initials--green'>{init}</div></div></div><div class='ms-Persona-details'><div class='ms-Persona-primaryText'>{display_name}</div><div class='ms-Persona-secondaryText'>{email}</div></div></div><div><button class='ms-PeoplePicker-resultAction'><i class='ms-Icon ms-Icon--Clear'></i></button></div>"

    function searchUsers(searchTerm) {

        $.ajax(
        {
            'url': restSource,
            'method': 'POST',
            'data': JSON.stringify({
                'queryParams': {
                    '__metadata': {
                        'type': 'SP.UI.ApplicationPages.ClientPeoplePickerQueryParameters'
                    },
                    'AllowEmailAddresses': true,
                    'AllowMultipleEntities': false,
                    'AllUrlZones': false,
                    'MaximumEntitySuggestions': 50,
                    'PrincipalSource': 15,
                    'PrincipalType': 1, // Only Users
                    'QueryString': searchTerm
                    //'Required':false,
                    //'SharePointGroupID':null,
                    //'UrlZone':null,
                    //'UrlZoneSpecified':false,
                    //'Web':null,
                    //'WebApplicationID':null
                }
            }),
            'headers': {
                'accept': 'application/json;odata=verbose',
                'content-type': 'application/json;odata=verbose',
                'X-RequestDigest': requestDigest
            },
            'success': function (data) {
                var d = data;
                var results = JSON.parse(data.d.ClientPeoplePickerSearchUser);
                if (results.length > 0) {
                    response($.map(results, function (item) {
                        return { label: item.DisplayText, value: item.DisplayText }
                    }));
                }
            },
            'error': function (err) {
                alert(JSON.stringify(err));
            }
        });
    }

    function getQueryStringParameter(parameter, url) {
        if (!url) {
            url = window.location.href;
        }

        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);

        if (!results) return null;
        if (!results[2]) return '';

        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }


    function clearResults() {

    }

    function init(element) {
        picker = new fabric['PeoplePicker'](element);
        picker._peoplePickerSearch.addEventListener("keyup", function (e) {
            console.log("Key up at people picker");
        });

        var resultsElement = picker._peoplePickerMenu.getElementsByClassName("ms-PeoplePicker-resultGroup");
        $(resultsElement).append("<label>test</label>");
    }

    return {
        init: init,
        clearResults: clearResults
    };
})(jQuery);
