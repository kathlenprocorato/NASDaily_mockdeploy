import { useState, useMemo, useEffect } from "react";
import { Dropdown } from "../../components/Dropdown";
import { calculateSchoolYear, calculateSemester } from "../../components/SySemUtils";
import { AddNASForm } from "../../components/OAS/AddNASForm";
import axios from "axios";
import { AddSuperiorForm } from "../../components/OAS/AddSuperiorForm";

const currentYear = calculateSchoolYear();
const currentSem = calculateSemester();

export const OASManageData = () => {
  const [selectedSY, setSelectedSY] = useState(currentYear);
  // eslint-disable-next-line no-unused-vars
  const [syOptions, setSyOptions] = useState([]);
  const [uniqueYears, setUniqueYears] = useState([]);
  const [selectedSem, setSelectedSem] = useState(currentSem);
  const [fileUploaded, setFileUploaded] = useState(false);
  const sem_options = ["First", "Second", "Summer"];
  // eslint-disable-next-line no-unused-vars
  const [attendanceSummaries, setAttendanceSummaries] = useState([]);

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

  const handleSelectSY = (event) => {
    const value = event.target.value;
    setSelectedSY(value);
  };

  const handleSelectSem = (event) => {
    const value = event.target.value;
    setSelectedSem(value);
  };

  const formatDtrTime = (timeStr) => {
    if (timeStr) {
      const [hours, minutes] = timeStr.split(":");
      const date = new Date();
      date.setHours(hours);
      date.setMinutes(minutes);
      const options = { hour: "numeric", minute: "numeric", hour12: true };
      return date.toLocaleTimeString("en-US", options);
    }
    return null;
  };

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

  const selectedSemValue = getSemesterValue(selectedSem);

  const handleFileUpload = async (e) => {
    const file = e.target.files[0];

    // List of allowed MIME types for Excel files
    const allowedTypes = [
      "application/vnd.ms-excel",
      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    ];

    if (!allowedTypes.includes(file.type)) {
      alert("Invalid file type. Please upload an Excel file.");
      return;
    }

    setFileUploaded(file);
  };

  const updateNasTimekeeping = async () => {
    try {
      const nasResponse = await api.get("/NAS");
      const nasList = nasResponse.data;

      await Promise.all(
        nasList.map(async (nas) => {
          const { id, firstName, lastName, middleName } = nas;
          console.log("NAS Info: ", id, firstName, middleName, lastName);
          await checkAttendanceAndSchedule(id, firstName, lastName, middleName);
        })
      );
    } catch (error) {
      console.error("Error fetching NAS data: ", error);
    }
  };

  //function for checking attendance and schedule for each NAS
  const checkAttendanceAndSchedule = async (nasId, firstName, lastName, middleName) => {
    try {
      const dtrresponse = await api.get(
        `DTR/${selectedSY}/${selectedSemValue}/${firstName}/${lastName}?middleName=${middleName}`
      );

      const dtrdata = dtrresponse.data;

      const scheduleResponse = await api.get(
        `/Schedule/${nasId}/${selectedSY}/${selectedSemValue}`
      );
      const scheduleData = scheduleResponse.data;

      // Calculate the totals for failedToPunch, lateOver10Mins, and lateOver45Mins
      let totalFailedToPunch = 0;
      let totalLateOver10Mins = 0;
      let totalLateOver45Mins = 0;

      const attendanceSummaries = dtrdata.dailyTimeRecords.map((attendance) => {
        const attendanceDate = new Date(attendance.date);

        // Adjust dayOfWeek calculation for Monday - Saturday week
        const dayOfWeek = (attendanceDate.getDay() + 6) % 7;
        const schedule = scheduleData.schedules.find((sched) => sched.dayOfWeek === dayOfWeek);

        //check if there is FTP in dtr
        if (attendance.timeIn === "FTP IN" || attendance.timeOut === "FTP OUT") {
          totalFailedToPunch = totalFailedToPunch + 1; //working
        }

        //check if there is L10 and L45
        const timeIn = new Date("2023-08-14 " + formatDtrTime(attendance.timeIn));
        const schedStartTime = new Date(schedule.startTime);

        // Extract only the hours and minutes from the date objects
        const hoursDiff = timeIn.getHours() - schedStartTime.getHours();
        const minutesDiff = timeIn.getMinutes() - schedStartTime.getMinutes();

        // Convert the time difference to minutes
        const totalMinutesDifference = hoursDiff * 60 + minutesDiff;

        if (totalMinutesDifference > 45) {
          totalLateOver45Mins = totalLateOver45Mins + 1;
        } else if (totalMinutesDifference > 10) {
          totalLateOver10Mins = totalLateOver10Mins + 1;
        }

        console.log("NAS Id: " + nasId + "- NAS timeIn/schedule :" + timeIn, schedStartTime);

        return attendance;
      });

      setAttendanceSummaries(attendanceSummaries);

      // Make the API call to post the summary
      const postResponse = await api.post(
        `/TimekeepingSummary/${nasId}/${selectedSY}/${selectedSemValue}`,
        {
          excused: 0,
          unexcused: 0,
          failedToPunch: totalFailedToPunch,
          lateOver10mins: totalLateOver10Mins,
          lateOver45mins: totalLateOver45Mins,
          makeUpDutyHours: 0,
        }
      );
      console.log(postResponse);
    } catch (error) {
      console.error(error);
    }
  };

  const handleSubmit = async () => {
    if (!fileUploaded) {
      alert("No file uploaded");
      return;
    }
    const formData = new FormData();
    formData.append("file", fileUploaded);

    try {
      const response = await api.post(
        `/DTR/UploadExcel/${selectedSY}/${getSemesterValue(selectedSem)}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );
      if (response.status === 200) {
        alert("File uploaded successfully");
        setFileUploaded(false); // Reset the file input after successful upload
        updateNasTimekeeping();
      } else {
        alert("File upload failed");
      }
    } catch (error) {
      console.error("Error uploading file: ", error);
    }
  };

  return (
    <>
      <div className="flex rounded-lg border border-gray-200 bg-white shadow-md dark:border-gray-700 dark:bg-gray-800 flex-col w-9/10 mb-10">
        <div className="flex h-full flex-col justify-center">
          <ul className="flex-wrap items-center text-lg font-medium rounded-t-lg bg-grey pr-4 py-4 grid grid-cols-2">
            <div className="flex flex-row items-center">
              <div className="flex items-center">
                <div className="flex flex-row justify-start items-center gap-10 ml-5 mr-4">
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
                </div>
              </div>
            </div>
          </ul>
          <div className="px-8 py-4">
            <div className="flex mt-2 mb-8">
              <p className="mr-5 font-bold text-xl">Upload DTR:</p>
              <div>
                <input
                  type="file"
                  id="fileUpload"
                  onChange={handleFileUpload}
                  accept=".xls,.xlsx"
                />
                {fileUploaded ? (
                  <button
                    className="py-2 rounded-md bg-secondary w-24 items-center justify center hover:bg-primary hover:text-white"
                    onClick={handleSubmit}
                  >
                    Submit
                  </button>
                ) : null}
              </div>
            </div>
            <hr className="my-5 border-t-2 border-gray-300" />
            <div>
              <AddNASForm />
              <hr className="my-5 border-t-2 border-gray-300" />
            </div>
            <div>
              <AddSuperiorForm />
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
