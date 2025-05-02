const countdownDate = new Date().getTime() + (24 * 60 * 60 * 1000); // 24 hours from now

const timerInterval = setInterval(() => {
    const now = new Date().getTime();
    const distance = countdownDate - now;

    if (distance <= 0) {
        clearInterval(timerInterval);
        document.getElementById("defaultCountdown").innerHTML = "Deal Expired";
        return;
    }

    const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((distance % (1000 * 60)) / 1000);

    document.getElementById("hours").innerHTML = hours;
    document.getElementById("minutes").innerHTML = minutes;
    document.getElementById("seconds").innerHTML = seconds;
}, 1000);

document.addEventListener('DOMContentLoaded', function () {
    const currentPath = window.location.pathname.toLowerCase();
    const navItems = document.querySelectorAll('.navbar-nav .nav-item');

    navItems.forEach(item => {
        const link = item.querySelector('a');
        if (!link) return;

        const linkPath = link.getAttribute('href')?.toLowerCase();
        if (linkPath === currentPath || (linkPath !== '/' && currentPath.startsWith(linkPath))) {
            item.classList.add('active');
        }
    });
});



const searchInput = document.getElementById("searchInput");
const searchBody = document.getElementById("searchBody"); 

if (searchInput) {
    searchInput.addEventListener("keyup", function () {
        console.log(this.value);

        fetch(`/product/search?search=${encodeURIComponent(this.value)}`)
            .then(res => res.text())
            .then(data => {
                if (searchBody) {
                    searchBody.innerHTML = data;
                }
            })
            .catch(error => console.error("Error fetching data:", error));
    });
}