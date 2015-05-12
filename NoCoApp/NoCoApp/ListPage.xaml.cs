using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NoCoApp
{
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }

        private async void GetCats()
        {
            var service = new CatService();
            var cats = await service.GetCatsAsync();

            BindingContext = cats;
        }

        protected override void OnAppearing()
        {
            if (BindingContext == null)
            {
                GetCats();
            }
            base.OnAppearing();
        }

        private void NavigateTo(object sender, ItemTappedEventArgs args)
        {
            var cat = args.Item;

            var page = new DetailsPage() {BindingContext = cat};
            Navigation.PushAsync(page);
        }
    }

    internal class CatService
    {
        public async Task<IEnumerable<CatModel>> GetCatsAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://catsapi.azurewebsites.net/api/cats");
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<CatModel>>(json);
        }
    }

    public class CatModel
    {
        private static int _id;
        public CatModel()
        {
            Id = _id++;
        }

        public string Url { get; set; }
        public int Id { get; set; }
    }
}
