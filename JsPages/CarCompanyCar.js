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
      </div>`;
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
document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
});

function addCarData() {
  event.preventDefault(); // Prevent default form submission
  const loadingIndicator = document.getElementById("loading-indicator");
  loadingIndicator.style.display = "block"; // Show loading indicator

  // Collect form data
  const make = document.getElementById("car-make").value.trim();
  const model = document.getElementById("car-model").value.trim();
  const year = document.getElementById("car-year").value.trim();
  const licensePlate = document
    .getElementById("car-license-plate")
    .value.trim();

  const fuelType = document.getElementById("car-fuel-type").value.trim();

  const transmissionType = document
    .getElementById("car-transmission-type")
    .value.trim();

  const color = document.getElementById("car-color").value.trim();

  const dailyRate = document.getElementById("car-daily-rate").value.trim();
  const imageFile = document.getElementById("car-image").files[0];
  const registrationId = localStorage.getItem("RegistrationId");

  // Check for empty fields
  if (
    !make ||
    !model ||
    !year ||
    !licensePlate ||
    !fuelType ||
    !transmissionType ||
    !color ||
    !dailyRate ||
    !registrationId
  ) {
    alert("Please fill out all required fields.");
    loadingIndicator.style.display = "none"; // Hide loading indicator
    return;
  }

  // Check if an image file was selected
  if (!imageFile) {
    alert("Please upload an image file.");
    loadingIndicator.style.display = "none"; // Hide loading indicator
    return;
  }

  let formData = new FormData();
  formData.append("make", make);
  formData.append("model", model);
  formData.append("year", year);
  formData.append("licensePlate", licensePlate);
  formData.append("FuelType", fuelType);
  formData.append("TransmissionType", transmissionType);
  formData.append("dailyRate", dailyRate);
  formData.append("Color", color);
  formData.append("ImageURL", imageFile); // Ensure this key matches server expectations
  formData.append("registrationId", registrationId);

  const url2 = "http://localhost:5150/Car/addCar";
  console.log("Sending data to the database...");

  fetch(url2, {
    method: "POST",
    body: formData, // FormData includes files and fields
  })
    .then(async (response) => {
      if (!response.ok) {
        const text = await response.text();
        console.error("Response text:", text);
        throw new Error(`Error ${response.status}: ${text}`);
      }

      // Return JSON only if response is not empty
      const contentType = response.headers.get("content-type");
      if (contentType && contentType.includes("application/json")) {
        return response.json();
      } else {
        return null; // Handle non-JSON responses
      }
    })
    .then((data) => {
      if (data) {
        alert("Car added successfully.");
        console.log("Response data:", data);
        window.location.href = "../HtmlPages/CompanyProfile.html";
      } else {
        alert("Car added successfully, but no JSON response.");
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("An error occurred while adding the Car.");
    });
}
window.addEventListener("load", () => {
  const sessionExpiration = localStorage.getItem("sessionExpiration");
  if (sessionExpiration && Date.now() > sessionExpiration) {
    localStorage.clear(); // Clear localStorage if session has expired
    alert("Session expired. Please log in again.");
    window.location.href = "../HtmlPages/Login.html"; // Redirect to login page
  }
});
