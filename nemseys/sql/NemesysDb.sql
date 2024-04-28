-- Create the database
CREATE DATABASE nemesis;

-- Create Profiles table
CREATE TABLE Profiles (
    ProfileId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    ProfilePicture VARBINARY(MAX),
    ProfileType NVARCHAR(20) CHECK (ProfileType IN ('Reporter', 'Investigator'))
);

-- Create Credentials table
CREATE TABLE Credentials (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    ProfileId INT NOT NULL,
    Salt VARBINARY(MAX),
    FOREIGN KEY (ProfileId) REFERENCES Profiles(ProfileId)
);

-- Create Reports table
CREATE TABLE Reports (
    ReportId INT PRIMARY KEY IDENTITY,
    DateOfReport DATETIME NOT NULL,
    HazardLocation NVARCHAR(255) NOT NULL,
    DateAndTimeSpotted DATETIME NOT NULL,
    TypeOfHazard NVARCHAR(50) NOT NULL,
    TitleOfReport NVARCHAR(255),
    Description NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    ImageUrl NVARCHAR(MAX),
    Upvotes INT NOT NULL DEFAULT 0
);

-- Create Investigations table
CREATE TABLE Investigations (
	InvestigationId INT PRIMARY KEY IDENTITY,
	DateOfInvestigation DATETIME NOT NULL,
	ReportId INT NOT NULL,
	InvestigatorId INT NOT NULL,
	FOREIGN KEY (ReportId) REFERENCES Reports(ReportId),
	FOREIGN KEY (InvestigatorId) REFERENCES Profiles(ProfileId) 
);

