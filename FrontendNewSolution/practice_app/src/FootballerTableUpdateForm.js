import React, { useState, useEffect } from "react";
import Button from "./Button";
import './CSSFiles/App.css'; 

function FootballerTableUpdateForm({ isVisible, footballer, onClose, onSubmit }) {
    const [updatedFootballer, setUpdatedFootballer] = useState({
        name: "",
        DOB: "",
        ClubId: ""
    });

    useEffect(() => {
        if (footballer) {
            setUpdatedFootballer({
                name: footballer.name || "",
                DOB: footballer.DOB || "",
                ClubId: footballer.ClubId || ""
            });
        }
    }, [footballer, isVisible]);

    if (!isVisible) return null;

    function handleInputChange(event) {
        setUpdatedFootballer({
            ...updatedFootballer,
            [event.target.name]: event.target.value,
        });
    }

    function handleSubmit(event) {
        event.preventDefault();
        const finalUpdatedFootballer = {
            ...footballer,
            ...(updatedFootballer.name && { name: updatedFootballer.name }),
            ...(updatedFootballer.DOB && { DOB: updatedFootballer.DOB }),
            ...(updatedFootballer.ClubId && { ClubId: updatedFootballer.ClubId })
        };
        onSubmit(finalUpdatedFootballer);
        onClose();
    }

    return (
        <div className="modal">
            <div className="modal-content">
                <h2>Update Footballer</h2>
                <form onSubmit={handleSubmit}>
                    <div>
                        <label>Footballer Name:</label>
                        <input
                            type="text"
                            name="name"
                            value={updatedFootballer.name}
                            onChange={handleInputChange}
                            placeholder="Enter new footballer name"
                        />
                    </div>
                    <div>
                        <label>Date of Birth:</label>
                        <input
                            type="date"
                            name="DOB"
                            value={updatedFootballer.DOB}
                            onChange={handleInputChange}
                            placeholder="Enter new footballer DOB"
                        />
                    </div>
                    <div>
                        <label>Club ID:</label>
                        <input
                            type="text"
                            name="ClubId"
                            value={updatedFootballer.ClubId}
                            onChange={handleInputChange}
                            placeholder="Enter footballer's club ID"
                        />
                    </div>
                    <Button text="Save" type="submit" />
                    <Button text="Close" onClick={onClose} />
                </form>
            </div>
        </div>
    );
}

export default FootballerTableUpdateForm;