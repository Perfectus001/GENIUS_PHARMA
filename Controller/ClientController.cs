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
		
		//
		public void MenuClient()
		{
			Console.Title = "GESTION CLIENT";
			bool while1 = true;
			string choix = null;
			while(while1){
				while(true){
					Console.Clear();
					Console.WriteLine("========================================================");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(":                      Menu Client                     :");
					Console.ResetColor();
					Console.WriteLine("========================================================");
	                Console.WriteLine("a. Enregistrer un client");
	                Console.WriteLine("b. Afficher tous les clients");
	                Console.WriteLine("c. Payer une dette");
	                Console.WriteLine("d. Supprimer un client");
	                Console.WriteLine("e. Retour au menu principal");
					Console.Write("========================================================\n>>");				
					choix = Console.ReadLine();
					if(Regex.IsMatch(choix, @"^[a-eA-E]$")){
						break;
					}else{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Veuillez saisir une lettre entre [a-e]");
						Console.ResetColor();
						Console.ReadKey(true);
					}
				}
				Console.Clear();
				switch(choix.ToLower()){
					case "a":
						Console.Clear();
						this.EnregistrerClient();
						break;
					case "b":

						break;
					case "c":

						break;
					case "d":

						break;
					case "e":
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
			var nom = DemanderEtValiderNom( "Nom (lettres uniquement) : ");
			var prenom = DemanderEtValiderNom("Prenom (lettres uniquement) : ");
			var adresse = DemanderEtValiderTexte("Adresse : ");
			var telephone = DemanderEtValiderTelephone("Téléphone (8 chiffres) : ");
			var email = DemanderEtValiderEmail("Email (format xxx@gmail.com) : ");
			var type = DemanderEtValiderType("Type (1: aucun, 2: à crédit) : ");

			// Générer un ID unique
			var clientId = clientIdCounter.ToString();

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
		
		//Fonction permettant de generer le code du client 
		private string generateCode(string nom, string prenom, string telephone){
        	
			// Obtenir la première lettre du prenom, du nom et num de tel
	        char premiereLettreprenom = prenom[0];
	        char premiereLettreNom = nom[0];
	        string tel2 = telephone.Substring(0,2);
	
	        // Générer un nombre aléatoire de 4 chiffres
	        Random random = new Random();
	        int nombreAleatoire = random.Next(1000, 10000); // entre 1000 et 9999
	
	        // Construire le code du client
	        string codeClient = "" + premiereLettreprenom + premiereLettreNom + tel2 + "-" + nombreAleatoire;
	
	        return codeClient;
		}
		
		public static Dictionary<string, Client> DataClient(){
			Dictionary<string, Client> clients = new Dictionary<string, Client>();
			
			if (File.Exists("client.txt"))
			{
				string[] lignes = File.ReadAllLines("client.txt");

				foreach (string ligne in lignes)
				{
					string[] parties = ligne.Split(':');

					if (parties.Length == 8)
					{
						Client client = new Client();
						
						client.Id = parties[0];
						client.Nom = parties[1];
						client.Prenom = parties[2];
						client.Adresse = parties[3];
						client.Telephone = parties[4];
						client.Email = parties[5];
						client.Type = parties[6];
						client.MontantDette = decimal.Parse(parties[7]);
						
						clients.Add(client.Id, client);
						
					}
				}
				
				foreach(var el in clients){
					Console.WriteLine(el.Key + " : " + el.Value);
				}
				Console.ReadKey(true);
				return clients;
			}
			else
			{
				Console.WriteLine("Le fichier client.txt n'existe pas.");
			}
			return null;
		}
		
	}
}
