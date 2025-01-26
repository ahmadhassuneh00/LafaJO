document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
  setupSearchButton(); // Setup search button event listener
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
        <div class="nameCompany"><a class="companyName" href="../HtmlPages/Profile.html"><h2 class="companyName">${data}</h2></a></div>
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
            <a href="../HtmlPages/CompanyLogin.html" onclick="logout()">Logout</a>
          </li>
        </ul>
      </div>`;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

// Handle response for fetch calls
function handleResponse(response) {
  if (!response.ok) {
    throw new Error(`Network response was not ok: ${response.statusText}`);
  }

  const contentType = response.headers.get("content-type");
  return contentType && contentType.includes("application/json")
    ? response.json()
    : response.text();
}

// Handle errors
function handleError(message, error) {
  console.error(message, error);
}
// Setup search button to fetch cars by make
function setupSearchButton() {
  const searchButton = document.getElementById("search-button");
  searchButton.addEventListener("click", fetchCarsbySearchInput); // On click, call search function
}

// Function to fetch cars by search input (Make)
function fetchCarsbySearchInput() {
  const searchInput = document.getElementById("search-bar").value.trim(); // Get input value and trim whitespace
  const fuelType = document.getElementById("fuel-type").value;
  const transmissionType = document.getElementById("Transmission-Type").value;
  const sortOrder = document.getElementById("sort-price").value;
  const carsDetailsSection = document.querySelector(".car-details");

  // Ensure search input is not empty
  if (!searchInput) {
    manageCars(); // Call fallback function if input is empty
    return;
  }

  // Clear previous search results
  carsDetailsSection.innerHTML = "<div class='loading'>Searching cars...</div>";

  let apiUrl = "http://localhost:5150/Car/";

  if (fuelType === "All" && transmissionType === "All" && sortOrder === "All") {
    apiUrl += `GetCarsbyMake/${searchInput}`;
  } else if (
    searchInput &&
    fuelType !== "All" &&
    transmissionType !== "All" &&
    sortOrder !== "All"
  ) {
    apiUrl += `GetCarsByFuelAndTransmissionSortedwithSearch?make=${searchInput}&FuelType=${fuelType}&TransmissionType=${transmissionType}&sortOrder=${sortOrder}`;
  } else if (fuelType !== "All" && transmissionType !== "All") {
    apiUrl += `GetCarsByFuelAndTransmissionAndMake?FuelType=${fuelType}&TransmissionType=${transmissionType}&Make=${searchInput}`;
  } else if (fuelType !== "All" && sortOrder !== "All") {
    apiUrl += `GetCarsByFuelAndMakeSorted?FuelType=${fuelType}&Make=${searchInput}&sortOrder=${sortOrder}`;
  } else if (transmissionType !== "All" && sortOrder !== "All") {
    apiUrl += `GetCarsByTransmissionAndMakeSorted?TransmissionType=${transmissionType}&Make=${searchInput}&sortOrder=${sortOrder}`;
  } else if (fuelType !== "All") {
    apiUrl += `GetCarsByFuelAndMake?FuelType=${fuelType}&Make=${searchInput}`;
  } else if (transmissionType !== "All") {
    apiUrl += `GetCarsByTransmissionAndMake?TransmissionType=${transmissionType}&Make=${searchInput}`;
  } else if (sortOrder !== "All") {
    apiUrl += `GetCarsByMakeSorted?Make=${searchInput}&sortOrder=${sortOrder}`;
  }

  // Fetch the cars based on the constructed API URL
  fetch(apiUrl)
    .then(handleResponse)
    .then((cars) => {
      displayCars(cars, carsDetailsSection); // Display search results
    })
    .catch((error) => {
      handleError("Error fetching cars:", error);
    });
}

function manageCars() {
  const carsDetailsSection = document.querySelector(".car-details");
  const fuelTypeSelect = document.getElementById("fuel-type");
  const transmissionTypeSelect = document.getElementById("Transmission-Type");
  const sortedCarsSelect = document.getElementById("sort-price");
  const searchInput = document.getElementById("search-input");

  // Function to fetch cars by fuel type
  function fetchCarsByFuelType(fuelType) {
    fetch(`http://localhost:5150/Car/GetCarByFuelType/${fuelType}`)
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by fuel type:", error)
      );
  }

  // Function to fetch cars by transmission type
  function fetchCarsByTransmissionType(transmissionType) {
    fetch(
      `http://localhost:5150/Car/GetCarByTransmissionType/${transmissionType}`
    )
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by transmission type:", error)
      );
  }

  // Function to fetch cars sorted by daily rate (ascending or descending)
  function fetchCarsByDailyRate(sortedCars) {
    fetch(
      `http://localhost:5150/Car/GetCarsSortedByPrice?sortOrder=${sortedCars}`
    )
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by daily rate:", error)
      );
  }

  // Function to fetch cars by both fuel type and transmission type
  function fetchCarsByFuelAndTransmission(fuelType, transmissionType) {
    fetch(
      `http://localhost:5150/Car/GetCarsByFuelAndTransmission?FuelType=${fuelType}&TransmissionType=${transmissionType}`
    )
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by both filters:", error)
      );
  }

  // Function to fetch cars by fuel type, transmission type, and sorted by daily rate
  function fetchCarsByFuelAndTransmissionSorted(
    fuelType,
    transmissionType,
    sortedCars
  ) {
    fetch(
      `http://localhost:5150/Car/GetCarsByFuelAndTransmissionSorted?FuelType=${fuelType}&TransmissionType=${transmissionType}&sortOrder=${sortedCars}`
    )
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by both filters and sorting:", error)
      );
  }
  function fetchCarsByFuelSorted(fuelType, sortedCars) {
    fetch(
      `http://localhost:5150/Car/GetCarsByFuelSorted?FuelType=${fuelType}&sortOrder=${sortedCars}`
    )
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by both filters and sorting:", error)
      );
  }

  function fetchCarsByTransmissionSorted(transmissionType, sortedCars) {
    fetch(
      `http://localhost:5150/Car/GetCarsByTransmissionSorted?TransmissionType=${transmissionType}&sortOrder=${sortedCars}`
    )
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) =>
        handleError("Error fetching cars by both filters and sorting:", error)
      );
  }

  // Function to fetch all available cars
  function fetchAllCars() {
    fetch("http://localhost:5150/Car/GetCar")
      .then(handleResponse)
      .then((cars) => {
        displayCars(cars, carsDetailsSection);
      })
      .catch((error) => handleError("Error fetching all cars:", error));
  }

  const selectedFuelType = fuelTypeSelect ? fuelTypeSelect.value : "All";
  const selectedTransmissionType = transmissionTypeSelect
    ? transmissionTypeSelect.value
    : "All";
  const selectedSortedCars = sortedCarsSelect ? sortedCarsSelect.value : "All";
  const searchQuery = searchInput ? searchInput.value.trim() : "";
  if (
    searchInput === "" &&
    selectedFuelType !== "All" &&
    selectedTransmissionType !== "All" &&
    selectedSortedCars !== "All"
  ) {
    fetchCarsByFuelAndTransmissionSorted(
      selectedFuelType,
      selectedTransmissionType,
      selectedSortedCars
    );
  } else if (
    searchInput === "" &&
    selectedFuelType !== "All" &&
    selectedTransmissionType !== "All" &&
    selectedSortedCars === "All"
  ) {
    fetchCarsByFuelAndTransmission(selectedFuelType, selectedTransmissionType);
  } else if (
    searchInput === "" &&
    selectedFuelType !== "All" &&
    selectedSortedCars !== "All" &&
    selectedTransmissionType === "All"
  ) {
    // Fetch cars by fuel type and sorting
    fetchCarsByFuelSorted(selectedFuelType, selectedSortedCars);
  } else if (
    searchInput === "" &&
    selectedTransmissionType !== "All" &&
    selectedSortedCars !== "All" &&
    selectedFuelType === "All"
  ) {
    // Fetch cars by transmission type and sorting
    fetchCarsByTransmissionSorted(selectedTransmissionType, selectedSortedCars);
  } else if (selectedFuelType !== "All") {
    // Fetch cars by fuel type
    fetchCarsByFuelType(selectedFuelType);
  } else if (selectedTransmissionType !== "All") {
    // Fetch cars by transmission type
    fetchCarsByTransmissionType(selectedTransmissionType);
  } else if (selectedSortedCars !== "All") {
    // Fetch cars by sorting only
    fetchCarsByDailyRate(selectedSortedCars);
  } else {
    // Fetch all cars if no filter or sorting is selected
    fetchAllCars();
  }
}

let currentPage = 1;
const carsPerPage = 6;

// Display cars for the current page
function displayCars(cars, carsDetailsSection) {
  carsDetailsSection.innerHTML = ""; // Clear previous results
  const totalCars = cars.length;
  const totalPages = Math.ceil(totalCars / carsPerPage);

  // Get the cars to display on the current page
  const startIndex = (currentPage - 1) * carsPerPage;
  const endIndex = startIndex + carsPerPage;
  const carsToDisplay = cars.slice(startIndex, endIndex);

  if (carsToDisplay.length === 0) {
    const message = document.createElement("div");
    message.className = "no-available-cars";
    message.innerHTML = "<p>No available cars found.</p>";
    carsDetailsSection.appendChild(message);
  } else {
    carsToDisplay.forEach((car) => {
      const carDiv = document.createElement("div");
      carDiv.className = "car available-car";
      carDiv.innerHTML = createCarHtml(car);
      carsDetailsSection.appendChild(carDiv);
      fetchCarImage(car.carId, carDiv, "available");
    });
  }

  // Add pagination controls
  addPagination(totalPages);
}

// Create car HTML structure (same as before)
function createCarHtml(car) {
  return `
    <h3>${car.make} ${car.model}</h3>
    <div class="image-container">
      <div class="image available-car-image" id="image-${car.carId}"></div>
    </div>
    <p><strong>Year:</strong> ${car.year}</p>
    <p><strong>Plate of Car:</strong> ${car.licensePlate}</p>
    <p><strong>Fuel Type:</strong> ${car.fuelType}</p>
    <p><strong>Transmission Type:</strong> ${car.transmissionType}</p>
    <p><strong>Color:</strong> ${car.color}</p>
    <p><strong>Daily Rent:</strong> ${car.dailyRate} JOD</p>
    <button class="BookNow" type="submit" onclick="BookNow(${car.carId})">Rent Now</button>`;
}

// Add pagination controls
function addPagination(totalPages) {
  const paginationContainer = document.querySelector(".pagination");
  paginationContainer.innerHTML = "";

  for (let i = 1; i <= totalPages; i++) {
    const pageButton = document.createElement("button");
    pageButton.className = "pagination-button";
    pageButton.innerText = i;
    pageButton.onclick = () => changePage(i);
    paginationContainer.appendChild(pageButton);
  }
}

// Change page function
function changePage(pageNumber) {
  currentPage = pageNumber;
  manageCars(); // Re-fetch and display the cars for the selected page
}

document.getElementById("fuel-type").addEventListener("change", manageCars);
document
  .getElementById("Transmission-Type")
  .addEventListener("change", manageCars);
document.getElementById("sort-price").addEventListener("change", manageCars);

manageCars();

function fetchCarImage(carId, carDiv, type) {
  if (!carId) {
    console.error("carId is undefined");
    return;
  }

  const imageUrl = `http://localhost:5150/Car/GetImageCar/${carId}`;
  fetch(imageUrl)
    .then((res) => {
      if (!res.ok) {
        throw new Error("Failed to fetch car image");
      }
      return res.blob();
    })
    .then((blob) => {
      const img = document.createElement("img");
      img.src = URL.createObjectURL(blob);
      img.alt = "Car Image";
      img.className = "car-image";

      const targetContainer = document.getElementById(`image-${carId}`);
      if (targetContainer) {
        targetContainer.innerHTML = ""; // Clear existing images
        targetContainer.appendChild(img); // Append new image
      } else {
        console.error(`Container not found for car ID: ${carId}`);
      }
    })
    .catch((error) => console.error("Error fetching car image:", error));
}

// Handle API responses
function handleResponse(response) {
  if (!response.ok) {
    throw new Error("Network response was not ok");
  }
  return response.json();
}

// Handle errors
function handleError(message, error) {
  console.error(message, error);
  carsDetailsSection.innerHTML =
    "<p class='error-message'>An error occurred while fetching cars. Please try again later.</p>";
}

// Show loading spinner
function showLoading(carsDetailsSection) {
  carsDetailsSection.innerHTML = "<div class='loading'>Loading...</div>";
}

// Call the manageCars function
manageCars();

// Function for logging out
function logout() {
  localStorage.removeItem("RegistrationId");
  localStorage.removeItem("UserId");
  location.reload(); // Reload the page after logout
}

function BookNow(carId) {
  const userId = localStorage.getItem("UserId");
  localStorage.setItem("CarId", carId);
  if (!userId) {
    window.open(
      "../HtmlPages/Login.html",
      "Login",
      "width=1000,height=1000,left=1000,top=1000"
    );
  } else {
    const win = `../HtmlPages/RentCar.html?carId=${carId}`;
    window.open(win, "Check Rent Car", "width=600,height=600,left=200,top=100");
  }
}
