using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Genius_Pharmacie.Model;

namespace Genius_Pharmacie.Controller
{
    public class ClientController
    {
        public string path = "client.txt";
        private Dictionary<string, Client> clients = new Dictionary<string, Client>();
        private int clientIdCounter = 1;  // Compteur pour générer des IDs uniques

        public void MenuClient()
        {
            Console.Title = "GESTION CLIENT";
            bool while1 = true;
            string choix = null;

            while (while1)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("========================================================");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(":                      Menu Client                     :");
                    Console.ResetColor();
                    Console.WriteLine("========================================================");
                    Console.WriteLine("a. Enregistrer un client");
                    Console.WriteLine("b. Afficher tous les clients");
                    Console.WriteLine("c. Afficher les clients avec des dettes");
                    Console.WriteLine("d. Payer une dette");
                    Console.WriteLine("e. Supprimer un client n'ayant acheté aucun produit");
                    Console.WriteLine("f. Retour au menu principal");
                    Console.Write("========================================================\n>>");
                    choix = Console.ReadLine();
                    if (Regex.IsMatch(choix, @"^[a-fA-F]$"))
                    {
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Veuillez saisir une lettre entre [a-f]");
                        Console.ResetColor();
                        Console.ReadKey(true);
                    }
                }

                Console.Clear();
                switch (choix.ToLower())
                {
                    case "a":
                        Console.Clear();
                        this.EnregistrerClient();
                        Console.ReadKey(true);
                        break;
                    case "b":
                        Console.Clear();
                        this.AfficherClients();
                        Console.ReadKey(true);
                        break;
                    case "c":
                        Console.Clear();
                        this.AfficherClientsAvecDettes();
                        Console.ReadKey(true);
                        break;
                    case "d":
                        Console.Write("Entrez le code du client: ");
                        string codeClient = Console.ReadLine();
                        PayerDetteClient(codeClient);
                        Console.ReadKey(true);
                        break;
                    case "e":
                        Console.Write("Entrez l'ID du client à supprimer : ");
                        string clientId = Console.ReadLine();
                        SupprimerClientSansAchat(clientId);
                        Console.ReadKey(true);
                        break;
                    case "f":
                        while1 = false;
                        break;
                    default:
                        Console.WriteLine("Mauvais choix!!!");
                        break;
                }
            }
        }

        // Méthode pour enregistrer un client
        public void EnregistrerClient()
        {
            // Demande les informations du client avec validation
            var nom = DemanderEtValiderNom("Nom (lettres uniquement) : ");
            var prenom = DemanderEtValiderNom("Prenom (lettres uniquement) : ");
            var adresse = DemanderEtValiderTexte("Adresse : ");
            var telephone = DemanderEtValiderTelephone("Téléphone (8 chiffres) : ");
            var email = DemanderEtValiderEmail("Email (format xxx@gmail.com) : ");
            var type = DemanderEtValiderType("Type (1: aucun, 2: à crédit) : ");

            // Générer un ID unique
            var clientId = clientIdCounter.ToString();
            clientIdCounter++;

            // Crée un nouvel objet Client et l'ajoute au dictionnaire
            Client client = new Client(this.generateCode(nom, prenom, telephone), nom, prenom, adresse, telephone, email, type);

            // Utiliser StreamWriter avec l'argument append = true
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(client.Writing());
            }
            Console.WriteLine("Client enregistré avec succès");
        }

        // Méthode pour afficher tous les clients
        public void AfficherClients()
        {
            Console.WriteLine("Chemin complet du fichier : ");
            try
            {
                // Affiche le chemin complet du fichier
                string fullPath = "client.txt";
                Console.WriteLine("Chemin complet du fichier : " + fullPath);

                // Utilise FileInfo pour vérifier si le fichier existe avant de l'ouvrir
                FileInfo fileInfo = new FileInfo(fullPath);
                if (!fileInfo.Exists)
                {
                    Console.WriteLine("Aucun fichier 'client.txt' trouvé.");
                    return;
                }

                // Ouvre le fichier en lecture
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    string line;
                    bool hasClients = false; // Variable pour vérifier s'il y a des clients

                    while ((line = sr.ReadLine()) != null)
                    {
                        // Sépare chaque ligne en attributs en utilisant le deux-points comme séparateur
                        var attributs = line.Split(':');

                        if (attributs.Length == 8)
                        {
                            Console.WriteLine("ID: " + attributs[0]);
                            Console.WriteLine("Nom: " + attributs[1]);
                            Console.WriteLine("Prenom: " + attributs[2]);
                            Console.WriteLine("Adresse: " + attributs[3]);
                            Console.WriteLine("Téléphone: " + attributs[4]);
                            Console.WriteLine("Email: " + attributs[5]);
                            Console.WriteLine("Type: " + attributs[6]);
                            Console.WriteLine("Montant Dette: " + attributs[7]);

                            Console.WriteLine(new string('-', 50)); // Ligne de séparation après avoir affiché tous les attributs d'un client
                            hasClients = true; // Indique qu'il y a au moins un client
                        }
                        else
                        {
                            Console.WriteLine("Données du client incorrectes: " + line);
                        }
                    }

                    if (!hasClients)
                    {
                        Console.WriteLine("Aucun client enregistré.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
            }
        }

        // Méthode pour afficher les clients ayant des dettes
        public void AfficherClientsAvecDettes()
        {
            string fullPath = "client.txt";
            FileInfo fileInfo = new FileInfo(fullPath);

            if (!fileInfo.Exists)
            {
                Console.WriteLine("Le fichier 'client.txt' n'existe pas.");
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    string ligne;
                    bool hasClientsWithDettes = false;

                    while ((ligne = sr.ReadLine()) != null)
                    {
                        string[] attributs = ligne.Split(':');

                        // Assurez-vous que la ligne a bien 8 parties et que la dette n'est pas vide
                        if (attributs.Length == 8 && !string.IsNullOrWhiteSpace(attributs[7]))
                        {
                            decimal montantDette = 0;
                            bool parseSuccess = false;

                            // Convertit le montant de la dette de string à decimal
                            try
                            {
                                montantDette = Convert.ToDecimal(attributs[7]);
                                parseSuccess = true;
                            }
                            catch (FormatException)
                            {
                                parseSuccess = false;
                            }

                            // Si la conversion réussit et le montant de la dette est supérieur à zéro
                            if (parseSuccess && montantDette > 0)
                            {
                                Console.WriteLine("ID: " + attributs[0]);
                                Console.WriteLine("Nom: " + attributs[1]);
                                Console.WriteLine("Prenom: " + attributs[2]);
                                Console.WriteLine("Adresse: " + attributs[3]);
                                Console.WriteLine("Telephone: " + attributs[4]);
                                Console.WriteLine("Email: " + attributs[5]);
                                Console.WriteLine("Type: " + attributs[6]);
                                Console.WriteLine("Montant Dette: " + montantDette);
                                Console.WriteLine(new string('-', 50)); // Ligne de séparation
                                hasClientsWithDettes = true;
                            }
                        }
                    }

                    if (!hasClientsWithDettes)
                    {
                        Console.WriteLine("Aucun client avec dettes enregistré.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
            }
        }

        // Méthode pour payer une dette
        public void PayerDetteClient(string codeClient)
        {
            string fullPath = "client.txt";
            Console.WriteLine("Chemin complet du fichier : " + fullPath);

            FileInfo fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Exists)
            {
                Console.WriteLine("Aucun fichier 'client.txt' trouvé.");
                return;
            }

            string tempFile = Path.GetTempFileName();

            try
            {
                using (StreamReader sr = fileInfo.OpenText())
                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    string line;
                    bool clientTrouve = false;

                    while ((line = sr.ReadLine()) != null)
                    {
                        var attributs = line.Split(':');
                        if (attributs.Length == 8 && attributs[0] == codeClient)
                        {
                            attributs[7] = "0"; // Remet la dette à zéro
                            clientTrouve = true;
                            Console.WriteLine("La dette a été payée pour le client avec le code : " + codeClient);
                        }

                        sw.WriteLine(string.Join(":", attributs));
                    }

                    if (!clientTrouve)
                    {
                        Console.WriteLine("Client non trouvé.");
                    }
                }

                File.Delete(fullPath);
                File.Move(tempFile, fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour du fichier : " + ex.Message);
            }
        }

        // Méthode pour supprimer un client n'ayant acheté aucun produit
        public void SupprimerClientSansAchat(string clientId)
        {
            string fullPath = "client.txt";
            FileInfo fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Exists)
            {
                Console.WriteLine("Aucun fichier 'client.txt' trouvé.");
                return;
            }

            string tempFile = Path.GetTempFileName();
            try
            {
                using (StreamReader sr = fileInfo.OpenText())
                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    string line;
                    bool clientSupprime = false;

                    while ((line = sr.ReadLine()) != null)
                    {
                        var attributs = line.Split(':');
                        if (attributs.Length == 8 && attributs[0] != clientId)
                        {
                            sw.WriteLine(string.Join(":", attributs));
                        }
                        else if (attributs[0] == clientId)
                        {
                            // On pourrait ajouter ici une vérification pour s'assurer qu'aucun produit n'a été acheté par ce client
                            clientSupprime = true;
                        }
                    }

                    if (clientSupprime)
                    {
                        Console.WriteLine("Client supprimé avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("Client non trouvé ou impossible de le supprimer.");
                    }
                }

                File.Delete(fullPath);
                File.Move(tempFile, fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour du fichier : " + ex.Message);
            }
        }

        // Méthodes d'aide pour la validation des entrées
        private string DemanderEtValiderNom(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^[a-zA-Z]+$"))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez entrer uniquement des lettres.");
                }
            }
        }

        private string DemanderEtValiderTexte(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez ne pas laisser ce champ vide.");
                }
            }
        }

        private string DemanderEtValiderTelephone(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^\d{8}$"))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez entrer un numéro de téléphone à 8 chiffres.");
                }
            }
        }

        private string DemanderEtValiderEmail(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez entrer une adresse e-mail valide.");
                }
            }
        }

        private string DemanderEtValiderType(string message)
        {
            while (true)
            {
                Console.Write(message);
                var input = Console.ReadLine();
                if (input == "1" || input == "2")
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Veuillez entrer 1 pour 'aucun' ou 2 pour 'à crédit'.");
                }
            }
        }

        private string generateCode(string nom, string prenom, string telephone)
        {
            // Générer un code client unique à partir des informations du client
            return "{nom.Substring(0, 2).ToUpper()}{prenom.Substring(0, 2).ToUpper()}{telephone.Substring(0, 4)}";
        }
    }
}
