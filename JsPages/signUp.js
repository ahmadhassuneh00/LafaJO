document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("signup-form");

  // Attach the event listener to the form's submit event
  form.addEventListener("submit", function (event) {
    event.preventDefault(); // Prevent default form submission
    sendUserData(); // Call the function to send user data
  });
});

function sendUserData() {
  // Get the input values from the form
  const firstName = document.getElementById("FirstName").value;
  const lastName = document.getElementById("LastName").value;
  const email = document.getElementById("email").value;
  const gender = document.getElementById("gender").value;
  const password = document.getElementById("password").value;
  const passwordConfirm = document.getElementById("confirm-password").value;

  // Create the data object to send in the request
  const userData = {
    firstName: firstName,
    lastName: lastName,
    email: email,
    gender: gender,
    password: password,
    passwordConfirm: passwordConfirm,
  };

  // Define the API endpoint
  const url = "http://localhost:5150/Auth/SignUp";

  // Send the POST request to the server
  fetch(url, {
    method: "POST", // HTTP method
    headers: {
      "Content-Type": "application/json", // Specify that we're sending JSON
    },
    body: JSON.stringify(userData), // Convert the object to JSON string
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        throw new Error("Network response was not ok");
      }
    })
    .then((data) => {
      // If successful, redirect to the login page
      console.log("Signup success:", data);
      window.location.href = "../HtmlPages/Login.html";
    })
    .catch((error) => {
      console.error("Error during signup:", error);
      alert("Signup failed. Please try again.");
    });
}
