using CommunityToolkit.Mvvm.Input;
using HOME.DOMAIN.Enums;
using HOME.DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;

namespace HOME.ViewModels
{
    public class EditEquipmentViewModel : INotifyPropertyChanged
    {
        private readonly IEquipmentRepository _repository;
        private Equipment _editableEquipment;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Equipment EditableEquipment
        {
            get => _editableEquipment;
            set
            {
                _editableEquipment = value;
                OnPropertyChanged();
            }
        }

        public Array EquipmentTypes => Enum.GetValues(typeof(EquipmentType));
        public Array StatusTypes => Enum.GetValues(typeof(Status));

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditEquipmentViewModel(IEquipmentRepository repository, Equipment equipmentToEdit)
        {
            _repository = repository;
            EditableEquipment = new Equipment(equipmentToEdit);

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            CancelCommand = new RelayCommand(Cancel);
        }

        private async Task SaveAsync()
        {
            try
            {
                Debug.WriteLine($"Попытка сохранения: ID={EditableEquipment.Id}, Name={EditableEquipment.Name}");

                // 1. Сохраняем в БД
                await _repository.UpdateAsync(EditableEquipment);

                // 2. Диагностика
                Debug.WriteLine("Успешно сохранено в БД");

                // 3. Закрываем окно
                CloseWindow(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка сохранения: {ex}");
                MessageBox.Show($"Не удалось сохранить изменения: {ex.Message}");
            }
        }

        private void Cancel()
        {
            CloseWindow(false);
        }

        private void CloseWindow(bool dialogResult)
        {
            var window = System.Windows.Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.DataContext == this);

            if (window != null)
            {
                window.DialogResult = dialogResult;
                window.Close();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
