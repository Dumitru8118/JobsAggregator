// import React from "react";

// const Admin = ({ user }) => {
//   console.log("JWT Decoded", user);
 
//   return <div>Admin</div>;
// };

// export default Admin;

import React, { useState } from "react";
import "./Admin.css"

const Admin = ({ user }) => {
  const [responseText, setResponseText] = useState("");
  const [numberOfPages, setNumberOfPages] = useState(1);

  const apiUrl = "https://localhost:7247/api/SelScrape";
  const token = localStorage.getItem("jwtToken");

  const handleApiCall = async (endpoint, method) => {
    try {
      const response = await fetch(`${apiUrl}/${endpoint}?pagesNumber=${numberOfPages}`, {
        method: method, 
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });
      // setResponseText(
      //   "The Scraper Has Started..."
      // )
      if (method == "DELETE"){
        const { success, rowsAffected } = await response.json(); 
        const message = `${rowsAffected} - Number of rows affected duplicates.`; 
        setResponseText(message); 
      }
      else{
        const data = await response.json();
        setResponseText(JSON.stringify(data, null, 2)); 
      }
  
    } catch (error) {
      setResponseText(`Error: ${error.message}`);
    }
  };

  const handleNumberOfPagesChange = (event) => {
    setNumberOfPages(parseInt(event.target.value, 10));
  };

  return (
    <div className="container">
    <div className="mt-5">
      <div className="col-12 mb-4">
        {/* <h1>Admin</h1> */}
        <div className="d-flex flex-column" style={{ width: '100%' }}>
            <button className="btn btn-success btn-block mb-3" onClick={() => handleApiCall('ScrapeFromHipo', 'POST')}>
              Scrape From Hipo
            </button>
            <button className="btn btn-success btn-block mb-3" onClick={() => handleApiCall('ScrapeFromJobRadar24', 'POST')}>
              Scrape From JobRadar24
            </button>
            <button className="btn btn-success btn-block mb-3" onClick={() => handleApiCall('ScrapeFromEJobs', 'POST')}>
              Scrape From EJobs
            </button>
            <button className="btn btn-danger btn-block mb-3" onClick={() => handleApiCall('duplicates', 'DELETE')}>
              Delete Duplicates
            </button>
            <div className="number-input mb-3">
              <label htmlFor="numberOfPages">Number of pages to scrape from the chosen Provider:</label>
              <input
                type="number"
                id="numberOfPages"
                name="numberOfPages"
                value={numberOfPages}
                onChange={handleNumberOfPagesChange}
                className="form-control"
              />
            </div>
        </div>

        <textarea 
          className="form-control" 
          rows="10" 
          cols="50" 
          value={responseText} 
          readOnly
        />
      </div>
    </div>
  </div>
  );
};

export default Admin;
