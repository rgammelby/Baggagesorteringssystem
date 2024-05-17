-- Opretter en ny database hvis den ikke allerede eksisterer
CREATE DATABASE IF NOT EXISTS airport_schedule;
USE airport_schedule;

-- Opretter tabellen for flyselskaber
CREATE TABLE IF NOT EXISTS Airlines (
    airline_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

-- Opretter tabellen for flymodeller
CREATE TABLE IF NOT EXISTS Aircrafts (
    aircraft_id INT AUTO_INCREMENT PRIMARY KEY,
    model VARCHAR(100) NOT NULL,
    total_seats INT NOT NULL
);

-- Opretter tabellen for destinationer
CREATE TABLE IF NOT EXISTS Destinations (
    destination_id INT AUTO_INCREMENT PRIMARY KEY,
    city VARCHAR(100) NOT NULL,
    country VARCHAR(100) NOT NULL,
    airport_code VARCHAR(10) NOT NULL
);

-- Opretter tabellen for flyveplaner
CREATE TABLE IF NOT EXISTS Flights (
    flight_id INT AUTO_INCREMENT PRIMARY KEY,
    flight_number VARCHAR(10) NOT NULL,
    departure_time DATETIME NOT NULL,
    arrival_time DATETIME NOT NULL,
    airline_id INT,
    aircraft_id INT,
    destination_id INT,
    FOREIGN KEY (airline_id) REFERENCES Airlines(airline_id),
    FOREIGN KEY (aircraft_id) REFERENCES Aircrafts(aircraft_id),
    FOREIGN KEY (destination_id) REFERENCES Destinations(destination_id)
);

-- Opretter tabellen for passagerer
CREATE TABLE IF NOT EXISTS Passengers (
    passenger_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    passport_number VARCHAR(50) NOT NULL,
    flight_id INT,
    boarding_pass_number VARCHAR(50) NOT NULL,
    FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);

-- Opretter tabellen for ledige sæder
CREATE TABLE IF NOT EXISTS AvailableSeats (
    flight_id INT,
    available_seats INT NOT NULL,
    PRIMARY KEY (flight_id),
    FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
);

-- Eksempeldata til flyselskaber
INSERT INTO Airlines (name) VALUES ('SAS'), ('Norwegian'), ('EasyJet'), ('Lufthansa'), ('British Airways'), ('Air France'), ('KLM');

-- Eksempeldata til flymodeller
INSERT INTO Aircrafts (model, total_seats) VALUES ('Boeing 737', 189), ('Airbus A320', 180), ('Embraer E190', 100), ('Boeing 747', 366), ('Airbus A380', 575);

-- Eksempeldata til destinationer
INSERT INTO Destinations (city, country, airport_code) VALUES 
('Copenhagen', 'Denmark', 'CPH'), 
('Stockholm', 'Sweden', 'ARN'), 
('London', 'UK', 'LHR'), 
('Paris', 'France', 'CDG'), 
('Amsterdam', 'Netherlands', 'AMS'),
('New York', 'USA', 'JFK'),
('Tokyo', 'Japan', 'NRT'),
('Sydney', 'Australia', 'SYD'),
('Dubai', 'UAE', 'DXB'),
('Los Angeles', 'USA', 'LAX'),
('Singapore', 'Singapore', 'SIN'),
('Hong Kong', 'China', 'HKG'),
('Frankfurt', 'Germany', 'FRA'),
('Madrid', 'Spain', 'MAD'),
('Rome', 'Italy', 'FCO'),
('Bangkok', 'Thailand', 'BKK'),
('Toronto', 'Canada', 'YYZ'),
('Moscow', 'Russia', 'SVO'),
('Istanbul', 'Turkey', 'IST'),
('São Paulo', 'Brazil', 'GRU');

-- Generer flyveplaner med forskellige destinationer og tilfældige data
DELIMITER $$
CREATE PROCEDURE GenerateFlights()
BEGIN
    DECLARE i INT DEFAULT 1;
    DECLARE flight_time_start DATETIME;
    DECLARE flight_time_end DATETIME;
    DECLARE airline_id INT;
    DECLARE aircraft_id INT;
    DECLARE destination_id INT;
    DECLARE flight_number VARCHAR(10);
    
    SET flight_time_start = '2024-06-01 06:00:00';
    
    WHILE i <= 2000 DO
        SET flight_time_start = DATE_ADD(flight_time_start, INTERVAL FLOOR(RAND() * 12) HOUR);
        SET flight_time_end = DATE_ADD(flight_time_start, INTERVAL FLOOR(RAND() * 10) + 2 HOUR);
        SET airline_id = FLOOR(RAND() * 7) + 1;
        SET aircraft_id = FLOOR(RAND() * 5) + 1;
        SET destination_id = FLOOR(RAND() * 20) + 1;
        SET flight_number = CONCAT('FL', LPAD(i, 4, '0'));
        
        INSERT INTO Flights (flight_number, departure_time, arrival_time, airline_id, aircraft_id, destination_id)
        VALUES (flight_number, flight_time_start, flight_time_end, airline_id, aircraft_id, destination_id);
        
        SET i = i + 1;
    END WHILE;
END$$
DELIMITER ;

CALL GenerateFlights();

-- Generer passagerer og fordel dem på flyafgange
DELIMITER $$
CREATE PROCEDURE GeneratePassengers()
BEGIN
    DECLARE i INT DEFAULT 1;
    DECLARE j INT DEFAULT 1;
    DECLARE flight_count INT;
    DECLARE passenger_count INT;
    DECLARE flight_id INT;
    DECLARE total_seats INT;
    DECLARE first_name VARCHAR(100);
    DECLARE last_name VARCHAR(100);
    DECLARE passport_number VARCHAR(50);
    DECLARE boarding_pass_number VARCHAR(50);

    -- Find total number of flights
    SELECT COUNT(*) INTO flight_count FROM Flights;

    WHILE i <= 15000 DO
        SET flight_id = FLOOR(RAND() * flight_count) + 1;
        SELECT total_seats INTO total_seats FROM Flights f JOIN Aircrafts a ON f.aircraft_id = a.aircraft_id WHERE f.flight_id = flight_id;
        SET first_name = CONCAT('FirstName', LPAD(FLOOR(RAND() * 10000), 4, '0'));
        SET last_name = CONCAT('LastName', LPAD(FLOOR(RAND() * 10000), 4, '0'));
        SET passport_number = CONCAT('P', LPAD(FLOOR(RAND() * 1000000), 6, '0'));
        SET boarding_pass_number = CONCAT('BP', LPAD(FLOOR(RAND() * 1000000), 6, '0'));
        
        INSERT INTO Passengers (first_name, last_name, passport_number, flight_id, boarding_pass_number)
        VALUES (first_name, last_name, passport_number, flight_id, boarding_pass_number);
        
        SET i = i + 1;
    END WHILE;
END$$
DELIMITER ;

CALL GeneratePassengers();

-- Beregn og indsæt ledige sæder for hver flyvning
INSERT INTO AvailableSeats (flight_id, available_seats)
SELECT f.flight_id, (a.total_seats - COUNT(p.passenger_id)) AS available_seats
FROM Flights f
JOIN Aircrafts a ON f.aircraft_id = a.aircraft_id
LEFT JOIN Passengers p ON f.flight_id = p.flight_id
GROUP BY f.flight_id;

-- Forespørgsel for at hente passagerliste med destination, fly og boardingkortnummer
SELECT
    p.first_name,
    p.last_name,
    p.passport_number,
    p.boarding_pass_number,
    f.flight_number,
    d.city AS destination_city,
    d.country AS destination_country,
    a.model AS aircraft_model,
    f.departure_time,
    f.arrival_time
FROM
    Passengers p
JOIN
    Flights f ON p.flight_id = f.flight_id
JOIN
    Destinations d ON f.destination_id = d.destination_id
JOIN
    Aircrafts a ON f.aircraft_id = a.aircraft_id
ORDER BY
    f.departure_time;
