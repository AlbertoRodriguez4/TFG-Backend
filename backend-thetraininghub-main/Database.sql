

-- Crear tabla Users
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Level INTEGER NOT NULL DEFAULT 1,
    Strength INTEGER NOT NULL DEFAULT 0,
    Endurance INTEGER NOT NULL DEFAULT 0,
    ConsistencyStreak INTEGER NOT NULL DEFAULT 0,
    Gold INTEGER NOT NULL DEFAULT 0,
    Role VARCHAR(50) NOT NULL DEFAULT 'userNormal',
    CONSTRAINT chk_Name_Type CHECK (Role IN ('userNormal', 'userStaff', 'userMaster'))
);

-- Crear tabla Items
CREATE TABLE Items (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Type VARCHAR(50) NOT NULL, -- 'Strength' o 'Endurance'
    Bonus INTEGER NOT NULL,
    Price INTEGER NOT NULL
);

-- Crear tabla Plans
CREATE TABLE Plans (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL,
    Description VARCHAR(500) NOT NULL,
    CONSTRAINT fk_Plans_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Crear tabla Purchases
CREATE TABLE Purchases (
    Id SERIAL PRIMARY KEY,
    UserId INTEGER NOT NULL,
    ItemId INTEGER NOT NULL,
    PurchaseDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_Purchases_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT fk_Purchases_Item FOREIGN KEY (ItemId) REFERENCES Items(Id) ON DELETE CASCADE
);

-- Crear tabla Rooms
CREATE TABLE Rooms (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    MinLevel INTEGER NOT NULL DEFAULT 1,
    MinStats INTEGER NOT NULL DEFAULT 0,
    MinConsistency INTEGER NOT NULL DEFAULT 0
);

-- Crear tabla intermedia UsersRooms
CREATE TABLE UsersRooms (
    UserId INTEGER NOT NULL,
    RoomId INTEGER NOT NULL,
    PRIMARY KEY (UserId, RoomId),
    CONSTRAINT fk_UsersRooms_User FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT fk_UsersRooms_Room FOREIGN KEY (RoomId) REFERENCES Rooms(Id) ON DELETE CASCADE
);

-- INSERTS

-- Users
INSERT INTO Users (Name, Email, PasswordHash, Level, Strength, Endurance, ConsistencyStreak, Gold, Role) VALUES
('Alice', 'alice@example.com', 'hashedpassword1', 3, 10, 8, 5, 200, 'userNormal'),
('Bob', 'bob@example.com', 'hashedpassword2', 2, 5, 4, 2, 100, 'userMaster'),
('Charlie', 'charlie@example.com', 'hashedpassword3', 5, 20, 10, 7, 500, 'userStaff');

-- Items
INSERT INTO Items (Name, Type, Bonus, Price) VALUES
('Iron Dumbbell', 'Strength', 5, 50),
('Endurance Boots', 'Endurance', 3, 40),
('Gold Sword', 'Strength', 10, 150),
('Marathon Shoes', 'Endurance', 8, 120);

-- Plans
INSERT INTO Plans (UserId, Description) VALUES
(1, 'Workout plan: Strength training every Monday and Wednesday'),
(2, 'Endurance plan: Running 3 times a week'),
(3, 'Hybrid plan: Mix of strength and endurance');

-- Purchases
INSERT INTO Purchases (UserId, ItemId) VALUES
(1, 1), 
(1, 2), 
(2, 4), 
(3, 3); 

-- Rooms
INSERT INTO Rooms (Name, MinLevel, MinStats, MinConsistency) VALUES
('Beginner Gym', 1, 0, 0),
('Intermediate Zone', 3, 10, 2),
('Elite Arena', 5, 25, 5);

-- UsersRooms
INSERT INTO UsersRooms (UserId, RoomId) VALUES
(1, 1), 
(1, 2), 
(2, 1), 
(3, 1), 
(3, 2), 
(3, 3);
