# MembershipManager - Project Requirements Document

## Objective

As part of the BDR course covering relational databases, we are required to develop a database application that must be submitted by `21 January, 2024`. The project will be carried out in 5 phases.

## Organization

### Project participants
Colin Jaques - Student - https://github.com/CoJaques

Walid Slimani - Student - https://github.com/SlWa99

Alexandre Iorio - Student - https://github.com/AlexandreIorio


### I. Requirements Specification - 11 October, 2023
- Detailed description of the needs analysis. This analysis will include both data requirements and functional requirements.

### II. Conceptual Modeling - 29 October, 2023
- Creation of the conceptual database schema in UML format.

### III. Relational Modeling - 22 November, 2023
- Transformation of the conceptual schema into a relational schema.
- Creation of the database.

### IV. Queries, Views, and Triggers - 10 December, 2023
- Writing queries and creating views and automatic triggers.

### V. Database Application - 21 February, 2024
- Development of an application in `C#` for visualizing and manipulating the database.
- Use of an API to connect the application to the database.

## Project

### Source of the Idea
In the professional context of one of the group members, a discussion with one of their colleagues triggered our project. This individual had taken over a fitness establishment in Lausanne that had outdated administrative processes. The overwhelming administrative workload of this person prompted us to offer our assistance.

### Problem Statement
The previous owner managed the affairs of the fitness establishment without any computer assistance; subscriber management was done using paper documents, most of which are either not retrievable or have been lost. To this day, individuals, who purchased a subscription, show up at the establishment, and the current manager struggles to determine when and how members took out their subscriptions. Furthermore, billing and payment reminders are handled manually using paper cards with hand-written dates to track the invoices due dates. Invoices are manually created in a word processing software without using any mailing process. Currently, there are just over 400 members to manage with limited computer support.

### Proposal
To address this significant administrative problem, it is crucial to compile a list of subscribers along with their billing and payment data. This would enable made payments tracking, overdue invoices identification, and, if necessary, automatic generation, sending or printing payment reminders.



## Technical specifications

### Data requirements
To establish such a computerized structure, a database system will be required to store and link the following elements:

#### Users
Users represent individuals who access the application. They may include both administrators and regular users who interact with various features and functionalities.

#### Roles and Access
Roles and Access refer to the permissions and privileges assigned to users within the application. This entity helps define who can perform specific actions and access certain parts of the system.

#### Employee
The Employee entity represents individuals who work within the organisation. This entity include attributes to manage and track employee information, roles, and employment details, including the ability to manage contracts and salaries.

#### Members
Members within the application represent individuals who have subscribed to a strucutre, such as gym or fitness center, Climbing center or many other types of strucutre. 

#### Courses
Courses represent educational content or training programs that members can enroll in or access. This entity may include course details, schedules, and resources.

#### Available Products/Subscriptions
Available Products/Subscriptions encompass the offerings members can purchase or subscribe inside the structure. This entity includes product information, pricing, and subscription plans.

#### Billing Process and Control
Billing Process and Control involve the mechanisms and procedures for managing payments and invoices. This entity helps track financial transactions and ensures proper billing procedures are in place.

#### Data Templates to Generate Documents
Data Templates to Generate Documents are templates used to create various types of documents within your application. These templates streamline the process of generating reports, certificates, or other customized documents.

#### Cashier
The Cashier entity is responsible for handling payment transactions and processing financial transactions inside the strucutre. It interacts with billing information and payment gateways.
 
#### Charges and maintenance
This entity covers activities and expenditure relating to the repair and overall maintenance of infrastructure, equipment and facilities.

#### Accounting Report
Accounting Reports generate financial summaries and insights based on the data collected by your application. These reports help users and administrators make informed financial decisions.


### Features
With this program, a `User` will be able to register `Members` who will subscribe to a `Subscription` provided by the organization. A `Member` can purchase `Products` which can be paid directly to the `Cashier` or recorded in an `Account`. Additionally, a `Member` can enroll in a `Course` within a designated `Room` with an assigned `Teacher`.

A `User` will have the capability to generate `Bills`, print them, or send them via email. Payments will be entered into the system to verify if the `Bills` have been settled. In cases where they are not paid, automatic `Reminders` will be sent.

A `User` with a sufficiently high `Role` will have the authority to manage `Employees`, handle `Charges`, and oversee `Maintenance`. They will also be able to generate various types of `Documents` and `Reports`.





