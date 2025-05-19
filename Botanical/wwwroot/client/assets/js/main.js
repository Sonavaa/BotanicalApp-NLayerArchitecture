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

document.addEventListener("DOMContentLoaded", function () {
    // Add grid-item2 classes
    const gridItems = document.querySelectorAll('.masonryHolder .grid-item');
    if (gridItems.length >= 4) {
        gridItems[2].classList.add('grid-item2');
        gridItems[3].classList.add('grid-item2');
    }

    document.addEventListener("DOMContentLoaded", function () {
        const btn = document.querySelector(".btnMore");
        const preview = document.querySelector(".text-preview");
        const full = document.querySelector(".text-full");

        if (btn) {
            btn.addEventListener("click", function () {
                const isExpanded = full.classList.contains("d-none");

                if (isExpanded) {
                    preview.classList.add("d-none");
                    full.classList.remove("d-none");
                    btn.innerHTML = "<i>Show Less</i>";
                } else {
                    full.classList.add("d-none");
                    preview.classList.remove("d-none");
                    btn.innerHTML = "<i>Learn More</i>";
                }
            });
        }
    });


    // Handle Add to Wishlist
    document.querySelectorAll(".add-to-wishlist-btn").forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();

            const url = btn.getAttribute("href");

            fetch(url)
                .then(response => {
                    if (response.ok) {
                        // Optionally: update icon or reload page
                        location.reload(); // quick fix to reflect updated icon
                    } else {
                        alert("Failed to add to wishlist.");
                    }
                })
                .catch(err => {
                    console.error("Error:", err);
                });
        });
    });

    // Handle Remove from Wishlist
    document.querySelectorAll(".remove-from-wishlist-btn").forEach(btn => {
        btn.addEventListener("click", function (e) {
            e.preventDefault();

            const url = btn.getAttribute("href");

            fetch(url)
                .then(response => {
                    if (response.ok) {
                        location.reload(); // reflect changes immediately
                    } else {
                        alert("Failed to remove from wishlist.");
                    }
                })
                .catch(err => {
                    console.error("Error:", err);
                });
        });
    });
});

function updateQuantity(input) {
    const productId = input.dataset.productId;
    const quantity = parseInt(input.value);

    if (!productId || isNaN(quantity) || quantity < 1) return;

    fetch(`/store/updatewishlistquantity?wishlistId=${productId}&count=${quantity}`, {
        method: "POST"
    })
        .then(response => {
            if (!response.ok) {
                alert("Failed to update quantity.");
            }
        })
        .catch(err => console.error(err));
}