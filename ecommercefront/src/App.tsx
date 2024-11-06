import React from "react";
import "./App.css";
import ProductList from "./pages/ProductList";
import ProductDetail from "./pages/ProductDetail";

function App() {
  return (
    <div>
      <ProductList />
      <ProductDetail productId={1} />
    </div>
  );
}

export default App;
