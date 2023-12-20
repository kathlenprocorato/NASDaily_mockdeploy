import PropType from "prop-types";
import { ScheduleRow } from "./SetScheduleRow";

export const ScheduleTable = ({
  days,
  schedule,
  scheduleChanges,
  handleToggleBrokenSchedule,
  handleAddScheduleRow,
  handleRemoveScheduleRow,
  handleStartTimeChange,
  handleEndTimeChange,
  openModal,
  overallHours,
}) => {
  return (
    <div>
      <table className="w-full">
        <thead>
          <tr className=" bg-primary text-white" style={{ border: "1px solid #000" }}>
            <th>Day</th>
            <th>Broken Schedule?</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th></th>
            <th>No. of Hours</th>
          </tr>
        </thead>
        <tbody>
          {days.map((day) =>
            schedule[day].isBroken ? (
              schedule[day].items.map((scheduleItem, index) => (
                <ScheduleRow
                  key={`${day}-${index}`}
                  day={day}
                  index={index}
                  scheduleItem={scheduleItem}
                  isBroken={schedule[day].isBroken}
                  handleToggleBrokenSchedule={handleToggleBrokenSchedule}
                  handleAddScheduleRow={handleAddScheduleRow}
                  handleRemoveScheduleRow={handleRemoveScheduleRow}
                  handleStartTimeChange={handleStartTimeChange}
                  handleEndTimeChange={handleEndTimeChange}
                />
              ))
            ) : (
              <tr key={day}>
                <td className="text-center pt-5">{day}</td>
                <td className="text-center">
                  <input
                    type="checkbox"
                    checked={schedule[day].isBroken}
                    onChange={(e) => handleToggleBrokenSchedule(day, e.target.checked)}
                  />
                </td>
                <td className="text-center">
                  <input
                    type="time"
                    value={schedule[day].items[0].start}
                    onChange={(e) => handleStartTimeChange(day, 0, e.target.value)}
                  />
                </td>
                <td className="text-center">
                  <input
                    type="time"
                    value={schedule[day].items[0].end}
                    onChange={(e) => handleEndTimeChange(day, 0, e.target.value)}
                  />
                </td>
                <td className="text-center">
                  <button
                    onClick={() => handleAddScheduleRow(day)}
                    disabled={!schedule[day].isBroken}
                    style={{ color: schedule[day].isBroken ? "green" : "#C5C5C5" }}
                  >
                    Add Row
                  </button>
                </td>
                <td className="text-center">
                  {schedule[day].items[0].totalHours.toFixed(2)} hours
                </td>
              </tr>
            )
          )}
          <tr>
            <td colSpan="6">
              <br />
            </td>
          </tr>
          <tr>
            <th colSpan="4"></th>
            <th colSpan="1" className="pt-3 text-center font-weight-bold">
              <button
                className={`py-2 px-4 rounded ${
                  overallHours === 24 ? "bg-primary text-white" : "bg-gray-300 text-gray-600"
                }`}
                disabled={overallHours < 24}
                onClick={openModal}
                style={{
                  backgroundColor: overallHours === 24 ? "#88333a" : "#c5c5c5",
                  color: "white",
                }}
              >
                Submit
              </button>
            </th>
            <th colSpan="1" className="pt-3 text-center font-weight-bold">
              Total Hours: {overallHours.toFixed(2)} hours
            </th>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

ScheduleTable.propTypes = {
  days: PropType.arrayOf(PropType.string).isRequired,
  schedule: PropType.objectOf(
    PropType.shape({
      isBroken: PropType.bool.isRequired,
      items: PropType.arrayOf(
        PropType.shape({
          start: PropType.string.isRequired,
          end: PropType.string.isRequired,
          totalHours: PropType.number.isRequired,
        })
      ).isRequired,
    })
  ).isRequired,
  scheduleChanges: PropType.objectOf(
    PropType.shape({
      isBroken: PropType.bool.isRequired,
      items: PropType.arrayOf(
        PropType.shape({
          start: PropType.string.isRequired,
          end: PropType.string.isRequired,
          totalHours: PropType.number.isRequired,
        })
      ).isRequired,
    })
  ).isRequired,
  handleToggleBrokenSchedule: PropType.func.isRequired,
  handleAddScheduleRow: PropType.func.isRequired,
  handleRemoveScheduleRow: PropType.func.isRequired,
  handleStartTimeChange: PropType.func.isRequired,
  handleEndTimeChange: PropType.func.isRequired,
  openModal: PropType.func.isRequired,
  overallHours: PropType.number.isRequired,
};
