/* eslint-disable no-case-declarations */
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Avatar } from "flowbite-react";
import { useParams } from "react-router-dom";
import placeholder from "../placeholders/user.png";
import axios from "axios";

export const Header = () => {
  const navigate = useNavigate();
  const { nasId } = useParams();
  const { superiorId } = useParams();
  const [user, setUser] = useState({});

  const handleLogout = () => {
    // Clear the authentication token from local storage
    localStorage.removeItem("token");
    // Redirect the user to the login page
    navigate("/");
  };

  useEffect(() => {
    const fetchUser = async () => {
      try {
        // Create an Axios instance with the Authorization header
        const api = axios.create({
          baseURL: "https://localhost:7001/api",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        });

        const userResponse = await api.get(`/Users/currentUser`);
        const userRole = userResponse.data.role;

        switch (userRole) {
          case "NAS":
            const nasResponse = await api.get(`/NAS/${nasId}`);
            setUser(nasResponse.data);
            break;
          case "OAS": //PLACEHOLDER
            const oasResponse = await api.get(`/OAS/1`);
            setUser(oasResponse.data);
            break;
          case "Superior":
            const superiorResponse = await api.get(`/Superiors/${superiorId}`);
            setUser(superiorResponse.data);
            break;
          default:
            console.log("Unknown role");
        }
      } catch (error) {
        console.error(error);
      }
    };

    fetchUser();
  }, [nasId, superiorId]);

  return (
    <>
      <div className="flex mt-5 ml-10 items-center justify-between">
        <div className="flex items-center">
          <div className="mr-4">
            {user.image ? (
              <Avatar
                alt={user.fullName}
                img={`data:image/png;base64,${user.image}`}
                rounded
              />
            ) : (
              <Avatar alt={user.fullName} img={placeholder} rounded />
            )}
          </div>
          <div className="text-base" style={{ textTransform: "capitalize" }}>
            {user.fullName}
          </div>
        </div>
        <button className="text-base text-red-500 mr-10" onClick={handleLogout}>
          Logout
        </button>
      </div>
      <hr className="my-5 border-t-2 border-gray-300 ml-7 mr-7" />
    </>
  );
};
