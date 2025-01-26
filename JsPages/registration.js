document
  .getElementById("registration-form")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    // Get the input values
    const formData = new FormData(event.target);
    const companyTypeID = parseInt(
      document.getElementById("company-type").value
    );

    const userData = {
      Username: formData.get("owner-name"),
      Email: formData.get("owner-email"),
      CompanyName: formData.get("company-name"),
      ContactPersonName: formData.get("contact-person"),
      ContactEmail: formData.get("contact-email"),
      ContactPhone: formData.get("contact-phone"),
      password: formData.get("registration-password"),
      passwordConfirm: formData.get("confirm-registration-password"),
      TypeID: companyTypeID,
    };

    if (userData.password !== userData.passwordConfirm) {
      alert("Passwords do not match!");
      return;
    }

    const url = "http://localhost:5150/Auth/RegisterCompany";

    fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userData),
    })
      .then((response) => {
        if (response.ok) {
          return response.text();
        } else {
          throw new Error("Registration failed");
        }
      })
      .then((data) => {
        alert("Company registered successfully!");
        window.location.href = "../HtmlPages/Login.html";
      })
      .catch((error) => {
        alert("Error: " + error.message);
      });
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
