CREATE TABLE Profiles (
    ProfileId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    ProfileType NVARCHAR(20) CHECK (ProfileType IN ('Reporter', 'Investigator'))
    PasswordHash NVARCHAR(255) NOT NULL
);

CREATE TABLE Credentials (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    ProfileId INT NOT NULL,
    FOREIGN KEY (ProfileId) REFERENCES Profiles(ProfileId)
);
