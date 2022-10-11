DROP DATABASE IF EXISTS bd;
CREATE DATABASE bd;

USE bd;

CREATE TABLE jugadores (
id INTEGER NOT NULL,
username VARCHAR(20) NOT NULL,
pword VARCHAR(20) NOT NULL,
mail VARCHAR(50),
PRIMARY KEY(ID)
);


CREATE TABLE partidas(
id INTEGER NOT NULL,
num INTEGER NOT NULL,
fecha VARCHAR(20),
hora INT,
resultado VARCHAR(20) NOT NULL,
PRIMARY KEY(ID)
);

CREATE TABLE registro (
id_j INTEGER NOT NULL,
id_p INTEGER NOT NULL,
FOREIGN KEY(id_j) REFERENCES jugadores(id),
FOREIGN KEY(id_p) REFERENCES partidas(id)
);


INSERT INTO jugadores (id,username,pword,mail) VALUES (1,'enric','1234','enric.guasch@estudiantat.upc.edu');

INSERT INTO jugadores (id,username,pword,mail) VALUES (2,'pau','1234','pau.gimenez@estudiantat.upc.edu');

INSERT INTO jugadores (id,username,pword,mail) VALUES (3,'gerard','1234','gerard.lopez@estudiantat.upc.edu');

INSERT INTO jugadores (id,username,pword,mail) VALUES (4,'oussama','1234','oussama.ayat@estudiantat.upc.edu');

INSERT INTO jugadores (id,username,pword,mail) VALUES (5,'alex','1234','alex.berrocal@estudiantat.upc.edu');

INSERT INTO jugadores (id,username,pword,mail) VALUES (6,'ingrid','1234','ingrid.hinno@estudiantat.upc.edu');

INSERT INTO partidas (id,num,fecha,hora,resultado) VALUES (1,3,'26/09/22',14,'Victoria');

INSERT INTO partidas (id,num,fecha,hora,resultado) VALUES (2,4,'26/09/22',18,'Derrota');

INSERT INTO partidas (id,num,fecha,hora,resultado) VALUES (3,3,'27/09/22',20,'Victoria');

INSERT INTO partidas (id,num,fecha,hora,resultado) VALUES (4,3,'28/09/22',10,'Derrota');

INSERT INTO partidas (id,num,fecha,hora,resultado) VALUES (5,2,'29/09/22',14,'Victoria');



INSERT INTO registro (id_j,id_p) VALUES (1,1);

INSERT INTO registro (id_j,id_p) VALUES (2,1);

INSERT INTO registro (id_j,id_p) VALUES (3,1);

INSERT INTO registro (id_j,id_p) VALUES (1,2);

INSERT INTO registro (id_j,id_p) VALUES (2,2);

INSERT INTO registro (id_j,id_p) VALUES (5,2);

INSERT INTO registro (id_j,id_p) VALUES (6,2);

INSERT INTO registro (id_j,id_p) VALUES (2,3);

INSERT INTO registro (id_j,id_p) VALUES (3,3);

INSERT INTO registro (id_j,id_p) VALUES (4,3);

INSERT INTO registro (id_j,id_p) VALUES (1,4);

INSERT INTO registro (id_j,id_p) VALUES (4,4);

INSERT INTO registro (id_j,id_p) VALUES (6,4);

INSERT INTO registro (id_j,id_p) VALUES (2,5);

INSERT INTO registro (id_j,id_p) VALUES (3,5);







