CREATE DATABASE CalendarDB
GO

USE CalendarDB
GO

CREATE TABLE Users (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Email NVARCHAR(255) NOT NULL,
	Password VARCHAR(88) NOT NULL,

	CONSTRAINT UQ_Email UNIQUE(Email)
)
GO

CREATE TABLE Calendars (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	UserId INT NOT NULL,

	CONSTRAINT FK_CalendarUserId_UserId FOREIGN KEY(UserId) REFERENCES Users(Id)
)
GO

CREATE TABLE Events (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	CalendarId INT NOT NULL,
	Title NVARCHAR(255) NULL,
	Location NVARCHAR(255) NULL,
	Description NVARCHAR(MAX) NULL,
	Color CHAR(7) NULL

	CONSTRAINT FK_EventCalendarId_CalendarId FOREIGN KEY(CalendarId) REFERENCES Calendars(Id)
)
GO

CREATE TABLE Schedules (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	StartDateUTC DATETIME2 NOT NULL,
	EndDateUTC DATETIME2 NOT NULL,
	SeriesEndDateUTC DATETIME2 NULL,
	FreqType INT NOT NULL,
	FreqSubtype INT NOT NULL,
	FreqInterval INT NOT NULL,

	CONSTRAINT FK_ScheduleId_EventId FOREIGN KEY(Id) REFERENCES Events(Id)
) 
GO

CREATE INDEX IX_StartDateUTC ON Schedules(StartDateUTC)
GO

-- Passowrd: emperor
INSERT INTO Users VALUES(N'skywalker@jedi.com', '$2a$10$B7gcQje1TQwSOeLEYvExyukZzgvEfkUzF./WZypSmUEMfqaLul6iu')
GO

INSERT INTO Calendars(Name, UserId) SELECT 'Basic Calendar', u.Id FROM Users u WHERE u.Email = N'skywalker@jedi.com'
GO