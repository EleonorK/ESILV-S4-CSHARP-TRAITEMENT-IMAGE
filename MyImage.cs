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
    class MyImage
    {
        private string typeImage;
        private int tailleFichier;
        private int tailleOffset;
        private int hauteur;
        private int largeur;
        private int nbBitsParCouleur;
        private byte[] myFile;
        private Pixel[,] matImage;
        private int offset;
        private byte[] header = new byte[54];

        #region propriétés
        public string TypeImage
        {
            get { return this.typeImage; }
        }
        public int TailleFichier
        {
            get { return this.tailleFichier; }
        }
        public int TailleOffset
        {
            get { return this.tailleOffset; }
        }
        public byte[] MyFile
        {
            get { return this.myFile; }
        }
        public Pixel[,] MatImage
        {
            get { return this.matImage; }
        }
        #endregion
        public MyImage(string fichier)
        {
            From_Image_To_File(fichier);
            for (int i = 0; i < 54; i++) { this.header[i] = this.myFile[i]; }
            byte[] monFichier = File.ReadAllBytes(fichier);
            byte[] signature = new byte[2];
            byte[] tailleFichiertab = new byte[4];
            int itaille = 0;
            byte[] offsetImageTab = new byte[4];
            int ioffset = 0;
            byte[] hauteurtab = new byte[4];
            byte[] largeurtab = new byte[4];
            byte[] nbBitsParCouleurtab = new byte[2];
            int ihaut = 0, ilarg = 0, inbB = 0;

            for(int i = 0; i < monFichier.Length; i++)
            {
                if(i <= 1) { signature[i] = monFichier[i]; }
                if(i >= 2 && i <= 5) { tailleFichiertab[itaille] = monFichier[i]; itaille++; }
                if(i >= 10 && i <= 13) { offsetImageTab[ioffset] = monFichier[i]; ioffset++; }
                if(i >= 18 && i <= 21) { largeurtab[ilarg] = monFichier[i];ilarg++; }
                if(i >= 22 && i <= 25) { hauteurtab[ihaut] = monFichier[i]; ihaut++; }
                if(i >= 28 && i <= 29) { nbBitsParCouleurtab[inbB] = monFichier[i];inbB++; }
            }
            //Conversion en int
            ASCIIEncoding ascii = new ASCIIEncoding();
            this.typeImage = ascii.GetString(signature, 0, 2);
            this.tailleFichier = ConvertEndianToInt(tailleFichiertab);
            this.tailleOffset = ConvertEndianToInt(offsetImageTab);
            this.hauteur = ConvertEndianToInt(hauteurtab);
            this.largeur = ConvertEndianToInt(largeurtab);
            this.nbBitsParCouleur = ConvertEndianToInt(nbBitsParCouleurtab);

            //transfomation de l'image dans une matrice de pixel
           TransformationMAtPixel();
            
        }
        #region méthodes de conversion
        //methodes conversion en int
        public int ConvertEndianToInt(byte[] tab)
        {
            double n = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                n += tab[i] * (Math.Pow(256,i));
            }
            return Convert.ToInt32(n);
        }
        //methodes de conversion int en Endian
        //https://stackoverflow.com/questions/2350099/how-to-convert-an-int-to-a-little-endian-byte-array
        public byte[] ConvertIntToEndian(int data, int taille)
        {
            byte[] b = new byte[taille];
            b[0] = (byte)data;
            b[1] = (byte)(((uint)data >> 8) & 0xFF);
            b[2] = (byte)(((uint)data >> 16) & 0xFF);
            b[3] = (byte)(((uint)data >> 24) & 0xFF);
            return b;
            /*byte[] tab = new byte[taille];
            for (int i = 0; i < taille; i++)
            {
                double n = Convert.ToDouble(tab[i]);
                tab[i] = Convert.ToByte(val / Convert.ToInt32((Math.Pow(256, Convert.ToDouble(i)))));
                val = Convert.ToInt32(val - tab[i]);
            }
            return tab;*/
        }
        //avoir le fichier (photo) et la transformer en fichier binaire
        public void From_Image_To_File(string fileName)
        {
            this.myFile = File.ReadAllBytes(fileName);
        }
        //transfomation de l'image dans une matrice de pixel
        public void TransformationMAtPixel()
        {
            this.offset = 54;
            this.matImage = new Pixel[this.hauteur, this.largeur];
            for(int i = 0; i < this.hauteur; i++)
            {
                for(int j = 0; j < this.largeur; j++)
                {
                    this.matImage[i, j] = new Pixel(this.myFile[this.offset], this.myFile[this.offset + 1], this.myFile[this.offset + 2]);
                    this.offset += 3;
                }
            }
        }
        #endregion
        #region transformation de l'image
        #region agrandir 
        public void Agrandir()
        {
            int coeff = 0;
            Pixel[,] imageAgrandie = null;
            byte[] convertion = new byte[4], tab = null;

            CentrerLeTexteL("Veuillez donner un coéfficient d'agrandissement pour l'image : ");
            coeff = SecuriteCentre(1,5);
            imageAgrandie = new Pixel[this.hauteur * coeff, this.largeur * coeff];
            for(int i = 0; i < imageAgrandie.GetLength(0); i++)
            {
                for(int j = 0; j < imageAgrandie.GetLength(1); j++)
                {
                    imageAgrandie[i, j] = this.matImage[i / coeff, j / coeff];
                }
            }
            this.matImage = imageAgrandie;

            tab = new byte[((this.largeur * coeff) * (this.hauteur * coeff) * 3) + 54];
            convertion = ConvertIntToEndian(this.largeur * coeff, 4);
            for (int i = 0; i < 4; i++)
            {
                this.header[18 + i] = convertion[i];
            }
            convertion = ConvertIntToEndian(this.hauteur * coeff, 4);
            for (int i = 0; i < 4; i++)
            {
                this.header[22 + i] = convertion[i];
            }
            Creation(tab);
            Console.Clear();
            Program pg = new Program();
            pg.Methode();
        }
        #endregion
        #region noir, blanc et gris
        public void Gris()
        {
            byte[] tab = null;
            for (int i = 0; i < this.hauteur; i++)
            {
                for(int j = 0; j<this.largeur; j++)
                {
                    this.matImage[i, j].Gris();
                }
            }
            tab = new byte[this.tailleFichier];
            Creation(tab);
            Console.Clear();
            Program pg = new Program();
            pg.Methode();
        }
        public void NoirEtBlanc()
        {
            byte[] tab = null;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    this.matImage[i, j].NoirEtBlanc();
                }
            }
            tab = new byte[this.tailleFichier];
            Creation(tab);
            Console.Clear();
            Program pg = new Program();
            pg.Methode();
        }
        #endregion
        #region rotation
        public void Rotation()
        {
            byte[] tab = new byte[this.tailleFichier];
            CentrerLeTexte("Veuillez choisir un angle de rotation");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\1 90°");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\2 180°");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\3 270°");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\4 Retour au menu");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\5 Quitter");
            CentrerLeTexteL("Votre Choix : ");
            int nb = SecuriteCentre(1, 5);
            switch (nb)
            {
                case 1:
                    Rotation90();
                    PermuterHauteurLargeur();
                    Creation(tab);
                    break;
                case 2:
                    Rotation90();
                    Rotation90();
                    Creation(tab);
                    break;
                case 3:
                    Rotation90();
                    Rotation90();
                    Rotation90();
                    PermuterHauteurLargeur();
                    Creation(tab);
                    break;
                case 4:
                    Console.Clear();
                    Program pg = new Program();
                    pg.Methode();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
            Console.Clear();
            Rotation();
        }
        public void Rotation90()
        {
            int autre = this.hauteur;
            this.hauteur = this.largeur;
            this.largeur = autre;
            Pixel[,] imageModif = new Pixel[this.hauteur, this.largeur];
            for(int i = 0; i < this.matImage.GetLength(0); i++)
            {
                for(int j = 0; j < this.matImage.GetLength(1); j++)
                {
                    imageModif[j, this.matImage.GetLength(0) - 1 - i] = this.matImage[i, j];
                }
            }
            this.matImage = imageModif;
        }
        public void PermuterHauteurLargeur()
        {
            byte[] autre = new byte[4];
            for(int i = 0; i < 4; i++)
            {
                autre[i] = this.header[22 + i];
                this.header[22 + i] = this.header[18 + i];
                this.header[18 + i] = autre[i];
            }
        }
        #endregion
        #region Reflet et Miroir
        public void Reflet()
        {
            int largeurModif = 2 * this.largeur, tailleModif = 0;
            byte[] tab = null, convertion = new byte[4];
            Pixel[,] imageModif = new Pixel[this.hauteur,largeurModif];
            for (int i = 0; i < this.hauteur; i++)
            {
                for(int j = 0; j < this.largeur; j++)
                {
                    imageModif[i, j] = this.matImage[i, j];
                    imageModif[i, imageModif.GetLength(1) - 1 - j] = this.matImage[i, j];
                }
            }
            this.matImage = imageModif;
            convertion = ConvertIntToEndian(largeurModif,4);
            tailleModif = (this.tailleFichier - 54) * 2 + 54;
            tab = new byte[tailleModif];
            for(int i = 0; i < 4; i++)
            {
                this.header[18 + i] = convertion[i];
            }
            Creation(tab);
        }
        #region miroir
        public void MenuMiroir()
        {
            byte[] tab = new byte[tailleFichier];
            CentrerLeTexte("Veuillez choisir un miroir \n");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\1 Horizontal");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\2 Vertical");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\3 Reflet");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\4 Retour au menu");
            Console.WriteLine("\t\t\t\t\t\t\t" + @"\5 Quitter"+"\n");
            CentrerLeTexteL("Votre Choix : ");
            int nb = SecuriteCentre(1, 5);
            switch (nb)
            {
                case 1:
                    MiroirHorizontal();
                    Creation(tab);
                    Console.Clear();
                    MenuMiroir();
                    break;
                case 2:
                    MiroirVertical();
                    Creation(tab);
                    Console.Clear();
                    MenuMiroir();
                    break;
                case 3:
                    Reflet();
                    Console.Clear();
                    Program pg1 = new Program();
                    pg1.Methode();
                    break;
                case 4:
                    Console.Clear();
                    Program pg = new Program();
                    pg.Methode();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
        public void MiroirHorizontal()
        {
            Pixel[,] imageModif = new Pixel[this.hauteur, this.largeur];
            for (int i = 0; i < this.matImage.GetLength(0); i++)
            {
                for (int j = 0; j < this.matImage.GetLength(1); j++)
                {
                    imageModif[this.matImage.GetLength(0) - 1 - i, j] = this.matImage[i, j];
                }
            }
            this.matImage = imageModif;
        }
        public void MiroirVertical()
        {

            Pixel[,] imageModif = new Pixel[this.hauteur, this.largeur];
            for (int i = 0; i < this.matImage.GetLength(0); i++)
            {
                for (int j = 0; j < this.matImage.GetLength(1); j++)
                {
                    imageModif[i, this.matImage.GetLength(1) - 1 - j] = this.matImage[i, j];
                }
            }
            this.matImage = imageModif;
        }
        #endregion
        #endregion
        #region création de l'image modifée
        public void Creation(byte[] tab)
        {
            int nvx = 0;
            for (int i = 0; i < 54; i++)
            {
                tab[i] = this.header[i];
            }
            for (int i = 0; i < this.matImage.GetLength(0); i++)
            {
                for (int j = 0; j < this.matImage.GetLength(1); j++)
                {
                    tab[54 + nvx] = Convert.ToByte(this.matImage[i, j].Bleu);
                    tab[55 + nvx] = Convert.ToByte(this.matImage[i, j].Vert);
                    tab[56 + nvx] = Convert.ToByte(this.matImage[i, j].Rouge);
                    nvx += 3;
                }
            }
            File.WriteAllBytes("test001Nouveau.bmp", tab);
            Process.Start("test001Nouveau.bmp");
        }
        #endregion
        #endregion
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
        #endregion
    }

}
