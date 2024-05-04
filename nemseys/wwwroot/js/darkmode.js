// Function to toggle dark mode
function toggleDarkMode() {
    const body = document.body;
    const themeCheckbox = document.getElementById("themeToggle");

    if (themeCheckbox.checked) {
        // Enable dark mode
        body.classList.add("bg-dark", "text-white");
        localStorage.setItem("darkModeEnabled", "true");
    } else {
        // Disable dark mode
        body.classList.remove("bg-dark", "text-white");
        localStorage.setItem("darkModeEnabled", "false");
    }
}

// Function to set initial dark mode state on page load
function setInitialDarkMode() {
    const body = document.body;
    const themeCheckbox = document.getElementById("themeToggle");

    // Check dark mode state from localStorage
    const darkModeEnabled = localStorage.getItem("darkModeEnabled");

    if (darkModeEnabled === "true") {
        themeCheckbox.checked = true;
        body.classList.add("bg-dark", "text-white");
    } else {
        themeCheckbox.checked = false;
        body.classList.remove("bg-dark", "text-white");
    }
}

// Call setInitialDarkMode() on page load to set default dark mode state
window.onload = function () {
    setInitialDarkMode();

    // Attach toggleDarkMode() to the checkbox change event
    const themeCheckbox = document.getElementById("themeToggle");
    themeCheckbox.addEventListener("change", toggleDarkMode);
};
