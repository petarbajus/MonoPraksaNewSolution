// Home.js
import React from 'react';
import Navbar from './Navbar';  // Ensure this import path is correct
import './CSSFiles/App.css';

function Home() {
    return (
      <div>
        <Navbar />
        <h1 className="page-title">Welcome to my Router Practice</h1>
      </div>
    );
  }
  
  export default Home;
