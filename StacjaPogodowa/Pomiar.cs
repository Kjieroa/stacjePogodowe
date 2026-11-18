using System;

namespace WpfApp1
{
    public class Pomiar
    {
        public string Stacja { get; set; }
        public string DataCzas { get; set; }
        public double Temperatura { get; set; }
        public int Cisnienie { get; set; }

        public string DoFormatuPliku()
        {
            return $"{Stacja} ; {DataCzas} ; {Temperatura} ; {Cisnienie}";
        }
    }
}