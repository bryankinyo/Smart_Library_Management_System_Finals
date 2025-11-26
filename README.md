# Smart_Library_Management_System_Finals

Name: Bryan Quiño & Leachim Dela Cerna

﻿Smart Library Dashboard
Overview

This is a web-based library management dashboard built with HTML, CSS (Bootstrap 5), and JavaScript. It allows administrators to manage books, users, loans, reservations, fines, and catalogs through a clean and dark-themed interface.

Features

Dashboard Overview: Displays total books, total users, active loans, and total fines.

Sidebar Navigation: Quickly switch between sections: Dashboard, Books, Users, Loans, Reservations, Fines, and Catalogs.

Search Bar: Positioned fixed at the top for global search (placeholder functionality).

Dynamic Forms: Add new entries for each category:

Books: title, author, ISBN, published year.

Users: name, email, role (Student or Faculty).

Loans: book, user, due date.

Reservations: book, user, reservation date.

Fines: user, amount, reason, paid status.

Catalogs: catalog name, multiple book selection.

Data Fetching: Uses REST API endpoints to fetch and update data dynamically.

Real-time Dashboard Updates: Dashboard summary cards update immediately after adding new data.

Responsive and Dark Themed: Uses Bootstrap 5 for styling with custom dark mode colors.

Technical Details

HTML Structure: Fixed sidebar and topbar layout with a main content area.

CSS Styling: Customized Bootstrap 5 dark theme with specific overrides for form controls, cards, buttons, and navigation.

JavaScript:

Fetches data from API endpoints like /books, /users, /loans, etc.

Populates lists and forms dynamically.

Handles form submissions with POST requests to add new records.

Updates dashboard stats after form submissions to reflect new data.

Navigation links load different sections without reloading the page.

How to Use

Setup API: Ensure the backend REST API is running and accessible at the configured base URL (https://localhost:7088/api).

Open the Dashboard: Load the index.html file in a modern browser.

Navigate Sections: Use the sidebar to manage books, users, loans, reservations, fines, and catalogs.

Add Records: Fill out and submit the forms in each section to add new records.

Dashboard Updates: Watch the dashboard cards update immediately after adding new records.

Known Issues & Notes

The global search bar currently displays a placeholder message; backend search implementation is needed.

API endpoints must conform to the expected structure for the dashboard to function correctly.

The dashboard relies on the presence of specific fields like id, title, name, email, etc., in the API responses.
