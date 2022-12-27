using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDtraitementImage
{
    class Pixel
    {
        private int rouge;
        private int vert;
        private int bleu;
        public Pixel(int r, int v, int b)
        {
            if(r >= 0 && r < 256 && b >= 0 && b < 256 && v >= 0 && v < 256)
            {
                rouge = b;
                vert = v;
                bleu = r;
            }
        }
        #region propriétés
        public int Rouge
        {
            get { return this.rouge; }
            //set { rouge = value; }
        }
        public int Vert
        {
            get { return this.vert; }
            //set { vert = value; }
        }
        public int Bleu
        {
            get { return this.bleu; }
            //set { bleu = value; }
        }
        #endregion
        #region méthodes pour image en noir et blanc
        public void Gris()
        {
            int gris = (this.rouge + this.vert + this.bleu) / 3;
            this.rouge = gris;
            this.vert = gris;
            this.bleu = gris;
        }
        public void NoirEtBlanc()
        {
            int gris = (this.rouge + this.vert + this.bleu) / 3;
            if (gris <= 128)
            {
                this.rouge = 0;
                this.vert = 0;
                this.bleu = 0;
            }
            else
            {
                this.rouge = 255;
                this.vert = 255;
                this.bleu = 255;
            }
        }
        #endregion
    }
}
