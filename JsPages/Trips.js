document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
  manageTrips();
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
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => {
      element.innerHTML = `
        <div class="dropdown">
          <div class="nameCompany">
            <a class="companyName" href="../HtmlPages/Profile.html">
              <h2 class="companyName">${data}</h2>
            </a>
          </div>
          <ul class="dropdown-menu">
            <li><a href="../HtmlPages/Login.html" onclick="logout()">Logout</a></li>
          </ul>
        </div>`;
    })
    .catch((error) => console.error("Error fetching user name:", error));
}

function fetchCompanyName(element, registrationId) {
  const url = `http://localhost:5150/Company/GetNameCompany/${registrationId}`;
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => {
      element.innerHTML = `
        <div class="dropdown">
          <div class="nameCompany">
            <a class="companyName" href="../HtmlPages/CompanyProfile.html">
              <h2 class="companyName">${data}</h2>
            </a>
          </div>
          <ul class="dropdown-menu">
            <li><a href="../HtmlPages/CompanyLogin.html" onclick="logout()">Logout</a></li>
          </ul>
        </div>`;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

function fetchCompanyNameForTrip(element, registrationId) {
  const url = `http://localhost:5150/Company/GetNameCompany/${registrationId}`;
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => (element.innerHTML = data))
    .catch((error) => console.error("Error fetching company name:", error));
}

function fetchCompanyPhone(element, registrationId) {
  const url = `http://localhost:5150/Company/GetPhoneCompany/${registrationId}`;
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => (element.innerHTML = data))
    .catch((error) => console.error("Error fetching company phone:", error));
}

function manageTrips() {
  const tripsDetailsSection = document.querySelector(".trips-details");
  const searchButton = document.getElementById("search-button");
  const searchInput = document.getElementById("search-bar");
  const sortPriceSelect = document.getElementById("sort-price");
  const paginationContainer = document.querySelector(".pagination");

  let currentPage = 1;
  const tripsPerPage = 6;

  function fetchTrips(searchTerm = "", sortOrder = "All") {
    let url = "http://localhost:5150/Trip/GetTrip"; // Default endpoint for "All" search and price.

    // Determine the correct API endpoint based on search and sort conditions
    if (searchTerm && sortOrder === "All") {
      // Search is not null, price is "All"
      url = `http://localhost:5150/Trip/GetTripsSortedByTitle?title=${searchTerm}`;
    } else if (!searchTerm && sortOrder !== "All") {
      // Search is null, price is not "All"
      url = `http://localhost:5150/Trip/GetTripsSortedByPrice?sortOrder=${sortOrder}`;
    } else if (searchTerm && sortOrder !== "All") {
      // Search is not null, price is not "All"
      url = `http://localhost:5150/Trip/GetTripsSortedByTitleandPrice?title=${searchTerm}&sortOrder=${sortOrder}`;
    }

    fetch(url)
      .then((response) => {
        if (!response.ok) throw new Error("Failed to fetch trips");
        return response.json();
      })
      .then((trips) => {
        // Filter the trips based on search term if it's not empty
        const filteredTrips = searchTerm
          ? trips.filter((trip) =>
              trip.title.toLowerCase().includes(searchTerm.toLowerCase())
            )
          : trips;

        const totalPages = Math.ceil(filteredTrips.length / tripsPerPage);
        const paginatedTrips = filteredTrips.slice(
          (currentPage - 1) * tripsPerPage,
          currentPage * tripsPerPage
        );

        renderTrips(paginatedTrips);
        renderPagination(totalPages, filteredTrips, sortOrder);
      })
      .catch((error) => {
        console.error("Error fetching trips:", error);
        tripsDetailsSection.innerHTML =
          "Error loading trips. Please try again later.";
      });
  }

  function renderTrips(trips) {
    tripsDetailsSection.innerHTML = "";
    trips.forEach((trip) => {
      const tripDiv = document.createElement("div");
      tripDiv.className = "trip";
      formatStartDate = formatDate(new Date(trip.departureDate));
      formatEndDate = formatDate(new Date(trip.returnDate));
      tripDiv.innerHTML = `
        <h3>${trip.title}</h3>
        <div id="image-${trip.tripId}"></div>
        <p><strong>Tourist Office:</strong> <span class="company-name"></span></p>
        <p><strong>Description:</strong> ${trip.content}</p>
        <p><strong>Price for one Traveler:</strong> ${trip.price}</p>
        <p><strong>Start Date:</strong> ${formatStartDate}</p>
        <p><strong>End Date:</strong> ${formatEndDate}</p>
        <p><strong>For more Information:</strong> <span class="company-phone"></span></p>
        <button class="BookNow" type="submit" onclick="BookTrip(${trip.tripId})">Book Now</button>`;

      const companyNameSpan = tripDiv.querySelector(".company-name");
      fetchCompanyNameForTrip(companyNameSpan, trip.registrationId);
      const companyPhoneSpan = tripDiv.querySelector(".company-phone");
      fetchCompanyPhone(companyPhoneSpan, trip.registrationId);

      tripsDetailsSection.appendChild(tripDiv);

      const output = document.getElementById(`image-${trip.tripId}`);
      if (trip.tripId && typeof trip.tripId === "number") {
        const imageUrl = `http://localhost:5150/Trip/GetImageTrip/${trip.tripId}`;
        fetch(imageUrl)
          .then((res) => {
            if (!res.ok) throw new Error(`Failed to fetch image.`);
            return res.blob();
          })
          .then((blob) => {
            const img = document.createElement("img");
            img.src = URL.createObjectURL(blob);
            img.alt = "Trip Image";
            img.className = "trip-image";
            output.appendChild(img);
          })
          .catch(() => {
            output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image">`;
          });
      } else {
        output.innerHTML = "Trip image not available.";
      }
    });
  }
  function formatDate(date) {
    const options = { year: "numeric", month: "long", day: "numeric" };
    return date.toLocaleDateString("en-US", options);
  }
  function renderPagination(totalPages, filteredTrips, sortOrder) {
    paginationContainer.innerHTML = "";
    for (let i = 1; i <= totalPages; i++) {
      const button = document.createElement("button");
      button.className = "pagination-button";
      button.innerText = i;

      if (i === currentPage) {
        button.style.backgroundColor = "#0056b3";
      }

      button.addEventListener("click", () => {
        currentPage = i;
        fetchTrips(searchInput.value, sortOrder);
      });

      paginationContainer.appendChild(button);
    }
  }

  searchButton.addEventListener("click", () => {
    currentPage = 1; // Reset to first page on new search
    fetchTrips(searchInput.value, sortPriceSelect.value);
  });

  sortPriceSelect.addEventListener("change", () => {
    currentPage = 1; // Reset to first page on new sort selection
    fetchTrips(searchInput.value, sortPriceSelect.value);
  });

  fetchTrips();
}

function BookTrip(TripId) {
  const userId = localStorage.getItem("UserId");
  localStorage.setItem("TripId", TripId);
  if (!userId) {
    window.open(
      "../HtmlPages/Login.html",
      "Login",
      "width=500,height=500,left=100,top=100"
    );
  } else {
    window.open(
      "../HtmlPages/BookTrip.html",
      "BookNow",
      "width=600,height=600"
    );
  }
}

function logout() {
  localStorage.clear();
  window.location.href = "../HtmlPages/Login.html";
}
