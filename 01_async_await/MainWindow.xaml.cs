using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _01_async_await
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random rand = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }
        //async - allow method to use await
        //await - wait task without freezing
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //int value = GenerateValue();  //Freez

            //Task<int> task = Task.Run(GenerateValue);
            //task.Wait();//Freez
            //list.Items.Add(task.Result);//Freez - end
            //end

            //int value = await task;//without freezing

            //int value = await Task.Run(GenerateValue);
            //int value = await GenerateValueAsync();
            //list.Items.Add(value);
            //File.Copy()

            list.Items.Add(await GenerateValueAsync());
           //  GenerateValueAsync();
           //  GenerateValueAsync();
           //int a =  await GenerateValueAsync();
           // await GenerateValueAsync();


            //MessageBox.Show("Generated!");
            //FileStream file = new FileStream();
            // await file.DisposeAsync();

        }
        int GenerateValue()
        {
            Thread.Sleep(rand.Next(5000));
            
            return rand.Next(1000);
        }
        Task<int> GenerateValueAsync()
        {

            return Task.Run(() =>
            {
                Thread.Sleep(rand.Next(5000));

                return rand.Next(1000);
            });
          
        }
    }
}
