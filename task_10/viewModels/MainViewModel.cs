using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using task_10.models;
using task_10.services;
using task_10.viewModels.PhoneBookApp.Helpers;

namespace task_10.viewModels
{
    /// Получает IDialogService через конструктор
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDialogService _dialogService;

        public ObservableCollection<Contact> Contacts { get; set; }

        private string _name;
        private string _phone;
        private Contact _selectedContact;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); AddCommand.RaiseCanExecuteChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); AddCommand.RaiseCanExecuteChanged(); }
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set { _selectedContact = value; OnPropertyChanged(); DeleteCommand.RaiseCanExecuteChanged(); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }

        /// Конструктор с внедрением
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            Contacts = new ObservableCollection<Contact>();

            AddCommand = new RelayCommand(_ => AddContact(), _ => CanAdd());
            DeleteCommand = new RelayCommand(p => DeleteContact(p as Contact), p => p is Contact);
        }

        private void AddContact()
        {
            if (!IsValidName(Name))
            {
                _dialogService.ShowError("Имя не должно быть пустым");
                return;
            }

            if (!IsValidPhone(Phone))
            {
                _dialogService.ShowError("Некорректный номер телефона");
                return;
            }

            // Проверка дубликатов
            if (Contacts.Any(c => c.Phone == Phone))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует");
                return;
            }

            Contacts.Add(new Contact { Name = Name, Phone = Phone });

            _dialogService.ShowInfo("Контакт успешно добавлен");

            Name = string.Empty;
            Phone = string.Empty;
        }

        private void DeleteContact(Contact contact)
        {
            if (contact == null) return;

            if (_dialogService.ShowConfirmation("Удалить контакт?"))
            {
                Contacts.Remove(contact);
            }
        }

        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone);
        }

        private bool IsValidName(string name)
            => !string.IsNullOrWhiteSpace(name);

        private bool IsValidPhone(string phone)
            => Regex.IsMatch(phone, @"^(\+7\d{10}|8\d{10})$");

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
