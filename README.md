# Programming 2B Portfolio of Evidence (POE)  Part 1

# Contract Monthly Claim System (CMCS) Documentation

## Database Structure

I chose to use a relational database to store the data for the Contract Monthly Claim System (CMCS). This will help the system manage the relationships between different entities. My database structure consists of several table listed below:

- User
- Admin
- Contract
- Claim
- ClaimItem
- Payment
- FAQ
- ClaimStatus

The User table is responsible for storing information about the users of the system, including lecturers and administrators. The Admin table contains information about the administrators who are responsible for processing claims. The Contract table will store information about the contracts between the lecturers and the institution. The claim table will store information about the claims submitted by the lecturers. The ClaimItem table will store information about the items included in each claim. The payment table will store financial information related to the claims. The FAQ table will store frequently asked questions and answers. The ClaimStatus table will store the status of each claim pending, approved, or rejected. The relationships between these tables are shown in the UML class diagram.

## GUI Layout


### Design Choices

I chose to design the application in ASP.NET Core MVC using C# because it provides a flexible and scalable framework for building web applications. This framework allows for quick deployment and is easy to develop and maintain. MVC has a clear separation of files which makes it easy to manage, structure and maintain the code. The web application is written in .NET 8.0 which is the LTS (Long Stable Version) and is continuously supported by Microsoft. 


### Landing Page

The landed page is designed to capture the users attention and provide a brief overview of the system. The page contains a background video to create a visually appealing experience for the user. The video will stop after 10 seconds ensuring the user is not distracted and able to read the contents of page. The page contains a list of FAQs to provide the user with quick answers to common questions. The design theme is light and clean with a simple layout to make it easy for the user to navigate. 

### Registration Form

Once logged in the user will be prompted to enter the following information when submitting a claim:

- Name 
- Surname 
- Email 
- Phone Number
- Claim
- Hours Worked
- Hourly Rate
- Additional Notes
- Choose File

I have designed the GUI to be user-friendly and simple to navigate. I achieved this with the use of input boxes with placeholders providing clear instructions on what information is required. The form has a clean layout with a background image to enhance the visual appeal whilst maintaining a professional look. The submit button is clear and easy to locate making it easy for user to submit their claims. There are hover effects on the buttons to provide visual feedback to the user as well as messages informing the user to fill in all the required fields ensuring that all necessary information is provided.


### Processing Form

The processing form is designed to help administrators process and track claims. The form displays the following information:

- Claim ID
- Contract ID
- Submission Date
- Pending, Approved, or Rejected

The form is designed to layout the information in a clear and concise manner. The buttons are clearly labeled and color coded to indicate Approve, Track, and Reject. The form has a clean look with a background  image to enhance the visual appeal. The form is easy to navigate with hover effects on the buttons to provide visual feedback to the user.

## Assumptions and Constraints

**Assumptions:** 

- The system will be used by lecturers and administrators.
- The system will be accessed via a standard web browser.
- User have an internet connection.
- User have basic computer skills.

**Constraints:**

- The system will not be accessible offline.
- The system is limited to lecturers and administrators only.
- The system must adhere to data protection regulations.
- Security standards must be maintained to protect user data.
