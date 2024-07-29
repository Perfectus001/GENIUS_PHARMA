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
                    attributs[7] = "0"; // Reset the debt amount to 0
                    line = string.Join(":", attributs);
                    clientTrouve = true;
                }
                sw.WriteLine(line);
            }

            if (clientTrouve)
            {
                Console.WriteLine("La dette du client avec le code " + codeClient + " a été payée.");
            }
            else
            {
                Console.WriteLine("Aucun client trouvé avec le code " + codeClient + ".");
            }
        }

        // Remplace le fichier d'origine par le fichier temporaire
        FileInfo tempFileInfo = new FileInfo(tempFile);
        tempFileInfo.Replace(fullPath, null, true);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur lors de la mise à jour de la dette du client : " + ex.Message);
    }
}
public static void SupprimerClientSansAchat(string clientId)
{
    string clientFilePath = "client.txt";
    string venteFilePath = "vente.txt";
    string tempFilePath = Path.GetTempFileName();

    FileInfo clientFileInfo = new FileInfo(clientFilePath);
    FileInfo venteFileInfo = new FileInfo(venteFilePath);
    FileInfo tempFileInfo = new FileInfo(tempFilePath);

    if (!clientFileInfo.Exists)
    {
        Console.WriteLine("Aucun fichier 'client.txt' trouvé.");
        return;
    }

    if (!venteFileInfo.Exists)
    {
        Console.WriteLine("Le fichier 'vente.txt' n'existe pas. Aucune vérification des achats n'a été effectuée.");
        // Nous pouvons décider ici de ne pas supprimer le client ou d'informer l'utilisateur.
        return;
    }

    try
    {
        bool clientSupprime = false;

        // Lire le fichier vente.txt pour vérifier si le client a acheté un produit
        bool hasPurchased = false;
        using (StreamReader sr = venteFileInfo.OpenText())
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var attributs = line.Split(':');
                if (attributs.Length > 1 && attributs[1] == clientId)
                {
                    hasPurchased = true;
                    break;
                }
            }
        }

        if (!hasPurchased)
        {
            // Si le client n'a fait aucun achat, supprimer du fichier client.txt
            using (StreamReader sr = clientFileInfo.OpenText())
            using (StreamWriter sw = tempFileInfo.CreateText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var attributs = line.Split(':');
                    if (attributs.Length == 8 && attributs[0] == clientId)
                    {
                        clientSupprime = true;
                        continue; // Ne pas écrire cette ligne dans le fichier temporaire
                    }
                    sw.WriteLine(line);
                }
            }

            if (clientSupprime)
            {
                // Remplacer l'ancien fichier client.txt par le fichier temporaire mis à jour
                tempFileInfo.Replace(clientFilePath, null);
                Console.WriteLine("Le client avec l'ID " + clientId + " a été supprimé.");
            }
            else
            {
                // Si le client n'a pas été trouvé, supprimer le fichier temporaire
                tempFileInfo.Delete();
                Console.WriteLine("Aucun client avec l'ID " + clientId + " trouvé.");
            }
        }
        else
        {
            // Si le client a effectué des achats, supprimer le fichier temporaire
            tempFileInfo.Delete();
            Console.WriteLine("Le client avec l'ID " + clientId + " a acheté des produits et ne peut pas être supprimé.");
        }
    }
    catch (Exception ex)
    {
        // Gestion des erreurs
        Console.WriteLine("Erreur lors de la suppression du client : " + ex.Message);
        if (tempFileInfo.Exists)
        {
            tempFileInfo.Delete();
        }
    }
}



        // Méthode pour demander et valider le nom
        private string DemanderEtValiderNom(string message)
        {
            string input;
            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (ValidateName(input))
                    break;
                Console.WriteLine("Le format est incorrect. Veuillez entrer des lettres uniquement.");
            }
            return input;
        }

        // Méthode pour demander et valider un texte (adresse)
        private string DemanderEtValiderTexte(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        // Méthode pour demander et valider le téléphone
        private string DemanderEtValiderTelephone(string message)
        {
            string input;
            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (ValidateTelephone(input))
                    break;
                Console.WriteLine("Le format est incorrect. Veuillez entrer 8 chiffres.");
            }
            return input;
        }

        // Méthode pour demander et valider l'email
        private string DemanderEtValiderEmail(string message)
        {
            string input;
            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (ValidateEmail(input))
                    break;
                Console.WriteLine("Le format est incorrect. Veuillez entrer un email au format xxx@gmail.com.");
            }
            return input;
        }

        // Méthode pour demander et valider le type
        private string DemanderEtValiderType(string message)
        {
            string input;
            while (true)
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (ValidateType(input))
                    break;
                Console.WriteLine("Le format est incorrect. Veuillez entrer '1' pour aucun ou '2' pour à crédit.");
            }
            // Retourne 'aucun' pour 1 et 'à crédit' pour 2
            return input == "1" ? "aucun" : "à crédit";
        }

        // Méthode de validation du nom
        private bool ValidateName(string name)
        {
            // Vérifie que le nom contient uniquement des lettres
            return Regex.IsMatch(name, @"^[a-zA-Z]+$");
        }

        // Méthode de validation du téléphone
        private bool ValidateTelephone(string telephone)
        {
            // Vérifie que le téléphone contient exactement 8 chiffres
            return Regex.IsMatch(telephone, @"^\d{8}$");
        }

        // Méthode de validation de l'email
        private bool ValidateEmail(string email)
        {
            // Vérifie que l'email suit le format de base d'un email
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Méthode de validation du type
        private bool ValidateType(string type)
        {
            // Vérifie que le type est '1' ou '2'
            return type == "1" || type == "2";
        }

        // Méthode pour générer un code unique pour un client
        private string generateCode(string nom, string prenom, string telephone)
        {
            // Crée un code basé sur les premières lettres du nom, du prénom et du téléphone
            return nom.Substring(0, 1) + prenom.Substring(0, 1) + telephone.Substring(telephone.Length - 4);
        }
    }
}
