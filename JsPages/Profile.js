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
      element.innerHTML = `
          <div class="nameCompany"><h2 class="companyName">${data}</h2></div>
          
        </div>
        `;
    })
    .catch((error) => console.error("Error fetching user name:", error));
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
      element.innerHTML = `
      <div class="nameCompany"><h2 class="companyName">${data}</h2></div>
      `;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

// Function to fetch rented cars
function GetRentCar() {
  const userId = localStorage.getItem("UserId");
  const urlForCarId = `http://localhost:5150/RentCar/GetCarId/${userId}`;
  const divRentCar = document.querySelector(".rentCar");

  // Helper function to format dates
  const formatDate = (date) => {
    return date.toISOString().split("T")[0];
  };

  // First fetch to get the car IDs based on the user ID
  fetch(urlForCarId, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`Error fetching car IDs: Status ${response.status}`);
      }
      return response.json(); // Parse the response as JSON
    })
    .then((carIds) => {
      if (Array.isArray(carIds) && carIds.length > 0) {
        carIds.forEach((carId) => {
          // Fetch car details based on car ID
          fetch(`http://localhost:5150/Car/${carId}`, {
            method: "GET",
            headers: { "Content-Type": "application/json" },
          })
            .then((response) => {
              if (!response.ok) {
                throw new Error(
                  `Error fetching car details: Status ${response.status}`
                );
              }
              return response.json();
            })
            .then((RentCar) => {
              // Fetch rental details for the car
              fetch(
                `http://localhost:5150/RentCar/GetRentCarsbyCarId/${RentCar.carId}`,
                {
                  method: "GET",
                  headers: { "Content-Type": "application/json" },
                }
              )
                .then((response) => {
                  if (!response.ok) {
                    throw new Error(
                      `Error fetching rental details: Status ${response.status}`
                    );
                  }
                  return response.json();
                })
                .then((rentedCar) => {
                  const formattedStartDate = formatDate(
                    new Date(rentedCar.rentalStartDate)
                  );
                  const formattedEndDate = formatDate(
                    new Date(rentedCar.rentalEndDate)
                  );

                  divRentCar.innerHTML += `
                    <div class="car" id="car-${RentCar.carId}">
                      <h3>${RentCar.make} ${RentCar.model}</h3>
                      <div class="image" id="image-${RentCar.carId}" style="height: 250px; width: 350px; border-radius: 25px;"></div>
                      <p><strong>Year:</strong> ${RentCar.year}</p>
                      <p><strong>Plate of Car:</strong> ${RentCar.licensePlate}</p>
                      <p><strong>Fuel Type:</strong> ${RentCar.fuelType}</p>
                      <p><strong>Transmission Type:</strong> ${RentCar.transmissionType}</p>
                      <p><strong>Color:</strong> ${RentCar.color}</p>
                      <p><strong>Rental Start Date:</strong> ${formattedStartDate}</p>
                      <p><strong>Rental End Date:</strong> ${formattedEndDate}</p>
                      <p><strong>Total Cost:</strong> ${rentedCar.totalCost} JOD</p>
                      <button class="BookNow" type="submit" onclick="deleteCar(${RentCar.carId})">Stop Rent</button>
                    </div>`;

                  const imageContainer = document.getElementById(
                    `image-${RentCar.carId}`
                  );
                  if (imageContainer) {
                    // Fetch and display the car image for this car
                    fetchRentCarImage(RentCar.carId, imageContainer);
                  } else {
                    console.error(
                      `Image container with ID 'image-${RentCar.carId}' not found.`
                    );
                  }
                })
                .catch((error) => {
                  console.error("Error fetching rental car data:", error);
                });
            })
            .catch((error) => {
              console.error("Error fetching car details:", error);
            });
        });
      } else {
        console.log("No rented cars found.");
      }
    })
    .catch((error) => {
      console.error(error); // Handle errors when fetching car IDs
    });
}

// Function to fetch the image for a rented car
function fetchRentCarImage(carId, imageContainer) {
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

      // Set CSS for the image
      img.style.height = "250px";
      img.style.width = "350px";
      img.style.borderRadius = "25px";

      // Check if the image container exists before appending
      if (imageContainer) {
        imageContainer.innerHTML = ""; // Clear any existing images
        imageContainer.appendChild(img); // Append the new image
      } else {
        console.error(`Container not found for car ID: ${carId}`);
      }
    })
    .catch((error) => console.error("Error fetching car image:", error));
}

// Function to delete a rented car
function deleteCar(rentedCarId) {
  const userConfirmed = confirm(
    "Are you sure you want to stop renting this car?"
  );
  if (userConfirmed) {
    fetch(`http://localhost:5150/RentCar/DeleteRentCar/${rentedCarId}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Failed to delete rented car with ID ${rentedCarId}`);
        }
        // Successfully deleted car, remove it from the UI
        const carElement = document.getElementById(`car-${rentedCarId}`);
        if (carElement) {
          carElement.remove();
        }
      })
      .catch((error) => {
        console.error(
          `Error deleting rented car with ID ${rentedCarId}:`,
          error
        );
      });
  } else {
    console.log("User canceled the action to stop renting the car.");
  }
}

// Call the function to initiate the process
GetRentCar();

function GetTrips() {
  const userId = localStorage.getItem("UserId");
  const urlForTripId = `http://localhost:5150/BookTrip/GetTripIdByUserId/${userId}`;
  const divTrips = document.querySelector(".bookTrips");

  fetch(urlForTripId, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => response.json())
    .then((tripIds) => {
      console.log("Processing trip IDs:", tripIds);
      if (Array.isArray(tripIds)) {
        tripIds.forEach((tripId) => {
          fetchTripDetails(tripId);
        });
      } else {
        console.error(
          "Error: Expected an array of trip IDs, but got:",
          tripIds
        );
      }
    })
    .catch((error) => {
      console.error("Error fetching trip data:", error);
    });
}

function fetchTripDetails(tripId) {
  fetch(`http://localhost:5150/Trip/${tripId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((text) => {
          throw new Error(
            `Error fetching trip details: ${response.status} - ${text}`
          );
        });
      }
      return response.json();
    })
    .then((trip) => {
      console.log("Processing trip details:", trip);
      if (!trip) {
        throw new Error(`Trip data not found for trip ID ${tripId}`);
      }
      fetchBookedTripDetails(trip); // Fetch booked trip details for this trip
    })
    .catch((error) => {
      console.error(
        `Error fetching trip details for trip ID ${tripId}:`,
        error
      );
    });
}

function fetchBookedTripDetails(trip) {
  fetch(`http://localhost:5150/BookTrip/${trip.tripId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => {
      if (!response.ok) {
        if (response.status === 404) {
          console.warn(
            `Booked trip details for trip ID ${trip.tripId} not found.`
          );
          return null;
        }
        return response.text().then((text) => {
          throw new Error(`Error: ${response.status} - ${text}`);
        });
      }
      return response.json();
    })
    .then((bookedTrip) => {
      if (!bookedTrip) {
        console.log(
          `No booked trip details available for trip ID ${trip.tripId}`
        );
        return;
      }
      displayTrip(trip, bookedTrip);
    })
    .catch((error) => {
      console.error(
        `Error fetching booked trip details for trip ID ${trip.tripId}:`,
        error
      );
    });
}

function displayTrip(trip, bookedTrip) {
  const divTrips = document.querySelector(".bookTrips");
  const formattedStartDate = formatDate(new Date(trip.departureDate));
  const formattedEndDate = formatDate(new Date(trip.returnDate));

  // Create trip element
  const tripDiv = document.createElement("div");
  tripDiv.id = `trip-${trip.tripId}`;
  tripDiv.innerHTML = `
    <h3 class="title">${trip.title}</h3>
    <div class="image" id="image-${trip.tripId}"></div>
    <p><strong>Content:</strong> ${trip.content}</p>
    <p><strong>Departure Date:</strong> ${formattedStartDate}</p>
    <p><strong>Return Date:</strong> ${formattedEndDate}</p>
    <p><strong>Booked Persons:</strong> ${bookedTrip.numberOfPersons}</p>
    <p><strong>Total Cost:</strong> ${bookedTrip.totalCost} JOD</p>
    <button class="BookNow" type="submit" onclick="deleteTrip(${trip.tripId})">Cancel Trip</button>
  `;
  divTrips.appendChild(tripDiv);

  // Fetch and display the image
  const imageContainer = document.getElementById(`image-${trip.tripId}`);
  fetchTripImage(trip.tripId, imageContainer);
}

function fetchTripImage(tripId, imageContainer) {
  const imageUrl = `http://localhost:5150/Trip/GetImageTrip/${tripId}`;
  fetch(imageUrl)
    .then((response) => {
      if (!response.ok) {
        console.warn(`Image not found for trip ID ${tripId}.`);
        return null; // Handle missing image gracefully
      }
      return response.blob();
    })
    .then((blob) => {
      if (blob) {
        const img = document.createElement("img");
        img.src = URL.createObjectURL(blob);
        img.alt = "Trip Image";
        img.className = "trip-image";
        imageContainer.appendChild(img);
      } else {
        imageContainer.innerHTML = `<p>No image available</p>`;
      }
    })
    .catch((error) => {
      console.error(`Error fetching trip image for trip ID ${tripId}:`, error);
      imageContainer.innerHTML = `<p>Error loading image</p>`;
    });
}

function deleteTrip(bookedTripId) {
  const userConfirmed = confirm(
    "Are you sure you want to Cancel trip reservation?"
  );
  if (userConfirmed) {
    fetch(`http://localhost:5150/BookTrip/DeleteBookTrip?id=${bookedTripId}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Failed to delete trip with ID ${bookedTripId}`);
        }
        const tripElement = document.getElementById(`trip-${bookedTripId}`);
        if (tripElement) {
          tripElement.remove();
          console.log(`Trip with ID ${bookedTripId} removed from UI.`);
        }
      })
      .catch((error) => {
        console.error(`Error deleting trip with ID ${bookedTripId}:`, error);
      });
  } else {
    console.log("User canceled the action to delete trip.");
  }
}

function formatDate(date) {
  const options = { year: "numeric", month: "long", day: "numeric" };
  return date.toLocaleDateString("en-US", options);
}

// Call GetTrips to load the trips
GetTrips();

function GetItems() {
  const userId = localStorage.getItem("UserId");
  const urlForItemId = `http://localhost:5150/BuyItem/GetItemIdByUserId/${userId}`;
  const divItems = document.querySelector(".buyItems");

  fetch(urlForItemId, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => response.json())
    .then((itemIds) => {
      console.log("Processing item IDs:", itemIds);
      if (Array.isArray(itemIds)) {
        itemIds.forEach((itemId) => {
          fetchItemDetails(itemId);
        });
      } else {
        console.error(
          "Error: Expected an array of item IDs, but got:",
          itemIds
        );
      }
    })
    .catch((error) => {
      console.error("Error fetching item data:", error);
    });
}

function fetchItemDetails(itemId) {
  fetch(`http://localhost:5150/Item/${itemId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((text) => {
          throw new Error(
            `Error fetching item details: ${response.status} - ${text}`
          );
        });
      }
      return response.json();
    })
    .then((item) => {
      console.log("Processing item details:", item);
      if (!item) {
        throw new Error(`Item data not found for item ID ${itemId}`);
      }
      fetchBoughtItemDetails(item); // Fetch bought item details for this item
    })
    .catch((error) => {
      console.error(
        `Error fetching item details for item ID ${itemId}:`,
        error
      );
    });
}

function fetchBoughtItemDetails(item) {
  fetch(`http://localhost:5150/BuyItem/${item.itemId}`, {
    method: "GET",
    headers: { "Content-Type": "application/json" },
  })
    .then((response) => {
      if (!response.ok) {
        if (response.status === 404) {
          console.warn(
            `Bought item details for item ID ${item.itemId} not found.`
          );
          return null;
        }
        return response.text().then((text) => {
          throw new Error(`Error: ${response.status} - ${text}`);
        });
      }
      return response.json();
    })
    .then((boughtItem) => {
      if (!boughtItem) {
        console.log(
          `No bought item details available for item ID ${item.itemId}`
        );
        return;
      }
      displayItem(item, boughtItem);
    })
    .catch((error) => {
      console.error(
        `Error fetching bought item details for item ID ${item.itemId}:`,
        error
      );
    });
}

function displayItem(item, boughtItem) {
  const divItems = document.querySelector(".buyItems");

  // Create item element
  const itemDiv = document.createElement("div");
  itemDiv.id = `item-${item.itemId}`;
  itemDiv.innerHTML = `
    <h3 class="name">${item.name}</h3>
    <div class="image" id="image-${item.itemId}"></div>
    <p><strong>Order Status:</strong> Pending....</p>
    <p><strong>Bought Items:</strong> ${boughtItem.numberOfItems}</p>
    <p><strong>Total Cost:</strong> ${boughtItem.totalCost} JOD</p>
    <button class="BuyNow" type="submit" onclick="deleteItem(${item.itemId})">Cancel Order</button>
  `;
  divItems.appendChild(itemDiv);

  // Fetch and display the image
  const imageContainer = document.getElementById(`image-${item.itemId}`);
  fetchItemImage(item.itemId, imageContainer);
}

function fetchItemImage(itemId, imageContainer) {
  const imageUrl = `http://localhost:5150/Item/GetImageItem/${itemId}`;
  fetch(imageUrl)
    .then((response) => {
      if (!response.ok) {
        console.warn(`Image not found for item ID ${itemId}.`);
        return null; // Handle missing image gracefully
      }
      return response.blob();
    })
    .then((blob) => {
      if (blob) {
        const img = document.createElement("img");
        img.src = URL.createObjectURL(blob);
        img.alt = "Item Image";
        img.className = "item-image";
        imageContainer.appendChild(img);
      } else {
        imageContainer.innerHTML = `<p>No image available</p>`;
      }
    })
    .catch((error) => {
      console.error(`Error fetching item image for item ID ${itemId}:`, error);
      imageContainer.innerHTML = `<p>Error loading image</p>`;
    });
}

function deleteItem(boughtItemId) {
  const userConfirmed = confirm(
    "Are you sure you want to remove bought item?"
  );
  if (userConfirmed) {
    fetch(`http://localhost:5150/BuyItem/DeleteBuyItem?id=${boughtItemId}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Failed to delete item with ID ${boughtItemId}`);
        }
        const itemElement = document.getElementById(`item-${boughtItemId}`);
        if (itemElement) {
          itemElement.remove();
          console.log(`Item with ID ${boughtItemId} removed from UI.`);
        }
      })
      .catch((error) => {
        console.error(`Error deleting item with ID ${boughtItemId}:`, error);
      });
  } else {
    console.log("User canceled the action to delete item.");
  }
}

// Call GetItems to load the items
GetItems();

function showTab(tabId) {
  // Hide all tab content
  const tabs = document.querySelectorAll(".tab-content");
  tabs.forEach((tab) => {
    tab.classList.remove("active");
  });

  // Show the selected tab
  const selectedTab = document.getElementById(tabId);
  if (selectedTab) {
    selectedTab.classList.add("active");
  }
}

function logout() {
  localStorage.clear();
}

function ReloadPages() {
  location.reload();
}

window.addEventListener("load", () => {
  const sessionExpiration = localStorage.getItem("sessionExpiration");
  if (sessionExpiration && Date.now() > sessionExpiration) {
    localStorage.clear(); // Clear localStorage if session has expired
    alert("Session expired. Please log in again.");
    window.location.href = "../HtmlPages/Login.html"; // Redirect to login page
  }
});
