document.addEventListener("DOMContentLoaded", () => {
  GetTrip();
});

function fetchCompanyNameForTrip(element, registrationId) {
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
      element.innerHTML = data;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

function GetTrip() {
  const tripDetailsSection = document.querySelector(".TripDetails");
  const tripId = localStorage.getItem("TripId");

  function fetchTrips(searchTerm = "") {
    fetch(`http://localhost:5150/Trip/${tripId}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to fetch trip details.");
        }
        return response.json();
      })
      .then((trips) => {
        tripDetailsSection.innerHTML = ""; // Clear previous trip details

        const filteredTrips = Array.isArray(trips) ? trips : [trips];

        const finalTrips = searchTerm
          ? filteredTrips.filter((trip) =>
              trip.title.toLowerCase().includes(searchTerm.toLowerCase())
            )
          : filteredTrips;

        finalTrips.forEach((trip) => {
          const tripDiv = document.createElement("div");
          tripDiv.className = "trip";

          tripDiv.innerHTML = `
            <h3>${trip.title}</h3>
            <div id="image-${trip.tripId}" class="trip-image-container"></div>
            <p><strong>Tourist Office:</strong> <span class="company-name"></span></p>
            <p><strong>Description:</strong> ${trip.content}</p>
            <p><strong>Price for one Traveler:</strong> ${trip.price}</p>
            <p><strong>Number of tickets available:</strong><span class="AvailableTickets"></span></p>
            <div class="input-container">
              <label for="trip-number-${trip.tripId}">Enter Number of Travelers:</label>
              <input type="number" class="trip-number" id="trip-number-${trip.tripId}" min="1" max="10" placeholder="1" onchange="updateTotalCost(${trip.price}, ${trip.tripId})">
            </div>
            <p><strong>Total Cost:</strong> <span class="totalCost" id="totalCost-${trip.tripId}">0</span> JOD</p>
            <button class="BookNow" type="submit" onclick="BookATrip(${trip.tripId})">Book Now</button>
          `;

          const companyNameSpan = tripDiv.querySelector(".company-name");
          fetchCompanyNameForTrip(companyNameSpan, trip.registrationId);

          const TicketsAvailable = tripDiv.querySelector(".AvailableTickets");
          GetTicketsAvailable(TicketsAvailable, trip.tripId);

          tripDetailsSection.appendChild(tripDiv);

          const output = document.getElementById(`image-${trip.tripId}`);
          if (trip.tripId && typeof trip.tripId === "number") {
            const imageUrl = `http://localhost:5150/Trip/GetImageTrip/${trip.tripId}`;
            fetch(imageUrl)
              .then((res) => {
                if (!res.ok) {
                  throw new Error(
                    `Failed to fetch image. Status: ${res.status}`
                  );
                }
                return res.blob();
              })
              .then((blob) => {
                const img = document.createElement("img");
                img.src = URL.createObjectURL(blob);
                img.alt = "Trip Image";
                img.className = "trip-image";
                output.appendChild(img);
              })
              .catch((error) => {
                console.error("Error fetching image:", error);
                output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image"> Failed to load image.`;
              });
          } else {
            output.innerHTML = "Trip image not available.";
          }
        });
      })
      .catch((error) => {
        console.error("Error fetching trip details:", error);
        tripDetailsSection.innerHTML =
          "Error loading trip details. Please try again later.";
      });
  }

  fetchTrips();
}

function updateTotalCost(price, tripId) {
  const numOfTraveller = document.getElementById(`trip-number-${tripId}`);
  const totalCostElement = document.getElementById(`totalCost-${tripId}`);

  if (!numOfTraveller || !price) {
    totalCostElement.innerText = "0";
    return;
  }

  const numOfTravellerValue = parseInt(numOfTraveller.value, 10) || 1;

  const totalCost = CalculateTotalCost(price, numOfTravellerValue);
  totalCostElement.innerText = totalCost.toFixed(2);
}

function CalculateTotalCost(price, numOfTraveller) {
  return price * numOfTraveller;
}

function BookATrip(tripId) {
  const userId = localStorage.getItem("UserId");
  const numOfTraveller = document.getElementById(`trip-number-${tripId}`).value;
  const totalCost = document.getElementById(`totalCost-${tripId}`).innerText;

  if (!numOfTraveller || !totalCost) {
    alert("Please fill out the required details.");
    return;
  }
  if (!userId) {
    alert("User ID is missing.");
    return;
  }

  console.log("User ID:", userId);
  console.log("Number of Travellers:", numOfTraveller);
  console.log("Total Cost:", totalCost);

  // Redirect to payment page with correct query parameters
  window.location.href = `../HtmlPages/Payment.html?tripId=${tripId}&userId=${userId}&numOfTraveller=${numOfTraveller}&totalCost=${totalCost}`;
}

function GetTicketsAvailable(element, TripId) {
  const url = `http://localhost:5150/BookTrip/TicketsAvailable?id=${TripId}`;

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
      element.innerHTML = data;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}
window.addEventListener("load", () => {
  const sessionExpiration = localStorage.getItem("sessionExpiration");
  if (sessionExpiration && Date.now() > sessionExpiration) {
    localStorage.clear(); // Clear localStorage if session has expired
    alert("Session expired. Please log in again.");
    window.location.href = "../HtmlPages/Login.html"; // Redirect to login page
  }
});