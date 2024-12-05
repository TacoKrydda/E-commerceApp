import React, { useEffect, useState } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import ScrollToTop from "./components/layout/ScrollToTop";
import { Navigation } from "./components/layout/Navigation";
import { LoginPage } from "./pages/LoginPage";
import { UserPage } from "./pages/UserPage";
import Cart, { CartItem } from "./components/layout/Cart";

import ProductList from "./pages/ProductList";
import ProductDetail from "./pages/ProductDetail";
import Home from "./pages/Home";
import { CartPage } from "./pages/CartPage";

function App() {
  const [isLogin, setIsLogin] = useState(false);

  const [showSidebar, setShowSidebar] = useState<boolean>(false);
  const [cartItems, setCartItems] = useState<CartItem[]>(
    () => JSON.parse(localStorage.getItem("cartItems") || "[]") // Ladda från localStorage
  );

  // Ladda kundvagn från localStorage när appen startar
  useEffect(() => {
    const savedCart = localStorage.getItem("cartItems");
    if (savedCart) {
      setCartItems(JSON.parse(savedCart));
    }
  }, []);

  // Spara kundvagn till localStorage varje gång cartItems ändras
  useEffect(() => {
    localStorage.setItem("cartItems", JSON.stringify(cartItems));
  }, [cartItems]);

  console.log(cartItems);

  const handleAddToCart = (item: CartItem) => {
    setCartItems((prevItems) => {
      const existingItem = prevItems.find(
        (cartItem) => cartItem.id === item.id
      );
      if (existingItem) {
        return prevItems.map((cartItem) =>
          cartItem.id === item.id
            ? { ...cartItem, quantity: cartItem.quantity + item.quantity }
            : cartItem
        );
      }
      return [...prevItems, item];
    });
  };

  const handleUpdateQuantity = (id: number, quantity: number) => {
    setCartItems((prevItems) =>
      prevItems.map((item) => (item.id === id ? { ...item, quantity } : item))
    );
  };

  const handleRemoveItem = (id: number) => {
    setCartItems((prevItems) => prevItems.filter((item) => item.id !== id));
  };

  const handleEmptyCart = () => {
    setCartItems([]);
  };

  const handleShowSidebar = () => {
    setShowSidebar(!showSidebar);
  };

  const handleLogin = () => {
    setIsLogin(!isLogin);
  };
  return (
    <Router>
      <ScrollToTop />
      <Navigation
        isLogin={isLogin}
        handleShowSidebar={handleShowSidebar}
        showSidebar={showSidebar}
        cartItems={cartItems}
        onUpdateQuantity={handleUpdateQuantity}
        onRemoveItem={handleRemoveItem}
      />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route
          path="/products"
          element={
            <ProductList
              handleShowSidebar={handleShowSidebar}
              showSidebar={showSidebar}
            />
          }
        />
        <Route path="/login" element={<LoginPage login={handleLogin} />} />
        <Route path="/user" element={<UserPage logOut={handleLogin} />} />
        <Route
          path="/cart"
          element={
            <CartPage
              cartItems={cartItems}
              onUpdateQuantity={handleUpdateQuantity}
              onRemoveItem={handleRemoveItem}
              onEmptyCart={handleEmptyCart}
            />
          }
        />
        <Route
          path="/product/:id"
          element={
            <ProductDetail handleAddToCart={(item) => handleAddToCart(item)} />
          }
        />
      </Routes>
    </Router>
  );
}

export default App;
