import React, { useState } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import ScrollToTop from "./components/layout/ScrollToTop";
import { Navigation } from "./components/layout/Navigation";
import { LoginPage } from "./pages/LoginPage";
import { UserPage } from "./pages/UserPage";

import ProductList from "./pages/ProductList";
import ProductDetail from "./pages/ProductDetail";
import Home from "./pages/Home";

function App() {
  const [isLogin, setIsLogin] = useState(false);

  const [showSidebar, setShowSidebar] = useState<boolean>(false);

  const handleShowSidebar = () => {
    setShowSidebar(!showSidebar);
  };
  console.log(showSidebar);

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
        handleCategoryChange={handleShowSidebar}
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
        <Route path="/product/:id" element={<ProductDetail />} />
      </Routes>
    </Router>
  );
}

export default App;
