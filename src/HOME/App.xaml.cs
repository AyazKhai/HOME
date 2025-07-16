using HOME.Infrastructure.Data;
using System;
using System.Configuration;
using System.Data;
using System.Windows;
using HOME.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HOME.DOMAIN.Interfaces;
using HOME.Infrastructure.Repository;
using HOME.ViewModels;
using HOME.Views;

namespace HOME
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=equipment.db"));

            services.AddScoped<IEquipmentRepository, EquipmentRepository>();

            services.AddTransient<EquipmentViewModel>();
            services.AddTransient<EditEquipmentViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создание БД при первом запуске
            var dbContext = _serviceProvider.GetService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();

            var mainWindow = new Views.MainWindow
            {
                DataContext = _serviceProvider.GetService<EquipmentViewModel>()
            };
            mainWindow.Show();
        }
    }

}
