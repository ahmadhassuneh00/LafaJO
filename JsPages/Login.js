document.getElementById("login-form").addEventListener("submit", function (e) {
  e.preventDefault();
  login();
});

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
            <a href="../HtmlPages/Login.html" onclick="logout()"
              >Logout</a
            >
          </li>
        </ul>
      </div>
      `;
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
            <a href="../HtmlPages/CompanyLogin.html" onclick="logout()"
              >Logout</a
            >
          </li>
        </ul>
      </div>
      `;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

function login() {
  const email = document.querySelector("#email").value;
  const password = document.querySelector("#password").value;

  const LogInData = { email: email, password: password };

  fetch("http://localhost:5150/Auth/Login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(LogInData),
  })
    .then((response) => {
      if (!response.ok) {
        return response.json().then((err) => {
          throw new Error(err.message || "Login failed");
        });
      }
      return response.json();
    })
    .then((data) => {
      if (data && data.token) {
        // Store token in localStorage and set session expiration time
        localStorage.setItem("authToken", data.token);
        const sessionExpiration = Date.now() + 30 * 60 * 1000; // 30 minutes from now
        localStorage.setItem("sessionExpiration", sessionExpiration);

        const url2 = "http://localhost:5150/Auth/RefreshToken";
        return fetch(url2, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${data.token}`,
          },
        });
      } else {
        console.error("Login failed. No token returned.");
      }
    })
    .then((response) => {
      if (response && response.ok) {
        return response.json();
      } else {
        throw new Error("Failed to refresh token");
      }
    })
    .then((data2) => {
      localStorage.setItem("UserId", data2.userId);
      // Redirect to the home page
      window.location.href = "../HtmlPages/Home.html";
    })
    .catch((error) => {
      console.error("Error:", error);
      alert("Login failed. Please check your email or password.");
    });
}

// Check session expiration on page load and clear localStorage if expired
window.addEventListener("load", () => {
  const sessionExpiration = localStorage.getItem("sessionExpiration");
  if (sessionExpiration && Date.now() > sessionExpiration) {
    localStorage.clear(); // Clear localStorage if session has expired
    alert("Session expired. Please log in again.");
    window.location.href = "../HtmlPages/Login.html"; // Redirect to login page
  }
});
