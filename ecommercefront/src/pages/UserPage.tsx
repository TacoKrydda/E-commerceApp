import React from "react";
import { useParams, useNavigate } from "react-router-dom";

interface UserPageProps {
  logOut: () => void;
}

export const UserPage: React.FC<UserPageProps> = ({ logOut }) => {
  const { id } = useParams();
  const navigate = useNavigate();

  const handleLogOut = () => {
    logOut();
    navigate(-1);
  };
  return (
    <div>
      <h1>Profile</h1>
      <p>User: </p>
      <p>Address: </p>
      <button onClick={handleLogOut}>Logga ut</button>
    </div>
  );
};
