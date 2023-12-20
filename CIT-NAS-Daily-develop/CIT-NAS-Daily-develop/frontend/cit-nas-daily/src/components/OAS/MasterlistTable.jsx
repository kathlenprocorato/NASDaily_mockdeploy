import { useState, useEffect, useMemo } from "react";
import PropTypes from "prop-types";
import axios from "axios";

export const MasterlistTable = ({ searchInput, selectedSY, selectedSem }) => {
  const [nasData, setNasData] = useState([]);
  const [filteredNASData, setFilteredNASData] = useState([]);

  const api = useMemo(
    () =>
      axios.create({
        baseURL: "https://localhost:7001/api",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }),
    []
  );

  const header = [
    "No.",
    "ID Number",
    "Last Name",
    "First Name",
    "Middle Name",
    "Gender",
    "Birthdate",
    "Program",
    "Year",
    "No. of Units Allowed",
    "Date Started",
    "Department Assigned",
    "EA",
    "UEA",
    "L>10 mins.",
    "L>45 mins.",
    "FTP",
    "FOR MAKE UP",
    "OT",
    "REMARKS",
  ];

  function getSemesterValue(sem) {
    switch (sem) {
      case "First":
        return 0;
      case "Second":
        return 1;
      case "Summer":
        return 2;
      default:
        return "Invalid semester";
    }
  }

  useEffect(() => {
    const fetchNas = async () => {
      try {
        const nasresponse = await api.get(
          `/NAS/${selectedSY}/${getSemesterValue(selectedSem)}/noimg`
        );
        const nasData = nasresponse.data;

        const nasDataWithOffice = await Promise.all(
          nasData.map(async (nas) => {
            const nasId = nas.id;

            try {
              const [officeResponse, timekeepingresponse] = await Promise.all([
                api.get(`Offices/${nasId}/NAS`),
                api.get(
                  `/TimekeepingSummary/${nasId}/${selectedSY}/${getSemesterValue(selectedSem)}`
                ),
              ]);
              nas.office = officeResponse.data;
              nas.timekeeping = timekeepingresponse.data;
            } catch (error) {
              console.error("Error fetching data for NAS:", error);
              nas.office = { name: "N/A" };
              nas.timekeeping = [];
            }
            return nas;
          })
        );
        setNasData(nasDataWithOffice);
      } catch (error) {
        console.error(error);
        if (error.response.status === 404) {
          setNasData([]);
        }
      }
    };

    fetchNas();
  }, [selectedSY, selectedSem, api]);

  useEffect(() => {
    const filteredData = nasData.filter(
      (nas) =>
        nas.lastName.toLowerCase().includes(searchInput.toLowerCase()) ||
        nas.firstName.toLowerCase().includes(searchInput.toLowerCase()) ||
        nas.middleName.toLowerCase().includes(searchInput.toLowerCase())
    );
    setFilteredNASData(filteredData);
  }, [searchInput, nasData]);

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
          {filteredNASData.map((nas, index) => (
            <tr key={index}>
              <td className="border-2 border-black text-center px-4 py-2">{nas.id}</td>
              <td className="border-2 border-black text-center px-4 py-2">{nas.studentIDNo}</td>
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
                style={{ textTransform: "capitalize" }}
              >
                {nas.gender}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {new Date(nas.birthDate).toLocaleDateString()}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "uppercase" }}
              >
                {nas.course}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">{nas.yearLevel}</td>
              <td className="border-2 border-black text-center px-4 py-2">{nas.unitsAllowed}</td>
              <td className="border-2 border-black text-center px-4 py-2">
                {new Date(nas.dateStarted).toLocaleDateString()}
              </td>
              <td
                className="border-2 border-black text-center px-4 py-2"
                style={{ textTransform: "uppercase" }}
              >
                {nas.office ? nas.office.name : "N/A"}{" "}
                {/* Display the office name or "N/A" if not available */}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.timekeeping ? nas.timekeeping.excused : "NR"}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.timekeeping ? nas.timekeeping.unexcused : "NR"}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.timekeeping ? nas.timekeeping.lateOver10Mins : "NR"}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.timekeeping ? nas.timekeeping.lateOver45Mins : "NR"}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.timekeeping ? nas.timekeeping.failedToPunch : "NR"}
              </td>
              <td className="border-2 border-black text-center px-4 py-2">
                {nas.timekeeping ? nas.timekeeping.makeUpDutyHours : "NR"}
              </td>
              <td className="border-2 border-black text-center px-4 py-2"> </td>
              <td className="border-2 border-black text-center px-4 py-2"> </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

MasterlistTable.propTypes = {
  searchInput: PropTypes.string.isRequired,
  selectedSY: PropTypes.string.isRequired,
  selectedSem: PropTypes.string.isRequired,
};
