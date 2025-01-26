document.addEventListener("DOMContentLoaded", () => {
  initializeCompanyProfile();
  manageItems();
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
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => {
      element.innerHTML = `
        <div class="dropdown">
          <div class="nameCompany">
            <a class="companyName" href="../HtmlPages/Profile.html">
              <h2 class="companyName">${data}</h2>
            </a>
          </div>
          <ul class="dropdown-menu">
            <li><a href="../HtmlPages/Login.html" onclick="logout()">Logout</a></li>
          </ul>
        </div>`;
    })
    .catch((error) => console.error("Error fetching user name:", error));
}

function fetchCompanyName(element, registrationId) {
  const url = `http://localhost:5150/Company/GetNameCompany/${registrationId}`;
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => {
      element.innerHTML = `
        <div class="dropdown">
          <div class="nameCompany">
            <a class="companyName" href="../HtmlPages/CompanyProfile.html">
              <h2 class="companyName">${data}</h2>
            </a>
          </div>
          <ul class="dropdown-menu">
            <li><a href="../HtmlPages/CompanyLogin.html" onclick="logout()">Logout</a></li>
          </ul>
        </div>`;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

function fetchCompanyNameForItem(element, registrationId) {
  const url = `http://localhost:5150/Company/GetNameCompany/${registrationId}`;
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => (element.innerHTML = data))
    .catch((error) => console.error("Error fetching company name:", error));
}

function fetchCompanyPhone(element, registrationId) {
  const url = `http://localhost:5150/Company/GetPhoneCompany/${registrationId}`;
  fetch(url, { method: "GET", headers: { "Content-Type": "application/json" } })
    .then((response) => {
      if (!response.ok) throw new Error("Network response was not ok");
      return response.text();
    })
    .then((data) => (element.innerHTML = data))
    .catch((error) => console.error("Error fetching company phone:", error));
}

function manageItems() {
  const itemsDetailsSection = document.querySelector(".market-details");
  const searchButton = document.getElementById("search-button");
  const searchInput = document.getElementById("search-bar");
  const sortPriceSelect = document.getElementById("sort-price");
  const paginationContainer = document.querySelector(".pagination");

  let currentPage = 1;
  const itemsPerPage = 6;

  function fetchItems(searchTerm = "", sortOrder = "All") {
    let url = "http://localhost:5150/Item/GetItem"; // Default endpoint for "All" search and price.

    // Determine the correct API endpoint based on search and sort conditions
    if (searchTerm && sortOrder === "All") {
      // Search is not null, price is "All"
      url = `http://localhost:5150/Item/GetItemsSortedByTitle?title=${searchTerm}`;
    } else if (!searchTerm && sortOrder !== "All") {
      // Search is null, price is not "All"
      url = `http://localhost:5150/Item/GetItemsSortedByPrice?sortOrder=${sortOrder}`;
    } else if (searchTerm && sortOrder !== "All") {
      // Search is not null, price is not "All"
      url = `http://localhost:5150/Item/GetItemsSortedByTitleandPrice?title=${searchTerm}&sortOrder=${sortOrder}`;
    }

    fetch(url)
      .then((response) => {
        if (!response.ok) throw new Error("Failed to fetch Items");
        return response.json();
      })
      .then((items) => {
        // Filter the items based on search term if it's not empty
        const filteredItems = searchTerm
          ? items.filter((item) =>
              item.name.toLowerCase().includes(searchTerm.toLowerCase())
            )
          : items;

        const totalPages = Math.ceil(filteredItems.length / itemsPerPage);
        const paginatedItems = filteredItems.slice(
          (currentPage - 1) * itemsPerPage,
          currentPage * itemsPerPage
        );

        renderItems(paginatedItems);
        renderPagination(totalPages, filteredItems, sortOrder);
      })
      .catch((error) => {
        console.error("Error fetching items:", error);
        itemsDetailsSection.innerHTML =
          "Error loading items. Please try again later.";
      });
  }

  function renderItems(items) {
    itemsDetailsSection.innerHTML = "";
    items.forEach((item) => {
      const itemDiv = document.createElement("div");
      itemDiv.className = "item";
      itemDiv.innerHTML = `
        <h3>${item.name}</h3>
        <div id="image-${item.itemId}"></div>
        <p><strong>Market:</strong> <span class="company-name"></span></p>
        <p><strong>Price:</strong> ${item.price}</p>
        <p><strong>For more Information:</strong> <span class="company-phone"></span></p>
        <button class="BuyNow" type="submit" onclick="BuyItem(${item.itemId})">Buy</button>`;

      const companyNameSpan = itemDiv.querySelector(".company-name");
      fetchCompanyNameForItem(companyNameSpan, item.registrationId);
      const companyPhoneSpan = itemDiv.querySelector(".company-phone");
      fetchCompanyPhone(companyPhoneSpan, item.registrationId);

      itemsDetailsSection.appendChild(itemDiv);

      const output = document.getElementById(`image-${item.itemId}`);
      if (item.itemId && typeof item.itemId === "number") {
        const imageUrl = `http://localhost:5150/Item/GetImageItem/${item.itemId}`;
        fetch(imageUrl)
          .then((res) => {
            if (!res.ok) throw new Error(`Failed to fetch image.`);
            return res.blob();
          })
          .then((blob) => {
            const img = document.createElement("img");
            img.src = URL.createObjectURL(blob);
            img.alt = "Item Image";
            img.className = "item-image";
            output.appendChild(img);
          })
          .catch(() => {
            output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image">`;
          });
      } else {
        output.innerHTML = "Item image not available.";
      }
    });
  }
  function formatDate(date) {
    const options = { year: "numeric", month: "long", day: "numeric" };
    return date.toLocaleDateString("en-US", options);
  }
  function renderPagination(totalPages, filteredItems, sortOrder) {
    paginationContainer.innerHTML = "";
    for (let i = 1; i <= totalPages; i++) {
      const button = document.createElement("button");
      button.className = "pagination-button";
      button.innerText = i;

      if (i === currentPage) {
        button.style.backgroundColor = "#0056b3";
      }

      button.addEventListener("click", () => {
        currentPage = i;
        fetchItems(searchInput.value, sortOrder);
      });

      paginationContainer.appendChild(button);
    }
  }

  searchButton.addEventListener("click", () => {
    currentPage = 1; // Reset to first page on new search
    fetchItems(searchInput.value, sortPriceSelect.value);
  });

  sortPriceSelect.addEventListener("change", () => {
    currentPage = 1; // Reset to first page on new sort selection
    fetchItems(searchInput.value, sortPriceSelect.value);
  });

  fetchItems();
}

function BuyItem(ItemId) {
  const userId = localStorage.getItem("UserId");
  localStorage.setItem("ItemId", ItemId);
  if (!userId) {
    window.open(
      "../HtmlPages/Login.html",
      "Login",
      "width=500,height=500,left=100,top=100"
    );
  } else {
    window.open(
      "../HtmlPages/BuyItem.html",
      "BuyNow",
      "width=600,height=600"
    );
  }
}

function logout() {
  localStorage.clear();
  window.location.href = "../HtmlPages/Login.html";
}
