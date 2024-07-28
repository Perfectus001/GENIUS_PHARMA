﻿/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 26/07/2024
 * Time: 22:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;
using Genius_Pharmacie.Controller;

namespace Genius_Pharmacie
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.Title = "HOME";
			bool while1 = true;
			string choix = null;
			while(while1){
				while(true){
					Console.Clear();
					Console.WriteLine("========================================================");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(":                    Menu Principal                    :");
					Console.ResetColor();
					Console.WriteLine("========================================================");
					Console.WriteLine("1- Gestion des Ventes");
					Console.WriteLine("2- Gestion des Clients");
					Console.WriteLine("3- Gestion des Produits");
					Console.WriteLine("4- Gestion des Sous-mesures de produit");
					Console.WriteLine("5- Auteurs du programme");
					Console.WriteLine("6- Quitter Programme");
					Console.Write("========================================================\n>> ");				
					choix = Console.ReadLine();
					if(Regex.IsMatch(choix, @"^[1-6]$")){
						break;
					}else{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Veuillez saisir un chiffre entre [1-6]");
						Console.ResetColor();
						Console.ReadKey(true);
					}
				}
				switch(choix){
					case "1":
						VenteController venteController = new VenteController();
						venteController.menuVente();
						break;
					case "2":
						ClientController clientController = new ClientController();
						clientController.MenuClient();
						break;
					case "3":
						ProduitController produitController = new ProduitController();
						produitController.MenuProduit();
						break;
					case "4":
						
						break;
					case "5":
						
						break;
					case "6":
						while1 = false;
						break;
					default:
						Console.WriteLine("Mauvais choix!!!");
						break;
				}
			}
			
			// TODO: Implement Functionality Here
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);			
		}
	}
}
