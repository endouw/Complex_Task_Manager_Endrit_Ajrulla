import React, { useState } from 'react';
import UpdateTaskModal from './UpdateTaskModal';
import TaskHistory from './TaskHistory';

const TaskViewModal = ({ task, onClose, onUpdate }) => {
    if (!task) return null;

    const [showUpdateModal, setShowUpdateModal] = useState(false);

    const handleUpdate = (updatedTask) => {
        onUpdate(updatedTask);
        setShowUpdateModal(false);
        onClose(); 
    };

    return (
        <div className="modal fade show" tabIndex="-1" style={{ display: 'block' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">Task Details</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">
                        <p><strong>Name:</strong> {task.name}</p>
                        <p><strong>Due Date:</strong> {new Date(task.dueDate).toLocaleDateString()}</p>
                        <p><strong>Category:</strong> {task.category}</p>
                        <p><strong>Status:</strong> {task.status}</p>
                        <p><strong>Description:</strong> {task.description}</p>
                        <TaskHistory taskId={task.id} />

                    </div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" onClick={onClose}>Close</button>
                        <button type="button" className="btn btn-primary" onClick={() => setShowUpdateModal(true)}>Update</button>
                    </div>
                    {showUpdateModal && (
                        <UpdateTaskModal
                            task={task}
                            onClose={() => {
                                setShowUpdateModal(false);
                                onClose(); // Close the ViewModal when the UpdateModal is closed
                            }}
                            onUpdate={handleUpdate}
                        />
                    )}
                </div>

            </div>
        </div >
    );
};

export default TaskViewModal;