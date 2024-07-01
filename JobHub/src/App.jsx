import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Container } from 'react-bootstrap';
import JobList from './components/JobList';
import PaginationComponent from './components/Pagination';

const App = () => {
    const [jobs, setJobs] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [hasNextPage, setHasNextPage] = useState(false); // Assuming backend indicates if there's a next page

    const fetchJobs = async (page) => {
        try {
            const response = await axios.get(`/api/jobs?page=${page}&pageSize=40`);
            setJobs(response.data.jobs);
            setHasNextPage(response.data.hasNextPage); // Adjust this based on your backend response
            setCurrentPage(page);
        } catch (error) {
            console.error('Error fetching jobs:', error);
        }
    };

    useEffect(() => {
        fetchJobs(1);  // Fetch jobs for the first page when component mounts
    }, []);

    const handleNextPage = (page) => {
        fetchJobs(page);
    };

    return (
        <Container className="mt-5">
            <JobList jobs={jobs} />
            <PaginationComponent currentPage={currentPage} totalPages={totalPages} onNextPage={handleNextPage} />
        </Container>
    );
};

export default App;
