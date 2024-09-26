import React, { useEffect, useState } from "react";
import api from "./api/clubsfootballers";
import './CSSFiles/App.css';

function FootballerDetailsModal({ footballer, onClose }) {
  const [footballerDetails, setFootballerDetails] = useState(null);

  useEffect(() => {
    const fetchFootballerDetails = async () => {
      try {
        const response = await api.get(`api/Footballer/getFootballerById/${footballer.id}`);
        if (response && response.data) {
          setFootballerDetails(response.data);
        }
      } catch (err) {
        console.error("Error fetching footballer details", err);
      }
    };

    fetchFootballerDetails();
  }, [footballer]);

  if (!footballerDetails) {
    return <div>Loading...</div>;
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>{footballerDetails.name}</h2>
        <p><strong>Date of Birth:</strong> {footballerDetails.dob}</p>
        <p><strong>Club ID:</strong> {footballerDetails.clubId}</p>
        <p><strong>Footballer ID:</strong> {footballerDetails.id}</p>

        <button onClick={onClose} className="close-modal-button">Close</button>
      </div>
    </div>
  );
}

export default FootballerDetailsModal;
