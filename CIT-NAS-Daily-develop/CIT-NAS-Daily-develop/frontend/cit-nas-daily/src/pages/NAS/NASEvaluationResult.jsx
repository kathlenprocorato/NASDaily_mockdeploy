"use client";
import { useState, useEffect, useMemo } from "react";
import { useParams } from "react-router-dom";
import { Dropdown } from "../../components/Dropdown.jsx";
import { calculateSchoolYear, calculateSemester } from "../../components/SySemUtils.js";
import axios from "axios";

const currentYear = calculateSchoolYear();
const currentSem = calculateSemester();

export const NASEvaluationResult = () => {
  const [selectedSY, setSelectedSY] = useState(currentYear);
  // eslint-disable-next-line no-unused-vars
  const [syOptions, setSyOptions] = useState([]);
  const [uniqueYears, setUniqueYears] = useState([]);
  const [selectedSem, setSelectedSem] = useState(currentSem);
  const [fileUploaded, setFileUploaded] = useState(false);
  const [submitted, setSubmitted] = useState(false);
  const [summaryEvaluation, setSummaryEvaluation] = useState({});
  const { nasId } = useParams();
  const sem_options = ["First", "Second", "Summer"];

  const handleSelectSY = (event) => {
    const value = event.target.value;
    setSelectedSY(value);
  };

  const handleSelectSem = (event) => {
    const value = event.target.value;
    setSelectedSem(value);
  };

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

  const handleFileUpload = async (e) => {
    const file = e.target.files[0];
    if (file) {
      const fileSizeInKB = file.size / 1024;
      const maxSizeAllowed = 500;

      if (fileSizeInKB > maxSizeAllowed) {
        alert("File size must be less than 500KB.");
        return;
      } else {
        setFileUploaded(file);
      }
    }
    if (fileUploaded) {
      setFileUploaded(file);
      localStorage.setItem("fileUploaded", JSON.stringify(file)); // Save fileUploaded to localStorage
    }
  };

  const handleSubmit = async () => {
    const semNum = sem_options.indexOf(selectedSem);
    console.log("NAS ID: " + nasId);
    console.log("selectedSY: " + selectedSY);
    console.log("semNum: " + semNum);

    if (fileUploaded) {
      try {
        const formData = new FormData();
        formData.append("file", fileUploaded);

        const response = await api.put(`/NAS/grades/${nasId}/${selectedSY}/${semNum}`, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });

        if (response.status === 200) {
          const responseData = response.data;
          setFileUploaded(responseData.grade);
          setSubmitted(true);
          alert("Grade uploaded successfully");
        } else {
          console.error("Grade upload failed");
        }
      } catch (error) {
        console.error("Error uploading grades:", error);
      }
      setSubmitted(true);
      localStorage.setItem("submitted", JSON.stringify(true)); // Save submitted to localStorage
    }
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
    const fetchEvalResult = async () => {
      try {
        const summaryEvaluationResponse = await api.get(
          `SummaryEvaluation/${selectedSY}/${getSemesterValue(selectedSem)}/${nasId}`
        );
        const summaryEvaluationData = summaryEvaluationResponse.data;
        setSummaryEvaluation(summaryEvaluationData);
        console.log(summaryEvaluationData);
      } catch (error) {
        console.error(error);
        setSummaryEvaluation({});
      }
    };
    fetchEvalResult();
  }, [selectedSY, selectedSem, nasId, api]);

  console.log("SUMMARY EVAL", summaryEvaluation);

  return (
    <div className="justify-center w-full h-full items-center border border-solid rounded-lg">
      <div className="m-3 mb-10">
        <div className="m-2">
          <div className="flex items-center justify-center text-xl font-bold">
            Evaluation Result
          </div>
          <div className="flex flex-row justify-start items-center gap-10 mt-2 mb-8">
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
                label="SEMESTER"
                options={sem_options}
                selectedValue={selectedSem}
                onChange={(e) => handleSelectSem(e)}
              />
            </div>
          </div>
          <hr className="my-5 border-t-2 border-gray-300 mx-2" />
          <div className="flex gap-44 ml-2">
            <div className="flex flex-col mt-2">
              <div className="flex flex-row gap-28 justify-start items-center text-lg">
                <div>SUPERIOR EVALUATION:</div>
                <div className="font-bold">{summaryEvaluation.superiorOverallRating}</div>
              </div>
              <div className="flex flex-row gap-28 justify-start items-center text-lg mt-2">
                <div>TIMEKEEPING STATUS:</div>
                <div className="font-bold">{summaryEvaluation.timekeepingStatus}</div>
              </div>
              <div className="flex flex-row gap-16 justify-start items-center text-lg mt-2">
                <div>ALLOWED FOR ENROLLMENT:</div>
                <div
                  className={`font-bold ${
                    summaryEvaluation.enrollmentAllowed ? "text-green" : "text-red"
                  }`}
                >
                  {summaryEvaluation.enrollmentAllowed ? "YES" : "NO"}
                </div>
              </div>
            </div>
            <div className="flex flex-col mt-2">
              <div className="flex flex-row gap-16 justify-start items-center text-lg">
                <div>NUMBER OF UNITS ALLOWED:</div>
                <div className="font-bold">{summaryEvaluation.unitsAllowed}</div>
              </div>
              <div className="flex flex-row gap-36 justify-start items-center text-lg mt-2">
                <div>GRADE STATUS:</div>
                {submitted ? (
                  summaryEvaluation.responded === null ||
                  summaryEvaluation.responded === undefined ? (
                    <span className="font-bold text-secondary">PENDING</span>
                  ) : summaryEvaluation.allCoursesPassed ? (
                    <span className="font-bold text-green">ALL PASSED</span>
                  ) : (
                    <span className="font-bold text-red">FAILED A COURSE</span>
                  )
                ) : Object.keys(summaryEvaluation).length ===
                  0 ? null : summaryEvaluation.academicPerformance === null ||
                  summaryEvaluation.academicPerformance === undefined ? (
                  <div className="text-sm">
                    <input
                      type="file"
                      id="fileUpload"
                      accept="jpeg, .jpg, .png"
                      onChange={handleFileUpload}
                    />
                    {fileUploaded ? (
                      submitted ? null : (
                        <button
                          className="py-2 rounded-md bg-secondary w-24 items-center justify center hover:bg-primary hover:text-white"
                          onClick={handleSubmit}
                        >
                          Submit
                        </button>
                      )
                    ) : null}
                  </div>
                ) : summaryEvaluation.responded === null ||
                  summaryEvaluation.responded === undefined ? (
                  <span className="font-bold text-secondary">PENDING</span>
                ) : summaryEvaluation.allCoursesPassed ? (
                  <span className="font-bold text-green">ALL PASSED</span>
                ) : (
                  <span className="font-bold text-red">FAILED A COURSE</span>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
