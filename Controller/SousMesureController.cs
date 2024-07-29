/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 28/07/2024
 * Time: 21:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using Genius_Pharmacie.Model;
using System.Text.RegularExpressions;

namespace Genius_Pharmacie.Controller
{
	/// <summary>
	/// Description of SousMesureController.
	/// </summary>
	public class SousMesureController
	{
		public string path = "sousMesure.txt";
		
		public SousMesureController()
		{
		}
		
		public void MenuSousMesure()
		{
			Console.Title = "GESTION SOUS MESURE";
			bool while1 = true;
			string choix = null;
			while(while1){
				while(true){
					Console.Clear();
					Console.WriteLine("========================================================");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(":                   Menu Sous-mesure                   :");
					Console.ResetColor();
					Console.WriteLine("========================================================");
					Console.WriteLine("a. Ajout de sous mesure");
					Console.WriteLine("b. ---");
					Console.WriteLine("c. ----");
					Console.Write("========================================================\n>>");
					choix = Console.ReadLine();
					if(Regex.IsMatch(choix, @"^[a-dA-D]$")){
						break;
					}else{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Veuillez saisir une lettre entre [a-d]");
						Console.ResetColor();
						Console.ReadKey(true);
					}
				}
				Console.Clear();
				switch(choix.ToLower()){
					case "a":
						Console.Clear();
						int a = Save();
						if(a == 1){
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Sous mesure ajouter avec succes");
							Console.ResetColor();
							Console.ReadKey(true);
						}
						break;
					case "b":
						//Display();
						Console.ReadKey(true);
						break;
					case "c":

						break;
					case "d":
						while1 = false;
						break;
					default:
						Console.WriteLine("Mauvais choix!!!");
						break;
				}
			}
		}
		
		private int Save(){
			Dictionary<string, Produit> produits = ProduitController.DataProduit();
			string idProduit = "";
			SousMesure sm = new SousMesure();
			SousMesure sm2 = null;
			//Choix de categorie de produit
			int i = 0;
			
			while(i < 2){
				Console.WriteLine("Saisissez le code du produit");
				Console.Write(">> ");
				idProduit = Console.ReadLine();
				
				if(!string.IsNullOrWhiteSpace(idProduit)){
					if(produits.ContainsKey(idProduit)){
						if(testNombreSousMesure(idProduit) < 2){
							//sous-mesure de produit
							while(true){
								Console.Write("Veuillez saisir la sous-mesure du produit\n>> ");
								string mesure = Console.ReadLine();
								if(Regex.IsMatch(mesure, @"^[A-Za-z]")){
									sm.Sous_Mesure = mesure;
									break;
								}else{
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine("DOIT DEBUTER PAR UNE LETTRE");
									Console.ResetColor();
								}
							}
							
							//Prix de vente
							while(true){
								Console.Write("Veuillez saisir le prix du sous-mesure\n>> ");
								string prixV = Console.ReadLine();
								if(Regex.IsMatch(prixV, @"^[+-]?(\d+(\.\d*)?|\d*\.\d+)$")){
									if(double.Parse(prixV) > 0){
										sm.Prix = double.Parse(prixV);
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
										sm.Quantite = int.Parse(qte);
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
							
							sm2 = new SousMesure(this.generateCode(idProduit, testNombreSousMesure(idProduit)), idProduit, sm.Sous_Mesure, sm.Prix, sm.Quantite);
							
							// Utiliser StreamWriter avec l'argument append = true
							using (StreamWriter writer = new StreamWriter(path, true))
							{
								writer.WriteLine(sm2.Writing());
								return 1;
							}
						}else{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Ce produit possede deja 2 sous-mesure dans notre base de donnees");
							Console.ResetColor();
							Console.ReadKey(true);
						}
					}else{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Ce produit n'existe pas dans notre base de donnees");
						Console.ResetColor();
						Console.ReadKey(true);
					}
				}else{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Aucune information n'a ete saisi");
					Console.ResetColor();
					Console.ReadKey(true);
				}
				i++;
			}
			return 0;
		}
		
		
		
		public static Dictionary<string, SousMesure> DataSousMesure(){
			Dictionary<string, SousMesure> smData = new Dictionary<string, SousMesure>();
			
			if (File.Exists("sousMesure.txt"))
			{
				string[] lignes = File.ReadAllLines("sousMesure.txt");
				if(lignes.Length !=0){
					foreach (string ligne in lignes)
					{
						string[] parties = ligne.Split(':');

						if (parties.Length == 5)
						{
							SousMesure sm = new SousMesure();
							
							sm.Code = parties[0];
							sm.CodeProduit = parties[1];
							sm.Sous_Mesure = parties[2];
							sm.Prix = double.Parse(parties[3]);
							sm.Quantite = int.Parse(parties[4]);
							
							smData.Add(sm.Code, sm);
							
						}
					}
				}
				return smData;
			}
			else
			{
				Console.WriteLine("Le fichier sousMesure.txt n'existe pas.");
			}
			return null;
		}
		
		public static int testNombreSousMesure(string code){
			Dictionary<string, SousMesure> smData = DataSousMesure();
			int val = 0;
			
			if(smData != null){
				foreach (var element in smData)
				{
					if(string.Compare(element.Value.CodeProduit, code,  StringComparison.OrdinalIgnoreCase)==0){
						val++;
					}
				}
			}

			if(val > 0){
				return val;
			}
			return 0;
		}
		
		//Fonction permettant de generer le code sous-mesure
		private string generateCode(string codeProd, int i){
			
			// Construire le code du produit
			string codeSm = ""+codeProd+"-"+i;
			
			return codeSm;
		}
		
	}
}
