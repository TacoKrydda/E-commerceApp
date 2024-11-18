import React from "react";
import Styles from "./LoginPage.module.css";

interface loginProps {
  login: () => void;
}

export const LoginPage: React.FC<loginProps> = ({ login }) => {
  return (
    <div className={Styles.loginContainer}>
      <h2>Loga in eller registrerar dig</h2>
      <div>
        <input placeholder="E-post" />
        <button disabled onClick={login}>
          Fortsätt
        </button>
      </div>
      <div className={Styles.infoSection}>
        <p>
          När du skapar ett konto godkänner du våra användarvillkor. Läs om hur
          vi hanterar dina uppgifter i vårt Integritetspolicy.
        </p>
      </div>
      <div>
        <button disabled>Google</button>
        <button disabled>Apple</button>
        <button disabled>Facebook</button>
      </div>
    </div>
  );
};
