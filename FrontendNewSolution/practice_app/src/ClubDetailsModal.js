import React, { useEffect, useState } from "react";
import api from "./api/clubsfootballers"; 
import './CSSFiles/App.css';

function ClubDetailsModal({ club, onClose }) {
  const [clubDetails, setClubDetails] = useState(null);

  useEffect(() => {
    const fetchClubDetails = async () => {
      try {
        const response = await api.get(`api/Club/getClubById/${club.id}`);
        if (response && response.data) {
          setClubDetails(response.data);
        }
      } catch (err) {
        console.error("Error fetching club details", err);
      }
    };

    fetchClubDetails();
  }, [club]);

  if (!clubDetails) {
    return <div>Loading...</div>;
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>{clubDetails.name}</h2>
        <p><strong>Characteristic Color:</strong> {clubDetails.characteristicColor}</p>
        <p><strong>Foundation Date:</strong> {clubDetails.foundationDate}</p>
        <p><strong>Club ID:</strong> {clubDetails.id}</p>

        <button onClick={onClose} className="close-modal-button">Close</button>
      </div>
    </div>
  );
}

export default ClubDetailsModal;
