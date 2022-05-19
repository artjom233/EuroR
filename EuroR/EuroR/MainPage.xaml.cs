using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EuroR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<riig> riigi { get; set; }
        Button Del_btn, lis_btn;
        Label lbl_list;
        ListView list;
        
        public MainPage()
        {
            riigi = new ObservableCollection<riig>
            {
                new riig { Nime = "USA", Plinn = "New-York", Peopl = "8 384 322", Pilt = "USA.png" },                
                new riig { Nime = "Россия", Plinn = "Москва", Peopl = "147 688 368", Pilt = "RUS.png" },
                new riig { Nime = "Україна", Plinn = "Киев", Peopl = "44 154 322", Pilt = "UKR.png" },
                new riig { Nime = "Estonia", Plinn = "Tallinn", Peopl = "5 695 981", Pilt = "EST.png" }
            };
            
            lbl_list = new Label
            {
                Text = "Страны",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            Del_btn = new Button
            {
                Text = "Удали"
            };
            Del_btn.Clicked += Kustuta_btn_Clicked;
            lis_btn = new Button
            {
                Text = "Добавь"
            };
            lis_btn.Clicked += Lisa_btn_Clicked;
            list = new ListView
            {
                SeparatorColor = Color.Black,
                Header = "Страна:",
                Footer = DateTime.Now.ToString("T"),
                
                HasUnevenRows = true,
                ItemsSource = riigi,
                ItemTemplate = new DataTemplate(() =>
                {
                    ImageCell imageCell = new ImageCell { TextColor = Color.Orange, DetailColor = Color.Red };
                    imageCell.SetBinding(ImageCell.TextProperty, "Название");
                    Binding bin = new Binding { Path = "Столица", StringFormat = "Сталица: {0}" };
                    imageCell.SetBinding(ImageCell.DetailProperty, bin);
                    Binding bin2 = new Binding { Path = "Naselenie", StringFormat = "Население: {0}" };
                    imageCell.SetBinding(ImageCell.DetailProperty, bin2);
                    imageCell.SetBinding(ImageCell.ImageSourceProperty, "Картинка");
                    return imageCell;
                })
            };
            list.ItemTapped += List_ItemTapped;
            this.Content = new StackLayout
            {
                Children = {
                    lbl_list, list,Del_btn,lis_btn }
            };
        }

        private async void Lisa_btn_Clicked(object sender, EventArgs e)
        {
            string Name = await DisplayPromptAsync("Какая страна", "Текст сюда:", keyboard: Keyboard.Text);
            string riik = await DisplayPromptAsync("Какая столица", "Текст сюда:", keyboard: Keyboard.Text);
            string people = await DisplayPromptAsync("Сколько насиление", "Текст сюда:", keyboard: Keyboard.Numeric);
            string png = await DisplayPromptAsync("Ссылка на картинку", "Текст сюда:", keyboard: Keyboard.Text);

            if (Name == "" || riik == "" || people == "" || png == "") return;
            riig newest = new riig { Nime = Name, Plinn = riik, Peopl = people, Pilt = png };
            foreach (riig thing in riigi)
            {
                if (thing.Nime == newest.Nime)
                    return;
            }
            riigi.Add(item: newest);
        }

        private void Kustuta_btn_Clicked(object sender, EventArgs e)
        {
            riig riig = list.SelectedItem as riig;
            if (riig != null)
            {
                riigi.Remove(riig);
                list.SelectedItem = null;
            }
        }
        
        private async void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            riig selectedRiig = e.Item as riig;
            if (selectedRiig != null)
                await DisplayAlert(selectedRiig.Nime, $"Сталица-{selectedRiig.Plinn}, \nНаселение-{selectedRiig.Peopl}", "ОК");
        }
    }
}
