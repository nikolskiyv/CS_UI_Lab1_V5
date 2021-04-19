using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ClassLibrary;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private V5MainCollection MainCollection = new V5MainCollection();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainCollection;
        }

        private void Add_FromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog Dialog = new Microsoft.Win32.OpenFileDialog();
                if ((bool)Dialog.ShowDialog())
                    MainCollection.AddFromFile(Dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add From File error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void AddDefault_V5DataCollection_Click(object sender, RoutedEventArgs e)
        {
            MainCollection.AddDefaultDataCollection();
            ErrorMsg();
        }

        private void AddDefaults_Click(object sender, RoutedEventArgs e)
        {
            MainCollection.AddDefaults();
            DataContext = MainCollection;
            ErrorMsg();
        }

        private void AddDefault_V5DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            MainCollection.AddDefaultDataOnGrid();
            ErrorMsg();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (MainCollection.IsChanged)
            {
                UnsavedChanges();
            } 
            MainCollection = new V5MainCollection();
            DataContext = MainCollection;
            ErrorMsg();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainCollection.IsChanged)
                {
                    UnsavedChanges();
                }
                Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
                if ((bool)fd.ShowDialog())
                {
                    MainCollection = new V5MainCollection();
                    MainCollection.Load(fd.FileName);
                    DataContext = MainCollection;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loading Error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog())
                    MainCollection.Save(dialog.FileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Saving Error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private bool UnsavedChanges()
        {
            MessageBoxResult Message = MessageBox.Show("Save Changes?", "Save", MessageBoxButton.YesNoCancel);
            if (Message == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)Dialog.ShowDialog())
                    MainCollection.Save(Dialog.FileName);
            }
            else if (Message == MessageBoxResult.Cancel)
            {
                return true;
            }
            return false;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainCollection.IsChanged)
            {
                e.Cancel = UnsavedChanges();
            }
            ErrorMsg();
        }

        public void ErrorMsg()
        {
            if (MainCollection.ErrorMessage != null)
            {
                MessageBox.Show(MainCollection.ErrorMessage, "Error");
                MainCollection.ErrorMessage = null;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedLB = listBox_Main.SelectedItems;
            List<V5Data> Items = new List<V5Data>();
            Items.AddRange(selectedLB.Cast<V5Data>());
            foreach (V5Data item in Items)
            {
                MainCollection.Remove(item.Info, item.Time);
            }
            ErrorMsg();
        }

        private void FilterDataCollection(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V5DataCollection)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void FilterDataOnGrid(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V5DataOnGrid)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void LB_DoG_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
