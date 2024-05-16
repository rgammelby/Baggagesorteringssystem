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
INSERT INTO Airlines (name) VALUES ('SAS'), ('Norwegian'), ('EasyJet');

-- Eksempeldata til flymodeller
INSERT INTO Aircrafts (model, total_seats) VALUES ('Boeing 737', 189), ('Airbus A320', 180), ('Embraer E190', 100);

-- Eksempeldata til destinationer
INSERT INTO Destinations (city, country, airport_code) VALUES ('Copenhagen', 'Denmark', 'CPH'), ('Stockholm', 'Sweden', 'ARN'), ('London', 'UK', 'LHR');

-- Eksempeldata til flyveplaner
INSERT INTO Flights (flight_number, departure_time, arrival_time, airline_id, aircraft_id, destination_id)
VALUES ('SK123', '2024-06-01 08:00:00', '2024-06-01 10:00:00', 1, 1, 3),
       ('DY456', '2024-06-01 12:00:00', '2024-06-01 14:00:00', 2, 2, 1),
       ('EJ789', '2024-06-01 15:00:00', '2024-06-01 17:00:00', 3, 3, 2);

-- Eksempeldata til passagerer
INSERT INTO Passengers (first_name, last_name, passport_number, flight_id, boarding_pass_number)
VALUES ('John', 'Doe', '123456789', 1, 'BP001'),
       ('Jane', 'Smith', '987654321', 2, 'BP002'),
       ('Alice', 'Johnson', '192837465', 3, 'BP003');

-- Beregn og indsæt ledige sæder for hver flyvning
INSERT INTO AvailableSeats (flight_id, available_seats)
SELECT f.flight_id, (a.total_seats - COUNT(p.passenger_id)) AS available_seats
FROM Flights f
JOIN Aircrafts a ON f.aircraft_id = a.aircraft_id
LEFT JOIN Passengers p ON f.flight_id = p.flight_id
GROUP BY f.flight_id;

-- Forespørgsel for at hente passagerliste med destination, fly og boardingkortnummer
DROP PROCEDURE IF EXISTS ExtractData;

DELIMITER //

CREATE PROCEDURE ExtractData()
BEGIN
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
END //
DELIMITER ;