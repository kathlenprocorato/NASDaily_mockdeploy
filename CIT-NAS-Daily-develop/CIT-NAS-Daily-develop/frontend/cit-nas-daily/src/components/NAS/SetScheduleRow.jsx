import React from "react";

export const ScheduleRow = ({
  day,
  index,
  scheduleItem,
  isBroken,
  handleToggleBrokenSchedule,
  handleAddScheduleRow,
  handleRemoveScheduleRow,
  handleStartTimeChange,
  handleEndTimeChange,
}) => {
  return (
    <tr key={`${day}-${index}`}>
      <td className="text-center pt-5">{index === 0 ? day : ""}</td>
      <td className="text-center">
        {index === 0 ? (
          <input
            type="checkbox"
            checked={isBroken}
            onChange={(e) => handleToggleBrokenSchedule(day, e.target.checked)}
          />
        ) : null}
      </td>
      <td className="text-center">
        <input
          type="time"
          value={scheduleItem.start}
          onChange={(e) => handleStartTimeChange(day, index, e.target.value)}
        />
      </td>
      <td className="text-center">
        <input
          type="time"
          value={scheduleItem.end}
          onChange={(e) => handleEndTimeChange(day, index, e.target.value)}
        />
      </td>
      {index === 0 ? (
        <td className="text-center">
          <button
            onClick={() => handleAddScheduleRow(day)}
            disabled={!isBroken}
            style={{ color: isBroken ? "green" : "#C5C5C5" }}
          >
            Add Row
          </button>
        </td>
      ) : (
        <td className="text-center">
          <button onClick={() => handleRemoveScheduleRow(day, index)} style={{ color: "red" }}>
            Remove Row
          </button>
        </td>
      )}
      <td className="text-center">{scheduleItem.totalHours.toFixed(2)} hours</td>
    </tr>
  );
};
