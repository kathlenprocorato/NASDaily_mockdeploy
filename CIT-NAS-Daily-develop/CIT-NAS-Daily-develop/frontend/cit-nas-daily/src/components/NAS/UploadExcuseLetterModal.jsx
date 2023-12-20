import { useRef, useState } from "react";
import PropTypes from "prop-types";
import { Modal } from "flowbite-react";

export const UploadExcuseLetterModal = ({ isOpen, closeModal, handleSubmit }) => {
  const fileInputRef = useRef(null);
  const [selectedFileName, setSelectedFileName] = useState(null);
  const [error, setError] = useState("");

  const handleFileChange = (event) => {
    const selectedFile = event.target.files[0];
    setSelectedFileName(selectedFile ? selectedFile.name : null);
  };

  const handleButtonClick = () => {
    fileInputRef.current.click();
  };

  const handleCloseUploadModal = () => {
    setSelectedFileName(null);
    setError("");
    closeModal();
  };

  const handleConfirm = async () => {
    try {
      const selectedFile = fileInputRef.current.files[0];

      if (selectedFile) {
        // Check if the selected file is a PDF
        if (selectedFile.type === "application/pdf") {
          // Convert the selected file to Base64
          const base64String = await fileToBase64(selectedFile);

          // Pass the Base64-encoded string to the handleSubmit function
          handleSubmit(base64String);
          setSelectedFileName(null);
          setError("");
          closeModal();
        } else {
          setError("Invalid file type. Please select a PDF file.");
        }
      } else {
        setError("No file selected");
      }
    } catch (error) {
      console.error("Error during file processing:", error);
    }
  };

  const fileToBase64 = (file) => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);

      reader.onload = () => {
        const base64String = reader.result.split(",")[1];
        resolve(base64String);
      };

      reader.onerror = (error) => {
        reject(error);
      };
    });
  };

  return (
    <div>
      {isOpen && (
        <div
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100%",
            height: "100%",
            background: "rgba(0, 0, 0, 0.3)",
            zIndex: 999,
          }}
        ></div>
      )}

      <Modal
        show={isOpen}
        className="rounded-2xl"
        style={{
          padding: "0",
          zIndex: 1000,
          width: "30rem",
          left: "50%",
          transform: "translateX(-50%)",
        }}
      >
        <Modal.Body>
          <div style={{ display: "flex", flexDirection: "column", alignItems: "flex-start" }}>
            <div>
              <p className="font-bold text-lg">Upload Excuse Letter</p>
            </div>
            <div className="pt-5">
              <input
                type="file"
                onChange={handleFileChange}
                ref={fileInputRef}
                style={{ display: "none" }}
              />
              <button className="bg-primary text-white py-1 px-5" onClick={handleButtonClick}>
                Choose Files
              </button>
              <span className="ml-3">{selectedFileName || "No file selected"}</span>
            </div>
            <div className="pt-2 text-red">
              <p>{error}</p>
            </div>
          </div>
        </Modal.Body>
        <Modal.Footer
          style={{
            paddingTop: "0.3rem",
            paddingBottom: "0.3rem",
            display: "flex",
            justifyContent: "flex-end",
          }}
        >
          <div className="flex justify-end items-center">
            <div className="flex m-2">
              <button
                className="bg-primary text-white py-2 px-6 rounded-full  hover:bg-secondary hover:text-primary"
                onClick={handleCloseUploadModal}
              >
                Cancel
              </button>
            </div>
            <div className="flex m-2">
              <button
                className="bg-primary text-white py-2 px-6 rounded-full  hover:bg-secondary hover:text-primary"
                onClick={handleConfirm}
              >
                Submit
              </button>
            </div>
          </div>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

UploadExcuseLetterModal.propTypes = {
  isOpen: PropTypes.bool,
  closeModal: PropTypes.func,
  handleSubmit: PropTypes.func,
};
