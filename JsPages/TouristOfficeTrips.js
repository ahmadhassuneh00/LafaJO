function addTripData(event) {
  event.preventDefault(); // Prevent form submission

  const Title = document.getElementById("destination").value;
  const Content = document.getElementById("Content").value;
  const RegistrationId = localStorage.getItem("RegistrationId");
  const departureDate = document.getElementById("departureDate").value;
  const returnDate = document.getElementById("returnDate").value; // Can be optional
  const ImageURL = document.getElementById("photo").files[0];
  const Price = document.getElementById("price").value;
  const NumOfTourist = document.getElementById("NumOfTourist").value;
  // Basic validation (make returnDate optional)
  if (
    !Title ||
    !Content ||
    !Price ||
    !ImageURL ||
    !RegistrationId ||
    !departureDate ||
    !NumOfTourist
  ) {
    alert(
      "Please fill out all required fields and ensure the RegistrationId is valid."
    );
    return;
  }

  let formData = new FormData();
  formData.append("Title", Title);
  formData.append("Content", Content);
  formData.append("RegistrationId", RegistrationId);
  formData.append("departureDate", departureDate);
  formData.append("returnDate", returnDate);
  formData.append("ImageURL", ImageURL);
  formData.append("Price", Price);
  formData.append("NumOfTourist", NumOfTourist);

  const url2 = "http://localhost:5150/Trip/addTrip";
  console.log("Sending data to the database...");

  // Track formData for debugging
  formData.forEach((value, key) => {
    console.log(`${key}: ${value}`);
  });

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
        alert("Trip added successfully.");
        console.log("Response data:", data);
        window.location.href = "../HtmlPages/CompanyProfile.html";
      } else {
        alert("Trip added successfully, but no JSON response.");
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("An error occurred while adding the trip.");
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
