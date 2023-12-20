import { Table } from "flowbite-react";
import { useEffect, useState, useMemo } from "react";
import { useParams } from "react-router-dom";
import { ActivitiesFormModal } from "./ActivitiesFormModal";
import PropTypes from "prop-types";
import axios from "axios";

export const ActivitiesSummaryTable = ({
  selectedMonth,
  selectedSem,
  selectedSY,
  currentYear,
  currentSem,
}) => {
  const { nasId } = useParams();
  const [activitySummaries, setActivitySummaries] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);

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

  const handleAdd = () => {
    setIsModalOpen(true);
  };

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

        const response = await api.get(
          `/ActivitiesSummary/${nasId}/${selectedSY}/${getSemesterValue(selectedSem)}`
        );
        const data = response.data;

        const filteredData = data.filter((item) => {
          const date = new Date(item.dateOfEntry);
          const month = date.getMonth();
          const year = date.getFullYear();
          const first = parseInt(
            year.toString().substring(0, 2) + selectedSY.toString().substring(0, 2)
          );
          const second = parseInt(
            year.toString().substring(0, 2) + selectedSY.toString().substring(2)
          );
          switch (selectedMonth) {
            case -1:
              return month >= 7 && month <= 11 && year === first;
            case -2:
              return month >= 0 && month <= 5 && year === second;
            case -3:
              return month >= 5 && month <= 7 && year === second;
          }

          switch (selectedSem) {
            case "First":
              return month === selectedMonth && year === first;
            case "Second":
              return month === selectedMonth && year === second;
            case "Summer":
              return month === selectedMonth && year === second;
          }
        });
        setActivitySummaries(filteredData);
      } catch (error) {
        console.error(error);
      }
    };

    fetchNas();
  }, [nasId, selectedMonth, selectedSem, selectedSY, activitySummaries, getSemesterValue]);

  return (
    <div>
      <Table hoverable className="border">
        <Table.Head className="border">
          <Table.HeadCell className="text-center border">DATE</Table.HeadCell>
          <Table.HeadCell className="text-center border">Activities of the Day</Table.HeadCell>
          <Table.HeadCell className="text-center border">Skills Learned</Table.HeadCell>
          <Table.HeadCell className="text-center border">Values Learned</Table.HeadCell>
        </Table.Head>
        <Table.Body className="divide-y">
          {activitySummaries.map((summary) => (
            <Table.Row key={summary.id}>
              <Table.Cell
                className="text-center border"
                style={{ overflowWrap: "break-word", maxWidth: "100px" }}
              >
                {new Date(summary.dateOfEntry).toLocaleDateString()}
              </Table.Cell>
              <Table.Cell
                className="text-center border"
                style={{ overflowWrap: "break-word", maxWidth: "100px" }}
              >
                {summary.activitiesOfTheDay}
              </Table.Cell>
              <Table.Cell
                className="text-center border"
                style={{ overflowWrap: "break-word", maxWidth: "100px" }}
              >
                {summary.skillsLearned}
              </Table.Cell>
              <Table.Cell
                className="text-center border"
                style={{ overflowWrap: "break-word", maxWidth: "100px" }}
              >
                {summary.valuesLearned}
              </Table.Cell>
            </Table.Row>
          ))}
          <Table.Row>
            <Table.Cell colSpan={4} className="text-center border">
              <button
                className="btn btn-primary bg-secondary px-4 py-2 rounded-lg m-1 text-sm hover:bg-primary hover:text-white"
                onClick={handleAdd}
              >
                Add an entry
              </button>
            </Table.Cell>
          </Table.Row>
        </Table.Body>
      </Table>
      <ActivitiesFormModal
        isOpen={isModalOpen}
        closeModal={() => setIsModalOpen(false)}
        currentYear={currentYear}
        currentSem={currentSem}
      />
    </div>
  );
};

ActivitiesSummaryTable.propTypes = {
  selectedMonth: PropTypes.number.isRequired,
  selectedSem: PropTypes.string.isRequired,
  selectedSY: PropTypes.string.isRequired,
  currentYear: PropTypes.number.isRequired,
  currentSem: PropTypes.string.isRequired,
};
