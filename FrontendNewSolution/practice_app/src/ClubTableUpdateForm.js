import React, { useState, useEffect } from "react";
import Button from "./Button";
import './CSSFiles/App.css'; 

function ClubTableUpdateForm({ isVisible, club, onClose, onSubmit }) {
    const [updatedClub, setUpdatedClub] = useState({
        name: "",
        characteristicColor: "",
        foundationDate: ""
    });
    

    useEffect(() => {
        if (club) {
            setUpdatedClub({
                name: club.name || "",
                characteristicColor: club.characteristicColor || "",
                foundationDate: club.foundationDate || ""
            });
        }
    }, [club, isVisible]);

    if (!isVisible) return null;

    function handleInputChange(event) {
        setUpdatedClub({
            ...updatedClub,
            [event.target.name]: event.target.value,
          });
    }

    function handleSubmit(event) {
        event.preventDefault();
        const finalUpdatedClub = {
            ...club,
            ...(updatedClub.name && { name: updatedClub.name }),
            ...(updatedClub.characteristicColor && { characteristicColor: updatedClub.characteristicColor }),
            ...(updatedClub.foundationDate && { foundationDate: updatedClub.foundationDate })
        };
        onSubmit(finalUpdatedClub); 
        onClose();
    }

    return (
        <div className="modal">
            <div className="modal-content">
                <h2>Update Club</h2>
                <form onSubmit={handleSubmit}>
                    <div>
                        <label>Club Name:</label>
                        <input
                            type="text"
                            name="name"
                            value={updatedClub.name}
                            onChange={handleInputChange}
                            placeholder="Enter new club name"
                        />
                    </div>
                    <div>
                        <label>Characteristic Color:</label>
                        <input
                            type="text"
                            name="characteristicColor"
                            value={updatedClub.characteristicColor}
                            onChange={handleInputChange}
                            placeholder="Enter new characteristic color"
                        />
                    </div>
                    <div>
                        <label>Foundation Date:</label>
                        <input
                            type="date"
                            name="foundationDate"
                            value={updatedClub.foundationDate}
                            onChange={handleInputChange}
                        />
                    </div>
                    <Button text="Save" type="submit" />
                    <Button text="Close" onClick={onClose} />
                </form>
            </div>
        </div>
    );
}

export default ClubTableUpdateForm;
