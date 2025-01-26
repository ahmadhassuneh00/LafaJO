document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
  manageReviews();
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
            <a href="../HtmlPages/Login.html" onclick="logout()"
              >Logout</a
            >
          </li>
        </ul>
      </div>
      `;
    })
    .catch((error) => console.error("Error fetching company name:", error));
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
            <a href="../HtmlPages/CompanyLogin.html" onclick="logout()"
              >Logout</a
            >
          </li>
        </ul>
      </div>
      `;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

// Unified function to fetch, display, and filter reviews
function manageReviews() {
  const reviewsDetailsSection = document.querySelector(".reviews-details");
  const searchButton = document.getElementById("search-button");
  const searchInput = document.getElementById("search-bar");

  // Function to fetch reviews from the API
  function fetchReviews(searchTerm = "") {
    fetch("http://localhost:5150/Review/GetReview")
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to fetch reviews");
        }
        return response.json();
      })
      .then((reviews) => {
        // Clear previous review details
        reviewsDetailsSection.innerHTML = "";

        // Filter reviews based on the search term if provided
        const filteredReviews = searchTerm
          ? reviews.filter((review) =>
              review.title.toLowerCase().includes(searchTerm.toLowerCase())
            )
          : reviews;

        // Display reviews
        filteredReviews.forEach((review) => {
          const reviewDiv = document.createElement("div");
          reviewDiv.className = "review";

          reviewDiv.innerHTML = `
              <h3>${review.name}</h3>
              <p><strong>Date:</strong> ${formatDate(review.date)}</p>
              <p><strong>Time:</strong> ${review.time}</p>
              <p><strong>Description:</strong> ${review.content}</p>
            `;

          reviewsDetailsSection.appendChild(reviewDiv);
        });
      })
      .catch((error) => {
        console.error("Error fetching reviews:", error);
        reviewsDetailsSection.innerHTML =
          "Error loading reviews. Please try again later.";
      });
  }

  // Search functionality
  searchButton.addEventListener("click", () => {
    const searchTerm = searchInput.value;
    fetchReviews(searchTerm);
  });

  // Initial fetch without search term
  fetchReviews();
}

// Helper function to format date
function formatDate(dateString) {
  const date = new Date(dateString);
  return date.toLocaleDateString(); // Format date as per locale
}

// Initialize the review management functionality
manageReviews();
