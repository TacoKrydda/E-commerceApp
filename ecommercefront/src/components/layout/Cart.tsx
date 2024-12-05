import React from "react";
import Styles from "./Cart.module.css";
import { Link } from "react-router-dom";

export interface CartItem {
  id: number;
  name: string;
  color: string;
  size: string;
  price: number;
  quantity: number;
  stock: number;
}

interface CartProps {
  cartItems: CartItem[];
  onUpdateQuantity: (id: number, quantity: number) => void;
  onRemoveItem: (id: number) => void;
  closeCart: () => void;
}

const Cart: React.FC<CartProps> = ({
  cartItems,
  onUpdateQuantity,
  onRemoveItem,
  closeCart,
}) => {
  const total = cartItems.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );
  const shipping = total > 0 ? 100 : 0; // Fraktkostnad om varukorgen är tom

  return (
    <div className={Styles.cartContainer}>
      <h2>Kundvagn</h2>
      <table className={Styles.cartTable}>
        <thead>
          <tr>
            <th>Produkt</th>
            <th>Färg</th>
            <th>Storlek</th>
            <th>Antal</th>
            <th>Pris</th>
          </tr>
        </thead>
        <tbody>
          {cartItems.map((item) => (
            <tr key={item.id}>
              <td>{item.name}</td>
              <td>{item.color}</td>
              <td>{item.size}</td>
              <td>
                <button
                  onClick={() => onUpdateQuantity(item.id, item.quantity - 1)}
                  disabled={item.quantity <= 1}
                >
                  -
                </button>
                {item.quantity}
                <button
                  onClick={() => onUpdateQuantity(item.id, item.quantity + 1)}
                  disabled={item.quantity >= item.stock}
                >
                  +
                </button>
              </td>
              <td>{item.price} kr</td>
              <td>
                <button onClick={() => onRemoveItem(item.id)}>🗑️</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className={Styles.cartSummary}>
        <p>Frakt: {shipping} kr</p>
        <p>Summa: {total + shipping} kr</p>
      </div>
      {cartItems.length > 0 && (
        <Link to="/cart">
          <button className={Styles.checkoutButton} onClick={closeCart}>
            Till kassan
          </button>
        </Link>
      )}
    </div>
  );
};

export default Cart;
