# MembershipManager
Welcome to the MembershipManagemer project! This repository houses our database application developed as part of the BDR course on relational databases. Our goal is to create an efficient system for managing subscribers, billing, and administrative tasks for various types of structures, such as fitness centers, climbing centers, and more.

---

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

<!-- code_chunk_output -->

- [MembershipManager](#membershipmanager)
  - [Project Phases](#project-phases)
    - [I. Requirements Specification - October 11, 2023](#i-requirements-specification---october-11-2023)
    - [II. Conceptual Modeling - October 29, 2023](#ii-conceptual-modeling---october-29-2023)
    - [III. Relational Modeling - November 22, 2023](#iii-relational-modeling---november-22-2023)
    - [IV. Queries, Views, and Triggers - December 10, 2023](#iv-queries-views-and-triggers---december-10-2023)
    - [V. Database Application - February 21, 2024](#v-database-application---february-21-2024)
  - [Project Background](#project-background)
    - [Problem Statement](#problem-statement)
    - [Proposal](#proposal)
  - [Technical Specifications](#technical-specifications)
    - [Data Requirements](#data-requirements)
    - [Features](#features)
  - [Deployment](#deployment)
    - [1. Install Database](#1-install-database)
    - [2. Create + populate database](#2-create--populate-database)
    - [3. Install .NET 8.0 Runtime](#3-install-net-80-runtime)
    - [4. Download Latest Release](#4-download-latest-release)
    - [5. Edit Configuration File](#5-edit-configuration-file)
    - [6. Run Application](#6-run-application)

<!-- /code_chunk_output -->


---

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

---

## Project Background
Source of the Idea
Our project idea was inspired by a real-world problem faced by one of our group members. They encountered administrative challenges when taking over a fitness establishment in Lausanne, which relied on outdated manual processes.

### Problem Statement
The existing administrative processes were paper-based, leading to difficulties in subscriber management and payment tracking. Our project aims to address these challenges by creating a digital solution that can manage subscribers, billing, and payment reminders efficiently.

### Proposal
To resolve this administrative problem, our solution will compile a list of subscribers along with their billing and payment data. This will enable us to track payments, identify overdue invoices, and generate, send, or print payment reminders as needed, in addition, we will be adding a number of improvements to simplify administrative tasks

---
 
## Technical Specifications

### Data Requirements
Our database system will store and link the following elements:

- **not yet implemented** - `Users`: Individuals who access the application, including administrators and regular users.
- **not yet implemented** `Employee`: Information about individuals working within the organization, including roles and employment details.
- **not yet implemented** `Roles` and Access: Permissions and privileges assigned to users within the application.
- `Members`: Individuals who have subscribed to various structures (e.g., gyms, fitness centers).
- **not yet implemented** `Courses`: Educational content or training programs that members can enroll in.
- Available `Products`/`Subscriptions`: Offerings that members can purchase or subscribe to.
- `Billing` process and Control: Mechanisms for managing payments and invoices.
- **not yet implemented** Data `Templates` to Generate `Documents`: Templates for creating various types of documents.
- `Cashier`: Responsible for handling payment transactions.
- **not yet implemented** `Charges` and `Maintenance`: Activities related to infrastructure repair and maintenance.
- **not yet implemented** Accounting `Report`: Generates financial summaries based on collected data.

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
Voici la version corrigée en anglais de votre texte :

---

## Deployment

This application works with `.NET 8.0` and uses [PostgreSQL](https://www.postgresql.org/) as the database driver.

### 1. Install Database

If you already have a Database running PostgreSQL, you can skip this section.

Dockerize or install the database on PC:

- For PC: https://www.postgresql.org/download/
- **Recommended** For Docker:
    - Install `Docker Desktop`: https://www.docker.com/products/docker-desktop/
    - Run `docker pull postgres`
    - Create a `docker-compose.yaml` file and add the following code block, editing `YOUR-DOCKER-PATH`, `YOUR-USERNAME`, and `YOUR-PASSWORD`:
    ```yaml
  version: '3.8'
  networks:
    membership-net:
      driver: bridge
  
  services:
    postgresql:
      image: 'bitnami/postgresql:16'
      container_name: membership_db
      environment:
        - POSTGRESQL_USERNAME=<Username>
        - POSTGRESQL_PASSWORD=<Password>
        - POSTGRESQL_DATABASE=membershipmanager
        - POSTGRESQL_POSTGRES_PASSWORD=root
      ports:
        - 5432:5432
      volumes:
        - .:/data:ro
      networks:
        - membership-net
    ```

### 2. Create + populate database

This is not mandatory as the database is automaticly created when you run the soft for the first time. 
But if you want to populate the databse to make some test, execute theses script from datagrip or others tools to query the db.

It's important to follow this order :
- database.sql
- canton.sql
- city.sql
- structure.sql
- franchise.sql
- product.sql
- entry.sql
- person.sql
- member.sql
- paiement.sql
- bill.sql
- consumption.sql

### 3. Install .NET 8.0 Runtime

If you don't have the `.NET 8.0 Runtime`, you must install it first. Here's the link to download the runtime:
- x64 installer: https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.1-windows-x64-installer

### 4. Download Latest Release

Download the latest release `MembershipManager<Latest>.zip` in this section: [Release](https://github.com/AlexandreIorio/MembershipManager/releases)

Extract the zip where you want to use the application. Example: `C:\Program Files\MembershipManager`

### 5. Edit Configuration File

Edit `MembershipManager.dll.config` and replace the values of `IP ADDRESS OF SERVER HOST`, `USER`, `PASSWORD` with your database configuration.
```xaml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <!-- Edit the following values to match your database configuration. -->
    <add key="Host" value="IP ADDRESS OF SERVER HOST"/>
    <add key="Port" value="5432"/>
    <add key="User" value="USER"/>
    <add key="Password" value="PASSWORD"/>
    <add key="Database" value="DATABASE NAME"/>

    <!-- Keep these values as default. -->
    <add key="UseSchema" value="True" />
    <add key="Schema" value="membershipmanager"/>

  </appSettings>
</configuration>

```

### 6. Run Application

Go to your MembershipManager folder and run `membershipManager.exe`


