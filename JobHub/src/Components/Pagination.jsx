import React from 'react';
import { Pagination } from 'react-bootstrap';

const PaginationComponent = ({ currentPage, totalPages, onNextPage }) => {
    const pageItems = [];

    // Generate pagination items for each page
    for (let page = 1; page <= totalPages; page++) {
        pageItems.push(
            <Pagination.Item key={page} active={page === currentPage} onClick={() => onNextPage(page)}>
                {page}
            </Pagination.Item>
        )
    }

    return (
        <Pagination className="mt-4">
            {pageItems}
        </Pagination>
    );
};

export default PaginationComponent;
