--dummy data voor database
INSERT INTO Camping (CampingNaam, Plattegrondfoto)
VALUES ('Camping', NULL);

INSERT INTO CampingPlaats (Plaatsnummer, CampingID, PrijsPerNacht, Grootte, PositieX, PositieY)
VALUES 
(1000, 1, 25.0, 50.0, 100, 150),
(1001, 1, 30.0, 60.0, 120, 200), 
(1002, 1, 35.0, 70.0, 150, 180),  
(1003, 1, 40.0, 80.0, 200, 220),
(1004, 1, 45.0, 90.0, 250, 170); 

INSERT INTO GebouwFaciliteit (Plaatsnummer, Naam, Afstand)
VALUES
(1000, 'Hoofdgebouw', 100.0),
(1000, 'Toiletgebouw', 50.0),
(1001, 'Hoofdgebouw', 150.0),
(1001, 'Toiletgebouw', 70.0),
(1002, 'Hoofdgebouw', 200.0),
(1002, 'Toiletgebouw', 80.0),
(1003, 'Hoofdgebouw', 50.0),
(1003, 'Toiletgebouw', 60.0),
(1004, 'Hoofdgebouw', 90.0),
(1004, 'Toiletgebouw', 40.0);

INSERT INTO AnderFaciliteit (Plaatsnummer, Naam)
VALUES
(1000, 'Elektriciteit'),
(1000, 'Water'),
(1001, 'Elektriciteit'),
(1002, 'Water'),
(1003, 'Elektriciteit'),
(1004, 'Water');

INSERT INTO Booking (BookingID, Plaatsnummer, Begindatum, Einddatum)
VALUES
(1, 1000, '2024-01-01', '2024-01-05'),
(2, 1001, '2024-02-10', '2024-02-15'),
(3, 1002, '2024-03-20', '2024-03-25'),
(4, 1003, '2024-04-01', '2024-04-07'),
(5, 1004, '2024-05-15', '2024-05-20');

INSERT INTO PersoonsGegevens (BookingID, Naam, Email, Telefoonnr)
VALUES
(1, 'Persoon1', 'persoon1@example.com', 1234567890),
(2, 'Persoon2', 'persoon2@example.com', 1234567891),
(3, 'Persoon3', 'persoon3@example.com', 1234567892),
(4, 'Persoon4', 'persoon4@example.com', 1234567893),
(5, 'Persoon5', 'persoon5@example.com', 1234567894);
