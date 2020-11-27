$(document).ready(function () {
        
    var rgb = getAverageRGB(document.getElementById('main-img'));
    $('.gradient-bg').css("background-color", 'rgb(' + rgb.r + ',' + rgb.g + ',' + rgb.b + ')');
    function getAverageRGB(imgEl) {

        var blockSize = 5, // only visit every 5 pixels
            defaultRGB = {
                r: 24,
                g: 24,
                b: 24
            }, // for non-supporting envs
            canvas = document.createElement('canvas'),
            context = canvas.getContext && canvas.getContext('2d'),
            data, width, height,
            i = -4,
            length,
            rgb = {
                r: 24,
                g: 24,
                b: 24
            },
            count = 0;

        if (!context) {
            return defaultRGB;
        }

        height = canvas.height = imgEl.naturalHeight || imgEl.offsetHeight || imgEl.height;
        width = canvas.width = imgEl.naturalWidth || imgEl.offsetWidth || imgEl.width;

        context.drawImage(imgEl, 0, 0);

        try {
            data = context.getImageData(0, 0, width, height);
        } catch (e) {
            /* security error, img on diff domain */
            console.log(e);
            return defaultRGB;
        }

        length = data.data.length;

        while ((i += blockSize * 4) < length) {
            ++count;
            rgb.r += data.data[i];
            rgb.g += data.data[i + 1];
            rgb.b += data.data[i + 2];
        }

        // ~~ used to floor values
        rgb.r = ~~(rgb.r / count);
        rgb.g = ~~(rgb.g / count);
        rgb.b = ~~(rgb.b / count);

        return rgb;

    }

    $(".os-host").on("scroll", function () {
        console.log($(this).scrollTop());
        if ($(this).scrollTop() > 100) {
            $("#main .top-bar").addClass("sticky-scroll");
            $("#main .top-bar").css('background', 'rgba(' + rgb.r + ',' + rgb.g + ',' + rgb.b + ', .7)');
        }
        else {
            $("#main .top-bar").removeClass("sticky-scroll");
            $("#main .top-bar").css('background', 'transparent');
        }
    });




    $('.search-input input').keyup(function () {
        var value = $(this).val();
        history.pushState(null, '', '/search/' + value);

        $.ajax({
            method: "GET",
            url: window.location.href,
            cache: true,
            beforeSend: function () {
                $(".loading").show();
            },
            success: function (data) {
                var res = $(data).find('#search-result').html()
                console.log(res);
                $('#search-result').html(res);
            },
            complete: function (data) {
                $(".loading").hide();
            },

        });
    })


    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (results == null) {
            return null;
        }
        else {
            return results[1] || 0;
        }
    }

});

function sortByTitle() {
    var pageNumber = 1;
    var sortOrder = $.urlParam('sortOrder');
    if (sortOrder == "title") {
        sortOrder = "title_desc";
    }
    else {
        sortOrder = "title";
    }
    var searchString = $.urlParam('searchString');
    var data = { pageNumber, sortOrder, searchString };

    const url = new URL(window.location);
    if (searchString != null) {
        url.searchParams.set('searchString', searchString);
    }
    url.searchParams.set('sortOrder', sortOrder);
    window.history.pushState({}, '', url);

    $.ajax({
        type: "GET",
        data: data,
        url: "https://localhost:5001/Tracks",
        beforeSend: function () {
            $(".loading").show();
        },
        success: function (data) {
            var response = $(data).find('#table-results')[0];
            var pageNumber = $(data).find('#table-results #pageNumber').val();
            $('#table-results #pageNumber').val(pageNumber);
            $('#table-results').html(response);
        },
        complete: function (data) {
            $(".loading").hide();
        },
    });

}


function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        // x.innerHTML = "Geolocation is not supported by this browser.";
    }
}

function showPosition(position) {
    $('#from').val(position.coords.latitude + ',' + position.coords.longitude);
}


function sortByDuration() {
    var pageNumber = 1;
    var sortOrder = $.urlParam('sortOrder');
    if (sortOrder == "duration") {
        sortOrder = "duration_desc";
    }
    else {
        sortOrder = "duration";
    }
    var searchString = $.urlParam('searchString');
    var data = { pageNumber, sortOrder, searchString };

    const url = new URL(window.location);
    if (searchString != null) {
        url.searchParams.set('searchString', searchString);
    }
    url.searchParams.set('sortOrder', sortOrder);
    window.history.pushState({}, '', url);

    $.ajax({
        type: "GET",
        data: data,
        url: "https://localhost:5001/Tracks",
        beforeSend: function () {
            $(".loading").show();
        },
        success: function (data) {
            var response = $(data).find('#table-results')[0];
            var pageNumber = $(data).find('#table-results #pageNumber').val();
            $('#table-results #pageNumber').val(pageNumber);
            $('#table-results').html(response);
        },
        complete: function (data) {
            $(".loading").hide();
        },
    });

}



function sortByArtist() {
    var pageNumber = 1;
    var sortOrder = $.urlParam('sortOrder');
    if (sortOrder == "artist") {
        sortOrder = "artist_desc";
    }
    else {
        sortOrder = "artist";
    }
    var searchString = $.urlParam('searchString');
    var data = { pageNumber, sortOrder, searchString };

    const url = new URL(window.location);
    if (searchString != null) {
        url.searchParams.set('searchString', searchString);
    }
    url.searchParams.set('sortOrder', sortOrder);
    window.history.pushState({}, '', url);
    //window.location.href = url;

    $.ajax({
        type: "GET",
        data: data,
        url: "https://localhost:5001/Tracks",
        beforeSend: function () {
            $(".loading").show();
        },
        success: function (data) {
            var response = $(data).find('#table-results')[0];
            var pageNumber = $(data).find('#table-results #pageNumber').val();
            $('#table-results #pageNumber').val(pageNumber);
            $('#table-results').html(response);
        },
        complete: function (data) {
            $(".loading").hide();
        },
    });

}



