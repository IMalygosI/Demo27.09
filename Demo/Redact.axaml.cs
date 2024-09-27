using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Demo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Demo;

public partial class Redact : Window
{
    public Redact() { }

    public Redact(string id)
    {
        InitializeComponent();
        Id.Text = id;

        if (string.IsNullOrEmpty(id)) // Если id пуст, это новый продукт
        {
            Udalit_Button.IsVisible = false; // Скрыть кнопку Удалить
        }
        else
        {
            Udalit_Button.IsVisible = true; // Показать кнопку Удалить
        }


        List<Manufacturer> manufacturers = Helper.DataBase.Manufacturers.ToList();
        manufacturer.ItemsSource = manufacturers;

        Product product = Helper.DataBase.Products.FirstOrDefault(p => p.Id.ToString() == id);
        if (product != null)
        {
            imag.DataContext = product;

            Manufacturer selectedItem = manufacturers.FirstOrDefault(m => m.Id == product.IdManufacturer);
            manufacturer.SelectedItem = selectedItem; 

            TovarName.Text = product.TovarName;
            Cost.Text = product.Cost.ToString();
            Description.Text = product.Description?.ToString() ?? string.Empty;
            ImageControl.Text = product.MainImagePath;
            IsActiveComboBox.SelectedItem = null;

            IsActiveComboBox.Items.Add("Активный");
            IsActiveComboBox.Items.Add("Неактивный");
            string activity = product.IdActivity == 1 ? "Активный" : "Неактивный";
            IsActiveComboBox.SelectedItem = IsActiveComboBox.Items.FirstOrDefault(item => item.ToString() == activity);

        }
        Otmena_Button.Click += Otmena_Button_Click;
        Redact_Button.Click += Redact_Button_Click;
        PictureButton.Click += Button_Picture;
        Udalit_Button.Click += Udalit_Button_Click;
        Button_History.Click += History_Click;
    }

    private void History_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

    }


    private void Udalit_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string id = Id.Text;
        Product product = Helper.DataBase.Products.FirstOrDefault(p => p.Id.ToString() == id);
        if (product != null)
        {
            Helper.DataBase.Products.Remove(product);
            Helper.DataBase.SaveChanges();
        }
        MainWindow vvv = new MainWindow();
        vvv.Show();
        Close();
    }
    private void Redact_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Получаем данные из полей ввода
        string id = Id?.Text;
        string tovarName = TovarName?.Text;
        string description = Description?.Text;
        string cost = Cost?.Text;
        string mainImagePath = ImageControl?.Text;

        string activity = (IsActiveComboBox?.SelectedItem as string) ?? string.Empty;

        var selectedManufacturer = manufacturer?.SelectedItem as Manufacturer;

        // Проверяем, пустой ли id, если да, создаем новый продукт
        if (string.IsNullOrEmpty(id))
        {
            if (string.IsNullOrEmpty(tovarName)                 
                || string.IsNullOrEmpty(description)
                || string.IsNullOrEmpty(mainImagePath) 
                || string.IsNullOrEmpty(cost) 
                || selectedManufacturer == null)
            {
                Console.WriteLine("Не все поля заполнены");
                return;
            }

            Product newProduct = new Product();
            newProduct.TovarName = tovarName;
            newProduct.Description = description;
            newProduct.MainImagePath = mainImagePath;
            newProduct.IdActivity = 1;
            newProduct.IdManufacturer = selectedManufacturer.Id;

            if (decimal.TryParse(cost, out decimal parsedCost))
            {
                newProduct.Cost = parsedCost;
            }
            else
            {
                Console.WriteLine("Недопустимый формат цены");
                return;
            }

            Helper.DataBase.Products.Add(newProduct);
        }
        else
        {
            Product product = Helper.DataBase.Products.FirstOrDefault(p => p.Id.ToString() == id);
            if (product != null)
            {
                if (string.IsNullOrEmpty(tovarName) 
                    || string.IsNullOrEmpty(description)
                    || string.IsNullOrEmpty(mainImagePath)
                    || string.IsNullOrEmpty(cost) 
                    || selectedManufacturer == null)
                {
                    Console.WriteLine("Не все поля заполнены");
                    return;
                }

                product.TovarName = tovarName;
                product.Description = description;
                product.MainImagePath = mainImagePath;
                product.IdActivity = activity == "Активный" ? 1 : 2;
                product.IdManufacturer = selectedManufacturer.Id;

                if (decimal.TryParse(cost, out decimal parsedCost))
                {
                    product.Cost = parsedCost;
                }
                else
                {
                    Console.WriteLine("Недопустимый формат цены");
                    return;
                }
            }
        }

        Helper.DataBase.SaveChanges();

        MainWindow vvv = new MainWindow();
        vvv.Show();
        this.Close();
    }

    private async void Button_Picture(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Файлы изображений", Extensions = { "jpg", "jpeg", "png", "bmp" } });

        string[] files = await openFileDialog.ShowAsync(this);
        if (files != null && files.Length > 0)
        {
            string selectedFile = files[0];
            string fileName = System.IO.Path.GetFileName(selectedFile);
            string folderPath = $@"Assets\\Товары школы\\"; // путь к папке Assets
            string filePath = System.IO.Path.Combine(folderPath, fileName);

            try
            {
                System.IO.File.Copy(selectedFile, filePath, true);
                ImageControl.Text = fileName;
                Product product = Helper.DataBase.Products.FirstOrDefault(p => p.Id.ToString() == Id.Text);
                if (product != null)
                {
                    product.MainImagePath = filePath;
                    Bitmap bitmap = new Bitmap(filePath);
                    imag.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка копирования файла: " + ex.Message);
            }
        }
    }

    private void Otmena_Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow vvv = new MainWindow();
        vvv.Show();
        Close();
    }
}