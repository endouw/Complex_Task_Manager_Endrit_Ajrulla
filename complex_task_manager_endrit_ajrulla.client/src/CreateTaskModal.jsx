import React, { useState } from 'react';

const CreateTaskModal = ({ userId , onClose, onCreate }) => {
    const [task, setTask] = useState({
        name: '',
        dueDate: '',
        category: '',
        status: '',
        description: ''
    });

    const handleChange = (e) => {
        setTask({ ...task, [e.target.name]: e.target.value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const userId = localStorage.getItem('userId');
        const taskWithUserId = { ...task, userId };
        onCreate(taskWithUserId);
    };

    return (
        <div className="modal fade show" tabIndex="-1" style={{ display: 'block' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">Create Task</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <form onSubmit={handleSubmit}>
                        <div className="modal-body">
                            <div className="mb-3">
                                <label htmlFor="taskName" className="form-label">Name</label>
                                <input type="text" className="form-control" id="taskName" name="name" value={task.name} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskDueDate" className="form-label">Due Date</label>
                                <input type="date" className="form-control" id="taskDueDate" name="dueDate" value={task.dueDate} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskCategory" className="form-label">Category</label>
                                <input type="text" className="form-control" id="taskCategory" name="category" value={task.category} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskStatus" className="form-label">Status</label>
                                <select className="form-select" id="taskStatus" name="status" value={task.status} onChange={handleChange} required>
                                    <option value="">Select a status</option>
                                    <option value="Incomplete">Incomplete</option>
                                    <option value="Complete">Complete</option>
                                </select>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="taskDescription" className="form-label">Description</label>
                                <textarea className="form-control" id="taskDescription" name="description" rows="3" value={task.description} onChange={handleChange} required></textarea>
                            </div>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" onClick={onClose}>Close</button>
                            <button type="submit" className="btn btn-primary">Create Task</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default CreateTaskModal;
