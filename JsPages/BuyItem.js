document.addEventListener("DOMContentLoaded", () => {
  GetItem();
});

function fetchCompanyNameForItem(element, registrationId) {
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
      element.innerHTML = data;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}

function GetItem() {
  const itemDetailsSection = document.querySelector(".ItemDetails");
  const itemId = localStorage.getItem("ItemId");

  function fetchItems(searchTerm = "") {
    fetch(`http://localhost:5150/Item/${itemId}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to fetch item details.");
        }
        return response.json();
      })
      .then((items) => {
        itemDetailsSection.innerHTML = ""; // Clear previous item details

        const filteredItems = Array.isArray(items) ? items : [items];

        const finalItems = searchTerm
          ? filteredItems.filter((item) =>
              item.name.toLowerCase().includes(searchTerm.toLowerCase())
            )
          : filteredItems;

        finalItems.forEach((item) => {
          const itemDiv = document.createElement("div");
          itemDiv.className = "item";

          itemDiv.innerHTML = `
            <h3>${item.name}</h3>
            <div id="image-${item.itemId}" class="item-image-container"></div>
            <p><strong>Market:</strong> <span class="company-name"></span></p>
            <p><strong>Price:</strong> ${item.price}</p>
            <p><strong>Number of Items available:</strong><span class="AvailableItems"></span></p>
            <div class="input-container">
              <label for="item-number-${item.itemId}">Enter Number of Items:</label>
              <input type="number" class="item-number" id="item-number-${item.itemId}" min="1" max="10" placeholder="1" onchange="updateTotalCost(${item.price}, ${item.itemId})">
            </div>
            <p><strong>Total Cost:</strong> <span class="totalCost" id="totalCost-${item.itemId}">0</span> JOD</p>
            <button class="BuyNow" type="submit" onclick="BuyAItem(${item.itemId})">Add To Checkout</button>
          `;

          const companyNameSpan = itemDiv.querySelector(".company-name");
          fetchCompanyNameForItem(companyNameSpan, item.registrationId);

          const ItemsAvailable = itemDiv.querySelector(".AvailableItems");
          GetItemsAvailable(ItemsAvailable, item.itemId);

          itemDetailsSection.appendChild(itemDiv);

          const output = document.getElementById(`image-${item.itemId}`);
          if (item.itemId && typeof item.itemId === "number") {
            const imageUrl = `http://localhost:5150/Item/GetImageItem/${item.itemId}`;
            fetch(imageUrl)
              .then((res) => {
                if (!res.ok) {
                  throw new Error(
                    `Failed to fetch image. Status: ${res.status}`
                  );
                }
                return res.blob();
              })
              .then((blob) => {
                const img = document.createElement("img");
                img.src = URL.createObjectURL(blob);
                img.alt = "Item Image";
                img.className = "item-image";
                output.appendChild(img);
              })
              .catch((error) => {
                console.error("Error fetching image:", error);
                output.innerHTML = `<img src="placeholder-image.png" alt="Placeholder Image"> Failed to load image.`;
              });
          } else {
            output.innerHTML = "Item image not available.";
          }
        });
      })
      .catch((error) => {
        console.error("Error fetching item details:", error);
        itemDetailsSection.innerHTML =
          "Error loading item details. Please try again later.";
      });
  }

  fetchItems();
}

function updateTotalCost(price, itemId) {
  const numOfItems = document.getElementById(`item-number-${itemId}`);
  const totalCostElement = document.getElementById(`totalCost-${itemId}`);

  if (!numOfItems || !price) {
    totalCostElement.innerText = "0";
    return;
  }

  const numOfItemsValue = parseInt(numOfItems.value, 10) || 1;

  const totalCost = CalculateTotalCost(price, numOfItemsValue);
  totalCostElement.innerText = totalCost.toFixed(2);
}

function CalculateTotalCost(price, numOfItems) {
  return price * numOfItems;
}

function BuyAItem(itemId) {
  const userId = localStorage.getItem("UserId");
  const numOfItems = document.getElementById(`item-number-${itemId}`).value;
  const totalCost = document.getElementById(`totalCost-${itemId}`).innerText;

  if (!numOfItems || !totalCost) {
    alert("Please fill out the required details.");
    return;
  }
  if (!userId) {
    alert("User ID is missing.");
    return;
  }

  console.log("User ID:", userId);
  console.log("Number of Items:", numOfItems);
  console.log("Total Cost:", totalCost);

  // Redirect to payment page with correct query parameters
  window.location.href = `../HtmlPages/Payment.html?itemId=${itemId}&userId=${userId}&numOfItems=${numOfItems}&totalCost=${totalCost}`;
}

function GetItemsAvailable(element, ItemId) {
  const url = `http://localhost:5150/BuyItem/ItemsAvailable?id=${ItemId}`;

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
      element.innerHTML = data;
    })
    .catch((error) => console.error("Error fetching company name:", error));
}
window.addEventListener("load", () => {
  const sessionExpiration = localStorage.getItem("sessionExpiration");
  if (sessionExpiration && Date.now() > sessionExpiration) {
    localStorage.clear(); // Clear localStorage if session has expired
    alert("Session expired. Please log in again.");
    window.location.href = "../HtmlPages/Login.html"; // Redirect to login page
  }
});