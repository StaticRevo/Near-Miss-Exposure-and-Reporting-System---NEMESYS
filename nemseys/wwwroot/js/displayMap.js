var mapDiv = document.getElementById('map');
var hazardLocation = mapDiv.getAttribute('data-location').split(', ');
var lat = parseFloat(hazardLocation[0]);
var lon = parseFloat(hazardLocation[1]);

var map = L.map('map').setView([lat, lon], 13); // Set initial location to the hazard location

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
}).addTo(map);

L.marker([lat, lon]).addTo(map);
