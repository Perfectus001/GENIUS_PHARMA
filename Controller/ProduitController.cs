/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 26/07/2024
 * Time: 23:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Genius_Pharmacie.Model;
using System.Text.RegularExpressions;


namespace Genius_Pharmacie.Controller
{
	/// <summary>
	/// Description of ProduitController.
	/// </summary>
	public class ProduitController
	{
		//Constructeur
		public ProduitController()
		{
		}
		
		public void MenuProduit()
		{
			Console.Title = "MANIPULATION DE MATRICE";
			bool while1 = true;
			string choix = null;
			while(while1){
				while(true){
					Console.Clear();
					Console.WriteLine("========================================================");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(":                     Menu Produit                     :");
					Console.ResetColor();
					Console.WriteLine("========================================================");
					Console.WriteLine("a. Enregistrer Produit\n");
					Console.WriteLine("b. Affichage de la liste des produits\n");
					Console.WriteLine("c. Modifier le prix unitaire et/ou de vente d’un produit par son Code\n");
					Console.WriteLine("d. Affichage de la liste des produits en ordre croissant\n");
					Console.WriteLine("e. Affichage de la liste des produits en ordre décroissant");
					Console.WriteLine("f. Quitter Menu matrice");
					Console.Write("========================================================\n>>");				
					choix = Console.ReadLine();
					if(Regex.IsMatch(choix, @"^[a-fA-F]$")){
						break;
					}else{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Veuillez saisir une lettre entre [a-f]");
						Console.ResetColor();
						Console.ReadKey(true);
					}
				}
				Console.Clear();
				switch(choix.ToLower()){
					case "a":
						int a = Save();
						if(a == 1){
							Console.WriteLine("Succes");
						}
						Console.Clear();
						break;
					case "b":

						break;
					case "c":

						break;
					case "d":

						break;
					case "e":

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
		
		private int Save(){
			string choix = "";
			Produit produit = new Produit();
			//Choix de categorie de produit
			while(true){
				Console.Write("Veuillez Choisir la categorie de produit\n>> ");
				Console.WriteLine("a. pharmaceutiques");
				Console.WriteLine("b. cosmetiques");
				Console.Write(">> ");
				choix = Console.ReadLine();
				
				if(Regex.IsMatch(choix, @"^[aAbB]$")){
					switch(choix.ToLower()){
						case "a":
							produit.Categorie = "pharmaceutiques";
							break;
						case "b":
							produit.Categorie = "cosmetiques";
							break;
						default:
							Console.WriteLine("Mauvais choix!!!");
							break;
					}
					break;
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Pressez [A|B]\n");
					Console.ResetColor();
				}
			}
			
			//mesure de produit
			while(true){
				Console.Write("Veuillez saisir la mesure du produit\n>> ");
				string mesure = Console.ReadLine();
				if(Regex.IsMatch(mesure, @"^[A-Za-z]")){
					produit.Mesure = mesure;
					break;
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("DOIT DEBUTER PAR UNE LETTRE");
					Console.ResetColor();
				}
			}
			
			//nom du produit
			while(true){
				Console.Write("Veuillez saisir le nom du produit\n>> ");
				string nom = Console.ReadLine();
				if(Regex.IsMatch(nom, @"^[A-Za-z]")){
					produit.Nom = nom;
					break;
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("DOIT DEBUTER PAR UNE LETTRE");
					Console.ResetColor();
				}
			}
			
			//Prix d'achat
			while(true){
				Console.Write("Veuillez saisir le prix d'achat du produit\n>> ");
				string prixA = Console.ReadLine();
				if(Regex.IsMatch(prixA, @"^[+-]?(\d+(\.\d*)?|\d*\.\d+)$")){
					if(double.Parse(prixA) > 0){
						produit.PrixAchat = double.Parse(prixA);
						break;						
					}else{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("LE NOMBRE DOIT ETRE SUPERIEUR A 0");
						Console.ResetColor();						
					}
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("LE FORMAT EST DIFFERENT DE CELUI DES NOMBRES");
					Console.ResetColor();
				}
			}
			
			//Prix de vente
			while(true){
				Console.Write("Veuillez saisir le prix de vente du produit\n>> ");
				string prixV = Console.ReadLine();
				if(Regex.IsMatch(prixV, @"^[+-]?(\d+(\.\d*)?|\d*\.\d+)$")){
					if(double.Parse(prixV) > 0){
						produit.PrixVente = double.Parse(prixV);
						break;						
					}else{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("LE NOMBRE DOIT ETRE SUPERIEUR A 0");
						Console.ResetColor();						
					}
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("LE FORMAT EST DIFFERENT DE CELUI DES NOMBRES");
					Console.ResetColor();
				}
			}
			
			//Quantite
			while(true){
				Console.Write("Veuillez saisir la quantite du produit\n>> ");
				string qte = Console.ReadLine();
				if(Regex.IsMatch(qte, @"^[+-]?\d+$")){
					if(int.Parse(qte) > 0){
						produit.Quantite = int.Parse(qte);
						break;						
					}else{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("LE NOMBRE DOIT ETRE SUPERIEUR A 0");
						Console.ResetColor();		
					}
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("LE FORMAT EST DIFFERENT DE CELUI DES NOMBRES");
					Console.ResetColor();
				}
			}
			
			//Mode de vente
			//Choix de categorie de produit
			while(true){
				Console.Write("Veuillez Choisir la categorie de produit\n>> ");
				Console.WriteLine("a. Gros");
				Console.WriteLine("b. Détail");
				Console.WriteLine("c. Gros et Detail");
				Console.Write(">> ");
				choix = Console.ReadLine();
				
				if(Regex.IsMatch(choix, @"^[aAbBcC]$")){
					switch(choix.ToLower()){
						case "a":
							produit.ModeVente = "Gros";
							break;
						case "b":
							produit.ModeVente = "Detail";
							break;
						case "c":
							produit.ModeVente = "Gros et Detail";
							break;
						default:
							Console.WriteLine("Mauvais choix!!!");
							break;
					}
					break;
				}else{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Pressez [A - C]\n");
					Console.ResetColor();
				}
			}
			
			Produit prod = new Produit(this.generateCode(produit.Nom, produit.Categorie), produit.Categorie, produit.Mesure, produit.Nom, produit.PrixAchat, produit.PrixVente, produit.Quantite, produit.ModeVente);
			Console.WriteLine(prod.ToString());
			Console.ReadKey(true);
			
			return 1;
		}
		
				//Fonction permettant de generer le code du produit 
		private string generateCode(string nom, string categorie){
        	// Obtenir la première lettre de la catégorie et du nom
	        char premiereLettreCategorie = categorie[0];
	        char premiereLettreNom = nom[0];
	
	        // Générer un nombre aléatoire de 4 chiffres
	        Random random = new Random();
	        int nombreAleatoire = random.Next(1000, 10000); // entre 1000 et 9999
	
	        // Construire le code du produit
	        string codeProduit = ""+premiereLettreCategorie+premiereLettreNom+"-"+nombreAleatoire;
	
	        return codeProduit;
		}
		
	}
}
