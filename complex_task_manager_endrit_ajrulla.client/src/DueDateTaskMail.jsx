import { useEffect, useState } from 'react';

const DueDateTaskMail = () => {
    const [tasks, setTasks] = useState([]);

    useEffect(() => {
        fetchTasksDueIn24Hours();
    }, []);

    const fetchTasksDueIn24Hours = async () => {
        const token = localStorage.getItem('token');

        try {
            const response = await fetch('/api/task/tasksDueIn24Hours', {
                headers: {
                    'Authorization': `Bearer ${token}`

                }
            });
            if (response.ok) {
                const data = await response.json();
                setTasks(data);
                sendEmails(data);
            } else {
                console.error('Failed to fetch tasks:', response.statusText);
            }
        } catch (error) {
            console.error('Failed to fetch tasks:', error);
        }
    };

    const sendEmails = (tasks) => {
        tasks.forEach(async (task) => {
            const emailContent = `
                <h1>Task Reminder</h1>
                <p>Task Name: ${task.name}</p>
                <p>Due Date: ${task.dueDate}</p>
                <p>Description: ${task.description}</p>
            `;

            // Example email sending logic
            const response = await fetch('/api/email/send', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    to: task.userEmail,
                    subject: 'Task Reminder',
                    html: emailContent
                })
            });

            if (!response.ok) {
                console.error(`Failed to send email for task ${task.id}`);
            }
        });
    };

    return (
        <div>
            <h1>Tasks Due in the Next 24 Hours</h1>
            <ul>
                {tasks.map((task) => (
                    <li key={task.id}>{task.name} - Due Date: {task.dueDate}</li>
                ))}
            </ul>
        </div>
    );
};

export default DueDateTaskMail;