    const countdownDate = new Date("2025-01-31T23:59:59").getTime(); 

    const timerInterval = setInterval(() => {
        const now = new Date().getTime();
    const distance = countdownDate - now;

    if (distance < 0) {
        clearInterval(timerInterval);
    document.getElementById("defaultCountdown").innerHTML = "Deal Expired";
    return;
        }

    const days = Math.floor(distance / (1000 * 60 * 60 * 24));
    const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));

    document.getElementById("defaultCountdown").innerHTML =
    `${days}d ${hours}h ${minutes}m`;
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