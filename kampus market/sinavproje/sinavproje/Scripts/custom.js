// Initialize the cart count
let cartCount = 0;

// Function to update the cart counter element
function updateCartCounter() {
    const counterElement = document.getElementById("cart-counter");
    counterElement.textContent = cartCount.toString();
}

// Example usage: Add an item to the cart
function addItemToCart() {
    // Your logic to add an item to the cart

    // Increment the cart count
    cartCount++;

    // Update the cart counter element
    updateCartCounter();
}
