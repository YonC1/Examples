use FinalProject;

DROP TABLE IF EXISTS Rating; 
DROP TABLE IF EXISTS Movie;
DROP TABLE IF EXISTS FinalUser;

DROP PROCEDURE IF EXISTS spGetAUserById;
DROP PROCEDURE IF EXISTS spGetAUserByUserAndPass;
DROP PROCEDURE IF EXISTS spGetAllUsers;
DROP PROCEDURE IF EXISTS spPostNewFinalUser;
DROP PROCEDURE IF EXISTS spPutUserActiveState;
DROP PROCEDURE IF EXISTS spDeleteAUserByGuid;

DROP PROCEDURE IF EXISTS spGetAllMovies;
DROP PROCEDURE IF EXISTS spGetMovieByMovieID;
DROP PROCEDURE IF EXISTS spGetMoviesAfterDate;
DROP PROCEDURE IF EXISTS spPostANewMovie;
DROP PROCEDURE IF EXISTS spPutAMovie;
DROP PROCEDURE IF EXISTS spDeleteMovieByMovieID;
DROP PROCEDURE IF EXISTS spGetMoviesAndRatingsByMovieID;
DROP PROCEDURE IF EXISTS spGetMoviesWithAndWithoutRatings;

DROP PROCEDURE IF EXISTS spGetAllRatings;
DROP PROCEDURE IF EXISTS spGetAllRatingsByAUser;
DROP PROCEDURE IF EXISTS spGetARatingByARating;
DROP PROCEDURE IF EXISTS spGetAllRatingsByID;
DROP PROCEDURE IF EXISTS spPostANewRating;
DROP PROCEDURE IF EXISTS spPutARating;
DROP PROCEDURE IF EXISTS spDeleteRatingByUserIDMovieID;
DROP PROCEDURE IF EXISTS spGetAverageRatingForMovie;
DROP PROCEDURE IF EXISTS spGetAllMoviesWithAveRating;
DROP PROCEDURE IF EXISTS spGetAllUserRatingsForAMovie;

CREATE TABLE FinalUser(
	Id varchar(50) NOT NULL,
	Email varchar(50) NOT NULL,
	UserPassword varchar(256) NOT NULL,
	FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
	ActiveUser int NOT NULL,
    primary key(Email, UserPassword),
    UNIQUE(Id)
);

CREATE TABLE Movie(
	MovieID INT PRIMARY KEY AUTO_INCREMENT,
	Title varchar(256) NOT NULL,
	Postdate date NOT NULL,
	Summary text NOT NULL,
	Link varchar(256) NOT NULL,
	OwnerID varchar(50) NOT NULL,
    FOREIGN KEY (OwnerID)
		REFERENCES FinalUser(Id),
    UNIQUE (Title)
);


CREATE TABLE Rating(	
    MovieID INT NOT NULL,
    UserID varchar(50) NOT NULL,
    Rating float NOT NULL,
    UserComment varchar(4096),
    primary key(MovieID, UserID),
    FOREIGN KEY (MovieID)
		REFERENCES Movie(MovieID)
);

/* Create stored procedures */
/* User stored procedures */
DELIMITER //
CREATE PROCEDURE `spGetAUserByUserAndPass`(in userEmail varchar(50), in password varchar(256))
BEGIN
	SELECT Id, Email, FirstName, LastName, ActiveUser, LevelID FROM FinalUser WHERE Email = UserEmail AND UserPassword = password; 
END//

CREATE PROCEDURE `spGetAUserById`(in Id varchar(50))
BEGIN
	SELECT Id, Email, UserPassword, FirstName, LastName, ActiveUser FROM FinalUser WHERE Id = Id;
END//

CREATE PROCEDURE `spGetAllUsers`()
BEGIN
	SELECT Id, Email, UserPassword, FirstName, LastName, ActiveUser FROM FinalUser WHERE ActiveUser = 1;
END//

CREATE PROCEDURE `spPostNewFinalUser`(Id varchar(50), mail varchar(50), pass varchar(256), fname varchar(50), lname varchar(50), isActive bit)
BEGIN
	INSERT INTO FinalUser (Id, Email, UserPassword, FirstName, LastName, ActiveUser) VALUES (Id, mail, pass, fname, lname, isActive);
END//

CREATE PROCEDURE `spPutUserActiveState`(in GUID varchar(50), in state bit)
BEGIN
	UPDATE FinalUser SET ActiveUser = state WHERE Id = GUID;
END//

CREATE PROCEDURE `spDeleteAUserByGuid`(in GUID varchar(50))
BEGIN
	DELETE FROM FinalUser WHERE Id = GUID;
END//



/* Movie stored procedures */

CREATE PROCEDURE `spGetAllMovies`()
BEGIN
	SELECT MovieID, Title, Postdate, Summary, Link, OwnerID FROM Movie;
END//

CREATE PROCEDURE `spGetMovieByMovieID`(in ID int)
BEGIN
	SELECT Title, Postdate, Summary, Link, OwnerID FROM Movie WHERE MovieID = ID;
END//

CREATE PROCEDURE `spGetMoviesAfterDate`(in movieDate date)
BEGIN
	SELECT MovieID, Title, Summary, Link, OwnerID FROM Movie WHERE Postdate >= movieDate;
END//

CREATE PROCEDURE `spPostANewMovie`(movieTitle varchar(256), movieDate date, movieSummary text, movieLink varchar(256), movieOwner varchar(50))
BEGIN
	INSERT INTO Movie (Title, Postdate, Summary, Link, OwnerID) VALUES (movieTitle, movieDate, movieSummary, movieLink, movieOwner);
END//

CREATE PROCEDURE `spPutAMovie`(in ID int, movieTitle varchar(256), movieDate date, movieSummary text, movieLink varchar(256), movieOwner varchar(50))
BEGIN
	UPDATE Movie SET Title = movieTitle, Postdate = movieDate, Summary = movieSummary, Link = movieLink, OwnerID = movieOwner WHERE MovieID = ID;
END//

CREATE PROCEDURE `spDeleteMovieByMovieID`(in ID int)
BEGIN
	DELETE FROM Movie WHERE MovieID = ID;
END//

CREATE PROCEDURE `spGetMoviesAndRatingsByMovieID`(in ID int)
BEGIN
	SELECT M.Title, M.Postdate, M.Summary, M.Link, R.Rating FROM Movie M
	LEFT JOIN Rating R ON M.MovieID = R.MovieID
    WHERE M.MovieID = ID    
    ORDER BY M.Title;
END//

CREATE PROCEDURE `spGetMoviesWithAndWithoutRatings`()
BEGIN
	SELECT M.MovieID, M.Title, M.Postdate, M.Summary, M.Link, R.Rating, R.UserComment FROM Movie M
	LEFT JOIN Rating R on M.movieID = R.movieID;
END//


/* Rating stored procedures */

CREATE PROCEDURE `spGetAllRatings`()
BEGIN
	SELECT MovieID, UserID, Rating FROM Rating;
END//

CREATE PROCEDURE `spGetAllRatingsByAUser`(in ID varchar(50))
BEGIN
	SELECT MovieID, UserID, Rating FROM Rating WHERE UserID = ID;
END//

CREATE PROCEDURE `spGetARatingByARating`(in mID int, in uID varchar(50))
BEGIN
	SELECT MovieID, UserID, Rating, UserComment FROM Rating WHERE MovieID = mID AND UserID = uID;
END//

CREATE PROCEDURE `spGetAllRatingsByID`(in movID int)
BEGIN
	SELECT M.Title, U.FirstName, R.Rating, R.UserComment FROM Rating R 
INNER JOIN Movie M on M.MovieID = R.MovieID
INNER JOIN FinalUser U on U.Id = R.UserID
WHERE R.MovieID = movID;
END//

CREATE PROCEDURE `spPostANewRating`(movID int, uID varchar(50), ratingValue float, uCom varchar(4096))
BEGIN
	INSERT INTO Movie (MovieID, UserID, Rating, UserComment) VALUES (movID, uID, ratingValue, uCom);
END//

CREATE PROCEDURE `spPutARating`(in movID int, uID varchar(50), movieRating float, uCom varchar(4096))
BEGIN
	UPDATE Rating SET MovieID = movID, UserID = uID, Rating = movieRating, UserComment = uCom WHERE MovieID = movID AND UserID = uID;
END//

CREATE PROCEDURE `spDeleteRatingByUserIDMovieID`(in movID int, uID varchar(50))
BEGIN
	DELETE FROM Rating WHERE MovieID = movID AND UserID = uID;
END//

CREATE PROCEDURE `spGetAverageRatingForMovie`(in movId int)
BEGIN
	SELECT M.Title, avg(R.Rating) as Avg_Rating FROM Rating R, Movie M
	WHERE M.MovieID = R.MovieID AND
    M.MovieID = movId;
END//

CREATE PROCEDURE `spGetAllMoviesWithAveRating`()
BEGIN
	SELECT M.MovieID, M.Title, round(avg(R.Rating), 2) as Avg_Rating FROM Rating R, Movie M
		WHERE M.MovieID = R.MovieID Group by M.MovieID order by M.Title;
END//

CREATE PROCEDURE `spGetAllUserRatingsForAMovie`(in movId int)
BEGIN
	SELECT round(R.Rating, 2) AS Rating, U.FirstName, U.Lastname, R.UserComment FROM Rating R, JetUser U WHERE R.UserID = U.UserGUID
		AND R.MovieID = movId;
END//

DELIMITER ;


INSERT INTO FinalUser (Id, Email, UserPassword, FirstName, LastName, ActiveUser) values ("f2161cae-5fe4-49d6-b61a-73203d94bdf7", "sride@northeaststate.edu", "19513fdc9da4fb72a4a05eb66917548d3c90ff94d5419e1f2363eea89dfee1dd", "Sally", "Ride", 1);
INSERT INTO FinalUser (Id, Email, UserPassword, FirstName, LastName, ActiveUser) values ("5500bdb6-d5f8-4258-91d3-1c2e7db4b317", "gjones@northeaststate.edu", "1be0222750aaf3889ab95b5d593ba12e4ff1046474702d6b4779f4b527305b23", "Grace", "Jones", 1);
INSERT INTO FinalUser (Id, Email, UserPassword, FirstName, LastName, ActiveUser) values ("894cde31-5592-49dc-8a13-803e0007f935", "hjwinkler@northeaststate.edu", "2538f153f36161c45c3c90afaa3f9ccc5b0fa5554c7c582efe67193abb2d5202", "Henry", "Winkler", 0);
INSERT INTO FinalUser (Id, Email, UserPassword, FirstName, LastName, ActiveUser) values ("22541e71-ceba-42c9-8908-eee3792543d1", "rbhayes@northeaststate.edu", "db514f5b3285acaa1ad28290f5fefc38f2761a1f297b1d24f8129dd64638825d", "Ruth", "Hayes", 1);

INSERT INTO Movie (MovieID, Title, PostDate, Summary, Link, OwnerID) values ("1", "Wendell and Wild", "2022-10-21", "Two devious demon brothers Wendell and Wild have to face their arch-enemy with the help of the nun Sister Helly", "https://www.netflix.com/browse/genre/movies", "f2161cae-5fe4-49d6-b61a-73203d94bdf7");
INSERT INTO Movie (MovieID, Title, PostDate, Summary, Link, OwnerID) values ("2", "Labyrinth", "1986-06-27", "Sixteen-year-old Sarah is given thirteen hours to solve a labyrinth and rescue her baby brother Toby when her wish for him to be taken away is granted by the Goblin King.", "https://www.amazon.com/gp/video/detail/amzn1.dv.gti.76a9f742-9562-69cc-0eca-569b042089c5?autoplay=0&ref_=atv_cf_strg_wb", "f2161cae-5fe4-49d6-b61a-73203d94bdf7");
INSERT INTO Movie (MovieID, Title, PostDate, Summary, Link, OwnerID) values ("3", "Ocean's 8", "2018-06-05", "Debbie Ocean assembles an all-female team of thieves and specialists for a heist during the Met Gala, aiming for the $150 million worth of diamonds around the neck of a world-famous actress.", "https://www.youtube.com/watch?v=7qgbh3zH_Dw", "f2161cae-5fe4-49d6-b61a-73203d94bdf7");
INSERT INTO Movie (MovieID, Title, PostDate, Summary, Link, OwnerID) values ("4" ,"Little Monsters", "2019-10-08", "A teacher, a struggling musician, and a kids' show personality join forces to protect young children during an unexpected zombie invasion.", "https://www.hulu.com/movie/little-monsters-1110bd42-c856-4ba3-8093-5377414f1c4c?entity_id=1110bd42-c856-4ba3-8093-5377414f1c4c", "f2161cae-5fe4-49d6-b61a-73203d94bdf7");
INSERT INTO Movie (MovieID, Title, PostDate, Summary, Link, OwnerID) values ("5", "Test Movie", "2022-07-20", "Test Synopsis", "https://www.google.com", "f2161cae-5fe4-49d6-b61a-73203d94bdf7");

INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("1", "f2161cae-5fe4-49d6-b61a-73203d94bdf7", 4.2, "Good movie");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("1", "5500bdb6-d5f8-4258-91d3-1c2e7db4b317", 3, "Ok I reckon");

INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("1", "894cde31-5592-49dc-8a13-803e0007f935", 4, "");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("1", "22541e71-ceba-42c9-8908-eee3792543d1", 5, "");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("2", "f2161cae-5fe4-49d6-b61a-73203d94bdf7", 3.5, "");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("2", "5500bdb6-d5f8-4258-91d3-1c2e7db4b317", 4, "");

INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("2", "894cde31-5592-49dc-8a13-803e0007f935", 1.0, "This movie kept me up at night");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("2", "22541e71-ceba-42c9-8908-eee3792543d1", 2.1, "Goofy film");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("3", "f2161cae-5fe4-49d6-b61a-73203d94bdf7", 0.5, "I lined my cat box with this movie");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("3", "5500bdb6-d5f8-4258-91d3-1c2e7db4b317", 4, "Awesome Movie");

INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("3", "894cde31-5592-49dc-8a13-803e0007f935", 3, "");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("3", "22541e71-ceba-42c9-8908-eee3792543d1", 5, "");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("4", "f2161cae-5fe4-49d6-b61a-73203d94bdf7", 4, "");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("4", "5500bdb6-d5f8-4258-91d3-1c2e7db4b317", 4.2, "");

INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("4", "894cde31-5592-49dc-8a13-803e0007f935", 4.2, "Great!");
INSERT INTO Rating (MovieID, UserID, Rating, UserComment) values ("4", "22541e71-ceba-42c9-8908-eee3792543d1", 2.5, "So-so I suppose.");