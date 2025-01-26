document.addEventListener("DOMContentLoaded", function () {
  // Login Form Validation
  const loginForm = document.getElementById("login-form");
  if (loginForm) {
    loginForm.addEventListener("submit", function (event) {
      const username = document.getElementById("username").value;
      const password = document.getElementById("password").value;

      if (!username || !password) {
        alert("Please fill in all fields.");
        event.preventDefault();
      }
    });
  }

  // Signup Form Validation
  const signupForm = document.getElementById("signup-form");
  if (signupForm) {
    signupForm.addEventListener("submit", function (event) {
      const password = document.getElementById("password").value;
      const confirmPassword = document.getElementById("confirm-password").value;

      if (password !== confirmPassword) {
        alert("Passwords do not match.");
        event.preventDefault();
      }
    });
  }

  // Registration Form Validation
  const registrationForm = document.getElementById("registration-form");
  if (registrationForm) {
    registrationForm.addEventListener("submit", function (event) {
      const password = document.getElementById("registration-password").value;
      const confirmPassword = document.getElementById(
        "confirm-registration-password"
      ).value;

      if (password !== confirmPassword) {
        alert("Passwords do not match.");
        event.preventDefault();
      }
    });
  }
});
