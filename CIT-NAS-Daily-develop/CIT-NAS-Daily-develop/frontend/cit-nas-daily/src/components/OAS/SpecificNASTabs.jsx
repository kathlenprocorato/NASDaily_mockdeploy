"use client";
import { useState } from "react";
import { SpecificNASAttendance } from "../../components/OAS/SpecificNASAttendance";
import { SpecificNASEvaluation } from "./SpecificNASEvaluation";
import { SpecificNASStatus } from "./SpecificNASStatus";
export const SpecificNASTabs = () => {
  const [activeTab, setActiveTab] = useState(1);

  const handleTabClick = (tabNumber) => {
    setActiveTab(tabNumber);
  };

  return (
    <div className="bg-gray-2000 w-full">
      <div className="mx-auto ml-10 mr-10">
        <div className="flex justify-center w-full">
          <button
            className={`${
              activeTab === 1 ? "font-bold bg-primary text-white" : "bg-secondary"
            } px-4 py-2 w-full rounded-lg m-1 text-sm hover:bg-primary hover:text-white`}
            onClick={() => handleTabClick(1)}
          >
            Attendance
          </button>
          <button
            className={`${
              activeTab === 2 ? "font-bold bg-primary text-white" : "bg-secondary"
            } px-4 py-2 w-full rounded-lg m-1 text-sm hover:bg-primary hover:text-white`}
            onClick={() => handleTabClick(2)}
          >
            Evaluation
          </button>
          <button
            className={`${
              activeTab === 3 ? "font-bold bg-primary text-white" : "bg-secondary"
            } px-4 py-2 w-full rounded-lg m-1 text-sm hover:bg-primary hover:text-white`}
            onClick={() => handleTabClick(3)}
          >
            NAS Status
          </button>
        </div>
        <div className="pt-4 pr-1 pb-4 pl-1 bg-white rounded-b-lg">
          {/* Content for each tab */}
          {activeTab === 1 && (
            <div>
              <SpecificNASAttendance />
            </div>
          )}
          {activeTab === 2 && (
            <div>
              <SpecificNASEvaluation />
            </div>
          )}
          {activeTab === 3 && (
            <div>
              <SpecificNASStatus />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
