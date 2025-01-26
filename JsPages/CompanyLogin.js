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

document
  .getElementById("loginForm")
  .addEventListener("submit", async function (e) {
    e.preventDefault();
    await login();
  });

async function login() {
  const email = document.querySelector("#email").value;
  const password = document.querySelector("#password").value;

  const LogInData = { email, password };

  try {
    const response = await fetch("http://localhost:5150/Auth/CompanyLogin", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(LogInData),
    });

    if (!response.ok) {
      console.error(`Login failed with status: ${response.status}`);
      throw new Error("Login failed. Please check your email or password.");
    }

    const data = await response.json();
    if (data && data.token) {
      localStorage.setItem("authToken", data.token);
      console.log("Token saved:", data.token);

      // Set session expiration time (e.g., 30 minutes from now)
      const sessionExpiration = Date.now() + 30 * 60 * 1000; // 30 minutes
      localStorage.setItem("sessionExpiration", sessionExpiration);

      // Proceed to refresh token
      const refreshSuccess = await refreshToken(data.token);
      if (!refreshSuccess) return; // Stop further execution if refresh fails

      // Get TypeID and redirect based on it
      const TypeID = await GetTypeID(email);

      window.location.href = "../HtmlPages/CompanyProfile.html";
    } else {
      console.error("Login failed. No token returned.");
    }
  } catch (error) {
    console.error("Error during login:", error);
    alert(error.message);
  }
}

async function refreshToken(token) {
  try {
    const url = "http://localhost:5150/Auth/CompanyRefreshToken";
    const response = await fetch(url, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    console.log("Refresh Token Response:", response); // For debugging

    if (!response.ok) {
      if (response.status === 401) {
        console.error("Unauthorized: Please log in again.");
        alert("Unauthorized: Please log in again.");
        return false;
      }
      throw new Error("Failed to refresh token.");
    }

    const data = await response.json();
    if (data.RegistrationID) {
      console.log("RegistrationID received:", data.RegistrationID);
      localStorage.setItem("RegistrationId", data.RegistrationID);
      return true;
    } else {
      throw new Error("RegistrationID not found in the response.");
    }
  } catch (error) {
    console.error("Error during token refresh:", error);
    alert(error.message);
    return false;
  }
}

async function GetTypeID(_ContactEmail) {
  try {
    const response = await fetch(
      `http://localhost:5150/Company/GetTypeID/${_ContactEmail}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      console.error(`Failed to fetch TypeID with status: ${response.status}`);
      throw new Error("Failed to fetch Type ID");
    }

    const TypeID = await response.json();
    console.log("TypeID received:", TypeID);
    localStorage.setItem("TypeId", TypeID);
    return TypeID;
  } catch (error) {
    console.error("Error fetching TypeID:", error);
    return null;
  }
}
