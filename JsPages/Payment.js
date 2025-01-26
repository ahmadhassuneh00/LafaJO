document.querySelector(".btn").addEventListener("click", function () {
  // Get input values using placeholders to target elements
  const cardHolder = document
    .querySelector('input[placeholder="Card holder"]')
    .value.trim();
  const cardNumber = document
    .querySelector('input[placeholder="Card Number"]')
    .value.trim();
  const expiryDate = document
    .querySelector('input[placeholder="00 / 00"]')
    .value.trim();
  const cvc = document.querySelector('input[placeholder="000"]').value.trim();

  // Regular expressions for validation
  const cardNumberRegex = /^[0-9]{13,19}$/; // Allow 13 to 19 digits
  const expiryDateRegex = /^(0[1-9]|1[0-2]) \/ [0-9]{2}$/; // MM / YY
  const cvcRegex = /^[0-9]{3}$/; // 3 digits

  // Validate fields
  if (!cardHolder) {
    alert("Card holder name is required.");
    return;
  }

  if (!cardNumberRegex.test(cardNumber.replace(/\s+/g, ""))) {
    alert("Card number must be between 13 and 19 digits without spaces.");
    return;
  }

  if (!isValidExpiryDate(expiryDate)) {
    alert(
      "Invalid expiry date. Ensure it's in MM / YY format and not expired."
    );
    return;
  }

  if (!cvcRegex.test(cvc)) {
    alert("CVC must be a 3-digit number.");
    return;
  }

  // Proceed with the API request
  addPaymentCard(cardHolder, cardNumber.replace(/\s+/g, ""), expiryDate, cvc);
});

function isValidExpiryDate(expiryDate) {
  // Regular expression to check MM / YY format
  const expiryRegex = /^(0[1-9]|1[0-2]) \/ \d{2}$/;

  // Check if the format is valid
  if (!expiryRegex.test(expiryDate)) {
    return false; // Invalid format
  }

  // Extract month and year
  const [month, year] = expiryDate.split(" / ").map(Number);

  // Get current date and year
  const currentDate = new Date();
  const currentMonth = currentDate.getMonth() + 1; // Months are 0-indexed
  const currentYear = currentDate.getFullYear() % 100; // Get last two digits of the year

  // Check if the expiry date is in the future
  if (year < currentYear || (year === currentYear && month < currentMonth)) {
    return false; // Expiry date is in the past
  }

  return true; // Expiry date is valid
}

function addPaymentCard(cardHolder, cardNumber, expiryDate, cvc) {
  let formData = new FormData();
  formData.append("CardNumber", cardNumber); // Ensure matching API parameter names
  formData.append("CardHolder", cardHolder);
  formData.append("ExpirationDate", expiryDate);
  formData.append("Cvv", cvc);

  // Send the POST request to the backend API
  fetch("http://localhost:5150/api/Payment/addPaymentCard", {
    method: "POST",
    body: formData,
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.message === "Card added successfully") {
        // After payment success, proceed with either renting a car or booking a trip
        getDataFromUrl();
      }
    })
    .catch((error) => {
      console.error("Error adding payment card:", error);
      // alert("There was an error adding your payment card.");
    });
}

function getDataFromUrl() {
  const params = new URLSearchParams(window.location.search);
  const userId = params.get("userId");
  const carId = params.get("carId");
  const rentalStartDate = params.get("rentalStartDate");
  const rentalEndDate = params.get("rentalEndDate");
  const tripId = params.get("tripId");
  const numOfTraveller = params.get("numOfTraveller");
  const itemId = params.get("itemId");
  const numOfItems = params.get("numOfItems");
  const totalCost = params.get("totalCost");

  // Check for missing parameters and handle accordingly
  if (!userId) {
    alert("User ID is missing.");
    return;
  }

  // Check if this is a rental scenario
  if (carId && rentalStartDate && rentalEndDate) {
    console.log(
      `Car ID: ${carId}, User ID: ${userId}, Rental Start: ${rentalStartDate}, Rental End: ${rentalEndDate}`
    );
    RentCar(userId, carId, rentalStartDate, rentalEndDate);
  }
  // Check if this is a trip booking scenario
  else if (tripId && numOfTraveller && totalCost) {
    console.log(
      `Trip ID: ${tripId}, User ID: ${userId}, Number of Travellers: ${numOfTraveller}, Total Cost: ${totalCost}`
    );
    BookATrip(userId, tripId, numOfTraveller, totalCost);
  }
  // Check if this is an item buying scenario
  else if (itemId && numOfItems && totalCost) {
    console.log(
      `Item ID: ${itemId}, User ID: ${userId}, Number Of Items: ${numOfItems}, Total Cost: ${totalCost}`
    );
    BuyAItem(userId, itemId, numOfItems, totalCost);
  }
  // If neither parameters are present, show an alert
  else {
    alert("Missing required parameters for either car or trip booking or item buy.");
  }
}

function RentCar(userId, carId, rentalStartDate, rentalEndDate) {
  // Check car availability before booking
  const availabilityUrl = `http://localhost:5150/RentCar/IsCarAvailable/${carId}?startDate=${rentalStartDate}&endDate=${rentalEndDate}`;
  fetch(availabilityUrl)
    .then((response) => response.json())
    .then((data) => {
      if (data.available) {
        // Proceed with reservation if car is available
        let formData = new FormData();
        formData.append("CarID", carId);
        formData.append("UserId", userId);
        formData.append("RentalStartDate", rentalStartDate);
        formData.append("RentalEndDate", rentalEndDate);

        const url = "http://localhost:5150/RentCar/addRentCar";
        fetch(url, {
          method: "POST",
          body: formData,
        })
          .then((response) => {
            if (response.ok) {
              alert("Reservation confirmed successfully!");
              localStorage.removeItem("CarId");
              window.close();
            } else {
              alert("Failed to confirm the reservation.");
            }
          })
          .catch((error) => {
            console.error("Error:", error);
          });
      } else {
        alert("The car is not available for the selected dates.");
      }
    })
    .catch((error) => {
      console.error("Error checking car availability:", error);
    });
}

function BookATrip(userId, tripId, numOfTraveller, totalCost) {
  let formData = new FormData();
  formData.append("TripId", tripId);
  formData.append("UserId", userId);
  formData.append("numberOfPersons", numOfTraveller);
  formData.append("TotalCost", totalCost);

  fetch(`http://localhost:5150/BookTrip/addBookTrip`, {
    method: "POST",
    body: formData,
  })
    .then((response) =>
      response.json().then((data) => ({
        ok: response.ok,
        status: response.status,
        body: data,
      }))
    )
    .then(({ ok, status, body }) => {
      if (ok) {
        alert("Trip booked successfully.");
        localStorage.removeItem("TripId");
        window.close();
      } else if (
        status === 400 &&
        body &&
        body.message.includes("already booked")
      ) {
        alert("You have already booked this trip. Please check your bookings.");
      } else if (
        status === 400 &&
        body &&
        body.message.includes("exceeds the maximum number")
      ) {
        alert("The number of travelers exceeds the available spots.");
      } else {
        alert("An unexpected error occurred. Please try again later.");
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("An error occurred. Please try again later.");
    });
}

function BuyAItem(userId, itemId, numOfItems, totalCost) {
  let formData = new FormData();
  formData.append("ItemId", itemId);
  formData.append("UserId", userId);
  formData.append("numberOfItems", numOfItems);
  formData.append("TotalCost", totalCost);

  fetch(`http://localhost:5150/BuyItem/addBuyItem`, {
    method: "POST",
    body: formData,
  })
    .then((response) =>
      response.json().then((data) => ({
        ok: response.ok,
        status: response.status,
        body: data,
      }))
    )
    .then(({ ok, status, body }) => {
      if (ok) {
        alert("Item bought successfully.");
        localStorage.removeItem("ItemId");
        window.close();
      } else if (
        status === 400 &&
        body &&
        body.message.includes("already bought")
      ) {
        alert("You have already bought this item. Please check your Bought Items.");
      } else if (
        status === 400 &&
        body &&
        body.message.includes("exceeds the maximum number")
      ) {
        alert("The number of items exceeds the available quantity.");
      } else {
        alert("An unexpected error occurred. Please try again later.");
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("An error occurred. Please try again later.");
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
