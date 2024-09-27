using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BoxOne.SelectionChanged += BoxOne_SelectionChanged;
            BoxTwo.SelectionChanged += BoxTwo_SelectionChanged;//   Сортировка по цене

            ListBox.SelectionChanged += ListBox_SelectionChanged;// Вызов окна редактирования элемента

            Dobavit.Click += Dobavit_Click;

            BoxOne.SelectedIndex = 0;
            List_Box();// Загрузка и сортировка
            Upd();
        }
        public void Upd()
        {
            List<Manufacturer> man = new List<Manufacturer>();// создание
            man = Helper.DataBase.Manufacturers.ToList();// Заносим информацию из листа с производителями
            man.Add(new Manufacturer() { ManufacturerName = "Все значения" });//Добавляем в лист элемент "Все занчения"

            BoxOne.ItemsSource = man.OrderByDescending(x => x.ManufacturerName == "Все значения").ToList();//Указываем чтобы добавленый элемент был первым
            BoxOne.SelectedIndex = 0;// делаем элемент заглавным
        }
        public void List_Box()
        {
            List<Product> Search;
            Search = Helper.DataBase.Products.ToList();

            // Сортировка по цене  
            if (BoxTwo.SelectedIndex == 1)// Сортировка по Убыванию
            {
                Search = Helper.DataBase.Products.OrderByDescending(z => z.Cost).ToList();
            }
            else if (BoxTwo.SelectedIndex == 2)// Сортировка по Воззрастанию
            {
                Search = Helper.DataBase.Products.OrderBy(z => z.Cost).ToList();
            }
            // Сортировка по производителю         
            if (BoxOne.SelectedIndex > 0)
            {
                var selectedManufacturer = (Manufacturer)BoxOne.SelectedItem; // Получаем выбранного производителя
                Search = Search.Where(p => p.IdManufacturer == selectedManufacturer.Id).ToList();
            }
            // Сортировка по поиску
            string SearchText = SearchTextBox.Text ?? "";
            if (SearchText.Length > 0)
            {
                Search = Search.Where(y => y.TovarName.Contains(SearchText) 
                                        || y.Description.Contains(SearchText)).ToList();
            }

            Vsego.Text = Convert.ToString(Helper.DataBase.Products.Count());// Сколько всего эл-тов
            Pokaz.Text = Convert.ToString(Search.Count);//Сколько отображается эл-ов сейчас
            ListBox.ItemsSource = Search;
        }// Загрузка данных в листбокс и сортировки

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ListBox.SelectedItem as Product;

            if (selectedItem != null)
            {
                Redact redactWindow = new Redact(selectedItem.Id.ToString());
                redactWindow.Show();
                Close();
            }
        }
        private void Dobavit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Redact redactWindow = new Redact(string.Empty); 
            redactWindow.Show();
            Close();
        }// Окно добавления
        private void BoxOne_SelectionChanged(object? sender, SelectionChangedEventArgs e) => List_Box();
        private void BoxTwo_SelectionChanged(object? sender, SelectionChangedEventArgs e) => List_Box();
        private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e) => List_Box();
     
    }
}