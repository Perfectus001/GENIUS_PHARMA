/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 28/07/2024
 * Time: 21:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Genius_Pharmacie.Model
{
	/// <summary>
	/// Description of SousMesure.
	/// </summary>
	public class SousMesure
	{
		//Attributs de la classe Sous-mesure
		private string code;
		private string codeProduit;
		private string sous_Mesure;
		private double prix;
		private int quantite;		
		
		public SousMesure()
		{
		}
		
		//
		public SousMesure(string code, string codeProduit, string sous_Mesure, double prix, int quantite)
		{
			this.code = code;
			this.codeProduit = codeProduit;
			this.prix = prix;
			this.sous_Mesure = sous_Mesure;
			this.quantite = quantite;
		}
		

		
		//getters et setters
		public string Code
		{
			get{return code;}
			set{code = value;}
		}
		
		public string CodeProduit
		{
			get{return codeProduit;}
			set{codeProduit = value;}
		}
		
		public string Sous_Mesure
		{
			get{return sous_Mesure;}
			set{sous_Mesure = value;}
		}
		
		public double Prix
		{
			get{return prix;}
			set{prix = value;}
		}
		
		public int Quantite
		{
			get{return quantite;}
			set{quantite = value;}
		}

		public override string ToString()
		{
			return string.Format("[SousMesure Code={0}, CodeProduit={1}, SousMesure={2}, Prix={3}, Quantite={4}]", code, codeProduit, sous_Mesure, prix, quantite);
		}
		
		public string Writing()
		{
			return string.Format("{0}:{1}:{2}:{3}:{4}", code, codeProduit, sous_Mesure, prix, quantite);
		}

	}
}
