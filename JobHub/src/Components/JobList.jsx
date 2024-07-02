import React from 'react';
import JobCard from './Card';

const JobList = ({ jobs }) => {
    return (
        <div>
            {jobs.map(job => (
                <JobCard key={job.id} job={job} />
            ))}
        </div>
    );
};

export default JobList;