import React from "react";
import './CSSFiles/App.css'; 

function Button({ text, onClick }) {
  return (
    <button onClick={onClick}>
      {text}
    </button>
  );
}

export default Button;
