using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<Pomiar> wszystkiePomiary = new List<Pomiar>();
        private readonly string sciezkaPliku = "C:\\Users\\PC\\source\\repos\\StacjaPogodowa\\StacjaPogodowa\\pomiary.txt";

        public MainWindow()
        {
            InitializeComponent();
            WczytajZPliku();
        }

        private void WczytajZPliku()
        {
            if (!File.Exists(sciezkaPliku))
            {
                MessageBox.Show("Nie znaleziono pliku pomiary.txt w folderze programu!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            wszystkiePomiary.Clear();
            string[] linie = File.ReadAllLines(sciezkaPliku);

            foreach (string linia in linie)
            {
                if (string.IsNullOrWhiteSpace(linia)) continue;

                string[] czesci = linia.Split(new string[] { " ; " }, StringSplitOptions.None);

                if (czesci.Length == 4)
                {
                    wszystkiePomiary.Add(new Pomiar
                    {
                        Stacja = czesci[0].Trim(),
                        DataCzas = czesci[1].Trim(),
                        Temperatura = double.Parse(czesci[2].Trim().Replace('.', ',')),
                        Cisnienie = int.Parse(czesci[3].Trim())
                    });
                }
            }

            AktualizujListeStacji();
            OdswiezWidok();
        }

        private void ZapiszDoPliku()
        {
            List<string> linieDoZapisu = wszystkiePomiary.Select(p => p.DoFormatuPliku()).ToList();
            File.WriteAllLines(sciezkaPliku, linieDoZapisu);
            MessageBox.Show("Dane zostały zapisane do pliku.", "Zapis", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AktualizujListeStacji()
        {
            string aktualnyWybor = cmbFiltrStacja.SelectedItem?.ToString() ?? "Wszystkie stacje";

            cmbFiltrStacja.Items.Clear();
            cmbFiltrStacja.Items.Add("Wszystkie stacje");

            var unikalneStacje = wszystkiePomiary
                .Select(p => p.Stacja)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            foreach (var stacja in unikalneStacje)
            {
                cmbFiltrStacja.Items.Add(stacja);
            }

            if (cmbFiltrStacja.Items.Contains(aktualnyWybor))
                cmbFiltrStacja.SelectedItem = aktualnyWybor;
            else
                cmbFiltrStacja.SelectedIndex = 0;
        }

        private void OdswiezWidok()
        {
            string wybranaStacja = cmbFiltrStacja.SelectedItem?.ToString();
            List<Pomiar> przefiltrowane;

            if (string.IsNullOrEmpty(wybranaStacja) || wybranaStacja == "Wszystkie stacje")
            {
                przefiltrowane = wszystkiePomiary.ToList();
            }
            else
            {
                przefiltrowane = wszystkiePomiary
                    .Where(p => p.Stacja.Equals(wybranaStacja, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            dgvPomiary.ItemsSource = null;
            dgvPomiary.ItemsSource = przefiltrowane;

            PodswietlMaxMin(przefiltrowane);
        }

        private void PodswietlMaxMin(List<Pomiar> lista)
        {
            if (lista == null || lista.Count == 0) return;

            double maxTemp = lista.Max(p => p.Temperatura);
            double minTemp = lista.Min(p => p.Temperatura);

            // Wymuszenie odświeżenia wierszy żeby DataGrid poprawnie złapał kontenery
            dgvPomiary.UpdateLayout();

            for (int i = 0; i < lista.Count; i++)
            {
                var row = (DataGridRow)dgvPomiary.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    double temp = lista[i].Temperatura;
                    if (temp == maxTemp)
                    {
                        row.Background = Brushes.LightCoral;
                    }
                    else if (temp == minTemp)
                    {
                        row.Background = Brushes.LightSkyBlue;
                    }
                    else
                    {
                        row.Background = Brushes.White;
                    }
                }
            }
        }

        private void btnWczytaj_Click(object sender, RoutedEventArgs e)
        {
            WczytajZPliku();
        }

        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            ZapiszDoPliku();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStacja.Text) ||
                !double.TryParse(txtTemperatura.Text.Replace('.', ','), out double temp) ||
                !int.TryParse(txtCisnienie.Text, out int cisnienie))
            {
                MessageBox.Show("Wprowadź poprawne dane (liczby w polu temp. i ciśnienia).", "Błąd");
                return;
            }

            Pomiar nowy = new Pomiar
            {
                Stacja = txtStacja.Text.Trim(),
                DataCzas = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Temperatura = temp,
                Cisnienie = cisnienie
            };

            wszystkiePomiary.Add(nowy);

            txtStacja.Clear();
            txtTemperatura.Clear();
            txtCisnienie.Clear();

            AktualizujListeStacji();
            OdswiezWidok();
        }

        private void cmbFiltrStacja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OdswiezWidok();
        }
    }
}