import { useEffect, useState, useRef } from 'react';
import './App.css';
import $ from 'jquery';
import 'datatables.net-bs5';
import 'datatables.net-bs5/css/dataTables.bootstrap5.min.css';
import 'datatables.net-buttons-bs5';
import 'datatables.net-buttons/js/buttons.html5.js';
import TaskViewModal from './TaskViewModal';
import CreateTaskModal from './CreateTaskModal';
import UpdateTaskModal from './UpdateTaskModal';
import { useNavigate } from 'react-router-dom';




const MainApp = ({ userId }) => {
    const [tasks, setTasks] = useState([]);
    const [selectedTask, setSelectedTask] = useState(null);
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [showUpdateModal, setShowUpdateModal] = useState(false);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const navigate = useNavigate(); 


    const dataTable = useRef(null);

    const tableRef = useRef(null);



    useEffect(() => {
        const token = localStorage.getItem('token');

        setIsAuthenticated(!!token);
    }, []);

    useEffect(() => {
        fetchTasks();
    }, []);

    useEffect(() => {
        if (tableRef.current) {
            if (!dataTable.current) {
                // Initialize the DataTable only once
                dataTable.current = $(tableRef.current).DataTable({
                    columns: [
                        { title: 'Name', data: 'name' },
                        { title: 'Due Date', data: 'dueDate', render: data => new Date(data).toLocaleDateString() },
                        { title: 'Category', data: 'category' },
                        { title: 'Status', data: 'status' },
                        {
                            title: 'Actions', data: null, render: function (data, type, row) {
                                return `<button class="btn btn-info btn-sm me-2" onclick="viewTask(${row.id})">View</button>` +
                                    `<button class="btn btn-danger btn-sm" onclick="deleteTask(${row.id})">Delete</button>`;
                            }, orderable: false, searchable: false
                        }
                    ]
                });
            } else {
                // Clear, add new data, and redraw the DataTable
                dataTable.current.clear().rows.add(tasks).draw();
            }
        }
    }, [tasks]);

    const fetchTasks = async () => {
        const token = localStorage.getItem('token');
        const response = await fetch('/api/task/getAllTasks', {
            headers: {
                'Authorization': `Bearer ${token}`,

            },
        });
        const data = await response.json();
        setTasks(data);
    };

    window.viewTask = (id) => {
        const task = tasks.find(t => t.id === id);
        setSelectedTask(task);
    };

    window.deleteTask = async (id) => {
        const token = localStorage.getItem('token');

        if (window.confirm(`Are you sure you want to delete the task?`)) {
            const response = await fetch(`/api/task/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                },
            });
            if (response.ok) {
                setTasks(tasks.filter(t => t.id !== id));
            }
        }
    };;

    const handleCloseModal = () => {
        setSelectedTask(null);
    };

    const handleCreateTask = async (newTask) => {
        const token = localStorage.getItem('token');

        try {
            const response = await fetch('/api/task', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(newTask)
            });

            if (response.ok) {
                const createdTask = await response.json();
                setTasks([...tasks, createdTask]);
            } else {
                // server errors or validation errors
                console.error('Failed to create task:', response.statusText);
            }
        } catch (error) {
            //  network errors
            console.error('Network error:', error.message);
        } setShowCreateModal(false);
    };

    const handleUpdateTask = async (updatedTask) => {
        const token = localStorage.getItem('token');

        try {
            const response = await fetch(`/api/task/${updatedTask.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`

                },
                body: JSON.stringify(updatedTask)
            });

            if (response.ok) {
                const updatedTasks = tasks.map(task => task.id === updatedTask.id ? updatedTask : task);
                setTasks(updatedTasks);
            } else {
                // Handle server errors or validation errors
                console.error('Failed to update task:', response.statusText);
            }
        } catch (error) {
            // Handle network errors
            console.error('Network error:', error.message);
        }

        setShowUpdateModal(false);
    };

    const handleCloseViewModal = () => {
        setSelectedTask(null);
    };

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/landingPage');
        setIsAuthenticated(false);

    };

    return (
        <div className="container my-5">

           
                <div className="container my-5">
                    <h1>Task List</h1>
                <button className="btn btn-primary mb-3" onClick={handleLogout}>Logout</button>
                    <button className="btn btn-primary mb-3" onClick={() => setShowCreateModal(true)}>Create Task</button>

                    <table ref={tableRef} className="table table-striped"></table>
                    {selectedTask && (
                        <TaskViewModal
                            task={selectedTask}
                            onClose={handleCloseViewModal}
                            onUpdate={handleUpdateTask}
                        />
                    )}
                {showCreateModal && <CreateTaskModal userId={userId} onClose={() => setShowCreateModal(false)} onCreate={handleCreateTask} />}
                    {showUpdateModal && <UpdateTaskModal task={selectedTask} onClose={() => setShowUpdateModal(false)} onUpdate={handleUpdateTask} />}
                </div>
            
          


        </div>
    );
};
 export default MainApp;
