import { useEffect, useState } from "react";

const Profile = () => {
  const [userData, setUserData] = useState(null);

  useEffect(() => {
    fetch("http://your-backend-api/profile", {
      method: "GET",
      credentials: "include",
    })
      .then((response) => response.json())
      .then((data) => setUserData(data))
      .catch((error) => console.error("Error fetching profile:", error));
  }, []);

  if (!userData) return <div>Loading...</div>;

  return (
    <div>
      <h1>Welcome, {userData}!</h1>
      <p>Email: {userData}</p>
    </div>
  );
};

export default Profile;
