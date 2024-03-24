import React, { useState } from 'react';

const UpdateTaskModal = ({ task, onClose, onUpdate }) => {
    const [updatedTask, setUpdatedTask] = useState({ ...task, dueDate: formatDate(task.dueDate) });

    const handleChange = (e) => {
        setUpdatedTask({ ...updatedTask, [e.target.name]: e.target.value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onUpdate(updatedTask);
    };

    function formatDate(date) {
        const d = new Date(date);
        let month = '' + (d.getMonth() + 1);
        let day = '' + d.getDate();
        const year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }

    return (
        <div className="modal fade show" tabIndex="-1" style={{ display: 'block' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">Update Task</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <form onSubmit={handleSubmit}>
                        <div className="modal-body">
                            <div className="mb-3">
                                <label htmlFor="taskName" className="form-label">Name</label>
                                <input type="text" className="form-control" id="taskName" name="name" value={updatedTask.name} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskDueDate" className="form-label">Due Date</label>
                                <input type="date" className="form-control" id="taskDueDate" name="dueDate" value={updatedTask.dueDate} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskCategory" className="form-label">Category</label>
                                <input type="text" className="form-control" id="taskCategory" name="category" value={updatedTask.category} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskStatus" className="form-label">Status</label>
                                <select className="form-select" id="taskStatus" name="status" value={updatedTask.status} onChange={handleChange} required>
                                    <option value="Incomplete">Incomplete</option>
                                    <option value="Complete">Complete</option>
                                </select>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskDescription" className="form-label">Description</label>
                                <textarea className="form-control" id="taskDescription" name="description" rows="3" value={updatedTask.description} onChange={handleChange} required></textarea>
                            </div>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" onClick={onClose}>Close</button>
                            <button type="submit" className="btn btn-primary">Update Task</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default UpdateTaskModal;
