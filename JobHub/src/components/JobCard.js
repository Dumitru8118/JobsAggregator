import React from 'react';
import { Card, Button } from 'react-bootstrap';

const JobCard = ({ job }) => {
    const { title, company, datePosted, url } = job;

    return (
        <Card className="mb-3">
            <Card.Body>
                <Card.Title>{title}</Card.Title>
                <Card.Subtitle className="mb-2 text-muted">{company}</Card.Subtitle>
                <Card.Text>Date Posted: {datePosted}</Card.Text>
                <Button variant="primary" href={url} target="_blank">Apply Now</Button>
            </Card.Body>
        </Card>
    );
};

export default JobCard;
