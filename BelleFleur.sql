DROP DATABASE IF EXISTS Fleurs; 
CREATE DATABASE IF NOT EXISTS Fleurs;
USE Fleurs; 

DROP TABLE IF EXISTS Client;
CREATE TABLE IF NOT EXISTS Client (
id_Client INT AUTO_INCREMENT PRIMARY KEY,
nom VARCHAR(50) NOT NULL,
prenom VARCHAR(50) NOT NULL,
adresse VARCHAR(100) NOT NULL,
email VARCHAR(50) NOT NULL UNIQUE, 
telephone VARCHAR(20) NOT NULL,
fidelite ENUM('bronze', 'or') NOT NULL DEFAULT 'bronze',
total_achats DECIMAL(10,2) DEFAULT 0.0,
CarteCredit INT,
Mdp VARCHAR(50)
);


DROP TABLE IF EXISTS Produit;
CREATE TABLE IF NOT EXISTS Produit ( 
    id_Produit INT AUTO_INCREMENT PRIMARY KEY, 
    nom VARCHAR(50) NOT NULL, 
    type_produit ENUM('fleur', 'accessoire') NOT NULL, 
    prix DECIMAL(10,2) NOT NULL 
);


DROP TABLE IF EXISTS Fleur;
CREATE TABLE IF NOT EXISTS Fleur (
id_Fleur INT NOT NULL,
nom VARCHAR(255) NOT NULL,
prix DECIMAL(10,2) NOT NULL,
disponibilite ENUM('à l année', 'de mai à novembre') NOT NULL,
PRIMARY KEY (id_Fleur),
id_Produit INT,
FOREIGN KEY (id_Produit) REFERENCES Produit(id_Produit)
);


DROP TABLE IF EXISTS Accessoire;
CREATE TABLE IF NOT EXISTS Accessoire (
    id_Accessoire INT AUTO_INCREMENT PRIMARY KEY,
    nom ENUM('Vase', 'Ruban', 'Boite') NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    seuil_alerte INT DEFAULT 10,
    id_Produit INT,
    FOREIGN KEY (id_Produit) REFERENCES Produit(id_Produit)
);

DROP TABLE IF EXISTS Magasin;
CREATE TABLE IF NOT EXISTS Magasin (
id_Magasin INT AUTO_INCREMENT PRIMARY KEY,
nom VARCHAR(50) NOT NULL,
adresse VARCHAR(100) NOT NULL
);

DROP TABLE IF EXISTS Commande;
CREATE TABLE IF NOT EXISTS Commande (
id_Commande INT AUTO_INCREMENT PRIMARY KEY,
id_Client INT,
id_Magasin INT NOT NULL,
DateCommande DATE,
EtatCommande ENUM('VINV', 'CC', 'CPAV', 'CAL', 'CL') NOT NULL,
PrixTotal DECIMAL(10, 2),
FOREIGN KEY (id_Client) REFERENCES Client(id_Client),
FOREIGN KEY (id_Magasin) REFERENCES Magasin(id_Magasin)
); 


#Changer nom tables ?
DROP TABLE IF EXISTS CompositionCommande_Personnalise;
CREATE TABLE IF NOT EXISTS CompositionCommande_Personnalise (
id_Client INT NOT NULL,
id_Commande INT NOT NULL,
bon_commande VARCHAR(1000) NOT NULL,
PRIMARY KEY (id_Client, id_Commande),
FOREIGN KEY (id_Commande) REFERENCES Commande(id_Commande),
FOREIGN KEY (id_Client) REFERENCES Client(id_Client)
);


DROP TABLE IF EXISTS BouquetStandard;
CREATE TABLE IF NOT EXISTS BouquetStandard (
id_BouquetStandard INT AUTO_INCREMENT PRIMARY KEY,
nom VARCHAR(50) NOT NULL,
prix_max DECIMAL(10,2) NOT NULL,
devise VARCHAR(10) NOT NULL, 
categorie ENUM('St-Valentin', 'Toute occasion', 'Fête des mères', 'Mariage') NOT NULL,
description_bouquet VARCHAR(200) NOT NULL,
delai_livraison ENUM('3 jours', 'plus de 3 jours') NOT NULL
);


DROP TABLE IF EXISTS CompositionBouquet_Standard;
CREATE TABLE IF NOT EXISTS CompositionBouquet_Standard(
bouquet_id INT NOT NULL,
produit_id INT NOT NULL,
quantite INT NOT NULL,
PRIMARY KEY (bouquet_id, produit_id),
FOREIGN KEY (bouquet_id) REFERENCES BouquetStandard(id_BouquetStandard),
FOREIGN KEY (produit_id) REFERENCES Produit(id_Produit)
);

DROP TABLE IF EXISTS Livraison;
CREATE TABLE IF NOT EXISTS Livraison (
id_Livraison INT AUTO_INCREMENT PRIMARY KEY,
commande_id INT NOT NULL,
date_livraison DATE NOT NULL,
adresse_livraison VARCHAR(100) NOT NULL,
message VARCHAR(400), 
FOREIGN KEY (commande_id) REFERENCES Commande(id_Commande)
);


DROP TABLE IF EXISTS Stock;
CREATE TABLE IF NOT EXISTS Stock(
id_Stock INT PRIMARY KEY,
magasin_id INT NOT NULL,
produit_id INT NOT NULL,
quantite INT NOT NULL,
seuil_alerte INT NOT NULL,
FOREIGN KEY (magasin_id) REFERENCES Magasin(id_Magasin),
FOREIGN KEY (produit_id) REFERENCES Produit(id_Produit)
);



