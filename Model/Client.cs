﻿using System;

namespace Genius_Pharmacie.Model
{
    public class Client
    {
        public string Id { get; set;}
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public decimal MontantDette { get; set; }

        public Client(string id, string nom, string prenom, string adresse, string telephone, string email, string type)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Telephone = telephone;
            Email = email;
            Type = type;
            MontantDette = 0; // Montant dette initialisé à zéro lors de l'enregistrement
        }
           public bool ClientHasNoDebt()
    {
        // Vérifie si le montant de la dette est nul ou inférieur ou égal à zéro
        return MontantDette <= 0;
    }
        
        
       


        public override string ToString()
        {
            return "ID: " + Id + "\nNom: " + Nom + "\nPrenom: " + Prenom + "\nAdresse: " + Adresse + 
                   "\nTelephone: " + Telephone + "\nEmail: " + Email + "\nType: " + Type + "\nMontant Dette: " + MontantDette;
        }
        
        public string Writing(){
            return Id +":" + Nom + ":" + Prenom + ":" + Adresse + ":" + Telephone + ":" + Email + ":" + Type + ":" + MontantDette;        	
        }
    }
}
