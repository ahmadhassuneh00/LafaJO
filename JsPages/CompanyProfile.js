document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
  loadCars();
});

function initializeCompanyProfile() {
  const nameCompany = document.querySelector(".nameCompany");
  const registrationId = localStorage.getItem("RegistrationId");

  if (nameCompany && registrationId) {
    fetchCompanyName(nameCompany, registrationId);
  } else {
    console.error(
      "Either the 'nameCompany' element or 'RegistrationId' is missing"
    );
  }
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
      element.innerHTML = `<h2 class="companyName">${data}</h2>`;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

function loadCars() {
  const typeId = localStorage.getItem("TypeId");
  const registrationId = localStorage.getItem("RegistrationId");

  if (typeId == 1) {
    const addTrip = document.querySelector(".card-container");
    addTrip.innerHTML =
      "<br><a href='../HtmlPages/TouristOfficeTrips.html'><img src='../Images/plus.png' alt='add Trip'></a>";
    fetchTrips(registrationId);
  } else if (typeId == 2) {
    const addCar = document.querySelector(".card-container");
    addCar.innerHTML =
      "<br><a href='../HtmlPages/CarCompanyCar.html'><img src='../Images/plus.png' alt='add car'></a>";
    fetchCars(registrationId);
  } else if (typeId == 3) {
    const addItem = document.querySelector(".card-container");
    addItem.innerHTML =
      "<br><a href='../HtmlPages/MarketVendorItem.html'><img src='../Images/plus.png' alt='add car'></a>";
    fetchItems(registrationId);
  } else {
    alert("The Type Id doesn't exist");
  }
}

//For Trips
function fetchTrips(registrationId) {
  const url = `http://localhost:5150/Trip/GetTripsbyRegistrationId/${registrationId}`;
  const cardContainer = document.querySelector(".card-container");

  fetch(url, { method: "GET" })
    .then(handleResponse)
    .then((Trips) => displayTrips(Trips, cardContainer))
    .catch((error) => {
      console.error("Error fetching trips:", error);
      cardContainer.innerHTML = "Error loading trips. Please try again later.";
    });
}

function handleResponse(response) {
  if (!response.ok) {
    throw new Error("Network response was not ok");
  }
  return response.json();
}

function displayTrips(Trips, container) {
  if (!Trips || Trips.length === 0) {
    container.innerHTML =
      "<br><a href='../HtmlPages/TouristOfficeTrips.html'><img src='../Images/plus.png' alt='add Trip'></a>";
    return;
  }

  Trips.forEach((trip) => {
    const tripCard = createTripCard(trip);
    container.appendChild(tripCard);
    loadTripImage(trip.tripId); // Call loadCarImage after appending to the container
  });
}
function createTripCard(trip) {
  const card = document.createElement("div");
  card.className = "card car-card";
  card.innerHTML = `
    <div id="card-image-${trip.tripId}" class="card-image"></div>
    <div class="card-content">
      <h6 style="display:none" id="tripId-${trip.tripId}">Trip Id: ${
    trip.tripId
  }</h6>
      <h3 class="card-title">${trip.title}</h3>
      <p class="card-description">${trip.content}</p>
      <p class="card-description">Price: $${trip.price}</p>
      <p class="card-description">Departure Date: ${new Date(
        trip.departureDate
      ).toLocaleDateString()}</p>
      <p class="card-description">Return Date: ${
        trip.returnDate
          ? new Date(trip.returnDate).toLocaleDateString()
          : "One-way Trip"
      }</p>
      <p class="card-description">Number Of Tourists:${trip.numOfTourist}</p>
      <button class="card-btn" onclick="DeleteTrip(${
        trip.tripId
      })">Delete Trip</button>
      <button class="card-btn" onclick="openUpdateWindow(${
        trip.tripId
      })">Update Trip</button>
    </div>
  `;
  return card;
}

function openUpdateWindow(tripId) {
  const TripId = localStorage.setItem("TripId", tripId);
  const win = `../HtmlPages/UpdateTrip.html?tripId=${tripId}`; // Pass the tripId as a query parameter
  const windowFeatures = "width=600,height=400,left=200,top=200";

  window.open(win, "Update Trip", windowFeatures);
}

function DeleteTrip(tripId) {
  const url = `http://localhost:5150/Trip/${tripId}`;
  fetch(url, {
    method: "DELETE", // Use 'DELETE' as the method string
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Failed to delete the trip");
      }
      alert("Trip deleted successfully");
      // Optionally remove the card from the DOM after deletion
      const card = document.getElementById(`tripId-${tripId}`).closest(".card");
      if (card) {
        card.remove();
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("Failed to delete the trip. Please try again.");
    });
}

function loadTripImage(tripId) {
  const imageUrl = `http://localhost:5150/Trip/GetImageTrip/${tripId}`;
  const output = document.getElementById(`card-image-${tripId}`);

  if (!output) {
    console.error(`Image output element not found for TripId: ${TripId}`);
    return; // Exit the function if the output element is not found
  }

  // Show loading text or spinner
  const loadingText = document.createElement("p");
  loadingText.textContent = "Loading image...";
  output.appendChild(loadingText); // Append loading indicator

  fetch(imageUrl)
    .then((res) => {
      if (!res.ok) {
        throw new Error(`Failed to fetch image. Status: ${res.status}`);
      }
      return res.blob();
    })
    .then((blob) => {
      // Clear the loading text
      output.innerHTML = "";

      const img = document.createElement("img");
      img.src = URL.createObjectURL(blob);
      img.alt = "Trip Image";
      img.className = "car-image";
      output.appendChild(img);
    })
    .catch((error) => {
      console.error("Error fetching image:", error);
      output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image"> Failed to load image.`;
    });
}

//For Cars
function fetchCars(registrationId) {
  const url = `http://localhost:5150/Car/GetCarsbyRegistrationId/${registrationId}`;
  const cardContainer = document.querySelector(".card-container");

  fetch(url, { method: "GET" })
    .then(handleResponse)
    .then((cars) => displayCars(cars, cardContainer))
    .catch((error) => {
      console.error("Error fetching cars:", error);
      addCar.innerHTML =
        "<br><a href='../HtmlPages/CarCompanyCar.html'><img src='../Images/plus.png' alt='add car'></a>";
    });
}

function handleResponse(response) {
  if (!response.ok) {
    throw new Error("Network response was not ok");
  }
  return response.json();
}

function displayCars(cars, container) {
  if (!cars || cars.length === 0) {
    container.innerHTML = "No cars available.";
    const addCar = document.querySelector(".card-container");
    addCar.innerHTML =
      "<br><a href='../HtmlPages/CarCompanyCar.html'><img src='../Images/plus.png' alt='add car'></a>";
    return;
  }

  cars.forEach((car) => {
    const carCard = createCarCard(car);
    container.appendChild(carCard);
    loadCarImage(car.carId); // Call loadCarImage after appending to the container
  });
}
function createCarCard(car) {
  const card = document.createElement("div");
  card.classList.add("card", "car-card"); // Combine class additions

  card.innerHTML = `
    <div id="card-image-${car.carId}" class="card-image"></div>
    <div class="card-content">
      <h3>${car.make} ${car.model}</h3>
      <div class="image" id="image-${car.carId}"></div>
      <p><strong>Year:</strong> ${car.year}</p>
      <p><strong>Plate of Car:</strong> ${car.licensePlate}</p>
      <p><strong>Fuel Type:</strong> ${car.fuelType}</p>
      <p><strong>Transmission Type:</strong> ${car.transmissionType}</p>
      <p><strong>Color:</strong> ${car.color}</p>
      <p><strong>Daily Rent:</strong> ${car.dailyRate} JOD</p>
      <button class="card-btn" onclick="DeleteCar(${car.carId})">Delete Car</button>
      <button class="card-btn" onclick="openUpdateWindowCar(${car.carId})">Update Car</button>
    </div>
  `;

  return card;
}

function openUpdateWindowCar(carId) {
  localStorage.setItem("CarId", carId); // Store the selected car's ID in localStorage

  const win = `../HtmlPages/UpdateCar.html?CarId=${carId}`; // Build the URL for the update page
  const windowFeatures = "width=600,height=400,left=200,top=200"; // Set the size and position of the new window

  window.open(win, "Update Car", windowFeatures); // Open the new window with the specified features
}

function DeleteCar(carId) {
  const url = `http://localhost:5150/Car/${carId}`;

  fetch(url, {
    method: "DELETE",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Failed to delete the car");
      }

      alert("Car deleted successfully");

      const card = document.getElementById(`carId-${carId}`)?.closest(".card");
      if (card) {
        card.remove();
      } else {
        location.reload();
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("Failed to delete the car. Please try again.");
    });
}

function loadCarImage(carId) {
  const imageUrl = `http://localhost:5150/Car/GetImageCar/${carId}`;
  const output = document.getElementById(`card-image-${carId}`);

  if (!output) {
    console.error(`Image output element not found for carId: ${carId}`);
    return; // Exit the function if the output element is not found
  }

  // Show loading text or spinner
  const loadingText = document.createElement("p");
  loadingText.textContent = "Loading image...";
  output.appendChild(loadingText); // Append loading indicator

  fetch(imageUrl)
    .then((res) => {
      if (!res.ok) {
        throw new Error(`Failed to fetch image. Status: ${res.status}`);
      }
      return res.blob();
    })
    .then((blob) => {
      // Clear the loading text
      output.innerHTML = "";

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
}

//For Market
function fetchItems(registrationId) {
  const url = `http://localhost:5150/Item/GetItemsbyRegistrationId/${registrationId}`;
  const cardContainer = document.querySelector(".card-container");

  fetch(url, { method: "GET" })
    .then(handleResponse)
    .then((items) => displayItems(items, cardContainer))
    .catch((error) => {
      console.error("Error fetching items:", error);
      cardContainer.innerHTML =
        "<br><a href='../HtmlPages/MarketVendorItem.html'><img src='../Images/plus.png' alt='add Item'></a>";
    });
}

function handleResponse(response) {
  if (!response.ok) {
    throw new Error("Network response was not ok");
  }
  return response.json();
}

function displayItems(items, container) {
  if (!items || items.length === 0) {
    container.innerHTML = "No items available.";
    const addItem = document.querySelector(".card-container");
    addItem.innerHTML =
      "<br><a href='../HtmlPages/MarketVendorItem.html'><img src='../Images/plus.png' alt='add car'></a>";
    return;
  }

  items.forEach((item) => {
    const itemCard = createItemCard(item);
    container.appendChild(itemCard);
    loadItemImage(item.itemId); // Call loadItemImage after appending to the container
  });
}

function createItemCard(item) {
  const card = document.createElement("div");
  card.classList.add("card", "item-card"); // Combine class additions
  card.id = `item-${item.itemId}`;

  card.innerHTML = `
    <div id="card-image-${item.itemId}" class="card-image"></div>
    <div class="card-content">
      <h3>${item.name}</h3>
      <p">Number Of Items:${item.numOfItems}</p>
      <p><strong>Price: ${item.price} Jod</strong></p>
      <button class="card-btn" onclick="DeleteItem(${item.itemId})">Delete Item</button>
      <button class="card-btn" onclick="openUpdateWindowItem(${item.itemId})">Update Item</button>
    </div>
  `;

  return card;
}

function openUpdateWindowItem(itemId) {
  localStorage.setItem("ItemId", itemId); // Store the selected car's ID in localStorage

  const win = `../HtmlPages/UpdateItem.html?ItemId=${itemId}`; // Build the URL for the update page
  const windowFeatures = "width=600,height=400,left=200,top=200"; // Set the size and position of the new window

  window.open(win, "Update Item", windowFeatures); // Open the new window with the specified features
}

function DeleteItem(itemId) {
  const url = `http://localhost:5150/Item/${itemId}`;

  fetch(url, {
    method: "DELETE",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Failed to delete the item");
      }

      alert("Item deleted successfully");

      const card = document.getElementById(`item-${itemId}`);
      if (card) {
        card.remove();
      } else {
        location.reload();
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("Failed to delete the item. Please try again.");
    });
}

function loadItemImage(itemId) {
  const imageUrl = `http://localhost:5150/Item/GetImageItem/${itemId}`;
  const output = document.getElementById(`card-image-${itemId}`);

  if (!output) {
    console.error(`Image output element not found for itemId: ${itemId}`);
    return; // Exit the function if the output element is not found
  }

  // Show loading text or spinner
  const loadingText = document.createElement("p");
  loadingText.textContent = "Loading image...";
  output.appendChild(loadingText); // Append loading indicator

  fetch(imageUrl)
    .then((res) => {
      if (!res.ok) {
        throw new Error(`Failed to fetch image. Status: ${res.status}`);
      }
      return res.blob();
    })
    .then((blob) => {
      // Clear the loading text
      output.innerHTML = "";

      const img = document.createElement("img");
      img.src = URL.createObjectURL(blob);
      img.alt = "Item Image";
      img.className = "item-image"; // Updated class name to match CSS
      output.appendChild(img);
    })
    .catch((error) => {
      console.error("Error fetching image:", error);
      output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image"> Failed to load image.`;
    });
}
