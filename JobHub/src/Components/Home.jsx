import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Card from './Card';
import "./Home.css"

const Dashboard = () => {
    const [jobs, setJobs] = useState([]);
    const [reference, setReference] = useState(0);
    const pageSize = 40; // Constant page size

    useEffect(() => {
        fetchJobs();
    }, []);

    const fetchJobs = async () => {
        try {
            const response = await axios.get(`https://localhost:7247/api/SelScrape/GetItems?reference=${reference}&pageSize=${pageSize}`);
            const newData = response.data.data;
            // Update jobs state by concatenating new data with existing jobs
             // Append new data to the existing jobs array
            // Append new data to the existing jobs array
            setJobs(prevJobs => [...prevJobs, ...newData]);
            
             // Ensure the reference is updated correctly
             if (newData.length > 0) {
                // Assuming the API provides a new reference value in the response
                const newReference = response.data.reference; // Replace with the actual field name for the new reference
                console.log('New Reference:', newReference);
                setReference(newReference);
            }
        } catch (error) {
            console.error('Error fetching jobs:', error);
        }
    };

    const handleViewMore = () => {
        fetchJobs(); // Fetch next set of data
    };

    return (
        <div className="container">
            <div className="mt-5">
                <div className="row">
                    {jobs.map((job, index) => (
                        <Card key={index} job={job} />
                    ))}
                </div>
            </div>
            <div className="text-center mt-3">
                <button className="btn btn-primary" onClick={handleViewMore}>
                    View More
                </button>
            </div>
        </div>
    );
};

export default Dashboard;
