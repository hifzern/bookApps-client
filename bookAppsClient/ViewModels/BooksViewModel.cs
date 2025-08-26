using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.ComponentModel;
using System.Collections.ObjectModel;
using bookAppsClient.Models;
using bookAppsClient.Utils;
using System.Security.Policy;


namespace bookAppsClient.ViewModels
{
    public class BooksViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _http = new HttpClient()
        {
            BaseAddress = new Uri("http://api.projectidek.dev/")
        };

        public ObservableCollection<Book> Books { get;  } = new();
        private Book? _selected;
        public Book? Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }
        private string _title = ""; public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        private string _author = ""; public string Author { get => _author; set { _author = value; OnPropertyChanged(); } }
        private int _year; public int Year { get => _year; set { _year = value; OnPropertyChanged(); } }

        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand NewCommand { get; }

        private bool _isBusy; public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }
        private string _status = ""; public string Status { get => _status; set {  _status = value; OnPropertyChanged(); } }
        public BooksViewModel()
        {
            LoadCommand = new RelayCommand(async _ => await LoadAsync(), _ => !IsBusy);
            AddCommand = new RelayCommand(async _ => await AddAsync(), _ => !IsBusy && !string.IsNullOrWhiteSpace(Title));
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync(), _ => !IsBusy && Selected != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => !IsBusy && Selected != null);
            NewCommand = new RelayCommand(_ => { Selected = null; Title = ""; Author = ""; Year = DateTime.Now.Year; });

        }
    }
}
