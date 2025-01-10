ALTER TABLE facility
DROP COLUMN LocationX,
DROP COLUMN LocationY;

ALTER TABLE facility
ADD COLUMN PositionX DOUBLE,
ADD COLUMN PositionY DOUBLE;

ALTER TABLE `campingapplicatie`.`facility` 
CHANGE COLUMN `Name` `Type` VARCHAR(255) NOT NULL ;

ALTER TABLE `campingapplicatie`.`facility` 
DROP FOREIGN KEY `fk_Faciliteit_campingplaats1`;
ALTER TABLE `campingapplicatie`.`facility` 
CHANGE COLUMN `SpotNr` `ID` INT(11) NOT NULL FIRST;

ALTER TABLE `campingapplicatie`.`facility` 
DROP FOREIGN KEY `fk_Faciliteit_campingplaats1`;

ALTER TABLE `campingapplicatie`.`facility` 
DROP PRIMARY KEY,
ADD PRIMARY KEY (`ID`, `Type`);

CREATE TABLE intersections (
	ID INT NOT NULL,
	PositionX DOUBLE NOT NULL,
	PositionY DOUBLE NOT NULL,
    PRIMARY KEY(ID)
);

CREATE TABLE roads (
	Intersection1_ID INT NOT NULL,
    Intersection2_ID INT NOT NULL,
    PRIMARY KEY (Intersection1_ID, Intersection2_ID),
    FOREIGN KEY (Intersection1_ID) REFERENCES intersections(ID) ON DELETE CASCADE,
    FOREIGN KEY (Intersection2_ID) REFERENCES intersections(ID) ON DELETE CASCADE
);