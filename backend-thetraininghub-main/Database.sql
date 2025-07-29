

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
INSERT INTO users (Name, Email, PasswordHash, Level, Strength, Endurance, ConsistencyStreak, Gold, Role) VALUES
('Alice', 'alice@example.com', 'hashedpassword1', 3, 10, 8, 5, 200, 'userNormal'),
('Bob', 'bob@example.com', 'hashedpassword2', 2, 5, 4, 2, 100, 'userMaster'),
('Charlie', 'charlie@example.com', 'hashedpassword3', 5, 20, 10, 7, 500, 'userStaff');

-- Items
INSERT INTO items (Name, Type, Bonus, Price) VALUES
('Iron Dumbbell', 'Strength', 5, 50),
('Endurance Boots', 'Endurance', 3, 40),
('Gold Sword', 'Strength', 10, 150),
('Marathon Shoes', 'Endurance', 8, 120);

-- Plans
INSERT INTO plans (UserId, Description) VALUES
(1, 'Workout plan: Strength training every Monday and Wednesday'),
(2, 'Endurance plan: Running 3 times a week'),
(3, 'Hybrid plan: Mix of strength and endurance');

-- Purchases
INSERT INTO purchases (UserId, ItemId, purchasedate) VALUES
(1, 1, '2023-06-01 10:00:00'), 
(1, 2, '2023-06-01 10:00:00'), 
(2, 4, '2023-06-01 10:00:00'), 
(3, 3, '2023-06-01 10:00:00'),; 

-- Rooms
INSERT INTO rooms (Name, MinLevel, MinStats, MinConsistency) VALUES
('Beginner Gym', 1, 0, 0),
('Intermediate Zone', 3, 10, 2),
('Elite Arena', 5, 25, 5);

-- UsersRooms
INSERT INTO usersrooms (UserId, RoomId) VALUES
(1, 1), 
(1, 2), 
(2, 3), 
(3, 3), 
(3, 2), 
(3, 1);

INSERT INTO tasks ("userId", "createdat", "id", "difficulty", "reward", "iscompleted", "name", "description")
VALUES
(1, NOW(), 1, 2, 50, false, 'Estudiar matemáticas', 'Repasar los capítulos 3 y 4 del libro de álgebra'),
(2, NOW() - INTERVAL '1 day', 2, 3, 70, false, 'Hacer ejercicio', '30 minutos de trote suave y estiramientos'),
(3, NOW() - INTERVAL '2 days', 3, 1, 30, true, 'Leer un libro', 'Leer 20 páginas de la novela asignada'),
(1, NOW(), 4, 4, 100, false, 'Desarrollar código', 'Implementar la función de login en el backend'),
(2, NOW(), 5, 2, 40, true, 'Organizar el escritorio', 'Limpiar y organizar documentos del escritorio');
