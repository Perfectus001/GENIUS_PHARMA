/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 28/07/2024
 * Time: 02:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using Genius_Pharmacie.Model;

namespace Genius_Pharmacie.Controller
{
	/// <summary>
	/// Description of VenteController.
	/// </summary>
	public class VenteController
	{
		public VenteController()
		{
		}
		
		public void menuVente(){
			Console.Title = "GESTION VENTE";
			bool while1 = true;
			string choix = null;
			while(while1){
				while(true){
					Console.Clear();
					Console.WriteLine("========================================================");
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(":                      Menu Vente                      :");
					Console.ResetColor();
					Console.WriteLine("========================================================");
					Console.WriteLine("a. Enregistrer la vente des produits");
					Console.WriteLine("b. Afficher la liste des ventes");
					Console.WriteLine("c. Afficher la liste de ventes payées par chèque, \ncarte de crédit ou débit");
					Console.WriteLine("d. Menu principal");
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
						int a = this.SaveVente();
						if(a == 1){
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Vente effectue avec succes");
							Console.ResetColor();
							Console.ReadKey(true);
						}
						break;
					case "b":
						
						Console.ReadKey(true);
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
		
		private int SaveVente(){
			Dictionary<string, Produit> produits = ProduitController.DataProduit();
			Dictionary<string, Client> clients = ClientController.DataClient();
			/*Dictionary<string, SousMesure> = SousMesureController.DataSousMesure();*/
			string idClient = "";
			int i = 1;
			
			while(i < 3){
				Console.WriteLine("Saisissez l'identifiant du client");
				Console.Write(">> ");
				idClient = Console.ReadLine();
				
				if(!string.IsNullOrWhiteSpace(idClient)){
					if(clients.ContainsKey(idClient)){
						i = 0;
						
					}else{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Ce client n'existe pas dans notre base de donnees");
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
				
				if(i == 0){
					string idProd = "";
					int k = 0;
					while(k < 2){
						Console.WriteLinez("Saisissez le code du produit");
						Console.Write(">> ");
						idProd = Console.ReadLine();
						
						if(!string.IsNullOrWhiteSpace(idProd)){
							if(produits.ContainsKey(idProd)){
								while(true){
									Console.Write("Veuillez Choisir le mode d'achat du produit\n>> ");
									Console.WriteLine("a. Gros");
									Console.WriteLine("b. Détail");
									Console.WriteLine("c. Gros et Detail");
									Console.Write(">> ");
									choix = Console.ReadLine();
									
									if(Regex.IsMatch(choix, @"^[aAbBcC]$")){
										switch(choix.ToLower()){
												object[] tab = new object[5];
												ArrayList liste = new ArrayList();
											case "a":
												while(true){
													Console.Write("Veuillez saisir la quantite du produit\n>> ");
													string qte = Console.ReadLine();
													if(Regex.IsMatch(qte, @"^[+-]?\d+$")){
														if(int.Parse(qte) > 0){
															if(produits[idProd].Quantite < int.Parse(qte)){
																Console.ForegroundColor = ConsoleColor.Red;
																Console.WriteLine("LA QUANTITE EN STOCK EST INSUFFISANTE");
																Console.ResetColor();
																Console.ReadKey(true);																
															}else{
																tab[0] = idProd;
																tab[1] = produits[idProd].Nom;
																tab[2] = produits[idProd].Mesure;
																tab[3] = int.Parse(qte);
																tab[4] = int.Parse(qte) * produits[idProd].PrixVente;
																
																produits[idProd].Quantite = produits[idProd].Quantite - int.Parse(qte);
																
																liste.Add(tab);
															}
															break;
														}else{
															Console.ForegroundColor = ConsoleColor.Red;
															Console.WriteLine("LE NOMBRE DOIT ETRE SUPERIEUR A 0");
															Console.ResetColor();
															Console.ReadKey(true);
														}
													}else{
														Console.ForegroundColor = ConsoleColor.Red;
														Console.WriteLine("LE FORMAT EST DIFFERENT DE CELUI DES NOMBRES");
														Console.ResetColor();
														Console.ReadKey(true);
													}
												}
												break;
											case "b":
												
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
								break;
							}else{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Ce Produit n'existe pas dans notre base de donnees");
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
						k++;
					}
				}
				i++;
			}
			return 0;
		}
	}
}
