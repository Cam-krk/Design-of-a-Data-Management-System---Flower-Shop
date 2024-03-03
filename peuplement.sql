INSERT INTO Client (id_Client, nom, prenom, adresse, email, telephone, fidelite, total_achats, CarteCredit, Mdp) 
VALUES 
(1, 'Jinane', 'Krika', '5 Rue des Lilas', 'root', '0627652423' , 'or', 560, 6578, 'root'),
(2, 'Nassir', 'Mohamed', '7 Rue Pierre Lescot', 'mohamed.nassir@gmail.com', '0765459876', 'or', 830.0, 5678, 'Moha'),
(3,'Bertrand', 'Julien', '30 Av. des Ternes', 'julien.bertrand@gmail.com','0762246708', 'bronze', 130.34, 3425, 'juju'), 
(4, 'Brenot', 'Cécile','rue du 8 mai' , 'cecile.brenot@gmail.com', '0626952434', 'or', 549.09, 6278, 'cecile'),
(5, 'BelleFleur', 'Jean', 'adresse magasin', 'Monsieur.BelleFleur@monmagasin.com', '0678931612', 'bronze', 0, 678, 'fleur');

INSERT INTO Produit (id_Produit, nom, type_produit, prix)
VALUES 
(1, 'Gerbera', 'fleur', 5.00),
(2, 'Ginger', 'fleur', 4.00),
(3, 'Glaïeul', 'fleur', 1.00),
(4, 'Vase', 'accessoire', 10.50),
(5, 'Ruban', 'accessoire', 8.00),
(6, 'Marguerite', 'fleur', 6.00),
(7, 'Rose rouge', 'fleur', 2.00);

INSERT INTO Fleur (id_Fleur, nom, prix, disponibilite, id_Produit) 
VALUES 
(1, 'Gerbera', 5.00, 'à l année',1), 
(2, 'Ginger', 4.00, 'à l année',2), 
(3, 'Glaïeul', 1.00, 'de mai à novembre',3), 
(4, 'Marguerite', 2.25, 'à l année',6), 
(5, 'Rose rouge', 2.50, 'à l année',7);

INSERT INTO Accessoire (id_Accessoire, nom, prix, seuil_alerte, id_Produit) VALUES 
(1, 'Vase', 10.50, 5, 4), 
(2, 'Ruban', 8.00, 10, 5); 

INSERT INTO Magasin (id_Magasin, nom, adresse)
VALUES (1, ' La main verte', '74 Av. des Champs-Élysées'), 
       (2, 'Orchidée sauvage', '109 Rue Saint-Lazare'), 
       (3, 'La fabrique à fleurs', '136 Rue de Rennes');

#
INSERT INTO Commande (id_Commande, id_Client, id_Magasin, DateCommande, EtatCommande, PrixTotal) #id_Produit : 4, 5, 3, 2
VALUES 
(1, 3, 3, '2023-05-02', 'VINV', 35.57),
(2, 4, 2, '2023-03-21', 'CC', 6.45),
(3, 2, 1, '2023-01-15', 'CAL', 53.99),
(4, 1, 2, '2022-08-16', 'CPAV', 18.20),
(5, 3, 2, '2023-05-04', 'VINV', 45.57),
(6, 3, 1, '2023-05-06', 'VINV', 78),
(7, 3, 1, '2023-05-07', 'CAL', 38);

INSERT INTO CompositionCommande_Personnalise (id_Client, id_Commande, bon_commande)
VALUES 
(1, 3, "6 Marguerites et une rose rouge , prix total : 17 euros"),
(2, 4, "3roses rouges, prix total : 9 euros"),
(3, 2, "5 vases , prix total : 55");


INSERT INTO BouquetStandard (id_BouquetStandard, nom, prix_max, devise, categorie, description_bouquet, delai_livraison) VALUES 
(1, 'Gros Merci', 45.00, '€', 'Toute occasion', 'Arrangement floral avec marguerites et verdure', '3 jours'),
(2, 'Amoureux', 65.00, '€', 'St-Valentin', 'Arrangement floral avec roses blanches et roses rouges',  'plus de 3 jours'),
(3, 'L’Exotique', 40.00, '€', 'Toute occasion', 'Arrangement floral avec ginger, oiseaux du paradis, roses et genet', '3 jours'),
(4, 'Maman', 80.00, '€', 'Fête des mères', 'Arrangement floral avec gerbera, roses blanches, lys et alstroméria', '3 jours'),
(5, 'Vive la mariée', 120.00, '$', 'Mariage', 'Arrangement floral avec lys et orchidées', 'plus de 3 jours');

INSERT INTO CompositionBouquet_Standard (bouquet_id, produit_id, quantite)
VALUES
(1, 2, 13),
(4, 1, 7);

INSERT INTO Livraison (id_Livraison, commande_id, date_livraison, adresse_livraison, message) 
VALUES 
(1, 3, '2023-05-25', '23 Rue Saint Jean', 'Je te remercie'),
(2, 2, '2023-06-04', '456 Avenue des Champs-Élysées', 'Joyeuse fête des mères');

INSERT INTO Stock (id_Stock, magasin_id, produit_id, quantite, seuil_alerte) 
VALUES 
(1, 1, 2, 15, 50),
(2, 3, 1, 46, 23),
(3, 2, 3, 76, 30),
(4, 1, 4, 43, 12),
(5, 2, 5, 67, 30),
(6, 3, 7, 67, 30),
(7, 2, 6, 67, 30);


