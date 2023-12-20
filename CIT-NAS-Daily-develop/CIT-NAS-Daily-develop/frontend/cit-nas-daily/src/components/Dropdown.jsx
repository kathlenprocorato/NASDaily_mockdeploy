import PropTypes from "prop-types";

export const Dropdown = ({ label, options, selectedValue, onChange }) => {
  return (
    <div className="flex flex-col sm:flex-row gap-2 items-center">
      <div className="mr-2">{label}:</div>
      <select
        value={selectedValue}
        onChange={onChange}
        className="w-full text-base border rounded-md sm:w-[7rem]"
      >
        {options.map((option, index) => (
          <option key={index} value={option}>
            {option}
          </option>
        ))}
      </select>
    </div>
  );
};

Dropdown.propTypes = {
  label: PropTypes.string.isRequired,
  options: PropTypes.arrayOf(PropTypes.string).isRequired,
  selectedValue: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
};
