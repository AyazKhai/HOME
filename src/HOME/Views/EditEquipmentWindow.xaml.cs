using HOME.DOMAIN.Enums;
using HOME.DOMAIN.Interfaces;
using HOME.ViewModels;
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
using System.Windows.Shapes;

namespace HOME.Views
{
    /// <summary>
    /// Логика взаимодействия для EditEquipmentWindow.xaml
    /// </summary>
    public partial class EditEquipmentWindow : Window
    {
        public EditEquipmentWindow(Equipment equipmentToEdit, IEquipmentRepository repository)
        {
            InitializeComponent();
            DataContext = new EditEquipmentViewModel(repository, equipmentToEdit);
        }
    }
}
