# MembershipManagement
Welcome to the MembershipManagement project! This repository houses our database application developed as part of the BDR course on relational databases. Our goal is to create an efficient system for managing subscribers, billing, and administrative tasks for various types of structures, such as fitness centers, climbing centers, and more.

## Project Phases
We have divided this project into five phases, each with its unique focus and objectives:

### I. Requirements Specification - October 11, 2023
In this phase, we will conduct a detailed needs analysis, encompassing both data and functional requirements. This stage will provide a solid foundation for the rest of the project.

### II. Conceptual Modeling - October 29, 2023
We will create a conceptual database schema in UML format during this phase. This schema will serve as a visual representation of our database structure.

### III. Relational Modeling - November 22, 2023
During this phase, we will transform the conceptual schema into a practical relational schema. This will involve creating the actual database to store our data.

### IV. Queries, Views, and Triggers - December 10, 2023
We will write queries, create views, and implement automatic triggers to enable seamless data manipulation and retrieval.

### V. Database Application - February 21, 2024
The final phase involves developing a cross-platform application using C# with Maui for visualizing and interacting with the database. We will also integrate an API to establish a connection between the application and the database.

## Project Background
Source of the Idea
Our project idea was inspired by a real-world problem faced by one of our group members. They encountered administrative challenges when taking over a fitness establishment in Lausanne, which relied on outdated manual processes.

### Problem Statement
The existing administrative processes were paper-based, leading to difficulties in subscriber management and payment tracking. Our project aims to address these challenges by creating a digital solution that can manage subscribers, billing, and payment reminders efficiently.

### Proposal
To resolve this administrative problem, our solution will compile a list of subscribers along with their billing and payment data. This will enable us to track payments, identify overdue invoices, and generate, send, or print payment reminders as needed, in addition, we will be adding a number of improvements to simplify administrative tasks

## Technical Specifications

### Data Requirements
Our database system will store and link the following elements:

- `Users`: Individuals who access the application, including administrators and regular users.
- `Employee`: Information about individuals working within the organization, including roles and employment details.
- `Roles` and Access: Permissions and privileges assigned to users within the application.
- `Members`: Individuals who have subscribed to various structures (e.g., gyms, fitness centers).
- `Courses`: Educational content or training programs that members can enroll in.
- Available `Products`/`Subscriptions`: Offerings that members can purchase or subscribe to.
- `Billing` process and Control: Mechanisms for managing payments and invoices.
- Data `Templates` to Generate `Documents`: Templates for creating various types of documents.
- `Cashier`: Responsible for handling payment transactions.
- `Charges` and `Maintenance`: Activities related to infrastructure repair and maintenance.
- Accounting `Report`: Generates financial summaries based on collected data.

### Features
Our application will provide the following features:

- Registration of Members and management of their subscriptions.
- Purchase of Products with payment handling.
- Enrollment in Courses within designated rooms with assigned teachers.
- Generation, printing, and emailing of Bills.
- Payment tracking and automatic Reminders for unpaid bills.
- User roles and permissions management.
- Employee management, charge handling, and maintenance oversight.
- Document and report generation for informed financial decisions.
