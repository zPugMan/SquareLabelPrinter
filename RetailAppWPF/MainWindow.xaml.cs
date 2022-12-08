using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using RetailAppWPF.Services;
using RetailAppWPF.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.ComponentModel;
using RetailAppWPF.ViewModels;
using Microsoft.Extensions.Logging;

namespace RetailAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _log;
        public MainWindow(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<MainWindow>();
            _log.LogInformation($"Application starting in: {Environment.CurrentDirectory}");
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            _log.LogInformation("RetailAppWPF started");
        }

    }
}
