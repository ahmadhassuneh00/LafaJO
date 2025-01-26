document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
});

function initializeCompanyProfile() {
  const nameCompany = document.querySelector(".auth-links");
  const registrationId = localStorage.getItem("RegistrationId");
  const userId = localStorage.getItem("UserId");
  if (nameCompany && registrationId) {
    fetchCompanyName(nameCompany, registrationId);
  } else if (nameCompany && userId) {
    fetchUserName(nameCompany, userId);
  } else {
    nameCompany.innerHTML = `<a href="../HtmlPages/Login.html">Login</a>
    <a href="../HtmlPages/signup.html">Signup</a>`;
  }
}

function fetchUserName(element, userId) {
  const url = `http://localhost:5150/Users/GetUserName/${userId}`;

  fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.text(); // Use text() for plain text responses
    })
    .then((data) => {
      element.innerHTML = `<div class="dropdown">
        <div class="nameCompany"><a class="companyName" href="../HtmlPages/Home.html"><h2 class="companyName">${data}</h2></a></div>
        <ul class="dropdown-menu">
          <li>
            <a href="../HtmlPages/Login.html" onclick="logout()">Logout</a>
          </li>
        </ul>
      </div>
      `;
    })
    .catch((error) => console.error("Error fetching user name:", error));
}
function logout() {
  localStorage.clear();
}
function fetchCompanyName(element, registrationId) {
  const url = `http://localhost:5150/Company/GetNameCompany/${registrationId}`;

  fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.text(); // Use text() for plain text responses
    })
    .then((data) => {
      element.innerHTML = `<div class="dropdown">
        <div class="nameCompany"><a class="companyName" href="../HtmlPages/CompanyProfile.html"><h2 class="companyName">${data}</h2></a></div>
        <ul class="dropdown-menu">
          <li>
            <a href="../HtmlPages/Login.html" onclick="logout()">Logout</a>
          </li>
        </ul>
      </div>
      `;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}


let currentIndex = 0; // Set the initial slide to 0 (first slide)
const slides = document.querySelectorAll('.slide .item');
const totalSlides = slides.length;

// Function to move the slide
function moveSlide() {
    // Hide current slide by removing the 'active' class
    slides[currentIndex].classList.remove('active');

    // Update currentIndex by moving to the next slide
    currentIndex += 1;

    // Loop around slides when reaching the end
    if (currentIndex >= totalSlides) {
        currentIndex = 0;
    }

    // Move all slides based on the currentIndex
    updateSlidePosition();

    // Add 'active' class to the new current slide to make it visible
    slides[currentIndex].classList.add('active');
}

// Function to update the position of the slides
function updateSlidePosition() {
    slides.forEach((slide, index) => {
        // Update the 'left' position for each slide
        slide.style.left = `${(index - currentIndex) * 100}%`;
    });
}

// Initialize the first slide as active and set positions
document.addEventListener('DOMContentLoaded', function() {
    slides[0].classList.add('active'); // Show the first slide
    updateSlidePosition(); // Set the initial positions of slides

    // Start the auto-slide every 5 seconds
    setInterval(moveSlide, 5000); // Change slide every 5 seconds (5000ms)
});
