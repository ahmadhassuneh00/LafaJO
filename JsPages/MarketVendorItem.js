function addItemData(event) {
  event.preventDefault(); // Prevent form submission

  const Name = document.getElementById("Name").value;
  const RegistrationId = localStorage.getItem("RegistrationId");
  const ImageURL = document.getElementById("photo").files[0];
  const Price = document.getElementById("price").value;
  const NumOfItems = document.getElementById("NumOfItems").value;

  // Basic validation (make returnDate optional)
  if (!Name || !Price || !ImageURL || !RegistrationId || !NumOfItems) {
    alert(
      "Please fill out all required fields and ensure the RegistrationId is valid."
    );
    return;
  }

  let formData = new FormData();
  formData.append("Name", Name);
  formData.append("NumOfItems", NumOfItems);
  formData.append("RegistrationId", RegistrationId);
  formData.append("ImageURL", ImageURL);
  formData.append("Price", Price);

  const url2 = "http://localhost:5150/Item/addItem";
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
        alert("Item added successfully.");
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
