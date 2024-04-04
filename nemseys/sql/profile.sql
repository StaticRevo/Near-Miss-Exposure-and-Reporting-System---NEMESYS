CREATE DATABASE nemesis

CREATE TABLE Profiles (
    ProfileId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    ProfileType NVARCHAR(20) CHECK (ProfileType IN ('Reporter', 'Investigator'))
);

CREATE TABLE Credentials (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    ProfileId INT NOT NULL,
    FOREIGN KEY (ProfileId) REFERENCES Profiles(ProfileId)
);

CREATE TABLE Reports (
    ReportId INT PRIMARY KEY IDENTITY,
    DateOfReport DATETIME NOT NULL,
    HazardLocation NVARCHAR(255) NOT NULL,
    DateAndTimeSpotted DATETIME NOT NULL,
    TypeOfHazard NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Open', 'Closed', 'Being Investigated', 'No Action Required')),
    ReporterId INT NOT NULL,
    Photo VARBINARY(MAX), -- optional, consider file storage for scalability
    Upvotes INT NOT NULL DEFAULT 0,
    FOREIGN KEY (ReporterId) REFERENCES Profiles(ProfileId)
);

CREATE TABLE Investigations (
    InvestigationId INT PRIMARY KEY IDENTITY,
    ReportId INT NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NOT NULL,
    DateOfAction DATETIME NOT NULL,
    InvestigatorId INT NOT NULL,
    FOREIGN KEY (ReportId) REFERENCES Reports(ReportId),
    FOREIGN KEY (InvestigatorId) REFERENCES Profiles(ProfileId)
);
