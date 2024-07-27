﻿using System;
using Genius_Pharmacie.Controller;

namespace Genius_Pharmacie
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientController = new ClientController();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menu Principal ===");
                Console.WriteLine("1. Gestion des Clients");
                Console.WriteLine("2. Quitter");
                Console.Write("Choisissez une option : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GestionClients(clientController);
                        break;

                    case "2":
                        return;

                    default:
                        Console.WriteLine("Option invalide. Veuillez essayer à nouveau.");
                        Pause();
                        break;
                }
            }
        }

        static void GestionClients(ClientController clientController)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestion des Clients ===");
                Console.WriteLine("1. Enregistrer un client");
                Console.WriteLine("2. Afficher tous les clients");
                Console.WriteLine("3. Afficher les clients avec dettes"); // Nouvelle option
                Console.WriteLine("4. Payer une dette");
                Console.WriteLine("5. Supprimer un client");
                Console.WriteLine("6. Retour au menu principal"); // Mise à jour de l'option
                Console.Write("Choisissez une option : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        clientController.EnregistrerClient();
                        Pause();
                        break;

                    case "2":
                        clientController.AfficherClients();
                        Pause();
                        break;

                    case "3":
                        clientController.AfficherClientsAvecDettes(); // Nouvelle méthode
                        Pause();
                        break;

                    case "4":
                        PayerDette(clientController);
                        break;

                    case "5":
                        SupprimerClient(clientController);
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Option invalide. Veuillez essayer à nouveau.");
                        Pause();
                        break;
                }
            }
        }

        static void PayerDette(ClientController clientController)
        {
            Console.Write("Entrez l'ID du client pour payer la dette : ");
            string payerDetteClientId = Console.ReadLine();
            Console.Write("Entrez le montant à payer : ");
            string montantInput = Console.ReadLine();

            // Conversion de la chaîne en decimal
            decimal montant;
            if (decimal.TryParse(montantInput, out montant))
            {
                clientController.PayerDette(payerDetteClientId, montant);
            }
            else
            {
                Console.WriteLine("Montant invalide.");
            }

            Pause();
        }

        static void SupprimerClient(ClientController clientController)
        {
            Console.Write("Entrez l'ID du client à supprimer : ");
            string supprimerClientId = Console.ReadLine();
            clientController.SupprimerClient(supprimerClientId);
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
