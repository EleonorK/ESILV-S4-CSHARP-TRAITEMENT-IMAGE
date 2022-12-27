using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Diagnostics;

namespace TDtraitementImage
{
    class Program
    {
        #region centrer le texte
        /// <summary>
        /// Pour centrer le texte écrit, code trouvé sur internet (c'est le seul programme pris sur internet)
        /// https://openclassrooms.com/fr/courses/1526901-apprenez-a-developper-en-c/2867826-une-console
        /// </summary>
        /// <param name="texte"></param> le texte affiché sur la console (à la ligne)
        static void CentrerLeTexte(string texte)
        {
            int nbEspaces = (Console.WindowWidth - texte.Length) / 2;
            Console.SetCursorPosition(nbEspaces, Console.CursorTop);
            Console.WriteLine(texte);
        }
        /// <summary>
        /// Pour centrer le texte écrit, code trouvé sur internet (c'est le seul programme pris sur internet)
        /// https://openclassrooms.com/fr/courses/1526901-apprenez-a-developper-en-c/2867826-une-console
        /// </summary>
        /// <param name="texte"></param>le texte affiché sur la console (à la suite)
        static void CentrerLeTexteL(string texte)
        {
            int nbEspaces = (Console.WindowWidth - texte.Length) / 2;
            Console.SetCursorPosition(nbEspaces, Console.CursorTop);
            Console.Write(texte);
        }
        #endregion
        /// <summary>
        /// c'est la sécurité pour la saisie d'un nombre ou un chiffre (pour le centre)
        /// réutilisation du code de mon binôme et modifier pour convenir à mon code
        /// </summary>
        /// <param name="min"></param> le minimum de la saisie
        /// <param name="max"></param> le maximum de la saisie
        /// <returns></returns> le chiffre ou nombre valide saisi par l'utilisateur
        static int SecuriteCentre(int min, int max)
        {
            int a = 0;
            bool test = true;
            string choix = null;
            do
            {
                choix = Console.ReadLine();
                try
                {
                    a = Int32.Parse(choix);
                    test = true;
                }
                catch
                {
                    test = false;
                }
                if (a < min || a > max || test == false)
                {
                    Console.WriteLine();
                    CentrerLeTexte("Cette option n'existe pas, veuillez saisir une option proposée ");
                    CentrerLeTexteL("Nouvelle saisie : ");
                }

            } while (a < min || a > max || test == false);
            return a;
        }
        public Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                Image image = Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);

            }
            return bitmap;
        }
        static void Test001()
        {
            string nomFichier = "Images/coco.bmp";
            MyImage m1 = new MyImage(nomFichier);
            m1.Agrandir();

            //Console.WriteLine(m1.MyFile);

            /*for(int i = 0; i < m1.MyFile.Length; i++)
            {
                Console.Write(m1.MyFile[i] + " ; ");
            }*/
        }
        static string Demande()
        {
            string fichier = null;
            CentrerLeTexte("Bienvenu sur le Traitement de l'image\n\n");
            CentrerLeTexte("Veuillez saisir l'image à modifier : \n");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"1\ Hibiscus");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"2\ Mario");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"3\ Parapluies");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"4\ Sticker");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"5\ beelzebub");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"6\ HxH");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"7\ Nana");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"8\ We Bare Bears");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"9\ Quitter" + "\n");
            CentrerLeTexteL("Votre Choix : ");
            int nb = SecuriteCentre(1, 9);
            switch (nb)
            {
                case 1:
                    fichier = "Images/images/hibiscus.bmp";
                    Console.Clear();
                    break;
                case 2:
                    fichier = "Images/images/mario.bmp";
                    Console.Clear();
                    break;
                case 3:
                    fichier = "Images/images/parapluies.bmp";
                    Console.Clear();
                    break;
                case 4:
                    fichier = "Images/images/fier.bmp";
                    Console.Clear();
                    break;
                case 5:
                    fichier = "Images/images/Beel.bmp";
                    Console.Clear();
                    break;
                case 6:
                    fichier = "Images/images/HxH.bmp";
                    Console.Clear();
                    break;
                case 7:
                    fichier = "Images/images/nana.bmp";
                    Console.Clear();
                    break;
                case 8:
                    fichier = "Images/images/we_bare_bear.bmp";
                    Console.Clear();
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
            }
            return fichier;
        }
        public void Methode()
        {
            string fichier = Demande();
            Console.Clear();
            CentrerLeTexte("Bienvenue sur le Traitement de l'image\n\n");
            CentrerLeTexte("Veuillez saisir ce que vous voulez modifier : \n");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"1\ Noir et Blanc");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"2\ Gris");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"3\ Effets de Miroir");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"4\ Rotation");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"5\ Agrandir");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"6\ Retour");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"7\ Quitter" +"\n");
            CentrerLeTexteL("Votre Choix : ");
            int nb = SecuriteCentre(1, 7);
            switch (nb)
            {
                case 1:
                    MyImage m1 = new MyImage(fichier);
                    Console.Clear();
                    m1.NoirEtBlanc();
                    break;
                case 2:
                    MyImage m2 = new MyImage(fichier);
                    Console.Clear();
                    m2.Gris();
                    break;
                case 3:
                    MyImage m3 = new MyImage(fichier);
                    Console.Clear();
                    m3.MenuMiroir();
                    break;
                case 4:
                    MyImage m4 = new MyImage(fichier);
                    Console.Clear();
                    m4.Rotation();
                    break;
                case 5:
                    MyImage m5 = new MyImage(fichier);
                    Console.Clear();
                    m5.Agrandir();
                    break;
                case 6:
                    Console.Clear();
                    string[] autre = null;
                    Main(autre);
                    //Demande();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
            }
        }
        static void Main(string[] args)
        {
            //Test001();
            Program p = new Program();
            p.Methode();
            //Methode();
            Console.ReadLine();
        }
    }
}
