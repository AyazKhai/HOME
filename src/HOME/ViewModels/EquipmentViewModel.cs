using CommunityToolkit.Mvvm.Input;
using HOME.DOMAIN.Enums;
using HOME.DOMAIN.Interfaces;
using HOME.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HOME.ViewModels
{
    public class EquipmentViewModel : INotifyPropertyChanged
    {
        private readonly IEquipmentRepository _repository;
        private Equipment _selectedEquipment;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<Equipment> Equipments { get; } = new();
        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged();
                ((RelayCommand)EditCommand).NotifyCanExecuteChanged();
            }
        }

        public EquipmentViewModel(IEquipmentRepository repository)
        {
            _repository = repository;

            LoadCommand = new AsyncRelayCommand(LoadEquipmentsAsync);
            AddCommand = new RelayCommand(AddEquipment);
            EditCommand = new RelayCommand(EditEquipment, CanEditEquipment);
            DeleteCommand = new AsyncRelayCommand(DeleteEquipmentAsync);
        }

        private async Task LoadEquipmentsAsync()
        {
            var equipments = await _repository.GetAllAsync();
            Equipments.Clear();
            foreach (var equipment in equipments)
            {
                Equipments.Add(equipment);
            }
        }

        private void AddEquipment()
        {
            var newEquipment = new Equipment
            {
                Name = "New Equipment",
                Type = EquipmentType.Printer,
                Status = Status.InStock
            };
            _repository.AddAsync(newEquipment); // Убедитесь, что это асинхронный вызов
            Equipments.Add(newEquipment); // Добавляем в ObservableCollection

            SelectedEquipment = newEquipment; // Выделяем новый элемент


        }

        private bool CanEditEquipment() => SelectedEquipment != null;

        private void EditEquipment()
        {
            var editWindow = new EditEquipmentWindow(SelectedEquipment, _repository);
            if (editWindow.ShowDialog() == true)
            {
                LoadEquipmentsAsync(); // Обновляем список после редактирования
            }
        }

        private async Task DeleteEquipmentAsync()
        {
            if (SelectedEquipment != null)
            {
                await _repository.DeleteAsync(SelectedEquipment.Id);
                Equipments.Remove(SelectedEquipment);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
