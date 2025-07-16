using CommunityToolkit.Mvvm.Input;
using HOME.DOMAIN.Enums;
using HOME.DOMAIN.Interfaces;
using HOME.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            // Загружаем данные при инициализации ViewModel
            LoadEquipmentsAsync().ConfigureAwait(false);
        }

        private async Task LoadEquipmentsAsync()
        {
            try
            {
                var equipments = await _repository.GetAllAsync();
                Equipments.Clear();
                foreach (var equipment in equipments)
                {
                    Equipments.Add(equipment);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при загрузке оборудования: {ex}");
                MessageBox.Show($"Не удалось загрузить данные: {ex.Message}");
            }
        }

        private async void AddEquipment()
        {
            var newEquipment = new Equipment
            {
                Name = "New Equipment",
                Type = EquipmentType.Printer,
                Status = Status.InStock
            };

            try
            {
                await _repository.AddAsync(newEquipment);
                Equipments.Add(newEquipment);
                SelectedEquipment = newEquipment;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при добавлении оборудования: {ex}");
                MessageBox.Show($"Не удалось добавить оборудование: {ex.Message}");
            }
        }

        private bool CanEditEquipment() => SelectedEquipment != null;

        private void EditEquipment()
        {
            var editWindow = new EditEquipmentWindow(SelectedEquipment, _repository);
            if (editWindow.ShowDialog() == true)
            {
                LoadEquipmentsAsync().ConfigureAwait(false);
            }
        }

        private async Task DeleteEquipmentAsync()
        {
            if (SelectedEquipment != null)
            {
                try
                {
                    await _repository.DeleteAsync(SelectedEquipment.Id);
                    Equipments.Remove(SelectedEquipment);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при удалении оборудования: {ex}");
                    MessageBox.Show($"Не удалось удалить оборудование: {ex.Message}");
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
