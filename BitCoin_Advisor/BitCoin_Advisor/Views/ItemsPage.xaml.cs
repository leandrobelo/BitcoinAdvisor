﻿using System;

using BitCoin_Advisor.Models;
using BitCoin_Advisor.ViewModels;

using Xamarin.Forms;

namespace BitCoin_Advisor.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    //if (viewModel.Items.Count == 0 && viewModel.LoadItemsCommand != null)
        //    //    viewModel.LoadItemsCommand.Execute(null);
        //}
    }
}
