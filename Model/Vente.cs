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
using System.Linq;
using System.Collections.Generic;

namespace Genius_Pharmacie.Model
{
	/// <summary>
	/// Description of Vente.
	/// </summary>
	public class Vente
	{
		private int id;
		private string idClient;
		private ArrayList infoProdVendu = new ArrayList();
		private string modePaiement;
		private DateTime dateVente;
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
		
		public DateTime DateVente
		{
			get{return dateVente;}
			set{dateVente = value;}
		}
		
		public double Total
		{
			get{return total;}
			set{total = value;}
		}
		
		
		public Vente()
		{
		}
		
		public Vente(int id, string idClient, ArrayList liste, string modepaiement, double total){
			this.id = id;
			this.idClient = idClient;
			this.infoProdVendu = new ArrayList(liste);
			this.modePaiement = modepaiement;
			this.dateVente = DateTime.Now;
			this.total = total;
		}
		
		public override string ToString()
		{
			return string.Format("[Vente Id={0}, IdClient={1}, InfoProdVendu={2}, ModePaiement={3}, DateVente={4}, Total={5}]", id, idClient, infoProdVendu.ToString(), modePaiement, dateVente, total);
		}

		public string Writing()
		{
            var infoProdStrings = infoProdVendu.Cast<object[]>().Select(array => string.Join(", ", array.Select(item => item.ToString())));
            return string.Format("{0}:{1}:{2}:{3}:{4}:{5}", id, idClient, string.Join(" | ", infoProdStrings), modePaiement, dateVente.ToString("yyyy-MM-dd"), total);
		}
	}
}
