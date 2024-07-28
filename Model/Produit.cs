/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 26/07/2024
 * Time: 22:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;

namespace Genius_Pharmacie.Model
{
	/// <summary>
	/// Description of Produit.
	/// </summary>
	public class Produit
	{
		// Attributs de la classe Produit
		private string code;
		private string categorie;
		private string mesure;
		private string nom;
		private double prixAchat;
		private double prixVente;
		private float quantite;
		private string modeVente;
		private DateTime dateEnregistrement;
		
		// getters et setters
		public string Code
		{
			get{return code;}
			set{code = value;}
		}
		
		public string Categorie
		{
			get{return categorie;}
			set{categorie = value;}
		}
		
		public string Mesure
		{
			get{return mesure;}
			set{mesure = value;}
		}
		
		public string Nom
		{
			get{return nom;}
			set{nom = value;}
		}
		
		public double PrixAchat
		{
			get{return prixAchat;}
			set{prixAchat = value;}
		}
		
		public double PrixVente
		{
			get{return prixVente;}
			set{prixVente = value;}
		}
		
		public float Quantite
		{
			get{return quantite;}
			set{quantite = value;}
		}
		
		public string ModeVente
		{
			get{return modeVente;}
			set{modeVente = value;}
		}
		
		public DateTime DateEnregistrement
		{
			get{return dateEnregistrement;}
			set{dateEnregistrement = value;}
		}
		
		//Constructeur sans parametre
		public Produit()
		{
		}
		
		//Constructeur avec parametre
		public Produit(string code, string categorie, string mesure, string nom, double prixAchat, double prixVente, float quantite, string modeVente)
		{
			this.code = code;
			this.categorie = categorie;
			this.mesure = mesure;
			this.nom = nom;
			this.prixAchat = prixAchat;
			this.prixVente = prixVente;
			this.quantite = quantite;
			this.modeVente = modeVente;
			this.dateEnregistrement = DateTime.Now;
		}
		
		
		public override string ToString()
		{
			return string.Format("[Produit Code={0}, Categorie={1}, Mesure={2}, Nom={3}, PrixAchat={4}, PrixVente={5}, Quantite={6}, ModeVente={7}, DateEnregistrement={8}]", code, categorie, mesure, nom, prixAchat, prixVente, quantite, modeVente, dateEnregistrement);
		}
		
	}
}
