document.addEventListener("DOMContentLoaded", () => {
  GetCar();
});

function GetCar() {
  const carsDetailsSection = document.querySelector(".car-details2");
  const carId = localStorage.getItem("CarId");

  function fetchCars(searchTerm = "") {
    fetch(`http://localhost:5150/Car/${carId}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to fetch car details.");
        }
        return response.json();
      })
      .then((cars) => {
        // Clear previous car details
        carsDetailsSection.innerHTML = "";

        // Check if 'cars' is an array, if not convert it to an array
        const filteredCars = Array.isArray(cars) ? cars : [cars];

        const finalCars = searchTerm
          ? filteredCars.filter((car) =>
              car.make.toLowerCase().includes(searchTerm.toLowerCase())
            )
          : filteredCars;

        // Display filtered cars
        finalCars.forEach((car) => {
          const carDiv = document.createElement("div");
          carDiv.className = "car";

          carDiv.innerHTML = `
            <h3>${car.make} ${car.model}</h3>
            <div class="image" id="image-${car.carId}"></div>
            <p><strong>Year:</strong> ${car.year}</p>
            <p><strong>Plate of Car:</strong> ${car.licensePlate}</p>
            <p><strong>Fuel Type:</strong> ${car.fuelType}</p>
            <p><strong>Transmission Type:</strong> ${car.transmissionType}</p>
            <p><strong>Color:</strong> ${car.color}</p>
            <p><strong>Daily Rent:</strong> ${car.dailyRate} JOD</p>
            <p><strong>Start Date:</strong></p>
            <input type="date" id="startDate" onchange="updateTotalCost(${car.dailyRate})">
            <p><strong>End Date:</strong></p>
            <input type="date" id="endDate" onchange="updateTotalCost(${car.dailyRate})">
            <p><strong>Total Cost:</strong> <span id="totalCost">0</span> JOD</p>
            <br>
            <button class="BookNow" type="submit" onclick="ConfirmRent(${car.carId})">
              Confirm your reservation
            </button>
          `;

          carsDetailsSection.appendChild(carDiv);

          const output = document.getElementById(`image-${car.carId}`);

          if (car.carId && typeof car.carId === "number") {
            const imageUrl = `http://localhost:5150/Car/GetImageCar/${car.carId}`;
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
                img.alt = "Car Image";
                img.className = "car-image";
                output.appendChild(img);
              })
              .catch((error) => {
                console.error("Error fetching image:", error);
                output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image"> Failed to load image.`;
              });
          } else {
            output.innerHTML = "Car image not available.";
          }
        });
      })
      .catch((error) => {
        console.error("Error fetching car details:", error);
        carsDetailsSection.innerHTML =
          "Error loading car details. Please try again later.";
      });
  }

  fetchCars();
}

function updateTotalCost(dailyRate) {
  const startDate = document.getElementById("startDate").value;
  const endDate = document.getElementById("endDate").value;

  if (!startDate || !endDate) {
    document.getElementById("totalCost").innerText = "0";
    return;
  }

  const numDays = CalculateNumberOfDays(new Date(startDate), new Date(endDate));

  if (numDays < 0) {
    alert("End date must be after the start date.");
    document.getElementById("totalCost").innerText = "0";
  } else {
    const totalCost = CalculateTotalCost(dailyRate, numDays);
    document.getElementById("totalCost").innerText = totalCost.toFixed(2);
  }
}

function CalculateNumberOfDays(startDate, endDate) {
  const timeDiff = endDate - startDate;
  const daysDiff = timeDiff / (1000 * 60 * 60 * 24); // Convert milliseconds to days
  return daysDiff;
}

function CalculateTotalCost(dailyRate, numberOfDays) {
  return dailyRate * numberOfDays;
}

function ConfirmRent(carId) {
  const userId = localStorage.getItem("UserId");
  const rentalStartDate = document.getElementById("startDate").value;
  const rentalEndDate = document.getElementById("endDate").value;

  // Check if dates are selected
  if (!rentalStartDate || !rentalEndDate) {
    alert("Please select both start and end dates.");
    return;
  }
  if (!carId) {
    alert("Car ID is missing.");
    return;
  }
  if (!userId) {
    alert("User ID is missing.");
    return;
  }

  console.log("Car ID:", carId);
  console.log("User ID:", userId);
  console.log("Rental Start Date:", rentalStartDate);
  console.log("Rental End Date:", rentalEndDate);

  // Redirect to payment page with correct query parameters
  window.location.href = `../HtmlPages/Payment.html?carId=${carId}&userId=${userId}&rentalStartDate=${rentalStartDate}&rentalEndDate=${rentalEndDate}`;
}

window.addEventListener("load", () => {
  const sessionExpiration = localStorage.getItem("sessionExpiration");
  if (sessionExpiration && Date.now() > sessionExpiration) {
    localStorage.clear(); // Clear localStorage if session has expired
    alert("Session expired. Please log in again.");
    window.location.href = "../HtmlPages/Login.html"; // Redirect to login page
  }
});
