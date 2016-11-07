using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace YemekDeneme
{
    public partial class MainPage : ContentPage
    {
        private SQLiteConnection database;

        public MainPage()
        {
            InitializeComponent();

            CreateDatabaseAsync();
        }


        private async void CreateDatabaseAsync()
        {
            await Task.Delay(0);

            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<YemekTarifi>();

            var r = GetTarifler();
            if (!r.Any(f => f.YemekAd == "Musakka")) InsertTarif("Musakka", "Patlıcan", "İyice pişirin");
            if (!r.Any(f => f.YemekAd == "Köfte")) InsertTarif("Köfte", "Kıyma", "Yağda kızartın");

            UpdateTarifList();
        }

        private void UpdateTarifList()
        {
            var r = GetTarifler();
            TarifList.ItemsSource = r;
        }

        public async void TarifEkleClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(yemekAd.Text) || string.IsNullOrWhiteSpace(malzeme.Text) || string.IsNullOrWhiteSpace(tarif.Text))
            {
                await DisplayAlert("Hata!", "Boş alan kalmamalı!", "OK");
                return;
            }

            InsertTarif(yemekAd.Text, malzeme.Text, tarif.Text);

            UpdateTarifList();
        }

        private void InsertTarif(string yemekAd, string malzeme, string tarif)
        {
            var t = new YemekTarifi();
            t.YemekAd = yemekAd;
            t.Malzeme = malzeme;
            t.Tarif = tarif;

            database.Insert(t, typeof(YemekTarifi));
        }

        public IEnumerable<YemekTarifi> GetTarifler()
        {
            return (from i in database.Table<YemekTarifi>() select i).ToList();
        }
    }
}
