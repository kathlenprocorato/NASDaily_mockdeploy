import PropType from "prop-types";

export const PersonalInformation = ({ label, id, value, onChange }) => {
  return (
    <div>
      <label htmlFor={id} className="block mb-2 font-bold text-gray-600">
        {label}
      </label>
      <input
        type="text"
        id={id}
        name={id}
        value={value}
        onChange={onChange}
        className="border border-gray-300 rounded-md px-3 py-2 w-full"
        readOnly={true}
        style={{ backgroundColor: "#E3E3E3" }}
      />
    </div>
  );
};

PersonalInformation.propTypes = {
  label: PropType.string.isRequired,
  id: PropType.string.isRequired,
  value: PropType.string.isRequired,
  onChange: PropType.func.isRequired,
};
