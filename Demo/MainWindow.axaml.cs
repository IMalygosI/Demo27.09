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
            BoxTwo.SelectionChanged += BoxTwo_SelectionChanged;//   ���������� �� ����

            ListBox.SelectionChanged += ListBox_SelectionChanged;// ����� ���� �������������� ��������

            Dobavit.Click += Dobavit_Click;

            BoxOne.SelectedIndex = 0;
            List_Box();// �������� � ����������
            Upd();
        }
        public void Upd()
        {
            List<Manufacturer> man = new List<Manufacturer>();// ��������
            man = Helper.DataBase.Manufacturers.ToList();// ������� ���������� �� ����� � ���������������
            man.Add(new Manufacturer() { ManufacturerName = "��� ��������" });//��������� � ���� ������� "��� ��������"

            BoxOne.ItemsSource = man.OrderByDescending(x => x.ManufacturerName == "��� ��������").ToList();//��������� ����� ���������� ������� ��� ������
            BoxOne.SelectedIndex = 0;// ������ ������� ���������
        }
        public void List_Box()
        {
            List<Product> Search;
            Search = Helper.DataBase.Products.ToList();

            // ���������� �� ����  
            if (BoxTwo.SelectedIndex == 1)// ���������� �� ��������
            {
                Search = Helper.DataBase.Products.OrderByDescending(z => z.Cost).ToList();
            }
            else if (BoxTwo.SelectedIndex == 2)// ���������� �� ������������
            {
                Search = Helper.DataBase.Products.OrderBy(z => z.Cost).ToList();
            }
            // ���������� �� �������������         
            if (BoxOne.SelectedIndex > 0)
            {
                var selectedManufacturer = (Manufacturer)BoxOne.SelectedItem; // �������� ���������� �������������
                Search = Search.Where(p => p.IdManufacturer == selectedManufacturer.Id).ToList();
            }
            // ���������� �� ������
            string SearchText = SearchTextBox.Text ?? "";
            if (SearchText.Length > 0)
            {
                Search = Search.Where(y => y.TovarName.Contains(SearchText) 
                                        || y.Description.Contains(SearchText)).ToList();
            }

            Vsego.Text = Convert.ToString(Helper.DataBase.Products.Count());// ������� ����� ��-���
            Pokaz.Text = Convert.ToString(Search.Count);//������� ������������ ��-�� ������
            ListBox.ItemsSource = Search;
        }// �������� ������ � �������� � ����������

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
        }// ���� ����������
        private void BoxOne_SelectionChanged(object? sender, SelectionChangedEventArgs e) => List_Box();
        private void BoxTwo_SelectionChanged(object? sender, SelectionChangedEventArgs e) => List_Box();
        private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e) => List_Box();
     
    }
}