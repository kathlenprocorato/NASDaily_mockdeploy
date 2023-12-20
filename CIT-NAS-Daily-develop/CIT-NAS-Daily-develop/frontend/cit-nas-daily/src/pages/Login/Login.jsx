/* eslint-disable no-case-declarations */
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../assets/CSS/Login.css";
import bg from "../../assets/glebuilding.png";
import axios from "axios";

export const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const response = await axios.post("https://localhost:7001/api/Auth/login", {
        username,
        password,
      });

      localStorage.setItem("token", response.data);

      // Create an Axios instance with the Authorization header after login
      const api = axios.create({
        baseURL: "https://localhost:7001/api",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });

      // Get the user's details
      const userResponse = await api.get(`/Users/currentUser`);
      const userRole = userResponse.data.role;

      // Navigate to the respective route based on the user's role
      switch (userRole) {
        case "NAS":
          const nasIdResponse = await api.get(`/NAS/${username}/id`);
          const nasId = nasIdResponse.data;
          navigate(`/nas/${nasId}`);
          break;
        case "OAS":
          navigate(`/oas`);
          break;
        case "Superior":
          const superiorIdResponse = await api.get(`/Superiors/${username}/id`);
          const superiorId = superiorIdResponse.data;
          navigate(`/superior/${superiorId}`);
          break;
        default:
          navigate("Unknown role");
      }
    } catch (error) {
      if ((error.response && error.response.status === 400) || error.response.status === 404) {
        setError("Invalid username or password");
      } else {
        setError("An error occurred");
      }
    }
  };

  return (
    <>
      <div className="center-container">
        <div className="main-container">
          <div className="login-image-container">
            <img src={bg} alt="glebuilding" className="bg-container" />
          </div>
          <div className="text-container">
            <span className="text-login">LOGIN</span>
            <form onSubmit={handleSubmit}>
              <div className="input-container">
                <label htmlFor="username" className="input-label">
                  Enter Username
                </label>
                <input
                  type="text"
                  id="username"
                  className="text-input text-black p-2"
                  value={username}
                  onChange={(e) => {
                    setUsername(e.target.value);
                    setError(""); // Clear error when there are changes in the password
                  }}
                />
                <label htmlFor="password" className="input-label">
                  Enter Password
                </label>
                <input
                  type="password"
                  id="password"
                  className="text-input mb-2 text-black p-2" // Add mb-2 to reduce margin-bottom
                  value={password}
                  onChange={(e) => {
                    setPassword(e.target.value);
                    setError(""); // Clear error when there are changes in the password
                  }}
                />
                <div className="h-5 mb-3 text-white">{error}</div>
                <input type="submit" className="button-submit hover:cursor-pointer" value="Login" />
              </div>
            </form>
          </div>
        </div>
      </div>
    </>
  );
};
