import { useState, useEffect } from "react";
import axios from "axios";

export const ManageUsersTable = () => {
  const [nasData, setNasData] = useState([]);

  const header = [
    "No.",
    "ID Number",
    "Last Name",
    "First Name",
    "Middle Name",
    "Program",
    "Year",
    "Department Assigned",
    "Actions",
  ];

  useEffect(() => {
    const fetchNas = async () => {
      try {
        // Create an Axios instance with the Authorization header
        const api = axios.create({
          baseURL: "https://localhost:7001/api",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        });

        const nasresponse = await api.get(`/NAS/noimg`);
        const nasData = nasresponse.data;

        const nasDataWithOffice = await Promise.all(
          nasData.map(async (nas) => {
            const nasId = nas.id;
            try {
              const officeResponse = await api.get(`Offices/${nasId}/NAS`);
              nas.office = officeResponse.data;
            } catch (officeError) {
              // Handle error when office data is not available
              console.error("Error fetching office data for NAS:", officeError);
              nas.office = { name: "N/A" };
            }
            return nas;
          })
        );
        setNasData(nasDataWithOffice);
      } catch (error) {
        console.error(error);
      }
    };

    fetchNas();
  });

  return (
    <div className="overflow-x-auto">
      <table className="table-auto mx-auto mb-8">
        <thead>
          <tr>
            {header.map((header, index) => (
              <th
                key={index}
                className="border-2 border-black text-white text-center uppercase font-semibold bg-primary px-4 py-2"
                value={header}
              >
                {header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {nasData.map((nas, index) => (
            <tr key={index}>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.id}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.studentIDNo}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "capitalize" }}
              >
                {nas.lastName}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "capitalize" }}
              >
                {nas.firstName}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "capitalize" }}
              >
                {nas.middleName}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "uppercase" }}
              >
                {nas.course}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.yearLevel}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "uppercase" }}
              >
                {nas.office ? nas.office.name : "N/A"}{" "}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                <div className="flex gap-4">
                  <div className="">
                    <button className="bg-secondary hover:bg-primary hover:text-white focus:outline-none rounded-lg text-sm px-5 py-2.5 ">
                      Edit
                    </button>
                  </div>

                  <div>
                    <button className="bg-secondary hover:bg-primary hover:text-white focus:outline-none rounded-lg text-sm px-5 py-2.5">
                      Delete
                    </button>
                  </div>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
