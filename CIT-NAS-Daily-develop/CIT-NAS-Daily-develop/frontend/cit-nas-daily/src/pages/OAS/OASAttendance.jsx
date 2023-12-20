import { useState, useEffect, useMemo } from "react";
import { MonthlySummary } from "../../components/MonthlySummary";
import { WeeklyAttendance } from "../../components/OAS/WeeklyAttendance";
import { Button } from "flowbite-react";
import { HiOutlineArrowLeft, HiOutlineArrowRight } from "react-icons/hi";
import { Dropdown } from "../../components/Dropdown";
import { calculateSchoolYear, calculateSemester } from "../../components/SySemUtils";
import axios from "axios";

const currentSchoolYear = calculateSchoolYear();
const currentSemester = calculateSemester();
const first_sem = ["All", "August", "September", "October", "November", "December"];
const second_sem = ["All", "January", "February", "March", "April", "May", "June"];
const summer = ["All", "June", "July", "August"];

export const OASAttendance = () => {
  const [nasId, setNasId] = useState("");
  const [firstName, setFirstname] = useState("");
  const [lastName, setLastname] = useState("");
  const [middleName, setMiddlename] = useState("");
  const [office, setOffice] = useState("");
  const [timekeepingSummaries, setTimekeepingSummaries] = useState([]);
  const [nasArray, setNasArray] = useState([]);
  const [searchInput, setSearchInput] = useState("");
  const [currentIndex, setCurrentIndex] = useState(0);
  const [maxNasId, setMaxNasId] = useState(1);
  const [selectedSem, setSelectedSem] = useState(currentSemester);
  const [monthOptions, setMonthOptions] = useState(first_sem);
  const [selectedMonth, setSelectedMonth] = useState("All");
  const [selectedMonthIndex, setSelectedMonthIndex] = useState(-1);
  const [selectedSY, setSelectedSY] = useState(currentSchoolYear);
  // eslint-disable-next-line no-unused-vars
  const [syOptions, setSyOptions] = useState([]);
  const [uniqueYears, setUniqueYears] = useState([]);
  const sem_options = ["First", "Second", "Summer"];

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

  const getSemesterValue = useMemo(() => {
    return (sem) => {
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
    };
  }, []);

  const handleSearchChange = (event) => {
    setSearchInput(event.target.value);
  };

  useEffect(() => {
    const fetchNasData = async () => {
      try {
        const response = await api.get(`/NAS/${selectedSY}/${getSemesterValue(selectedSem)}/noimg`);
        setNasArray(response.data);

        setNasId(response.data[currentIndex].id);
        setFirstname(response.data[currentIndex].firstName);
        setMiddlename(response.data[currentIndex].middleName);
        setLastname(response.data[currentIndex].lastName);
        setOffice(response.data[currentIndex].officeName);
        setMaxNasId(response.data.length - 1);
      } catch (error) {
        console.error(error);
        if (error.response.status === 404) {
          setNasArray([]);
          setMaxNasId(0);
          setFirstname(null);
          setMiddlename(null);
          setLastname(null);
          setOffice(null);
        }
      }
    };

    fetchNasData();
  }, [api, selectedSY, selectedSem, currentIndex, getSemesterValue]);

  //getting school year from the /NAS/sysem
  useEffect(() => {
    const fetchSchoolYearSemesterOptions = async () => {
      try {
        const response = await api.get("/NAS/sysem");
        setSyOptions(response.data);

        // Extract unique years from syOptions
        const years = [...new Set(response.data.map((option) => option.year))];
        setUniqueYears(years);
      } catch (error) {
        console.error(error);
      }
    };
    fetchSchoolYearSemesterOptions();
  }, [api]);

  useEffect(() => {
    let selectedMonthIndex;
    switch (selectedSem) {
      case "First":
        setMonthOptions(first_sem);
        selectedMonthIndex = first_sem.indexOf(selectedMonth) + 6;
        if (selectedMonth === "All") {
          selectedMonthIndex = -1;
        }
        break;
      case "Second":
        setMonthOptions(second_sem);
        selectedMonthIndex = second_sem.indexOf(selectedMonth);
        if (selectedMonth === "All") {
          selectedMonthIndex = -2;
        }
        break;
      case "Summer":
        setMonthOptions(summer);
        selectedMonthIndex = summer.indexOf(selectedMonth) + 4;
        if (selectedMonth === "All") {
          selectedMonthIndex = -3;
        }
        break;
      default:
        break;
    }

    setSelectedMonthIndex(selectedMonthIndex);
  }, [selectedSY, selectedSem, selectedMonth]);

  useEffect(() => {
    const fetchTimekeepingSummary = async () => {
      try {
        const timekeepingresponse = await api.get(
          `/TimekeepingSummary/${nasArray[currentIndex].id}/${selectedSY}/${getSemesterValue(
            selectedSem
          )}`
        );
        const timekeepingdata = timekeepingresponse.data;
        setTimekeepingSummaries(timekeepingdata);
      } catch (error) {
        console.error(error);
        if (error.response.status === 404) {
          setTimekeepingSummaries([]);
        }
      }
    };

    fetchTimekeepingSummary();
  }, [api, nasArray, currentIndex, selectedSY, selectedSem, getSemesterValue]);

  useEffect(() => {
    if (searchInput.trim() !== "") {
      const results = nasArray.filter((data) =>
        data.fullName.toLowerCase().includes(searchInput.toLowerCase())
      );
      if (results[0]) {
        setCurrentIndex(nasArray.indexOf(results[0]));
      }
    }
  }, [searchInput, nasArray]);

  const handleSelectSY = (event) => {
    const value = event.target.value;
    setSelectedSY(value);
    setCurrentIndex(0);
    setMaxNasId(0);
  };

  const handleSelectSem = (event) => {
    const value = event.target.value;
    setSelectedSem(value);
    setCurrentIndex(0);
    setMaxNasId(0);
    setSelectedMonth("All");
  };

  const handleSelectedMonth = (event) => {
    const value = event.target.value;
    if (value === "All") {
      setSelectedMonth("All");
    } else {
      setSelectedMonth(value);
    }
  };

  return (
    <>
      <div className="flex rounded-lg border border-gray-200 bg-white shadow-md dark:border-gray-700 dark:bg-gray-800 flex-col w-9/10 mb-10">
        <div className="flex h-full flex-col justify-center">
          <ul className="flex-wrap items-center text-lg font-medium rounded-t-lg bg-grey pr-4 py-4 grid grid-cols-2">
            <div className={`flex items-center w-auto ${currentIndex === 0 ? "ml-9" : ""}`}>
              <div>
                {currentIndex != 0 ? (
                  <Button
                    className="text-black"
                    onClick={() => {
                      setCurrentIndex((currentIndex - 1) % nasArray.length);
                    }}
                  >
                    <HiOutlineArrowLeft className="h-6 w-6" />
                  </Button>
                ) : (
                  ""
                )}
              </div>
              <div className="flex flex-row justify-start items-center gap-10">
                <div className="flex flex-row gap-2 items-center">
                  <Dropdown
                    label="SY"
                    options={uniqueYears}
                    selectedValue={selectedSY}
                    onChange={(e) => handleSelectSY(e)}
                  />
                </div>
                <div className="flex flex-row gap-2 items-center">
                  <Dropdown
                    label="Semester"
                    options={sem_options}
                    selectedValue={selectedSem}
                    onChange={(e) => handleSelectSem(e)}
                  />
                </div>
                <div className="flex flex-row gap-2 items-center">
                  <Dropdown
                    label="Month"
                    options={monthOptions}
                    selectedValue={selectedMonth}
                    onChange={(e) => handleSelectedMonth(e)}
                  />
                </div>
              </div>
            </div>
            <li className="flex justify-end">
              <div className="flex ">
                <div className="relative w-auto">
                  <input
                    type="search"
                    className="block p-2.5 w-full z-20 text-sm text-gray-900 bg-gray-50 rounded border"
                    placeholder="Search NAS..."
                    value={searchInput}
                    onChange={handleSearchChange}
                    required
                  />
                  <button
                    type="submit"
                    className="absolute top-0 right-0 p-2.5 text-sm font-medium h-full"
                  >
                    <svg
                      className="w-4 h-4"
                      aria-hidden="true"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 20 20"
                    >
                      <path
                        stroke="currentColor"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth="2"
                        d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
                      />
                    </svg>
                  </button>
                </div>
              </div>
              {currentIndex != maxNasId ? (
                <Button
                  className="text-black"
                  onClick={() => {
                    setCurrentIndex((currentIndex + 1) % nasArray.length);
                  }}
                >
                  <HiOutlineArrowRight className="h-6 w-6" />
                </Button>
              ) : (
                ""
              )}
            </li>
          </ul>
          <div className="px-9 py-4">
            <div className="flex gap-10 mb-7 text-lg">
              <div className="font-bold" style={{ textTransform: "uppercase" }}>
                NAS NAME: {lastName}, {firstName} {middleName}
              </div>
              <div>
                <p className="font-bold text-center" style={{ textTransform: "uppercase" }}>
                  DEPT/OFFICE: {office}
                </p>
              </div>
            </div>
            <div className="flex flex-col justify-center items-center gap-4">
              <p className="text-xl font-bold text-primary">MONTHLY SUMMARY OF ABSENCES/LATE</p>
              <MonthlySummary timekeepingSummaries={timekeepingSummaries} />
              <p className="text-xl font-bold text-primary">WEEKLY ATTENDANCE</p>
              <WeeklyAttendance
                nasId={nasId}
                firstName={firstName}
                lastName={lastName}
                middleName={middleName}
                selectedMonth={selectedMonthIndex}
                selectedSem={selectedSem}
                selectedSY={selectedSY}
              />
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
