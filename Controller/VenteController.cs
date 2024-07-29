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
		string path = "vente.txt";
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
						SaveVente();
						break;
					case "b":
						Console.Clear();
						displayVente();
						Console.ReadKey(true);
						break;
					case "c":
						Console.Clear();
						AfficherVentePaiement();
						Console.ReadKey(true);
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
		
		private void SaveVente(){
			Dictionary<string, Produit> produits = ProduitController.DataProduit();
			Dictionary<string, Client> clients = ClientController.DataClient();
			Dictionary<string, SousMesure> smData = SousMesureController.DataSousMesure();
			/*Dictionary<string, SousMesure> = SousMesureController.DataSousMesure();*/
			string idClient = "";
			int i = 1;
			int k = 0;
			string paiement = "";
			int test2 = 0;
			object[] tab = new object[5];
			ArrayList liste = new ArrayList();
			
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
					
					while(k < 2){
						Console.WriteLine("Saisissez le code du produit");
						Console.Write(">> ");
						idProd = Console.ReadLine();
						
						if(!string.IsNullOrWhiteSpace(idProd)){
							if(produits.ContainsKey(idProd)){
								while(true){
									Console.Write("Veuillez Choisir le mode d'achat du produit\n ");
									Console.WriteLine("a. Gros");
									Console.WriteLine("b. Détail");
									Console.Write(">> ");
									string choix = Console.ReadLine();
									
									if(Regex.IsMatch(choix, @"^[aAbBcC]$")){
										switch(choix.ToLower()){
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
																
																clients[idClient].MontantDette = (decimal)clients[idClient].MontantDette - (decimal)tab[4];
																
																produits[idProd].Quantite = produits[idProd].Quantite - int.Parse(qte);
																
																liste.Add(tab);
																test2++;
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
												SousMesure sm = null;
												int test = 0;
												if(SousMesureController.testNombreSousMesure(idProd) == 0){
													Console.ForegroundColor = ConsoleColor.Red;
													Console.WriteLine("LE FORMAT EST DIFFERENT DE CELUI DES NOMBRES");
													Console.ResetColor();
													Console.ReadKey(true);
												}else if (SousMesureController.testNombreSousMesure(idProd) == 1){
													foreach (var element in smData)
													{
														if(string.Compare(element.Value.CodeProduit, idProd,  StringComparison.OrdinalIgnoreCase)==0){
															sm = element.Value;
															test++;
														}
													}
												}else{
													while(true){
														Console.WriteLine("Saisissez le code du sousMesure");
														Console.Write(">> ");
														string codeSm = Console.ReadLine();
														
														if(!string.IsNullOrWhiteSpace(codeSm)){
															if(smData.ContainsKey(codeSm)){
																sm = smData[codeSm];
																test++;
																break;
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
													}
												}
												if(test > 0){
													while(true){
														Console.Write("Veuillez saisir la quantite du produit\n>> ");
														string qte = Console.ReadLine();
														if(Regex.IsMatch(qte, @"^[+-]?\d+$")){
															if(int.Parse(qte) > 0){
																if(sm.Quantite == int.Parse(qte)){
																	Console.ForegroundColor = ConsoleColor.Red;
																	Console.WriteLine("IL EST CONSEILLE DE FAIRE UNE ACHAT EN GROS");
																	Console.ResetColor();
																	Console.ReadKey(true);
																}else if(sm.Quantite < int.Parse(qte)){
																	Console.WriteLine("LA QUANTITE EN STOCK EST INSUFFISANTE");
																}else{
																	tab[0] = idProd;
																	tab[1] = sm.Sous_Mesure;
																	tab[2] = produits[idProd].Nom;
																	tab[3] = int.Parse(qte);
																	tab[4] = int.Parse(qte) * sm.Prix;
																	
																	produits[idProd].Quantite = produits[idProd].Quantite - int.Parse(qte)/(float)sm.Quantite;
																	clients[idClient].MontantDette = clients[idClient].MontantDette - decimal.Parse(""+tab[4]);
																	liste.Add(tab);
																	test2++;
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
													
												}
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
								if(test2 > 0){
									while(true){
										Console.Write("Veuillez Choisir la mode de paiement\n>> ");
										Console.WriteLine("a. Credit");
										Console.WriteLine("b. Cash");
										Console.WriteLine("c. Cheque");
										Console.WriteLine("d. Carte de crédit");
										Console.WriteLine("e. Carte de débit");
										Console.Write(">> ");
										string choix = Console.ReadLine();
										
										if(Regex.IsMatch(choix, @"^[aAbBcCdDeE]$")){
											switch(choix.ToLower()){
												case "a":
													paiement = "Credit";
													break;
												case "b":
													paiement = "Cash";
													break;
												case "c":
													paiement = "Cheque";
													break;
												case "d":
													paiement = "Carte de credit";
													break;
												case "e":
													paiement = "Carte de debit";
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
									double temponTot = 0;
									object[] newTab;
									foreach(var items in liste){
										newTab = (object[])items;
										temponTot += (double)newTab[4];
									}
									Vente vente = new Vente(testNombreVente(), idClient, liste, paiement, temponTot);
									Console.WriteLine(vente.ToString());
									k = 3;
									i = 4;
									// Utiliser StreamWriter avec l'argument append = true
									using (StreamWriter writer = new StreamWriter(path, true))
									{
										writer.WriteLine(vente.Writing());
										
									}
									
									using (StreamWriter writer = new StreamWriter("produit.txt", false))
									{
										writer.WriteLine();
									}
									if(produits.Count != 0){
										foreach(var element in produits){
											using (StreamWriter writer = new StreamWriter("produit.txt", true))
											{
												writer.WriteLine(element.Value.Writing());
												
											}
										}
										
									}
									if(string.Compare(paiement, "Credit", StringComparison.OrdinalIgnoreCase) == 0){
										using (StreamWriter writer = new StreamWriter("client.txt", false))
										{
											writer.WriteLine();
										}
										if(clients.Count != 0){
											foreach(var element in clients){
												using (StreamWriter writer = new StreamWriter("client.txt", true))
												{
													writer.WriteLine(element.Value.Writing());
												}
											}
											
										}
									}
									Console.ForegroundColor = ConsoleColor.Green;
									Console.WriteLine("Vente effectue avec succes");
									Console.ResetColor();
									Console.ReadKey(true);
								}
							}else{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Ce Produit n'existe pas dans notre base de donnees");
								Console.ResetColor();
								Console.ReadKey(true);
							}
						}
						k++;
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
		}
		
		private void displayVente(){
			Dictionary<string, Vente> venteData = DataVente();
			if(venteData != null){
				// Afficher les données du deuxième dictionnaire pour vérifier la copie
				Console.WriteLine("AFFICHAGE DES VENTES\n");

				foreach (var element in venteData)
				{
					Console.WriteLine("---------------------------------------------\n");
					Console.WriteLine("Code: " + element.Value.Id);
					Console.WriteLine("ID CLient : " + element.Value.IdClient);
					foreach(var item in element.Value.InfoProdVendu){
						string[] tableau = item.ToString().Split(',');
						Console.WriteLine("ID Produit : " + tableau[0]);
						Console.WriteLine("Mesure : " + tableau[1]);
						Console.WriteLine("Nom produit : " + tableau[2]);
						Console.WriteLine("Quantite : " + tableau[3]);
					}
					
					Console.WriteLine("Mode de Paiement : " + element.Value.ModePaiement);
					Console.WriteLine("Total a payer : " + element.Value.Total);
					Console.WriteLine("Date vente: " + element.Value.DateVente);
				}
				
			}
		}
		
		private void AfficherVentePaiement(){
			Dictionary<string, Vente> venteData = DataVente();
			
			if(venteData != null){
				// Afficher les données du deuxième dictionnaire pour vérifier la copie
				Console.WriteLine("AFFICHAGE DES VENTES PAR CARTE DEBIT/ CREDIT ET PAR CHEQUE\n");

				foreach (var element in venteData)
				{
					if(string.Compare(element.Value.ModePaiement, "Cheque", StringComparison.OrdinalIgnoreCase) == 0)
					{
						Console.WriteLine("---------------------------------------------\n");
						Console.WriteLine("Code: " + element.Value.Id);
						Console.WriteLine("ID CLient : " + element.Value.IdClient);
						foreach(var item in element.Value.InfoProdVendu){
							string[] tableau = item.ToString().Split(',');
							Console.WriteLine("ID Produit : " + tableau[0]);
							Console.WriteLine("Mesure : " + tableau[1]);
							Console.WriteLine("Nom produit : " + tableau[2]);
							Console.WriteLine("Quantite : " + tableau[3]);
						}
						
						Console.WriteLine("Mode de Paiement : " + element.Value.ModePaiement);
						Console.WriteLine("Total a payer : " + element.Value.Total);
						Console.WriteLine("Date vente: " + element.Value.DateVente);
					}else if(string.Compare(element.Value.ModePaiement, "Carte de credit", StringComparison.OrdinalIgnoreCase) == 0){
						Console.WriteLine("---------------------------------------------\n");
						Console.WriteLine("Code: " + element.Value.Id);
						Console.WriteLine("ID CLient : " + element.Value.IdClient);
						foreach(var item in element.Value.InfoProdVendu){
							string[] tableau = item.ToString().Split(',');
							Console.WriteLine("ID Produit : " + tableau[0]);
							Console.WriteLine("Mesure : " + tableau[1]);
							Console.WriteLine("Nom produit : " + tableau[2]);
							Console.WriteLine("Quantite : " + tableau[3]);
						}
						
						Console.WriteLine("Mode de Paiement : " + element.Value.ModePaiement);
						Console.WriteLine("Total a payer : " + element.Value.Total);
						Console.WriteLine("Date vente: " + element.Value.DateVente);
					}else if(string.Compare(element.Value.ModePaiement, "Carte de debit", StringComparison.OrdinalIgnoreCase) == 0){
						Console.WriteLine("---------------------------------------------\n");
						Console.WriteLine("Code: " + element.Value.Id);
						Console.WriteLine("ID CLient : " + element.Value.IdClient);
						foreach(var item in element.Value.InfoProdVendu){
							string[] tableau = item.ToString().Split(',');
							Console.WriteLine("ID Produit : " + tableau[0]);
							Console.WriteLine("Mesure : " + tableau[1]);
							Console.WriteLine("Nom produit : " + tableau[2]);
							Console.WriteLine("Quantite : " + tableau[3]);
						}
						
						Console.WriteLine("Mode de Paiement : " + element.Value.ModePaiement);
						Console.WriteLine("Total a payer : " + element.Value.Total);
						Console.WriteLine("Date vente: " + element.Value.DateVente);
					}else{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Vide...");
						Console.ResetColor();
						Console.ReadKey(true);
					}
				}
				
			}
		}
		
		public static Dictionary<string, Vente> DataVente(){
			Dictionary<string, Vente> venteData = new Dictionary<string, Vente>();
			
			if (File.Exists("vente.txt"))
			{
				string[] lignes = File.ReadAllLines("vente.txt");
				if(lignes.Length !=0){
					foreach (string ligne in lignes)
					{
						string[] parties = ligne.Split(':');

						if (parties.Length == 6)
						{
							Vente v = new Vente();
							v.Id = int.Parse(parties[0]);
							v.IdClient = parties[1];
							
							v.InfoProdVendu.Add(parties[2]);
							v.ModePaiement = parties[3];
							v.DateVente = DateTime.Parse(parties[4]);
							v.Total = double.Parse(parties[5]);
							
							venteData.Add(""+v.Id, v);
						}
					}
				}
				return venteData;
			}
			else
			{
				Console.WriteLine("Le fichier vente.txt n'existe pas.");
			}
			return null;
		}
		
		public static int testNombreVente(){
			Dictionary<string, Vente> venteData = DataVente();
			int val = 0;

			if(venteData != null){
				foreach (var element in venteData)
				{
					Console.WriteLine(element.Value.Id);
					val = element.Value.Id;
					Console.ReadKey(true);
					val++;
				}
			}
			if(val > 0){
				return val;
			}
			return 0;
		}
	}
}


