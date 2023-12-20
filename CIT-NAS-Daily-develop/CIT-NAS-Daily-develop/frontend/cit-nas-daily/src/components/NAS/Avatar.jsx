import PropTypes from "prop-types";

export const Avatar = ({ avatar, handleAvatarChange }) => {
  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file.size > 500 * 1024) {
      // 500KB in bytes
      alert("File size must be less than 500KB.");
    } else {
      handleAvatarChange(e);
    }
  };

  return (
    <div
      className="avatar-square"
      style={{
        width: "200px",
        height: "200px",
        border: "2px solid gray",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      {avatar ? (
        <img
          src={`data:image/png;base64,${avatar}`}
          //src={URL.createObjectURL(avatar)}
          alt="Avatar"
          className="avatar-image"
          style={{ width: "100%", height: "100%" }}
        />
      ) : (
        <label htmlFor="avatar" style={{ cursor: "pointer" }}>
          Upload Photo
          <input
            type="file"
            id="avatar"
            accept="image/*"
            onChange={handleFileChange}
            style={{ display: "none" }}
          />
        </label>
      )}
    </div>
  );
};

Avatar.propTypes = {
  avatar: PropTypes.string,
  handleAvatarChange: PropTypes.func.isRequired,
};
