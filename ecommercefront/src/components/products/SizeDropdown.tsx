import { useState } from "react";
import Styles from "./SizeDropdown.module.css";
interface SizeDropdownProps {
  size: string[];
}

const SizeDropdown: React.FC<SizeDropdownProps> = ({ size }) => {
  const [isSize, setIsSize] = useState(false);

  return (
    <div className={Styles.sizeOptions}>
      <button onClick={() => setIsSize(!isSize)}>Storlek</button>

      <div className={`${Styles.sizePopUp} ${isSize ? Styles.open : ""}`}>
        <p onClick={() => setIsSize(!isSize)}>Close</p>
        {size.map((size, index) => (
          <p key={index}>{size}</p>
        ))}
      </div>
    </div>
  );
};

export default SizeDropdown;
