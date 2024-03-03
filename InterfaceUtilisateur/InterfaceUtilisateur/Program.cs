using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Xml;

namespace InterfaceUtilisateur
{
    internal class Program
    {
        



        static void Main(string[] args)
        {
            // Connexion du client , Identification du compte
     
            Console.WriteLine("Bienvenue dans notre système de commande en ligne BelleFleur!");
            Console.WriteLine("Veuillez entrer votre adresse email");
            string email = Console.ReadLine();

            string requete1 = "SELECT COUNT(*) FROM Client WHERE email = @Email";

            string connectionString = "server=localhost;port=3306;database=fleurs;uid=root;password=root;";

            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                //objet MySqlCommand de la bibliothèque MySQL Connector pour exécuter la requête SQL dans la BDD Fleurs de MySQL en C#

                long id_Client = 0;

                using (MySqlCommand commande1 = new MySqlCommand(requete1, connection))
                {
                    commande1.Parameters.AddWithValue("@Email", email);
                    int row = Convert.ToInt32(commande1.ExecuteScalar());
                    

                    if (row == 0)
                    {
                        Console.WriteLine("Vous êtes un nouveau client.");
                        Console.Write("Entrez votre nom : ");
                        string nom = Console.ReadLine();
                        Console.Write("Entrez votre prénom : ");
                        string prenom = Console.ReadLine();
                        Console.Write("Entrez votre numéro de téléphone : ");
                        string telephone = Console.ReadLine();
                        Console.Write("Entrez votre adresse de facturation : ");
                        string adresse = Console.ReadLine();
                        Console.Write("Entrez votre carte de crédit : ");
                        string carteCredit = Console.ReadLine();
                        Console.Write("Entrez votre mot de passe : ");
                        string motDePasse = Console.ReadLine();

                        string requete2_checkEmail = "SELECT COUNT(*) FROM Client WHERE email = @Email";

                        using (MySqlCommand checkEmailCmd = new MySqlCommand(requete2_checkEmail, connection))
                        {
                            checkEmailCmd.Parameters.AddWithValue("@Email", email);
                            int row_email = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                            if(row_email > 0)
                            {
                                Console.WriteLine("Un compte avec cet email existe déjà. Veuillez choisir une autre adresse email.");
                                return;
                            }

                            string insertClient_Requete = "INSERT INTO Client (nom, prenom, adresse, email, telephone, CarteCredit, Mdp) VALUES (@Nom, @Prenom, @Adresse, @Email, @Telephone, @CarteCredit, @Mdp)";
                            using (MySqlCommand insertClientCmd = new MySqlCommand(insertClient_Requete, connection))
                            {
                                insertClientCmd.Parameters.AddWithValue("@Nom", nom);
                                insertClientCmd.Parameters.AddWithValue("@Prenom", prenom);
                                insertClientCmd.Parameters.AddWithValue("@Telephone", telephone);
                                insertClientCmd.Parameters.AddWithValue("@Email", email);
                                insertClientCmd.Parameters.AddWithValue("@Mdp", motDePasse);
                                insertClientCmd.Parameters.AddWithValue("@Adresse", adresse);
                                insertClientCmd.Parameters.AddWithValue("@CarteCredit", carteCredit);

                                insertClientCmd.ExecuteNonQuery(); //retourne combien de valeur on été retourné 
                                id_Client = insertClientCmd.LastInsertedId; // Récupérer l'id généré automatiquement
                            }
                        }

                    }
                    else if (email == "Monsieur.BelleFleur@monmagasin.com")
                    {
                        Console.WriteLine("Bienvenue Monsieur BelleFleur.");
                        Console.WriteLine("Vous avez accès à la gestion des commandes, des clients et des produits en stock.");
                        Console.WriteLine("Vous pouvez également consulter l'historique des commandes de vos clients.");

                        //Modules

                        Console.WriteLine("Quel module voulez-vous explorer ? : ");
                        Console.WriteLine("1. Clients"); // ses caractéristiques, son statut de fidélité, ses achats passés 
                        Console.WriteLine("2. Produits "); //ses caractéristiques et stock par magasin
                        Console.WriteLine("3. Commandes "); // état des commandes (à livrer, en livraison, livrées) par magasin et par jour
                        Console.WriteLine("4. Statistiques "); //par mois, par an, sur les bouquets, les clients, les produits, les commandes 

                        int choix = Convert.ToInt32(Console.ReadLine());

                        switch (choix) 
                        {
                            case 1:  
                                Console.WriteLine("Vous avez choisi le module Clients.");
                                
                                Console.WriteLine("Voici la liste de tous les clients , ainsi que l'historique de leur commande : ");

                                string queryClients = "SELECT id_Client, nom, prenom, adresse, telephone, fidelite, total_achats FROM Client";
                                MySqlCommand commandClients = new MySqlCommand(queryClients, connection);
                                MySqlDataReader readerClients = commandClients.ExecuteReader();

                                while (readerClients.Read())
                                {
                                    Console.WriteLine("Nom : {0}", readerClients.GetString(1));
                                    Console.WriteLine("Prenom : {0}", readerClients.GetString(2));
                                    Console.WriteLine("Adresse : {0}", readerClients.GetString(3));
                                    Console.WriteLine("Tel : {0}", readerClients.GetString(4));
                                    Console.WriteLine("Fidelite : {0}", readerClients.GetString(5));
                                    Console.WriteLine("Total achats : {0}", readerClients.GetDecimal(6));

                                    // Afficher les commandes associées à chaque client
                                    Console.WriteLine("Historique des commandes : ");
                                    int clientId = readerClients.GetInt32(0);

                                    // Create a new connection
                                    MySqlConnection connectionCommandes = new MySqlConnection(connectionString);
                                    connectionCommandes.Open();

                                    string queryCommandes = string.Format("SELECT DateCommande, PrixTotal, EtatCommande FROM Commande WHERE id_Client = {0}", clientId);
                                    MySqlCommand commandCommandes = new MySqlCommand(queryCommandes, connectionCommandes);
                                    MySqlDataReader readerCommandes = commandCommandes.ExecuteReader();

                                    while (readerCommandes.Read())
                                    {
                                        Console.WriteLine("Date de la commande : {0}", readerCommandes.GetDateTime(0));
                                        Console.WriteLine("Montant de la commande : {0}", readerCommandes.GetDecimal(1));
                                        Console.WriteLine("Etat de la commande : {0}", readerCommandes.GetString(2));
                                        Console.WriteLine("---------------------------------------");
                                    }

                                    readerCommandes.Close();
                                    connectionCommandes.Close();
                                }
                                readerClients.Close();
                                break;
                            case 2: 
                                Console.WriteLine("Vous avez choisi le module Produits.");
                                
                                // Récupérer les informations de chaque produit et son stock dans chaque magasin
                                string query = @"SELECT Produit.nom, Produit.type_produit, Produit.prix, Magasin.nom, Magasin.adresse, Stock.quantite, Stock.seuil_alerte
                                                FROM Produit 
                                                INNER JOIN Stock ON Produit.id_Produit = Stock.produit_id 
                                                INNER JOIN Magasin ON Stock.magasin_id = Magasin.id_Magasin";


                                MySqlCommand command = new MySqlCommand(query, connection);
                                MySqlDataReader reader = command.ExecuteReader();

                                // Afficher les informations de chaque produit et son stock dans chaque magasin
                                while (reader.Read())
                                {
                                    Console.WriteLine("Nom du produit : {0}", reader.GetString(0));
                                    Console.WriteLine("Type du produit : {0}", reader.GetString(1));
                                    Console.WriteLine("Prix du produit : {0}", reader.GetDecimal(2));
                                    Console.WriteLine("Nom du magasin : {0}", reader.GetString(3));
                                    Console.WriteLine("Adresse du magasin : {0}", reader.GetString(4));
                                    int stock = reader.GetInt32(5);
                                    Console.WriteLine("Quantité en stock : {0}", stock);
                                    int seuilAlerte = reader.GetInt32(6);
                                    if (stock < seuilAlerte)
                                    {
                                        Console.WriteLine("Attention ! La quantité en stock est inférieure au seuil d'alerte.");
                                    }
                                    Console.WriteLine("---------------------------------------");
                                }
                                reader.Close();
                                break;
                            case 3:
                                Console.WriteLine("Vous avez choisi le module Commandes.");
                                Console.WriteLine();
                                Console.WriteLine("Affichage de l'état des commandes par magasin et par jour :");
                                string requete3 = "SELECT Magasin.nom AS Magasin, Commande.DateCommande AS Date, COUNT(Commande.id_Commande) AS Nombre, Commande.EtatCommande AS Etat FROM Commande " +
                                                 "INNER JOIN Magasin ON Commande.id_Magasin = Magasin.id_Magasin " +
                                                 "WHERE Commande.EtatCommande IN ('CAL', 'CL', 'CC', 'VINV', 'CPAV') " +
                                                 "GROUP BY Magasin.id_Magasin, Commande.DateCommande, Commande.EtatCommande " +
                                                 "ORDER BY Magasin.nom, Commande.DateCommande";
                                using (MySqlCommand cmd = new MySqlCommand(requete3, connection))
                                {
                                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                                    {
                                        while (rdr.Read())
                                        {
                                            string magasin = rdr.GetString("Magasin");
                                            DateTime dateCommande = rdr.GetDateTime("Date");
                                            int nombreCommandes = rdr.GetInt32("Nombre");
                                            string etatCommande = rdr.GetString("Etat");
                                            string etatCommandeLabel = "";

                                            switch (etatCommande)
                                            {
                                                case "VINV":
                                                    etatCommandeLabel = "Commande standard pour laquelle un employé doit vérifier l’inventaire.";
                                                    break;
                                                case "CC":
                                                    etatCommandeLabel = "Commande complète. Tous les items de la commande ont été indiqués (pour les commandes personnalisées) et tous ces items se trouvent en stock.";
                                                    break;
                                                case "CPAV":
                                                    etatCommandeLabel = "Commande personnalisée à vérifier. Lorsqu’un client passe une commande personnalisée, son état est « CPAV ». Un employé vérifie la commande et rajoute des items si nécessaire. Ensuite, il change l’état de la commande à « CC ».";
                                                    break;
                                                case "CAL":
                                                    etatCommandeLabel = "Commande à livrer. La commande est prête !";
                                                    break;
                                                case "CL":
                                                    etatCommandeLabel = "Commande livrée. La commande a été livrée à l’adresse indiquée par le client.";
                                                    break;
                                                default:
                                                    etatCommandeLabel = "État inconnu.";
                                                    break;
                                            }

                                            Console.WriteLine($"Magasin : {magasin}, Date de commande : {dateCommande.ToShortDateString()}, État de commande : {etatCommandeLabel}, Nombre de commandes : {nombreCommandes}");
                                            Console.WriteLine();
                                        }
                                        
                                    }
                                }

                                break;
                            case 4:
                                Console.WriteLine("Vous avez choisi le module Statistiques.");
                                Console.WriteLine();

                                // Requête synchronisée pour les statistiques par an sur les bouquets
                                MySqlCommand cmd1 = new MySqlCommand("SELECT YEAR(DateCommande) AS annee, COUNT(*) AS nombre_de_commandes, \r\n       SUM(PrixTotal) AS chiffre_affaires, COUNT(DISTINCT CompositionBouquet_Standard.bouquet_id) AS nombre_de_bouquets\r\nFROM Commande\r\nJOIN CompositionCommande_Personnalise ON CompositionCommande_Personnalise.id_Commande = Commande.id_Commande\r\nJOIN CompositionBouquet_Standard ON CompositionBouquet_Standard.bouquet_id = CompositionCommande_Personnalise.bon_commande\r\nWHERE CompositionBouquet_Standard.bouquet_id IS NOT NULL\r\nGROUP BY YEAR(DateCommande);\r\n", connection);


                                // Exécution de la requête et récupération des résultats
                                MySqlDataReader reader1 = cmd1.ExecuteReader();

                                Console.WriteLine("Statistiques par an sur les bouquets :");
                                Console.WriteLine("Année\tNom du bouquet\tNombre de commandes\tTotal des ventes");

                                while (reader1.Read())
                                {
                                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader1.GetInt32(0), reader1.GetString(1), reader1.GetInt32(2), reader1.GetDecimal(3));
                                }

                                reader1.Close();
                                Console.WriteLine();

                                // Requête avec auto-jointure pour les statistiques par mois sur les clients
                                MySqlCommand cmd2 = new MySqlCommand("SELECT YEAR(c1.DateCommande) as annee, MONTH(c1.DateCommande) as mois, COUNT(*) as nb_commandes, COUNT(DISTINCT c1.id_Client) as nb_clients FROM Commande c1 INNER JOIN Commande c2 ON YEAR(c1.DateCommande) = YEAR(c2.DateCommande) AND MONTH(c1.DateCommande) = MONTH(c2.DateCommande) AND c1.id_Client = c2.id_Client GROUP BY YEAR(c1.DateCommande), MONTH(c1.DateCommande) ORDER BY YEAR(c1.DateCommande) DESC, MONTH(c1.DateCommande);", connection);

                                // Exécution de la requête et récupération des résultats
                                MySqlDataReader reader2 = cmd2.ExecuteReader();

                                Console.WriteLine("\nStatistiques par mois sur les clients :");
                                Console.WriteLine("Année\tMois\tNombre de commandes\tNombre de clients");

                                while (reader2.Read())
                                {
                                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader2.GetInt32(0), reader2.GetInt32(1), reader2.GetInt32(2), reader2.GetInt32(3));
                                }

                                reader2.Close();
                                Console.WriteLine();

                                // Requête par mois sur les produits avec une union
                                MySqlCommand cmd3 = new MySqlCommand("SELECT MONTH(DateCommande) AS Mois, Produit.nom, SUM(PrixTotal) AS TotalVentes\r\nFROM Commande\r\nJOIN CompositionCommande_Personnalise ON Commande.id_Commande = CompositionCommande_Personnalise.id_Commande\r\nJOIN Produit ON CompositionCommande_Personnalise.bon_commande LIKE CONCAT('%', CAST(Produit.id_Produit AS CHAR), '%')\r\nJOIN (\r\n  SELECT id_Fleur AS id_Produit, nom, 'fleur' AS type_produit, prix FROM Fleur\r\n  UNION\r\n  SELECT id_Accessoire AS id_Produit, nom, 'accessoire' AS type_produit, prix FROM Accessoire\r\n) AS ProduitType ON ProduitType.id_Produit = Produit.id_Produit\r\nWHERE EtatCommande = 'CL'\r\nGROUP BY MONTH(DateCommande), Produit.nom\r\nORDER BY MONTH(DateCommande), Produit.nom;\r\n", connection);

                                // Exécution de la requête et récupération des résultats
                                MySqlDataReader reader3 = cmd3.ExecuteReader();

                                Console.WriteLine("Statistiques par mois sur les produits :");
                                Console.WriteLine("Type de produit\tAnnée\tMois\tNombre de ventes\tTotal des ventes");

                                while (reader3.Read())
                                {
                                    Console.WriteLine("{0}\t\t{1}\t{2}\t{3}\t\t{4}", reader3.GetString(0), reader3.GetInt32(1), reader3.GetInt32(2), reader3.GetInt32(3), reader3.GetDecimal(4));
                                }

                                reader3.Close();
                                Console.WriteLine();

                                // Requête pour les statistiques sur les commandes :
                                MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) as nb_commandes, SUM(PrixTotal) as total_ventes, AVG(PrixTotal) as prix_moyen FROM Commande;", connection);

                                // Exécution de la requête et récupération des résultats
                                MySqlDataReader reader4 = cmd4.ExecuteReader();

                                Console.WriteLine("Statistiques sur les commandes :");
                                Console.WriteLine("Nombre de commandes\tTotal des ventes\tPrix moyen");

                                while (reader4.Read())
                                {
                                    Console.WriteLine("{0}\t{1}\t{2}", reader4.GetInt32(0), reader4.GetDecimal(1), reader4.GetDecimal(2));
                                }

                                reader4.Close();


                                break;
                            default:
                                Console.WriteLine("Choix invalide.");
                                break;
                        }
                        Environment.Exit(0);

                    }
                    else
                    {
                        Console.WriteLine("Vous êtes un client existant.");
                        Console.Write("Entrez votre mot de passe : ");
                        string motDePasse = Console.ReadLine();

                        string requete_mdp = "SELECT COUNT(*), id_Client FROM Client WHERE email = @Email AND Mdp = @Mdp";

                        using (MySqlCommand mdpCmd = new MySqlCommand(requete_mdp, connection))
                        {
                            mdpCmd.Parameters.AddWithValue("@Email", email);
                            mdpCmd.Parameters.AddWithValue("@Mdp", motDePasse);

                            using (MySqlDataReader reader = mdpCmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    reader.Read();
                                    id_Client = reader.GetInt64(1); // Récupérer l'ID client à partir du deuxième champ de la requête
                                }
                                reader.Close();
                                // fermer les reader
                            }
                            
                            
                            int row_mdp = Convert.ToInt32(mdpCmd.ExecuteScalar());

                            if(row_mdp == 0)
                            {
                                Console.WriteLine("Mot de passe incorrect. Réessayer ...");
                            }
                            else
                            {
                                Console.WriteLine("Bienvenue"); 
                            }
                            
                        }
                    }
                    
                }
                int id_Commande = 0;
                Console.WriteLine("Souhaitez-vous commander un bouquet standard, faire une commande personnalisée ?");
                string reponse1 = Console.ReadLine();

                if (reponse1.ToLower() == "bouquet standard")
                {
                    Console.WriteLine("Quel est votre budget (nos prix : 40 et 45 ~Toute occasion , 65 ~ St-Valentin, 80 ~ Fête des mères, 120 ~ Mariage) ?");
                    
                    decimal prix_max = Convert.ToDecimal(Console.ReadLine());

                    Console.WriteLine("Quelle est la catégorie de bouquet que vous recherchez ?");
                    string categorie = Console.ReadLine();

                    string selectBouquet = "SELECT id_BouquetStandard, nom, prix_max, description_bouquet FROM BouquetStandard WHERE categorie = '" + categorie + "' AND prix_max = " + prix_max +"; ";
                     
                    // Exécuter la requête SQL en utilisant le paramètre "categorie" et "prixMax"
                    using (MySqlCommand cmd = new MySqlCommand(selectBouquet, connection))
                    {
                        cmd.Parameters.AddWithValue("@categorie", categorie);
                        cmd.Parameters.AddWithValue("@prix_max", prix_max);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Le bouquet standard dans cette catégorie est : ");
                                Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2), reader.GetString(3));

                            }
                            reader.Close(); // fermer les reader
                        }
                    }
                    // Récupérer les informations du bouquet standard choisi
                    string nomBouquetStandard = "";
                    int idBouquetStandard = 0; // déclaration de la variable idBouquetStandard
                    decimal prixBouquetStandard = 0;
                    using (MySqlCommand cmd = new MySqlCommand("SELECT id_BouquetStandard, nom, prix_max FROM BouquetStandard WHERE categorie = @categorie AND prix_max <= @prix_max ORDER BY prix_max DESC LIMIT 1", connection))
                    {
                        cmd.Parameters.AddWithValue("@categorie", categorie);
                        cmd.Parameters.AddWithValue("@prix_max", prix_max);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idBouquetStandard = reader.GetInt32(0);
                                nomBouquetStandard = reader.GetString(1);
                                prixBouquetStandard = reader.GetDecimal(2);
                            }
                            reader.Close (); // fermer les reader
                        }
                    }
                    // 
                    // Enregistrer la commande dans la base de données
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Commande (id_Client, id_Magasin, DateCommande, EtatCommande, PrixTotal) VALUES (@idClient, @idMagasin, @dateCommande, @etatCommande, @prixTotal)", connection))
                    {
                        cmd.Parameters.AddWithValue("@idClient", id_Client);
                        cmd.Parameters.AddWithValue("@idMagasin", 1);
                        cmd.Parameters.AddWithValue("@dateCommande", DateTime.Now);
                        cmd.Parameters.AddWithValue("@etatCommande", "VINV");
                        cmd.Parameters.AddWithValue("@prixTotal", prixBouquetStandard);
                        cmd.ExecuteNonQuery();


                    }

                }
                else
                {
                    // Demander le type et la quantité de fleurs
                    Dictionary<string, int> fleurs = new Dictionary<string, int>();
                    while (true)
                    {
                        Console.Write("Entrez le type de fleur (ou 'fini' pour terminer la saisie) : ");
                        string type = Console.ReadLine().ToLower();
                        if (type == "fini") break;
                        Console.Write("Entrez la quantité de cette fleur : ");
                        int quantite = int.Parse(Console.ReadLine());
                        fleurs[type] = quantite;
                    }

                    // Demander le type et la quantité d'accessoires
                    Dictionary<string, int> accessoires = new Dictionary<string, int>();
                    while (true)
                    {
                        Console.Write("Entrez le type d'accessoire (ou 'fini' pour terminer la saisie) : ");
                        string type = Console.ReadLine().ToLower();
                        if (type == "fini") break;
                        Console.Write("Entrez la quantité de cet accessoire : ");
                        int quantite = int.Parse(Console.ReadLine());
                        accessoires[type] = quantite;
                    }

                    // Calculer le prix total
                    decimal prixTotal = 0;
                    foreach (var paire in fleurs)
                    {
                        string type = paire.Key;
                        int quantite = paire.Value;
                        MySqlCommand cmd = new MySqlCommand("SELECT prix FROM Fleur WHERE nom=@nom", connection);
                        cmd.Parameters.AddWithValue("@nom", type);
                        decimal prix = (decimal)cmd.ExecuteScalar();
                        prixTotal += quantite * prix;
                    }

                    foreach (var paire in accessoires)
                    {
                        string type = paire.Key;
                        int quantite = paire.Value;
                        MySqlCommand cmd = new MySqlCommand("SELECT prix FROM Accessoire WHERE nom=@nom", connection);
                        cmd.Parameters.AddWithValue("@nom", type);
                        decimal prix = (decimal)cmd.ExecuteScalar();
                        prixTotal += quantite * prix;
                    }

                    Console.WriteLine(prixTotal);

                    // Générer le bon de commande
                    string bon_commande = "Commande :\n";
                    foreach (var paire in fleurs)
                    {
                        string type = paire.Key;
                        int quantite = paire.Value;
                        MySqlCommand cmd = new MySqlCommand("SELECT prix FROM Fleur WHERE nom=@nom", connection);
                        cmd.Parameters.AddWithValue("@nom", type);
                        decimal prix = (decimal)cmd.ExecuteScalar();
                        bon_commande += quantite + " x " + type + " : " + (quantite * prix) + " euros\n";
                    }

                    foreach (var paire in accessoires)
                    {
                        string type = paire.Key;
                        int quantite = paire.Value;
                        MySqlCommand cmd = new MySqlCommand("SELECT prix FROM Accessoire WHERE nom=@nom", connection);
                        cmd.Parameters.AddWithValue("@nom", type);
                        decimal prix = (decimal)cmd.ExecuteScalar();
                        bon_commande += quantite + " x " + type + " : " + (quantite * prix) + " euros\n";
                    }



                    // Enregistrer la commande dans la base de données
                    
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Commande (id_Client, DateCommande, EtatCommande, PrixTotal, id_Magasin) VALUES (@idClient, @dateCommande, @etatCommande, @prixTotal, @id_Magasin)", connection))
                    {
                        
                        cmd.Parameters.AddWithValue("@idClient", id_Client);
                        cmd.Parameters.AddWithValue("@dateCommande", DateTime.Now);
                        cmd.Parameters.AddWithValue("@etatCommande", "VINV");
                        cmd.Parameters.AddWithValue("@prixTotal", prixTotal);
                      
                        cmd.Parameters.AddWithValue("@id_Magasin", 1);
                        cmd.ExecuteNonQuery();

                        id_Commande = (int)cmd.LastInsertedId;
                    }



                    //Inscrire les informations dans CompositionCommande_Personnalisee(id_Client, id_Commande, bonCommande, prixTotal)
                    MySqlCommand cmdInsert = new MySqlCommand("INSERT INTO CompositionCommande_Personnalise(id_Client,id_Commande, bon_commande) VALUES (@id_Client, @idCommande, @bon_commande)", connection);
                    cmdInsert.Parameters.AddWithValue("@id_Client", id_Client);
                    cmdInsert.Parameters.AddWithValue("@idCommande", id_Commande);
                    cmdInsert.Parameters.AddWithValue("@bon_commande", bon_commande);
                    cmdInsert.ExecuteNonQuery();



                }
                // Etape Livraison :

                // Récupérer la commande du client à livrer
                //int id_Commande = 0;
                MySqlCommand command3 = new MySqlCommand ("SELECT MAX(id_Commande) FROM Commande", connection);

                // Exécution de la commande SQL et récupération du résultat
                id_Commande = Convert.ToInt32(command3.ExecuteScalar());

                
                // Vérifier si une commande a été trouvée pour le client
                if (id_Commande == 0)
                {
                    Console.WriteLine("Aucune commande en attente de livraison pour ce client.");
                }
                else
                {
                    // Demander les informations de livraison
                    Console.WriteLine("Quelle est votre adresse de livraison ?");
                    string adresse = Console.ReadLine();

                    Console.WriteLine("Entrez le message accompagnant votre commande");
                    string mess = Console.ReadLine();

                    Console.WriteLine("Quelle est la date de livraison souhaitée ?");
                    DateTime d = DateTime.Parse(Console.ReadLine());

                    // Vérifier si la commande est un bouquet standard et si la date de livraison est inférieure ou égale à 3 jours
                    if (reponse1.ToLower() == "bouquet standard")
                    {
                        Console.WriteLine("Attention : si la date de livraison souhaitée est <= 3 jours, il peut y avoir une difficulté d'approvisionnement.");
                    }

                    // Enregistrer la livraison dans la base de données
                    int idLivraison = 0;
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Livraison (commande_id, date_livraison, adresse_livraison, message) VALUES (@id_Commande, @dateLivraison, @adresseLivraison, @message)", connection))
                    {
                        cmd.Parameters.AddWithValue("@id_Commande", id_Commande);
                        cmd.Parameters.AddWithValue("@dateLivraison", d);
                        cmd.Parameters.AddWithValue("@adresseLivraison", adresse);
                        cmd.Parameters.AddWithValue("@message", mess);
                        cmd.ExecuteNonQuery();

                        idLivraison = (int)cmd.LastInsertedId;
                    }




                }
                // Appliquer une remise si le client est fidèle

                // Récupérer la fidélité du client
                string queryFidelite = "SELECT fidelite FROM Client LIMIT 1";
                MySqlCommand commandFidelite = new MySqlCommand(queryFidelite, connection);
                MySqlDataReader readerFidelite = commandFidelite.ExecuteReader();
                readerFidelite.Read();
                string fidelite = readerFidelite.GetString(0);
                readerFidelite.Close();

                // Récupérer le montant total de la commande
                
                string queryPrixTotal = "SELECT PrixTotal FROM Commande WHERE id_Commande=@idCommande";
                MySqlCommand commandPrixTotal = new MySqlCommand(queryPrixTotal, connection);
                commandPrixTotal.Parameters.AddWithValue("@idCommande", id_Commande);
                //commandPrixTotal.Parameters.AddWithValue("@idClient", id_Client);
                MySqlDataReader readerPrixTotal = commandPrixTotal.ExecuteReader();
                readerPrixTotal.Read();
                decimal prixtotal = readerPrixTotal.GetDecimal(0);
                readerPrixTotal.Close();

                // Calculer la remise en fonction de la fidélité du client
                decimal prixFinal = 0;
                if (fidelite == "bronze")
                {
                    prixFinal = prixtotal * 95/100;
                    Console.WriteLine("Remise de 5%");
                }
                else
                {
                    prixFinal = prixtotal * 85/100;
                    Console.WriteLine("Remise de 15%");
                }
                
                Console.WriteLine("Prix final : " + prixFinal);

                // Mettre à jour le montant total de la commande dans la table "Commande"
                string queryUpdatePrixTotal = "UPDATE Commande SET PrixTotal=@prixFinal WHERE id_commande=@idCommande AND id_Client=@idClient";
                MySqlCommand commandUpdatePrixTotal = new MySqlCommand(queryUpdatePrixTotal, connection);
                commandUpdatePrixTotal.Parameters.AddWithValue("@prixFinal", prixFinal);
                commandUpdatePrixTotal.Parameters.AddWithValue("@idCommande", id_Commande);
                commandUpdatePrixTotal.Parameters.AddWithValue("@idClient", id_Client);
                int rowsAffected = commandUpdatePrixTotal.ExecuteNonQuery();



                




            }

            

            

        }
    }
}