"use client";
import { Card, Avatar } from "flowbite-react";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useParams } from "react-router-dom";
import axios from "axios";

export const SuperiorList = () => {
  const { superiorId } = useParams();
  const [nasList, setNasList] = useState([]);
  const [office, setOffice] = useState({});
  const [selectedSY, setSelectedSY] = useState(2324);
  const [selectedSem, setSelectedSem] = useState("First");
  const sy_options = ["2324", "2223", "2122", "2021"];
  const sem_options = ["First", "Second", "Summer"];
  const navigate = useNavigate();

  const handleSelectSY = (event) => {
    const value = event.target.value;
    setSelectedSY(value);
  };

  const handleSelectSem = (event) => {
    const value = event.target.value;
    setSelectedSem(value);
  };

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
    const fetchNasList = async () => {
      try {
        const api = axios.create({
          baseURL: "https://localhost:7001/api",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        });

        const officeresponse = await api.get(`/Offices/${superiorId}`);
        const officeData = officeresponse.data;

        const response = await api.get(
          `/NAS/${officeData.id}/${selectedSY}/${getSemesterValue(selectedSem)}`
        );

        setOffice(officeData);
        setNasList(response.data.nasEntries);
      } catch (error) {
        console.error(error);
      }
    };

    fetchNasList();
  }, [superiorId, selectedSY, selectedSem]);

  const handleNasClick = (nasId) => {
    navigate(`/superior/${superiorId}/evaluation/${nasId}`);
  };

  return (
    <div>
      <div className="flex flex-row justify-start items-center gap-10 mt-5 mb-8 ml-10">
        <div className="flex flex-row gap-2 items-center">
          <div className="mr-2">SY:</div>
          <select
            id="sy"
            name="sy"
            value={selectedSY}
            onChange={handleSelectSY}
            className=" w-full text-base border rounded-md"
          >
            {Array.isArray(sy_options) &&
              sy_options.map((sy, index) => (
                <option key={index} value={sy}>
                  {sy}
                </option>
              ))}
          </select>
        </div>
        <div className="flex flex-row gap-2 items-center">
          <div className="mr-2">SEMESTER:</div>
          <select
            id="sem"
            name="sem"
            value={selectedSem}
            onChange={handleSelectSem}
            className=" w-full text-base border rounded-md"
          >
            {sem_options.map((sem, index) => (
              <option key={index} value={sem}>
                {sem}
              </option>
            ))}
          </select>
        </div>
      </div>
      <div className="flex justify-center items-center">
        <Card className="w-3/5 m-5">
          <h5 className="text-2xl font-bold tracking-tight">
            <p>{office.name}</p>
          </h5>
          <div className="grid gap-3">
            {nasList.map((nas) => (
              <button
                key={nas.id}
                className="border-solid border-2 p-3 flex items-center hover:bg-grey"
                onClick={() => handleNasClick(nas.id)}
              >
                <Avatar rounded />
                <span className="ml-5" style={{ textTransform: "capitalize" }}>
                  {nas.firstName} {nas.lastName}
                </span>
              </button>
            ))}
          </div>
        </Card>
      </div>
    </div>
  );
};

export default SuperiorList;
