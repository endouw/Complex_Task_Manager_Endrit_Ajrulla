import React, { useState, useEffect } from 'react';

const TaskHistory = ({ taskId }) => {
    const [history, setHistory] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(5);
    useEffect(() => {
        const token = localStorage.getItem('token');
        fetch(`/api/task//history/${taskId}`, {
            headers: {
                'Authorization': `Bearer ${token}`,

            },
        })
            .then(response => response.json())
            .then(setHistory);
    }, [taskId, currentPage, itemsPerPage]);

    return (
        <div>
            <h5 className="mt-3">Task History</h5>
            <div className="table-responsive">
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th scope="col">Timestamp</th>
                            <th scope="col">User</th>
                            <th scope="col">Field</th>
                            <th scope="col">Old Value</th>
                            <th scope="col">New Value</th>
                        </tr>
                    </thead>
                    <tbody>
                        {history.map(entry => (
                            <tr key={entry.id}>
                                <td>{new Date(entry.timestamp).toLocaleString()}</td>
                                <td>{entry.user.email}</td>
                                <td>{entry.changedField}</td>
                                <td>{entry.oldValue}</td>
                                <td>{entry.newValue}</td>
                            </tr>
                        ))}
                    </tbody>
                    <div className="d-flex mt-3">
                        <button
                            className="btn btn-primary"
                            onClick={() => setCurrentPage(currentPage - 1)}
                            disabled={currentPage === 1}
                        >
                            Previous
                        </button>
                        <button
                            className="btn btn-primary"
                            onClick={() => setCurrentPage(currentPage + 1)}
                        >
                            Next
                        </button>
                    </div>

                </table>
            </div>
        </div>
    );
};

export default TaskHistory;