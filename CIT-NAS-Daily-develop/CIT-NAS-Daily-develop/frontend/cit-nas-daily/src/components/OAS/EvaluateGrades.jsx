"use client";
import PropTypes from "prop-types";
import { useState, useMemo } from "react";
import { ShowGrades } from "./ShowGrades";
import axios from "axios";

export const EvaluateGrades = ({
  show,
  close,
  grade,
  nasId,
  selectedSY,
  selectedSem,
  onEvaluationSubmit,
}) => {
  const [isViewingShowGrades, setIsViewingShowGrades] = useState(false);
  const [numCoursesFailed, setNumCoursesFailed] = useState(0);
  const [allCoursesPassed, setAllCoursesPassed] = useState(true);
  const [enrollmentAllowed, setEnrollmentAllowed] = useState(null);
  const [responded, setResponded] = useState("true");

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

  const handleCoursePassedChange = (event) => {
    const value = event.target.value;
    setAllCoursesPassed(value === "yes");
  };

  const handleCoursesFailed = (event) => {
    const value = event.target.value;
    setNumCoursesFailed(value);
  };

  const handleAllowEnrollment = (event) => {
    const value = event.target.value;
    setEnrollmentAllowed(value === "yes");
  };

  const handleGoBack = () => {
    setAllCoursesPassed(true);
    setNumCoursesFailed(0);
    setEnrollmentAllowed(null);
    close();
  };

  const handleSubmit = async () => {
    try {
      const api = axios.create({
        baseURL: "https://localhost:7001/api",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });

      setResponded("true");

      const requestData = {
        nasId: nasId,
        semester: getSemesterValue(selectedSem),
        schoolYear: selectedSY,
        enrollmentAllowed: enrollmentAllowed,
        allCoursesPassed: allCoursesPassed,
        noOfCoursesFailed: numCoursesFailed,
        responded: responded,
      };

      console.log(requestData);

      const response = await api.put(`/SummaryEvaluation`, requestData);

      if (response.status === 200 || response.status === 201) {
        alert("Submitted successfully");
        onEvaluationSubmit();
      } else {
        alert("Submission failed");
      }
      close();
    } catch (error) {
      console.error(error);
    }
  };

  const openShowGrades = () => {
    setIsViewingShowGrades(true);
  };

  const closeShowGrades = () => {
    setIsViewingShowGrades(false);
  };

  return (
    show && (
      <div className="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
        <div className="relative w-4/6">
          <div className="relative bg-white rounded-lg shadow dark:bg-gray-700">
            <div className="bg-opacity-50">
              <div className="flex flex-col items-center justify-center px-20 py-10 rounded-t">
                <p className="text-4xl text-center w-full font-bold text-primary">
                  GRADE EVALUATION
                </p>
                <button
                  type="button"
                  className="text-white bg-primary hover:bg-secondary hover:text-primary font-medium rounded-lg text-sm px-5 py-2.5 my-10"
                  onClick={openShowGrades}
                >
                  EVALUATE GRADES
                </button>
                <ShowGrades show={isViewingShowGrades} close={closeShowGrades} grade={grade} />
                <div className="flex flex-row w-full items-center gap-6 mb-10">
                  <p className="text-xl text-left w-2/4">ALL COURSES PASSED:</p>
                  <div className="flex flex-row gap-2 justify-center items-center w-1/4">
                    <input
                      id="default-radio-1"
                      type="radio"
                      value="yes"
                      name="course-passed"
                      className="h-5 w-5"
                      onChange={handleCoursePassedChange}
                    />
                    <label
                      htmlFor="default-radio-1"
                      className="ml-2 text-xl font-medium text-green"
                    >
                      YES
                    </label>
                  </div>
                  <div className="flex flex-row gap-2 justify-center items-center w-1/4">
                    <input
                      id="default-radio-2"
                      type="radio"
                      value="no"
                      name="course-passed"
                      className="h-5 w-5"
                      onChange={handleCoursePassedChange}
                    />
                    <label htmlFor="default-radio-2" className="ml-2 text-xl font-medium text-red">
                      NO
                    </label>
                  </div>
                </div>

                <div className="flex flex-row w-full items-center gap-6 mb-10">
                  <p className="text-xl text-left w-2/4">ALLOW ENROLLMENT:</p>
                  <div className="flex flex-row gap-2 justify-center items-center w-1/4">
                    <input
                      id="default-radio-1"
                      type="radio"
                      value="yes"
                      name="allow-enroll"
                      className="h-5 w-5"
                      onChange={handleAllowEnrollment}
                    />
                    <label
                      htmlFor="default-radio-1"
                      className="ml-2 text-xl font-medium text-green"
                    >
                      YES
                    </label>
                  </div>
                  <div className="flex flex-row gap-2 justify-center items-center w-1/4">
                    <input
                      id="default-radio-2"
                      type="radio"
                      value="no"
                      name="allow-enroll"
                      className="h-5 w-5"
                      onChange={handleAllowEnrollment}
                    />
                    <label htmlFor="default-radio-2" className="ml-2 text-xl font-medium text-red">
                      NO
                    </label>
                  </div>
                </div>
                {allCoursesPassed ? null : (
                  <div className="flex flex-row w-full items-center mb-10">
                    <p className="text-xl text-left w-3/4">NUMBER OF COURSES FAILED:</p>
                    <input
                      type="number"
                      name="num-courses-failed"
                      className="w-1/4 border-2 border-gray-300 rounded-md px-2"
                      onChange={handleCoursesFailed}
                    />
                  </div>
                )}

                <button
                  type="button"
                  className="text-white bg-primary hover:bg-secondary hover:text-primary font-medium rounded-xl text-sm px-20 py-2.5"
                  onClick={handleSubmit}
                >
                  SUBMIT
                </button>
                <button
                  type="button"
                  className="text-primary hover:underline font-medium text-sm px-20 py-2.5"
                  onClick={handleGoBack}
                >
                  Go Back
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  );
};

EvaluateGrades.propTypes = {
  show: PropTypes.bool.isRequired,
  close: PropTypes.func.isRequired,
  grade: PropTypes.string.isRequired,
  nasId: PropTypes.number.isRequired,
  selectedSY: PropTypes.string.isRequired,
  selectedSem: PropTypes.string.isRequired,
  onEvaluationSubmit: PropTypes.func.isRequired,
};
