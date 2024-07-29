/*
 * Created by SharpDevelop.
 * User: Perfectus
 * Date: 28/07/2024
 * Time: 02:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace Genius_Pharmacie.Model
{
	/// <summary>
	/// Description of Vente.
	/// </summary>
	public class Vente
	{
		private int id;
		private string idClient;
		private ArrayList infoProdVendu = null;
		private string modePaiement;
		private double total;
		
		//getter et setters
		public int Id
		{
			get{return id;}
			set{id = value;}
		}
		
		public string IdClient
		{
			get{return idClient;}
			set{idClient = value;}
		}
		
		public ArrayList InfoProdVendu
		{
			get{return infoProdVendu;}
			set{infoProdVendu.Add(value);}
		}
		
		public string ModePaiement
		{
			get{return modePaiement;}
			set{modePaiement = value;}
		}
		
		public double Total
		{
			get{return total;}
			set{total = value;}
		}
		
		
		public Vente()
		{
		}
	}
}
