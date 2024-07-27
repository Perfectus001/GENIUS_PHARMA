using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Genius_Pharmacie.Model;

namespace Genius_Pharmacie.Controller
{
    public class ClientController
    {
        private Dictionary<string, Client> clients = new Dictionary<string, Client>();
        private int clientIdCounter = 1;  // Compteur pour générer des IDs uniques

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
            var client = new Client(clientId, nom, prenom, adresse, telephone, email, type);
            clients[clientId] = client;
            Console.WriteLine("Client enregistré avec succès. ID: " + clientId);
        }

   // Méthode pour afficher tous les clients
public void AfficherClients()
{
    if (clients.Count == 0)
    {
        Console.WriteLine("Aucun client à afficher.");
    }
    else
    {
        foreach (var client in clients.Values)
        {
            Console.WriteLine(client);
            Console.WriteLine(new string('-', 50)); // Ligne de séparation
        }
    }
}


          // Méthode pour afficher les clients ayant des dettes
        public void AfficherClientsAvecDettes()
        {
            foreach (var client in clients.Values)
            {
                if (client.MontantDette > 0)
                {
                    Console.WriteLine(client);
                    Console.WriteLine(new string('-', 50)); // Ligne de séparation
                }
            }
        }
 // Méthode pour payer une dette
public void PayerDette(string clientId, decimal montant)
{
    // Vérifie si le client avec l'ID fourni existe dans le dictionnaire
    if (clients.ContainsKey(clientId))
    {
        // Récupère le client correspondant à l'ID
        var client = clients[clientId];

        // Vérifie que le montant est valide et ne dépasse pas la dette du client
        if (montant > 0 && client.MontantDette >= montant)
        {
            // Réduit le montant de la dette du client
            client.MontantDette -= montant;

            // Affiche un message confirmant le paiement
            Console.WriteLine("Le montant de " + montant.ToString("C") + " a été payé. Dette restante: " + client.MontantDette.ToString("C"));
        }
        else
        {
            // Affiche un message d'erreur si le montant est invalide ou supérieur à la dette
            Console.WriteLine("Montant invalide ou montant supérieur à la dette.");
        }
    }
    else
    {
        // Affiche un message d'erreur si le client n'est pas trouvé
        Console.WriteLine("Client non trouvé.");
    }
}


        // Méthode pour supprimer un client
public void SupprimerClient(string clientId)
{
    // Vérifie si le client existe dans le dictionnaire
    if (clients.ContainsKey(clientId))
    {
        Client client = clients[clientId];

        // Vérifie si le client n'a acheté aucun produit et n'a aucune dette
        if (!client.AacheteProduit() && client.MontantDette == 0)
        {
            clients.Remove(clientId); // Supprime le client du dictionnaire
            Console.WriteLine("Client supprimé avec succès.");
        }
        else
        {
            Console.WriteLine("Client ne peut pas être supprimé car il a acheté des produits ou a une dette.");
        }
    }
    else
    {
        Console.WriteLine("Client non trouvé.");
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

        // Méthode de validation du numéro de téléphone
        private bool ValidateTelephone(string telephone)
        {
            // Vérifie que le téléphone contient exactement 8 chiffres
            return Regex.IsMatch(telephone, @"^\d{8}$");
        }

        // Méthode de validation de l'email
        private bool ValidateEmail(string email)
        {
            // Vérifie que l'email a le format xxx@gmail.com
            return Regex.IsMatch(email, @"^[^@\s]+@gmail\.com$");
        }

        // Méthode de validation du type
        private bool ValidateType(string type)
        {
            // Vérifie que le type est soit '1' soit '2'
            return type == "1" || type == "2";
        }
        
        
    }
}
