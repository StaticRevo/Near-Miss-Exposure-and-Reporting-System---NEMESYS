var map = L.map('map').setView([35.9375, 14.3754], 13); // Set initial location and zoom level

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
}).addTo(map);

var marker;

map.on('click', function (e) {
    if (marker) {
        map.removeLayer(marker);
    }

    marker = L.marker(e.latlng).addTo(map);

    document.getElementById('HazardLocation').value = e.latlng.lat + ', ' + e.latlng.lng;
});
