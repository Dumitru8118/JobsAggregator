import React from 'react';
import "./Card.css"

const Card = ({ job }) => {
    // Determine the image based on the URL
    let imageUrl = '';
    if (job.url.includes('ejobs')) {
        imageUrl = '../Assets/ejobs.png';
    } else if (job.url.includes('hipo')) {
        imageUrl = '../Assets/hipo.png';
    } else if (job.url.includes('jobradar24')) {
        imageUrl = './Assets/jobradar24.png';
    }

    return (
        <div className="col-12 mb-4">
            <div className="card d-flex flex-row align-items-right">
                {imageUrl && <img src={imageUrl} alt="Job Source Image" className="card-img-left" />}
                <div className="card-body">
                    <h5 className="card-title">{job.companyName}</h5>
                    <h6 className="card-subtitle mb-2 text-muted">{job.jobName}</h6>
                    <p className="card-text">Date Posted: {job.datePosted}</p>
                    <a href={job.url} className="card-link" target="_blank" rel="noopener noreferrer">Job Link</a>
                </div>
            </div>
        </div>
    );
};

export default Card;